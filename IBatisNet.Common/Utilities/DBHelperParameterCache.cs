
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

using System;
using System.Collections;
using System.Data;
using System.Reflection;
using IBatisNet.Common.Exceptions;

namespace IBatisNet.Common.Utilities
{
	/// <summary>
	/// DBHelperParameterCache provides functions to leverage a 
	/// static cache of procedure parameters, and the
	/// ability to discover parameters for stored procedures at run-time.
	/// </summary>
	public class DBHelperParameterCache
	{
		//Since this class provides only static methods, make the default constructor private to prevent 
		//instances from being created.
		private DBHelperParameterCache() {}

		#region Private fields
		private static Hashtable _paramCache = Hashtable.Synchronized(new Hashtable());
		#endregion

		#region private methods

		/// <summary>
		/// Resolve at run time the appropriate set of Parameters for a stored procedure
		/// </summary>
		/// <param name="dataSource">a valid dataSource</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
		/// <returns></returns>
		private static IDataParameter[] DiscoverSpParameterSet(DataSource dataSource, string spName, bool includeReturnValueParameter)
		{
			using (IDbConnection connection = dataSource.Provider.GetConnection())
			{
				connection.ConnectionString = dataSource.ConnectionString;
				connection.Open();
				return InternalDiscoverSpParameterSet(dataSource.Provider, connection, spName, includeReturnValueParameter);
			}		
		}

		/// <summary>
		/// resolve at run time the appropriate set of Parameters for a stored procedure
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="connection">a valid open IDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
		/// <returns></returns>
		private static IDataParameter[] InternalDiscoverSpParameterSet(Provider provider,
			IDbConnection connection, string spName, bool includeReturnValueParameter)
		{
			using (IDbCommand cmd = connection.CreateCommand())
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = spName;

				DeriveParameters(provider, cmd);

				if (cmd.Parameters.Count > 0) {
					IDataParameter firstParameter = (IDataParameter)cmd.Parameters[0];
					if (firstParameter.Direction == ParameterDirection.ReturnValue) {
						if (!includeReturnValueParameter) {
							cmd.Parameters.RemoveAt(0);
						}
					}	
				}


				IDataParameter[] discoveredParameters = new IDataParameter[cmd.Parameters.Count];

				cmd.Parameters.CopyTo(discoveredParameters, 0);

				return discoveredParameters;
			}
		}
		
		private static void DeriveParameters(Provider provider, IDbCommand command)
		{
			Type commandBuilderType;

			// Find the CommandBuilder
			if (provider == null)
				throw new ArgumentNullException("provider");
			if ((provider.CommandBuilderClass == null) || (provider.CommandBuilderClass.Length < 1))
				throw new Exception(String.Format(
					"CommandBuilderClass not defined for provider \"{0}\".",
					provider.Name));
			commandBuilderType = provider.GetCommandBuilderType();

			// Invoke the static DeriveParameter method on the CommandBuilder class
			// NOTE: OracleCommandBuilder has no DeriveParameter method
			try
			{
				commandBuilderType.InvokeMember("DeriveParameters",
				                                BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, null, null,
					new object[] {command});
			}
			catch(Exception ex)
			{
				throw new IBatisNetException("Could not retrieve parameters for the store procedure named "+command.CommandText, ex);
			}
		}

		/// <summary>
		/// Deep copy of cached IDataParameter array.
		/// </summary>
		/// <param name="originalParameters"></param>
		/// <returns></returns>
		private static IDataParameter[] CloneParameters(IDataParameter[] originalParameters)
		{
			IDataParameter[] clonedParameters = new IDataParameter[originalParameters.Length];

			for (int i = 0, j = originalParameters.Length; i < j; i++)
			{
				clonedParameters[i] = (IDataParameter) ((ICloneable) originalParameters[i]).Clone();
			}

			return clonedParameters;
		}

		#endregion private methods, variables, and constructors

		#region caching functions

		/// <summary>
		/// Add parameter array to the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for an IDbConnection</param>
		/// <param name="commandText">the stored procedure name or SQL command</param>
		/// <param name="commandParameters">an array of IDataParameters to be cached</param>
		public static void CacheParameterSet(string connectionString, string commandText, params IDataParameter[] commandParameters)
		{
			string hashKey = connectionString + ":" + commandText;

			_paramCache[hashKey] = commandParameters;
		}


		// FM Added
		/// <summary>
		/// Clear the parameter cache.
		/// </summary>
		public static void Clear()
		{
			_paramCache.Clear();
		}

		/// <summary>
		/// retrieve a parameter array from the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for an IDbConnection</param>
		/// <param name="commandText">the stored procedure name or SQL command</param>
		/// <returns>an array of IDataParameters</returns>
		public static IDataParameter[] GetCachedParameterSet(string connectionString, string commandText)
		{
			string hashKey = connectionString + ":" + commandText;

			IDataParameter[] cachedParameters = (IDataParameter[]) _paramCache[hashKey];
			
			if (cachedParameters == null)
			{			
				return null;
			}
			else
			{
				return CloneParameters(cachedParameters);
			}
		}

		#endregion caching functions

		#region Parameter Discovery Functions

		/// <summary>
		/// Retrieves the set of IDataParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="dataSource">a valid dataSource</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an array of IDataParameters</returns>
		public static IDataParameter[] GetSpParameterSet(DataSource dataSource, string spName)
		{
			return GetSpParameterSet(dataSource, spName, false);
		}

		/// <summary>
		/// Retrieves the set of IDataParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="dataSource">a valid dataSource</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
		/// <returns>an array of IDataParameters</returns>
		public static IDataParameter[] GetSpParameterSet(DataSource dataSource
			, string spName, bool includeReturnValueParameter)
		{
			string hashKey = dataSource.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter":"");

			IDataParameter[] cachedParameters;
			
			cachedParameters = (IDataParameter[]) _paramCache[hashKey];

			if (cachedParameters == null)
			{
				_paramCache[hashKey] = DiscoverSpParameterSet(dataSource, spName, includeReturnValueParameter);
				cachedParameters = (IDataParameter[]) _paramCache[hashKey];
			}
			
			return CloneParameters(cachedParameters);
		}

		#endregion Parameter Discovery Functions
	}
}
