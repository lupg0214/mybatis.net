
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
using System.Reflection;
using Castle.DynamicProxy;
using IBatisNet.Common.Logging;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.Common.Utilities.Proxy;
using IBatisNet.DataMapper.MappedStatements;

#endregion

namespace IBatisNet.DataMapper
{
	/// <summary>
	/// Summary description for LazyLoadList.
	/// </summary>
	[Serializable]
	internal class LazyLoadList : IInterceptor	
	{
		#region Fields
		private object _param = null;
		private object _target = null;
		private string _propertyName= string.Empty;
		private SqlMapper _sqlMap = null;
		private string _statementName = string.Empty;
		private bool _loaded = false;
		private IList _innerList = null;
		private object _loadLock = new object();
		private static ArrayList _passthroughMethods = new ArrayList();

		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );
		#endregion

		#region  Constructor (s) / Destructor

		/// <summary>
		/// Constructor for a lazy list loader
		/// </summary>
		static LazyLoadList()
		{
			_passthroughMethods.Add("GetType");
			_passthroughMethods.Add("ToString");
		}

		/// <summary>
		/// Constructor for a lazy list loader
		/// </summary>
		/// <param name="mappedSatement">The mapped statement used to build the list</param>
		/// <param name="param">The parameter object used to build the list</param>
		/// <param name="propertyName">The property's name which been proxified.</param>
		/// <param name="target">The target object which contains the property proxydied.</param>
		internal LazyLoadList(IMappedStatement mappedSatement, object param, object target,string propertyName)
		{
			_param = param;
			_statementName = mappedSatement.Id;
			_sqlMap = mappedSatement.SqlMap;
			_target = target; 
			_propertyName = propertyName;
		}		
		#endregion

		#region Methods
		/// <summary>
		/// Static constructor
		/// </summary>
		/// <param name="mappedSatement">The statement used to build the list</param>
		/// <param name="param">The parameter object used to build the list</param>
		/// <param name="propertyName">The property's name which been proxified.</param>
		/// <param name="target">The target object which contains the property proxydied.</param>
		/// <returns>A proxy</returns>
		internal static IList NewInstance(IMappedStatement mappedSatement, object param, object target,string propertyName)
		{
			object proxList = null;
			IInterceptor handler = new LazyLoadList(mappedSatement, param, target, propertyName);

			if (mappedSatement.Statement.ListClass != null)
			{
				proxList = ProxyGeneratorFactory.GetProxyGenerator().CreateProxy(typeof(IList), handler, mappedSatement.Statement.CreateInstanceOfListClass());
			}
			else
			{
				proxList = ProxyGeneratorFactory.GetProxyGenerator().CreateProxy(typeof(IList), handler, new ArrayList());
			}

			return (IList) proxList;
		}
		
		#region IInterceptor members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invocation"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public object Intercept(IInvocation invocation, params object[] arguments)
		{
			if (_logger.IsDebugEnabled) 
			{
				_logger.Debug("Proxyfying call to " + invocation.Method.Name);
			}

			lock(_loadLock)
			{
				if ((_loaded == false) && (!_passthroughMethods.Contains(invocation.Method.Name)))
				{
					if (_logger.IsDebugEnabled) 
					{
						_logger.Debug("Proxyfying call, query statement " + _statementName);
					}

					_innerList = _sqlMap.QueryForList(_statementName, _param);

					_loaded = true;
				}
			}

			object returnValue = invocation.Method.Invoke( _innerList, arguments);
			
			ObjectProbe.SetPropertyValue( _target, _propertyName, _innerList);

			if (_logger.IsDebugEnabled) 
			{
				_logger.Debug("End of proxyfied call to " + invocation.Method.Name);
			}

			return returnValue;
		}

		#endregion

		#endregion

	}
}
