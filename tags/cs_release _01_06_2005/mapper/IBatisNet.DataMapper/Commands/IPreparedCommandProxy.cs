
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
using System.Data;
using System.Collections;
using System.Reflection;
using System.Xml.Serialization;
using System.Text;

using Castle.DynamicProxy;

using IBatisNet.Common;
using IBatisNet.Common.Utilities.Objects;

using IBatisNet.Common.Exceptions;

using log4net;
#endregion

namespace IBatisNet.DataMapper.Commands
{
	/// <summary>
	/// Summary description for IPreparedCommandProxy.
	/// </summary>
	public class IPreparedCommandProxy : IInterceptor
	{

		#region Fields
		private IPreparedCommand _preparedCommand = null;
		private static ArrayList _passthroughMethods = new ArrayList();
		private static readonly ILog _logger = LogManager.GetLogger( "IBatisNet.DataMapper.Commands.IPreparedCommand" );

		#endregion 

		#region Constructors

		static IPreparedCommandProxy()
		{
			_passthroughMethods.Add("GetType");
			_passthroughMethods.Add("ToString");
		}

		/// <summary>
		/// Constructor for a connection proxy
		/// </summary>
		/// <param name="preparedCommand">The connection which been proxified.</param>
		internal IPreparedCommandProxy(IPreparedCommand preparedCommand)
		{
			_preparedCommand = preparedCommand;
		}
		#endregion 

		#region Methods

		/// <summary>
		/// Static constructor
		/// </summary>
		/// <param name="preparedCommand">The connection which been proxified.</param>
		/// <returns>A proxy</returns>
		internal static IPreparedCommand NewInstance(IPreparedCommand preparedCommand)
		{
			object proxyIPreparedCommand = null;
			IInterceptor handler = new IPreparedCommandProxy(preparedCommand);

			ProxyGenerator proxyGenerator = new ProxyGenerator();

			proxyIPreparedCommand = proxyGenerator.CreateProxy(typeof(IPreparedCommand), handler, preparedCommand);

			return (IPreparedCommand) proxyIPreparedCommand;
		}
		#endregion 

		#region IInterceptor Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invocation"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public object Intercept(IInvocation invocation, params object[] arguments)
		{
			object returnValue = null;

			returnValue = invocation.Method.Invoke( _preparedCommand, arguments);

			if (invocation.Method.Name=="Create")
			{
				if (_logger.IsDebugEnabled)
				{
					IDbCommand command = (IDbCommand)returnValue;

					_logger.Debug("PreparedStatement : [" + command.CommandText + "]");
					if (command.Parameters.Count>0)
					{
						StringBuilder valueList = new StringBuilder(); // Log info
						StringBuilder typeList = new StringBuilder(); // Log info 

						for ( int i = 0; i < command.Parameters.Count; ++i )
						{
							IDataParameter sqlParameter = (IDataParameter)command.Parameters[i];
							if (sqlParameter.Value == System.DBNull.Value) 
							{
								valueList.Append("null,");
								typeList.Append("null,");
							} 
							else 
							{
								valueList.Append( sqlParameter.Value.ToString() );
								valueList.Append( "," );
								typeList.Append( sqlParameter.Value.GetType().ToString() );
								typeList.Append( "," );
							}
						}

						_logger.Debug("Parameters: [" + valueList.ToString().Remove(valueList.ToString().Length-1,1) + "]");
						_logger.Debug("Types: [" + typeList.ToString().Remove(typeList.ToString().Length-1,1) + "]");
					}
				}
			}

			return returnValue;
		}

		#endregion
	}
}
