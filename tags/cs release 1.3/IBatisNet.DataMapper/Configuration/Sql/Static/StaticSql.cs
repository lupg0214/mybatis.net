
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
	/// Summary description for StaticSql.
	/// </summary>
	public class StaticSql : ISql
	{

		#region Fields

		private IStatement _statement = null ;
		private PreparedStatement _preparedStatement = null ;

		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="statement">The statement.</param>
		public StaticSql(IStatement statement)
		{
			_statement = statement;
		}
		#endregion

		#region ISql Members


		/// <summary>
		/// Get the sql command text to execute.
		/// </summary>
		/// <param name="parameterObject">The parameter object (used in DynamicSql)</param>
		/// <param name="session"></param>
		/// <returns>The sql command text.</returns>
		public RequestScope GetRequestScope(object parameterObject, IDalSession session)
		{
			RequestScope request = new RequestScope();

			request.ParameterMap = _statement.ParameterMap;
			request.ResultMap = _statement.ResultMap;
			request.PreparedStatement = _preparedStatement;

			return request;
		}

		/// <summary>
		/// Build the PreparedStatement
		/// </summary>
		/// <param name="session"></param>
		/// <param name="sqlStatement"></param>
		public void BuildPreparedStatement(IDalSession session, string sqlStatement)
		{
			RequestScope request = new RequestScope();

			request.ParameterMap = _statement.ParameterMap;

			PreparedStatementFactory factory = new PreparedStatementFactory( session, request, _statement, sqlStatement);
			_preparedStatement = factory.Prepare();
		}

		#endregion
	}
}
