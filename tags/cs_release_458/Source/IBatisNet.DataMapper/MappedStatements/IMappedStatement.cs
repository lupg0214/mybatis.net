
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

using IBatisNet.Common;
using IBatisNet.DataMapper.Configuration.Statements;
#endregion

namespace IBatisNet.DataMapper.MappedStatements
{
	/// <summary>
	/// 
	/// </summary>
	public delegate void ExecuteEventHandler(object sender, ExecuteEventArgs e);

	/// <summary>
	/// Summary description for IMappedStatement.
	/// </summary>
	public interface IMappedStatement
	{
		#region Properties
		/// <summary>
		/// Name used to identify the MappedStatement amongst the others.
		/// This the name of the SQL statment by default.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// The SQL statment used by this MappedStatement
		/// </summary>
		IStatement Statement
		{
			get;
		}

		
		/// <summary>
		/// The SqlMap used by this MappedStatement
		/// </summary>
		SqlMapper SqlMap
		{
			get;
		}
		#endregion

		#region Methods
		#region ExecuteQueryForMap

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <param name="keyProperty"></param>
		/// <param name="valueProperty"></param>
		/// <returns></returns>
		IDictionary ExecuteQueryForMap( IDalSession session, object parameterObject, string keyProperty, string valueProperty );

		#endregion

		#region ExecuteUpdate

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		int ExecuteUpdate(IDalSession session, object parameterObject );

		#endregion

		#region ExecuteInsert

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		object ExecuteInsert(IDalSession session, object parameterObject );

		#endregion

		#region ExecuteQueryForList

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <param name="resultObject"></param>
		void ExecuteQueryForList(IDalSession session, object parameterObject, IList resultObject );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <param name="skipResults"></param>
		/// <param name="maxResults"></param>
		/// <returns></returns>
		IList ExecuteQueryForList( IDalSession session, object parameterObject, int skipResults, int maxResults );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		IList ExecuteQueryForList( IDalSession session, object parameterObject );

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <param name="rowDelegate"></param>
		/// <returns></returns>
		IList ExecuteQueryForRowDelegate( IDalSession session, object parameterObject, SqlMapper.RowDelegate rowDelegate );

		#region ExecuteForObject

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		object ExecuteQueryForObject( IDalSession session, object parameterObject );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <param name="resultObject"></param>
		/// <returns></returns>
		object ExecuteQueryForObject( IDalSession session, object parameterObject, object resultObject );

		#endregion
		#endregion

	}
}
