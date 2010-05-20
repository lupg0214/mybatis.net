
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
using System.IO;

using IBatisNet.Common.Exceptions;

namespace IBatisNet.Common.Utilities
{
	/// <summary>
	/// Description résumée de ScriptRunner.
	/// </summary>
	public class ScriptRunner
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ScriptRunner()
		{
		}

		/// <summary>
		/// Run an sql script
		/// </summary>
		/// <param name="dataSource">The dataSouce that will be used to run the script.</param>
		/// <param name="sqlScriptPath">a path to an sql script file.</param>
		public void RunScript(DataSource dataSource, string sqlScriptPath) {
			RunScript(dataSource, sqlScriptPath, true);
		}

		/// <summary>
		/// Run an sql script
		/// </summary>
		/// <param name="dataSource">The dataSouce that will be used to run the script.</param>
		/// <param name="sqlScriptPath">a path to an sql script file.</param>
		/// <param name="doParse">parse out the statements in the sql script file.</param>
		public void RunScript(DataSource dataSource, string sqlScriptPath, bool doParse)
		{
			// Get script file
			FileInfo fi = new FileInfo(sqlScriptPath);
			string script = fi.OpenText().ReadToEnd();
			
			ArrayList sqlStatements = new ArrayList();

			if (doParse) {
				switch(dataSource.Provider.Name) {         
					case "oracle9.2":   
					case "oracleClient1.0":   
					case "ByteFx":
						sqlStatements = ParseScript(script);
						break;                  
					default:            
						sqlStatements.Add(script);
						break;      
				}
			}
			else {
				sqlStatements.Add(script);
			}

			try {
				ExecuteStatements(dataSource, sqlStatements);
			}
			catch(System.Exception e) {
				throw new IBatisNetException("Unable to execute the sql: " + fi.Name, e);
			}
		}

		/// <summary>
		/// Execute the given sql statements
		/// </summary>
		/// <param name="dataSource">The dataSouce that will be used.</param>
		/// <param name="sqlStatements">An ArrayList of sql statements to execute.</param>
		private void ExecuteStatements(DataSource dataSource, ArrayList sqlStatements) {
			IDbConnection connection = dataSource.Provider.GetConnection();
			connection.ConnectionString = dataSource.ConnectionString;
			connection.Open();
			IDbTransaction transaction = connection.BeginTransaction();
			
			IDbCommand command = dataSource.Provider.GetCommand();
			command.Connection = connection;
			command.Transaction = transaction;			

			try {
				foreach (string sqlStatement in sqlStatements) {
					command.CommandText = sqlStatement;
					command.ExecuteNonQuery();
				}
				transaction.Commit();
			}
			catch(System.Exception e) {
				transaction.Rollback();
				throw (e);
			}
			finally {
				connection.Close();
			}
		}

		/// <summary>
		/// Parse and tokenize the sql script into multiple statements
		/// </summary>
		/// <param name="script">the script to parse</param>
		private ArrayList ParseScript(string script) {
			ArrayList statements = new ArrayList();
			StringTokenizer parser = new StringTokenizer(script, ";");
			IEnumerator enumerator = parser.GetEnumerator();

			while (enumerator.MoveNext()) {
				string statement= ((string)enumerator.Current).Replace("\r\n"," ");
				statement = statement.Trim();
				if (statement != string.Empty) {
					statements.Add(statement);
				}
			}

			return statements;
		}	
	}
}
