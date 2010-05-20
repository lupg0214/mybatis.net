
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
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Text;

using log4net;

using IBatisNet.Common;
using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.Scope;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Utilities.Objects;
#endregion

namespace IBatisNet.DataMapper.Configuration.Statements
{
	/// <summary>
	/// Summary description for PreparedStatementFactory.
	/// </summary>
	public class PreparedStatementFactory
	{

		#region Fields

		private PreparedStatement _preparedStatement = null;

		private string _parameterPrefix = string.Empty;
		private IStatement _statement = null;
		private IDalSession _session = null;
		private string _commandText = string.Empty;
		private RequestScope _request = null;
		// (property, DbParameter)
		private HybridDictionary _propertyDbParameterMap = new HybridDictionary();
		private ArrayList _mapping = new ArrayList();

		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="session"></param>
		/// <param name="statement"></param>
		/// <param name="commandText"></param>
		/// <param name="request"></param>
		public PreparedStatementFactory(IDalSession session, RequestScope request, IStatement statement, string commandText)
		{
			_session = session;
			_request = request;
			_statement = statement;
			_commandText = commandText;
		}


		/// <summary>
		/// Create a list of IDataParameter for the statement and build the sql string.
		/// </summary>
		public PreparedStatement Prepare()
		{
			_preparedStatement = new PreparedStatement();
			_parameterPrefix = _session.DataSource.Provider.ParameterPrefix;

			if (_statement.CommandType == CommandType.Text)
			{
				if (_request.ParameterMap != null) // Use a ParameterMap
				{
					CreateParametersForStatementText();
					EvaluateParameterMap();
				}
				else // InLine parameter
				{
					Regex regEx = new Regex( "#(?<parameterName>\\w.*?)#", RegexOptions.Multiline );
				
					// Create a parameterMap
					ParameterMap parameterMap = new ParameterMap();
					parameterMap.Id =  "InLineParameterMap-"+_statement.Id +System.DateTime.Now.Ticks.ToString();
					_request.ParameterMap = parameterMap;

                    
					_preparedStatement.PreparedSql = regEx.Replace( _commandText, new MatchEvaluator( this.EvaluatorInlineParameter ) );
				}
			}
			else if (_statement.CommandType == CommandType.StoredProcedure) // StoredProcedure
			{
				_preparedStatement.PreparedSql = _commandText;

				if (_request.ParameterMap == null) // No parameterMap --> error
				{
					throw new DataMapperException("A procedure statement tag must have a parameterMap attribut, which is not the case for the procedure '"+_statement.Id+"."); 
				}
				else // use the parameterMap
				{
					DiscoverParameter(_session);
				}

				#region Fix for Odbc
				// Although executing a parameterized stored procedure using the ODBC .NET Provider 
				// is slightly different from executing the same procedure using the SQL or 
				// the OLE DB Provider, there is one important difference 
				// -- the stored procedure must be called using the ODBC CALL syntax rather than 
				// the name of the stored procedure. 
				// For additional information on this CALL syntax, 
				// see the page entitled "Procedure Calls" in the ODBC Programmer's Reference 
				// in the MSDN Library. 
				//http://support.microsoft.com/default.aspx?scid=kb;EN-US;Q309486

				if ( _session.DataSource.Provider.IsObdc == true )
				{
					StringBuilder commandTextBuilder = new StringBuilder("{ call ");
					commandTextBuilder.Append( _commandText );

					if (_preparedStatement.DbParameters.Count >0)
					{
						commandTextBuilder.Append(" (");
						int supIndex = _preparedStatement.DbParameters.Count-1;
						for(int i=0;i<supIndex;i++)
						{
							commandTextBuilder.Append("?,");
						}
						commandTextBuilder.Append("?) }");
					}
					_preparedStatement.PreparedSql = commandTextBuilder.ToString();
				}

				#endregion
			}

			if (_logger.IsDebugEnabled) 
			{
				_logger.Debug("SQL for statement '"+ _statement.Id +"' :" + _preparedStatement.PreparedSql);
			}

			return _preparedStatement;
		}


		#region Private methods

		/// <summary>
		/// For store procedure, auto discover IDataParameters for stored procedures at run-time.
		/// </summary>
		/// <param name="session">The current session.</param>
		private void DiscoverParameter(IDalSession session)
		{
			// pull the parameters for this stored procedure from the parameter cache 
			// (or discover them & populate the cache)
			IDataParameter[] commandParameters = DBHelperParameterCache.GetSpParameterSet(session, _commandText);
			
			int start = session.DataSource.Provider.ParameterPrefix.Length;
			foreach(IDataParameter dataParameter in commandParameters)
			{
				if (session.DataSource.Provider.UseParameterPrefixInParameter == false)
				{
					if (dataParameter.ParameterName.StartsWith(session.DataSource.Provider.ParameterPrefix)) {
						dataParameter.ParameterName = dataParameter.ParameterName.Substring(start);
					}
				}
				_preparedStatement.DbParametersName.Add( dataParameter.ParameterName );
				_preparedStatement.DbParameters.Add( dataParameter );
			}
		}


		/// <summary>
		/// Create IDataParameters for statement Text.
		/// </summary>
		private void CreateParametersForStatementText()
		{
			string sqlParamName = string.Empty;
			string dbTypePropertyName = _session.DataSource.Provider.ParameterDbTypeProperty;
			Type enumDbType = _session.DataSource.Provider.ParameterDbType;
			IList list = null;

			if (_session.DataSource.Provider.UsePositionalParameters) //obdc/oledb
			{
				list = _request.ParameterMap.Properties;
			}
			else 
			{
				list = _request.ParameterMap.PropertiesList;
			}

			foreach(ParameterProperty property in list)
			{
				StringBuilder stringBuilder = new StringBuilder(property.PropertyName);
				stringBuilder = stringBuilder.Replace('.', '_').Replace('[','P').Replace(']','D');
				string paramName =  stringBuilder.ToString();

				//				if (_session.DataSource.Provider.UseParameterPrefixInParameter )
				//				{
				sqlParamName = _parameterPrefix + paramName;
				//				}
				//				else //obdc/oledb
				//				{
				//					sqlParamName = paramName;
				//				}

				IDataParameter dataParameter = _session.CreateCommand(_statement.CommandType).CreateParameter();

				// Manage dbType attribut if any
				if (property.DbType.Length >0) 
				{
					// Exemple : Enum.parse(System.Data.SqlDbType, 'VarChar')
					object dbType = Enum.Parse( enumDbType, property.DbType, true );

					// Exemple : ObjectHelper.SetProperty(sqlparameter, 'SqlDbType', SqlDbType.Int);
					ObjectProbe.SetPropertyValue(dataParameter, dbTypePropertyName, dbType);
				}

				// Set IDbDataParameter
				if (property.Size != -1)
				{
					((IDbDataParameter)dataParameter).Size = property.Size;
				}
				((IDbDataParameter)dataParameter).Precision = property.Precision;
				((IDbDataParameter)dataParameter).Scale = property.Scale;

				// Set as direction parameter
				dataParameter.Direction = property.Direction;

				dataParameter.ParameterName = sqlParamName;

				_preparedStatement.DbParametersName.Add( paramName );
				_preparedStatement.DbParameters.Add( dataParameter );	

				if ( _session.DataSource.Provider.UsePositionalParameters == false)
				{
					_propertyDbParameterMap.Add(property, dataParameter);
				}
				
			}
		}


		/// <summary>
		/// Retrieve IDataParameter name and create a IDataParameter template
		/// from inline parameter.
		/// </summary>
		/// <param name="match">A regular expression match which give 
		/// the name of the IDataParameter.
		///  </param>
		/// <returns>The name of the IDataParameter.</returns>
		private string EvaluatorInlineParameter( Match match )
		{
			string paramName = match.Groups["parameterName"].Value;
			string sqlParamName = string.Empty;
			ParameterProperty property = new ParameterProperty();
			
			StringBuilder stringBuilder = new StringBuilder(paramName);
			stringBuilder = stringBuilder.Replace(".", "_");
			paramName = stringBuilder.ToString();

			if (_session.DataSource.Provider.UseParameterPrefixInParameter )
			{
				sqlParamName = _parameterPrefix + paramName;
			}
			else
			{
				sqlParamName = paramName;
			}

			IDataParameter dataParameter = _session.CreateCommand(_statement.CommandType).CreateParameter();
				
			if ( sqlParamName != "?" )
			{
				dataParameter.ParameterName = sqlParamName;
			}	
			_preparedStatement.DbParametersName.Add( paramName );
			_preparedStatement.DbParameters.Add( dataParameter );

			// ---- Create a ParameterProperty and add it to the parameterMap
			property.PropertyName = match.Groups["parameterName"].Value;
			_request.ParameterMap.AddParameterProperty(property);

			if (_session.DataSource.Provider.UsePositionalParameters)
			{
				// OLEDB/OBDC doesn't support named parameters !!!
				return "?";
			}
			else
			{
				return sqlParamName;
			}
		}


		/// <summary>
		/// Parse sql command text.
		/// </summary>
		private void EvaluateParameterMap()
		{
			string delimiter = "?";
			string token = null;
			int index = 0;
			string sqlParamName = string.Empty;			
			StringTokenizer parser = new StringTokenizer(_commandText, delimiter, true);
			StringBuilder newCommandTextBuffer = new StringBuilder();

			IEnumerator enumerator = parser.GetEnumerator();

			while (enumerator.MoveNext()) 
			{
				token = (string)enumerator.Current;

				if (delimiter.Equals(token)) // ?
				{
					ParameterProperty property = (ParameterProperty)_request.ParameterMap.Properties[index];
					IDataParameter dataParameter = null;
					
					if (_session.DataSource.Provider.UsePositionalParameters)
					{
						// OLEDB/OBDC doesn't support named parameters !!!
						sqlParamName = "?";
					}
					else
					{
						dataParameter = (IDataParameter) _propertyDbParameterMap[property];
						
						// Fix ByteFX.Data.MySqlClient.MySqlParameter
						// who strip prefix in Parameter Name ?!
						if (_session.DataSource.Provider.Name.IndexOf("ByteFx")>=0)
						{
							sqlParamName = _parameterPrefix+dataParameter.ParameterName;
						}
						else
						{
							sqlParamName = dataParameter.ParameterName;
						}
					}			
		
					newCommandTextBuffer.Append(" ");
					newCommandTextBuffer.Append(sqlParamName);

					sqlParamName = string.Empty;
					index ++;
				}
				else
				{
					newCommandTextBuffer.Append(token);
				}
			}

			_preparedStatement.PreparedSql = newCommandTextBuffer.ToString();
		}

		#endregion
	}
}
