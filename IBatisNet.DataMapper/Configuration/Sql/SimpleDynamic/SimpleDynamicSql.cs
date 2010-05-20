
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
using System.Text;

using IBatisNet.Common;
using IBatisNet.DataMapper.Configuration.Sql;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.TypeHandlers;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Utilities.Objects;
#endregion


namespace IBatisNet.DataMapper.Configuration.Sql.SimpleDynamic
{
	/// <summary>
	/// Summary description for SimpleDynamicSql.
	/// </summary>
	internal class SimpleDynamicSql : ISql
	{

		#region private

		private const string ELEMENT_TOKEN = "$";

		private string _simpleSqlStatement = string.Empty;
		private IStatement _statement = null ;
		private TypeHandlerFactory _typeHandlerFactory = null;

		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sqlStatement">The sql statement.</param>
		/// <param name="statement"></param>
		/// <param name="typeHandlerFactory"></param>
		internal SimpleDynamicSql(TypeHandlerFactory typeHandlerFactory,string sqlStatement, IStatement statement)
		{
			_simpleSqlStatement = sqlStatement;
			_statement = statement;
			_typeHandlerFactory = typeHandlerFactory;
		}
		#endregion

		
		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		public string GetSql(object parameterObject)
		{
			return ProcessDynamicElements(parameterObject);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sqlStatement"></param>
		/// <returns></returns>
		public static bool IsSimpleDynamicSql(string sqlStatement) 
		{
			return ( (sqlStatement != null) && (sqlStatement.IndexOf(ELEMENT_TOKEN) > -1) );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		private string ProcessDynamicElements(object parameterObject) 
		{
			// define which character is seperating fields
			char[] splitter  = ELEMENT_TOKEN.ToCharArray();

			StringTokenizer parser = new StringTokenizer(_simpleSqlStatement, ELEMENT_TOKEN, true);

			StringBuilder newSql = new StringBuilder();

			string token = null;
			string lastToken = null;
			
			IEnumerator enumerator = parser.GetEnumerator();

			while (enumerator.MoveNext()) 
			{
				token = ((string)enumerator.Current);

				if (ELEMENT_TOKEN.Equals(lastToken)) 
				{
					if (ELEMENT_TOKEN.Equals(token)) 
					{
						newSql.Append(ELEMENT_TOKEN);
						token = null;
					} 
					else 
					{
						object value = null;
						if (parameterObject != null) 
						{
							if ( _typeHandlerFactory.IsSimpleType( parameterObject.GetType() ) == true) 
							{
								value = parameterObject;
							} 
							else 
							{
								value = ObjectProbe.GetPropertyValue(parameterObject, token);
							}
						}
						if (value != null) 
						{
							newSql.Append(value.ToString());
						}

						enumerator.MoveNext();
						token = ((string)enumerator.Current);

						if (!ELEMENT_TOKEN.Equals(token)) 
						{
							throw new DataMapperException("Unterminated dynamic element in sql (" + _simpleSqlStatement + ").");
						}
						token = null;
					}
				} 
				else 
				{
					if (!ELEMENT_TOKEN.Equals(token)) 
					{
						newSql.Append(token);
					}
				}

				lastToken = token;
			}

			return newSql.ToString();
		}


		#region ISql Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterObject"></param>
		/// <param name="session"></param>
		/// <returns></returns>
		public RequestScope GetRequestScope(object parameterObject, IDalSession session)
		{
			string sqlStatement = ProcessDynamicElements(parameterObject);
			
			RequestScope request = new RequestScope();

			request.ParameterMap = _statement.ParameterMap;
			request.ResultMap = _statement.ResultMap;
			request.PreparedStatement = BuildPreparedStatement(session, request, sqlStatement);
			
			return request;
		}

		/// <summary>
		/// Build the PreparedStatement
		/// </summary>
		/// <param name="session"></param>
		/// <param name="request"></param>
		/// <param name="sqlStatement"></param>
		private PreparedStatement BuildPreparedStatement(IDalSession session, RequestScope request, string sqlStatement)
		{
			PreparedStatementFactory factory = new PreparedStatementFactory( session, request, _statement, sqlStatement);
			return factory.Prepare();
		}
		#endregion

		#endregion

	}
}
