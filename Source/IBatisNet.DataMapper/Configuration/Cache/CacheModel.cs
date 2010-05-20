
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: $
 * $Date: $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

#region Imports
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Xml.Serialization;

using log4net;

using IBatisNet.Common.Exceptions;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.MappedStatements;
#endregion

namespace IBatisNet.DataMapper.Configuration.Cache
{

	/// <summary>
	/// Summary description for CacheModel.
	/// </summary>
	[Serializable]
	[XmlRoot("cacheModel")]
	public class CacheModel
	{
		#region Fields

		[NonSerialized]
		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
		
		/// <summary>
		/// Constant to turn off periodic cache flushes
		/// </summary>
		[NonSerialized]
		public const long NO_FLUSH_INTERVAL = -99999;

		[NonSerialized]
		private object _statLock = new Object();
		[NonSerialized]
		private int _requests = 0;
		[NonSerialized]
		private int _hits = 0;
		[NonSerialized]
		private string _id = string.Empty;
		[NonSerialized]
		private ICacheController _controller = null;
		[NonSerialized]
		private FlushInterval _flushInterval = null;
		[NonSerialized]
		private long _lastFlush = 0;
		[NonSerialized]
		private HybridDictionary _properties = new HybridDictionary();
		[NonSerialized]
		private string _implementation = string.Empty;
		[NonSerialized]
		static private Hashtable _cacheControllerAliases = new Hashtable();

		#endregion

		#region Properties
		/// <summary>
		/// Identifier used to identify the CacheModel amongst the others.
		/// </summary>
		[XmlAttribute("id")]
		public string Id
		{
			get { return _id; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The id attribut is mandatory in a cacheModel tag.");

				_id = value; 
			}
		}

		/// <summary>
		/// Cache controller implementation name.
		/// </summary>
		[XmlAttribute("implementation")]
		public string Implementation
		{
			get { return _implementation; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The implementation attribut is mandatory in a cacheModel tag.");

				_implementation = value; 
			}
		}

		/// <summary>
		/// Set or get the flushInterval (in Ticks)
		/// </summary>
		[XmlElement("flushInterval",typeof(FlushInterval))]
		public FlushInterval FlushInterval
		{
			get 
			{
				return _flushInterval;
			}
			set 
			{
				_flushInterval = value;
			}
		}
		#endregion

		#region Constructor (s) / Destructor

		/// <summary>
		/// Constructor
		/// </summary>
		static CacheModel()
		{
			_cacheControllerAliases.Add("MEMORY","IBatisNet.DataMapper.Configuration.Cache.Memory.MemoryCacheControler");
			_cacheControllerAliases.Add("LRU","IBatisNet.DataMapper.Configuration.Cache.Lru.LruCacheController");
			_cacheControllerAliases.Add("FIFO","IBatisNet.DataMapper.Configuration.Cache.Fifo.FifoCacheController");
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CacheModel() 
		{
			_lastFlush = System.DateTime.Now.Ticks;
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		public void Initialize() 
		{
			// Initialize FlushInterval
			_flushInterval.Initialize();

			// Initialize controller
			Assembly assembly = null;
			Type type = null;

			_implementation = _cacheControllerAliases[_implementation.ToUpper()] as string;

			try 
			{
				if (_implementation == null)
				{
					throw new DataMapperException ("Error instantiating cache controller for cache named '"+_id+"'. Cause: The class for name '"+_implementation+"' could not be found.");
				}

				assembly = Assembly.GetCallingAssembly();

				// Build the CacheController
				type = assembly.GetType(_implementation, true);
				object[] arguments = new object[0];

				_controller = (ICacheController)Activator.CreateInstance(type, arguments);
			} 
			catch (Exception e) 
			{
				throw new ConfigurationException("Error instantiating cache controller for cache named '"+_id+". Cause: " + e.Message, e);
			}

			//------------ configure Controller---------------------
			try 
			{
				_controller.Configure(_properties);
			} 
			catch (Exception e) 
			{
				throw new ConfigurationException ("Error configuring controller named '"+_id+". Cause: " + e.Message, e);
			}
			finally
			{

			}
		}


		/// <summary>
		/// Event listener
		/// </summary>
		/// <param name="mappedStatement">A MappedStatement on which we listen ExecuteEventArgs event.</param>
		public void RegisterTriggerStatement(MappedStatement mappedStatement)
		{
			mappedStatement.Execute +=new ExecuteEventHandler(FlushHandler);
		}
		
		
		/// <summary>
		/// FlushHandler which clear the cache 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FlushHandler(object sender, ExecuteEventArgs e)
		{
			if (_logger.IsDebugEnabled) 
			{
				_logger.Debug("Flush cacheModel named "+_id+" for statement '"+e.StatementName+"'");
			}

			Flush();
		}


		/// <summary>
		/// Clears all elements from the cache.
		/// </summary>
		public void Flush() 
		{
			_lastFlush = System.DateTime.Now.Ticks;
			_controller.Flush();
		}


		/// <summary>
		/// Adds an item with the specified key and value into cached data.
		/// Gets a cached object with the specified key.
		/// </summary>
		/// <value>The cached object or <c>null</c></value>
		/// <remarks>
		/// A side effect of this method is that is may clear the cache
		/// if it has not been cleared in the flushInterval.
		/// </remarks> 
		public object this [object key] 
		{
			get
			{
				lock(this) 
				{
					if (_lastFlush != NO_FLUSH_INTERVAL
						&& (System.DateTime.Now.Ticks - _lastFlush > _flushInterval.Interval)) 
					{
						Flush();
					}
				}

				object value = _controller[key];

				lock(_statLock) 
				{
					_requests++;
					if (value != null) 
					{
						_hits++;
					}
				}
				return value;
			}
			set
			{
				_controller[key] = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double HitRatio 
		{
			get 
			{
				if (_requests!=0)
				{
					return
						(double)_hits/(double)_requests;
				}
				else
				{
					return 0;
				}
			}
		}


		/// <summary>
		/// Add a propertie
		/// </summary>
		/// <param name="name">The name of the propertie</param>
		/// <param name="value">The value of the propertie</param>
		public void AddPropertie(string name, string value)
		{
			_properties.Add(name, value);
		}
		#endregion

	}
}
