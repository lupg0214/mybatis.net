
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 591621 $
 * $Date: 2008-10-16 12:14:45 -0600 (Thu, 16 Oct 2008) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2008/2005 - The Apache Software Foundation
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
using System.Data;
using System.Reflection;
using MyBatis.DataMapper.Session;
using System.Collections.Generic;
using MyBatis.Common.Data;
using MyBatis.Common.Exceptions;

namespace MyBatis.DataMapper.Data
{
    /// <summary>
    /// DBHelperParameterCache provides functions to leverage a 
    /// static cache of procedure parameters, and the
    /// ability to discover parameters for stored procedures at run-time.
    /// </summary>
    public sealed class DBHelperParameterCache
    {
        private readonly object syncLock = new object();
        private readonly IDictionary<string, IDataParameter[]> paramCache = new Dictionary<string, IDataParameter[]>();

        #region private methods

        /// <summary>
        /// Resolve at run time the appropriate set of Parameters for a stored procedure
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
        /// <returns></returns>
        private static IDataParameter[] DiscoverSpParameterSet(ISession session, string spName, bool includeReturnValueParameter)
        {
            return InternalDiscoverSpParameterSet(
                session,
                spName,
                includeReturnValueParameter);
        }


        /// <summary>
        /// Discover at run time the appropriate set of Parameters for a stored procedure
        /// </summary>
        /// <param name="session">An IDalSession object</param>
        /// <param name="?">The ?.</param>
        /// <param name="spName">Name of the stored procedure.</param>
        /// <param name="includeReturnValueParameter">if set to <c>true</c> [include return value parameter].</param>
        /// <returns>The stored procedure parameters.</returns>
		private static IDataParameter[] InternalDiscoverSpParameterSet(
            ISession session,
            string spName,
            bool includeReturnValueParameter)
        {
            IDbCommand dbCommand = session.SessionFactory.DataSource.DbProvider.CreateCommand();
            dbCommand.CommandType = CommandType.StoredProcedure;

            using (dbCommand)
            {
                dbCommand.CommandText = spName;
                dbCommand.Connection = session.Connection;

                // Assign transaction
                if (session.Transaction != null)
                {
                    session.Transaction.Enlist(dbCommand);
                }

                // The session connection object is always created but the connection is not alwys open
                // so we try to open it in case.
                session.OpenConnection();

                DeriveParameters(session.SessionFactory.DataSource.DbProvider, dbCommand);

                if (dbCommand.Parameters.Count > 0)
                {
                    IDataParameter firstParameter = (IDataParameter)dbCommand.Parameters[0];
                    if (firstParameter.Direction == ParameterDirection.ReturnValue)
                    {
                        if (!includeReturnValueParameter)
                        {
                            dbCommand.Parameters.RemoveAt(0);
                        }
                    }
                }


                IDataParameter[] discoveredParameters = new IDataParameter[dbCommand.Parameters.Count];
                dbCommand.Parameters.CopyTo(discoveredParameters, 0);
                return discoveredParameters;
            }
        }

        /// <summary>
        /// Derives the parameters.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="command">The command.</param>
		private static void DeriveParameters(IDbProvider provider, IDbCommand command)
        {
            // Find the CommandBuilder
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            if (string.IsNullOrEmpty(provider.CommandBuilderClass))
            {
                throw new Exception(String.Format(
                                        "CommandBuilderClass not defined for provider \"{0}\".",
                                        provider.Id));
            }

            Type commandBuilderType = provider.CommandBuilderType;
            // Invoke the static DeriveParameter method on the CommandBuilder class
            // NOTE: OracleCommandBuilder has no DeriveParameter method

            //修改采用OLEDB调用Oracle数据库时，方法“DeriveParameters”执行不成功问题，采用自己的方法。
            if (command.Connection is System.Data.OleDb.OleDbConnection &&
                command.Connection.ConnectionString.ToUpper().Contains("ORAOLEDB.ORACLE.1"))
            {
                try
                {
                    string commandTextOld = command.CommandText;
                    CommandType commandTypeOld = command.CommandType;
                    string procedureSchema = "";
                    string ProcedureName = "";
                    string sql = string.Format(@"select * 
                              from (select null PROCEDURE_CATALOG,
                                           owner PROCEDURE_SCHEMA,
                                           object_name PROCEDURE_NAME,
                                           decode(object_type, 'PROCEDURE', 2, 'FUNCTION', 3, 1) PROCEDURE_TYPE,
                                           null PROCEDURE_DEFINITION,
                                           null DESCRIPTION,
                                           created DATE_CREATED,
                                           last_ddl_time DATE_MODIFIED
                                      from all_objects
                                     where object_type in ('PROCEDURE', 'FUNCTION')
                                    union all
                                    select null PROCEDURE_CATALOG,
                                           arg.owner PROCEDURE_SCHEMA,
                                           arg.package_name || '.' || arg.object_name PROCEDURE_NAME,
                                           decode(min(arg.position), 0, 3, 2) PROCEDURE_TYPE,
                                           null PROCEDURE_DEFINITION,
                                           decode(arg.overload, '', '', 'OVERLOAD') DESCRIPTION,
                                           min(obj.created) DATE_CREATED,
                                           max(obj.last_ddl_time) DATE_MODIFIED
                                      from all_objects obj, all_arguments arg
                                     where arg.package_name is not null
                                       and arg.owner = obj.owner
                                       and arg.object_id = obj.object_id
                                     group by arg.owner, arg.package_name, arg.object_name, arg.overload) PROCEDURES
                             WHERE PROCEDURE_NAME = '{0}' or PROCEDURE_SCHEMA||'.'||PROCEDURE_NAME ='{1}'
                             order by 1,2, 3
                            ", command.CommandText.Trim(new char[] { ' ','\n',(char)13}), command.CommandText.Trim(new char[] { ' ','\n',(char)13}));
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    System.Data.IDataReader idataRead = command.ExecuteReader();
                    if (idataRead.Read())
                    {
                        procedureSchema = Convert.ToString(idataRead.GetValue(1));
                        ProcedureName = Convert.ToString(idataRead.GetValue(2));
                        idataRead.Close();
                    }
                    else
                    {
                        command.CommandText = commandTextOld;
                        command.CommandType = commandTypeOld;
                        throw new IbatisException("Could not retrieve parameters for the store procedure named " + command.CommandText);
                    }
                    sql = string.Format(@"select procedure_catalog,
                                            procedure_schema,
                                            procedure_name,
                                            parameter_name,
                                            ordinal_position,
                                            parameter_type,
                                            parameter_hasdefault,
                                            parameter_default,
                                            is_nullable,
                                            data_type,
                                            character_maximum_length,
                                            character_maximum_length character_octet_length,
                                            numeric_precision,
                                            numeric_scale,
                                            description,
                                            type_name,
                                            overload
                                        from (select null procedure_catalog,
                                                    owner procedure_schema,
                                                    decode(package_name,
                                                            NULL,
                                                            object_name,
                                                            package_name || '.' || object_name) procedure_name,
                                                    decode(position,
                                                            0,
                                                            'RETURN_VALUE',
                                                            nvl(argument_name, chr(0))) parameter_name,
                                                    position ordinal_position,
                                                    decode(in_out,
                                                            'IN',
                                                            1,
                                                            'IN/OUT',
                                                            2,
                                                            'OUT',
                                                            decode(argument_name, null, 4, 3),
                                                            null) parameter_type,
                                                    null parameter_hasdefault,
                                                    null parameter_default,
                                                    65535 is_nullable,
                                                    decode(data_type,
                                                            'CHAR',
                                                            129,
                                                            'NCHAR',
                                                            129,
                                                            'DATE',
                                                            135,
                                                            'FLOAT',
                                                            139,
                                                            'LONG',
                                                            129,
                                                            'LONG RAW',
                                                            128,
                                                            'NUMBER',
                                                            139,
                                                            'RAW',
                                                            128,
                                                            'ROWID',
                                                            129,
                                                            'VARCHAR2',
                                                            129,
                                                            'NVARCHAR2',
                                                            129,
                                                            'REF CURSOR',
                                                            12,
                                                            13) data_type,
                                                    decode(data_type,
                                                            'CHAR',
                                                            decode(data_length, null, 2000, data_length),
                                                            'LONG',
                                                            2147483647,
                                                            'LONG RAW',
                                                            2147483647,
                                                            'ROWID',
                                                            18,
                                                            'RAW',
                                                            decode(data_length, null, 2000, data_length),
                                                            'VARCHAR2',
                                                            decode(data_length, null, 4000, data_length),
                                                            'DATE',
                                                            null,
                                                            'FLOAT',
                                                            null,
                                                            'NUMBER',
                                                            null,
                                                            null) character_maximum_length,
                                                    decode(data_type,
                                                            'DATE',
                                                            19,
                                                            'FLOAT',
                                                            15,
                                                            'NUMBER',
                                                            decode(data_precision, null, 0, data_precision),
                                                            'CHAR',
                                                            null,
                                                            'NCHAR',
                                                            null,
                                                            'LONG',
                                                            null,
                                                            'LONG RAW',
                                                            null,
                                                            'RAW',
                                                            null,
                                                            'VARCHAR2',
                                                            null,
                                                            'NVARCHAR2',
                                                            null,
                                                            null) numeric_precision,
                                                    decode(data_type,
                                                            'DATE',
                                                            0,
                                                            'NUMBER',
                                                            decode(data_scale, null, 0, data_scale),
                                                            'CHAR',
                                                            null,
                                                            'NCHAR',
                                                            null,
                                                            'FLOAT',
                                                            null,
                                                            'LONG',
                                                            null,
                                                            'LONG RAW',
                                                            null,
                                                            'RAW',
                                                            null,
                                                            'VARCHAR2',
                                                            null,
                                                            'NVARCHAR2',
                                                            null,
                                                            null) numeric_scale,
                                                    null description,
                                                    data_type type_name,
                                                    overload
                                                from all_arguments
                                                where data_level = 0
                                                and data_type is not null) procedure_parameters
                                        where 1 = 1
                                        and procedure_schema = '{0}'
                                        and procedure_name = '{1}'
                                        order by 1, 2, 3, 5", procedureSchema, ProcedureName);
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    System.Data.IDataReader idataReadParm = command.ExecuteReader();
                    while (idataReadParm.Read())
                    {
                        var parm = new System.Data.OleDb.OleDbParameter(Convert.ToString(idataReadParm.GetValue(3)),
                            (System.Data.OleDb.OleDbType)Enum.Parse(typeof(System.Data.OleDb.OleDbType), Convert.ToString(idataReadParm.GetValue(9))));
                        switch (Convert.ToString(idataReadParm.GetValue(5)))
                        {
                            case "1":
                                parm.Direction = ParameterDirection.Input;
                                break;
                            case "2":
                                parm.Direction = ParameterDirection.InputOutput;
                                break;
                            case "3":
                                parm.Direction = ParameterDirection.Output;
                                break;
                            default:
                                parm.Direction = ParameterDirection.ReturnValue;
                                break;
                        }

                        command.Parameters.Add(parm);
                    }
                    idataReadParm.Close();
                    command.CommandText = commandTextOld;
                    command.CommandType = commandTypeOld;

                }
                catch (Exception ex)
                {
                    throw new IbatisException("Could not retrieve parameters for the store procedure named " + command.CommandText, ex);
                }
            }
            else
            {
                try
                {
                    commandBuilderType.InvokeMember("DeriveParameters",
                                                    BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, null, null,
                        new object[] { command });
                }
                catch (Exception ex)
                {
                    throw new IbatisException("Could not retrieve parameters for the store procedure named " + command.CommandText, ex);
                }
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

            int length = originalParameters.Length;
            for (int i = 0, j = length; i < j; i++)
            {
                clonedParameters[i] = (IDataParameter)((ICloneable)originalParameters[i]).Clone();
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
        public void CacheParameterSet(string connectionString, string commandText, params IDataParameter[] commandParameters)
        {
            string hashKey = connectionString + ":" + commandText;

            lock (syncLock)
            {
                paramCache[hashKey] = commandParameters;
            }
        }


        /// <summary>
        /// Clear the parameter cache.
        /// </summary>
        public void Clear()
        {
            paramCache.Clear();
        }

        /// <summary>
        /// retrieve a parameter array from the cache
        /// </summary>
        /// <param name="connectionString">a valid connection string for an IDbConnection</param>
        /// <param name="commandText">the stored procedure name or SQL command</param>
        /// <returns>an array of IDataParameters</returns>
        public IDataParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = connectionString + ":" + commandText;

            IDataParameter[] cachedParameters = null;
            paramCache.TryGetValue(hashKey, out cachedParameters);

            if (cachedParameters == null)
            {
                return null;
            }
            return CloneParameters(cachedParameters);
        }

        #endregion caching functions

        #region Parameter Discovery Functions

        /// <summary>
        /// Retrieves the set of IDataParameters appropriate for the stored procedure
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <returns>an array of IDataParameters</returns>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        public IDataParameter[] GetSpParameterSet(ISession session, string spName)
        {
            return GetSpParameterSet(session, spName, false);
        }

        /// <summary>
        /// Retrieves the set of IDataParameters appropriate for the stored procedure
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>an array of IDataParameters</returns>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        public IDataParameter[] GetSpParameterSet(
            ISession session,
            string spName,
            bool includeReturnValueParameter)
        {
            string hashKey = session.SessionFactory.DataSource.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            IDataParameter[] cachedParameters = null;

            paramCache.TryGetValue(hashKey, out cachedParameters);

            if (cachedParameters == null)
            {
                cachedParameters = DiscoverSpParameterSet(session, spName, includeReturnValueParameter);
                paramCache[hashKey] = cachedParameters;
            }

            return CloneParameters(cachedParameters);
        }

        #endregion Parameter Discovery Functions
    }
}
