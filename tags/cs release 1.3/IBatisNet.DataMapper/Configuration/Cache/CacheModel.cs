
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

#region Using

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Logging;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.MappedStatements;

#endregion

namespace IBatisNet.DataMapper.Configuration.Cache
{

	/// <summary>
	/// Summary description for CacheModel.
	/// </summary>
	[Serializable]
	[XmlRoot("cacheModel", Namespace="http://ibatis.apache.org/mapping")]
	public class CacheModel
	{
		#region Fields

		private static IDictionary  _lockMap = new HybridDictionary();

		[NonSerialized]
		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );
		/// <summary>
		/// This is used to represent null objects that are returned from the cache so 
		/// that they can be cached, too.
		/// </summary>
		[NonSerialized] 
		public readonly static object NULL_OBJECT = new Object(); 

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
					throw new ArgumentNullException("The id attribute is mandatory in a cacheModel tag.");

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
					throw new ArgumentNullException("The implementation attribute is mandatory in a cacheModel tag.");

				_implementation = value; 
			}
		}

		/// <summary>
		/// Set or get the flushInterval (in Ticks)
		/// </summary>
		[XmlElement("flushInterval",typeof(FlushInterval))]
		public FlushInterval FlushInterval
		{
			get { return _flushInterval; }
			set { _flushInterval = value; }
		}
		#endregion

		#region Constructor (s) / Destructor


		/// <summary>
		/// Constructor
		/// </summary>
		public CacheModel() 
		{
			_lastFlush = DateTime.Now.Ticks;
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

			try 
			{
				if (_implementation == null)
				{
					throw new DataMapperException ("Error instantiating cache controller for cache named '"+_id+"'. Cause: The class for name '"+_implementation+"' could not be found.");
				}

				// Build the CacheController
				Type type = Resources.TypeForName(_implementation);
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
				throw new ConfigurationException ("Error configuring controller named '"+_id+"'. Cause: " + e.Message, e);
			}
			finally
			{

			}
		}


		/// <summary>
		/// Event listener
		/// </summary>
		/// <param name="mappedStatement">A MappedStatement on which we listen ExecuteEventArgs event.</param>
		public void RegisterTriggerStatement(IMappedStatement mappedStatement)
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
			_lastFlush = DateTime.Now.Ticks;
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
		public object this [CacheKey key] 
		{
			get
			{
				lock(this) 
				{
					if (_lastFlush != NO_FLUSH_INTERVAL
						&& (DateTime.Now.Ticks - _lastFlush > _flushInterval.Interval)) 
					{
						Flush();
					}
				}

				object value = null;
				lock (GetLock(key)) 
				{
					value = _controller[key];
				}

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
				if (null == value) value = NULL_OBJECT;
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
					return (double)_hits/(double)_requests;
				}
				else
				{
					return 0;
				}
			}
		}


		/// <summary>
		/// Add a property
		/// </summary>
		/// <param name="name">The name of the property</param>
		/// <param name="value">The value of the property</param>
		public void AddProperty(string name, string value)
		{
			_properties.Add(name, value);
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public object GetLock(CacheKey key) 
		{
			int controllerId = HashCodeProvider.GetIdentityHashCode(_controller);
			int keyHash = key.GetHashCode();
			int lockKey = 29 * controllerId + keyHash;
			object lok =_lockMap[lockKey];
			if (lok == null) 
			{
				lok = lockKey; //might as well use the same object
				_lockMap[lockKey] = lok;
			}
			return lok;
		}
	}
}
