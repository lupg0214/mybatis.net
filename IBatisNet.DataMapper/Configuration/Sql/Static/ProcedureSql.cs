
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

using IBatisNet.Common;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Scope;
#endregion


namespace IBatisNet.DataMapper.Configuration.Sql.Static
{
	/// <summary>
	/// Summary description for ProcedureSql.
	/// </summary>
	public class ProcedureSql : ISql
	{
		#region Fields

		private IStatement _statement = null ;
		private PreparedStatement _preparedStatement = null ;
		private string _sqlStatement = string.Empty;
		private object _synRoot = new Object();

		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="statement">The statement.</param>
		/// <param name="sqlStatement"></param>
		public ProcedureSql(string sqlStatement, IStatement statement)
		{
			_sqlStatement = sqlStatement;
			_statement = statement;
		}
		#endregion

		#region ISql Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterObject"></param>
		/// <param name="session"></param>
		/// <returns></returns>
		public RequestScope GetRequestScope(object parameterObject, IDalSession session)
		{
			RequestScope request = new RequestScope();

			request.ParameterMap = _statement.ParameterMap;
			request.ResultMap = _statement.ResultMap;
			request.PreparedStatement = BuildPreparedStatement(session, request, _sqlStatement);

			return request;
		}

		/// <summary>
		/// Build the PreparedStatement
		/// </summary>
		/// <param name="session"></param>
		/// <param name="commandText"></param>
		/// <param name="request"></param>
		public PreparedStatement BuildPreparedStatement(IDalSession session, RequestScope request, string commandText)
		{
			if ( _preparedStatement == null )
			{
				lock(_synRoot)
				{
					if (_preparedStatement==null)
					{
						PreparedStatementFactory factory = new PreparedStatementFactory( session, request, _statement, commandText);
						_preparedStatement = factory.Prepare();
					}
				}
			}
			return _preparedStatement;
		}

		#endregion
	}
}
