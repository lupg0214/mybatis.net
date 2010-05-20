
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

using IBatisNet.DataMapper.Exceptions;
using IBatisNet.Common;
using IBatisNet.DataMapper.Configuration.Statements;

namespace IBatisNet.DataMapper.MappedStatements
{
	/// <summary>
	/// Summary description for InsertMappedStatement.
	/// </summary>
	public class InsertMappedStatement : MappedStatement
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sqlMap">An SqlMap</param>
		/// <param name="statement">An SQL statement</param>
		internal InsertMappedStatement( SqlMapper sqlMap, IStatement statement ): base(sqlMap, statement)
		{
		}

		#region ExecuteQueryForMap

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <param name="keyProperty"></param>
		/// <param name="valueProperty"></param>
		/// <returns></returns>
		public override IDictionary ExecuteQueryForMap( IDalSession session, object parameterObject, string keyProperty, string valueProperty )
		{
			throw new DataMapperException("Insert statements cannot be executed as a query for map.");
		}

		#endregion

		#region ExecuteUpdate

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		public override int ExecuteUpdate(IDalSession session, object parameterObject )
		{
			throw new DataMapperException("Insert statements cannot be executed as a update query.");
		}

		#endregion

		#region ExecuteQueryForList

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <param name="resultObject"></param>
		public override void ExecuteQueryForList(IDalSession session, object parameterObject, IList resultObject )
		{
			throw new DataMapperException("Insert statements cannot be executed as a query for list.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <param name="skipResults"></param>
		/// <param name="maxResults"></param>
		/// <returns></returns>
		public override IList ExecuteQueryForList( IDalSession session, object parameterObject, int skipResults, int maxResults )
		{
			throw new DataMapperException("Insert statements cannot be executed as a query for list.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		public override IList ExecuteQueryForList( IDalSession session, object parameterObject )
		{
			throw new DataMapperException("Insert statements cannot be executed as a query for list.");
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <param name="rowDelegate"></param>
		/// <returns></returns>
		public override IList ExecuteQueryForRowDelegate( IDalSession session, object parameterObject, SqlMapper.RowDelegate rowDelegate )
		{
			throw new DataMapperException("Insert statements cannot be executed as a query for row delegate.");
		}

		#region ExecuteForObject

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		public override object ExecuteQueryForObject( IDalSession session, object parameterObject )
		{
			throw new DataMapperException("Insert statements cannot be executed as a query for object.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <param name="resultObject"></param>
		/// <returns></returns>
		public override object ExecuteQueryForObject( IDalSession session, object parameterObject, object resultObject )
		{
			throw new DataMapperException("Insert statements cannot be executed as a query for object.");
		}

		#endregion
	}
}
