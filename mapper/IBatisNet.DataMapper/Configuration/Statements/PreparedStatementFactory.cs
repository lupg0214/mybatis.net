
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
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Text;
using IBatisNet.Common;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.Scope;
using log4net;

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

		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

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

			_preparedStatement.PreparedSql = _commandText;

			if (_statement.CommandType == CommandType.Text)
			{
				if (_request.ParameterMap != null) 
				{
					CreateParametersForTextCommand();
					EvaluateParameterMap();
				}
			}
			else if (_statement.CommandType == CommandType.StoredProcedure) // StoredProcedure
			{
				if (_request.ParameterMap == null) // No parameterMap --> error
				{
					throw new DataMapperException("A procedure statement tag must have a parameterMap attribute, which is not the case for the procedure '"+_statement.Id+"."); 
				}
				else // use the parameterMap
				{
					if (_session.DataSource.Provider.UseDeriveParameters)
					{
						DiscoverParameter(_session);
					}
					else
					{
						CreateParametersForProcedureCommand();
						// EvaluateParameterMap(); // Did we need that ? I don't think for the procedure
					}
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
		/// Create IDataParameters for command text statement.
		/// </summary>
		private void CreateParametersForTextCommand()
		{
			string sqlParamName = string.Empty;
			string dbTypePropertyName = _session.DataSource.Provider.ParameterDbTypeProperty;
			Type enumDbType = _session.DataSource.Provider.ParameterDbType;
			IList list = null;
			int i = 0;

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
				if (_session.DataSource.Provider.UseParameterPrefixInParameter)
				{
					// From Ryan Yao: JIRA-27, used "param" + i++ for sqlParamName
					sqlParamName = _parameterPrefix + "param" + i++;
				}
				else
				{
					sqlParamName = "param" + i++;
				}

				IDataParameter dataParameter = _session.CreateCommand(_statement.CommandType).CreateParameter();

				// Manage dbType attribute if any
				if (property.DbType != null && property.DbType.Length >0) 
				{
					// Exemple : Enum.parse(System.Data.SqlDbType, 'VarChar')
					object dbType = Enum.Parse( enumDbType, property.DbType, true );

					// Exemple : ObjectHelper.SetProperty(sqlparameter, 'SqlDbType', SqlDbType.Int);
					ObjectProbe.SetPropertyValue(dataParameter, dbTypePropertyName, dbType);
				}

				// Set IDbDataParameter
				// JIRA-49 Fixes (size, precision, and scale)
				if (_session.DataSource.Provider.SetDbParameterSize) 
				{
					if (property.Size != -1)
					{
						((IDbDataParameter)dataParameter).Size = property.Size;
					}
				}

				if (_session.DataSource.Provider.SetDbParameterPrecision) 
				{
					((IDbDataParameter)dataParameter).Precision = property.Precision;
				}
				
				if (_session.DataSource.Provider.SetDbParameterScale) 
				{
					((IDbDataParameter)dataParameter).Scale = property.Scale;
				}
				
				// Set as direction parameter
				dataParameter.Direction = property.Direction;

				dataParameter.ParameterName = sqlParamName;

				_preparedStatement.DbParametersName.Add( property.PropertyName );
				_preparedStatement.DbParameters.Add( dataParameter );	

				if ( _session.DataSource.Provider.UsePositionalParameters == false)
				{
					_propertyDbParameterMap.Add(property, dataParameter);
				}
			}
		}


		/// <summary>
		/// Create IDataParameters for procedure statement.
		/// </summary>
		private void CreateParametersForProcedureCommand()
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

			// ParemeterMap are required for procedure and we tested existance in Prepare() method
			// so we don't have to test existence here.
			// A ParameterMap used in CreateParametersForProcedureText must
			// have property and column attributes set.
			// The column attribute is the name of a procedure parameter.
			foreach(ParameterProperty property in list)
			{
				if (_session.DataSource.Provider.UseParameterPrefixInParameter)
				{
					sqlParamName = _parameterPrefix + property.ColumnName;
				}
				else //obdc/oledb
				{
					sqlParamName =  property.ColumnName;
				}

				IDataParameter dataParameter = _session.CreateCommand(_statement.CommandType).CreateParameter();

				// Manage dbType attribute if any
				if (property.DbType!=null && property.DbType.Length >0) 
				{
					// Exemple : Enum.parse(System.Data.SqlDbType, 'VarChar')
					object dbType = Enum.Parse( enumDbType, property.DbType, true );

					// Exemple : ObjectHelper.SetProperty(sqlparameter, 'SqlDbType', SqlDbType.Int);
					ObjectProbe.SetPropertyValue(dataParameter, dbTypePropertyName, dbType);
				}

				// Set IDbDataParameter
				// JIRA-49 Fixes (size, precision, and scale)
				if (_session.DataSource.Provider.SetDbParameterSize) 
				{
					if (property.Size != -1)
					{
						((IDbDataParameter)dataParameter).Size = property.Size;
					}
				}

				if (_session.DataSource.Provider.SetDbParameterPrecision) 
				{
					((IDbDataParameter)dataParameter).Precision = property.Precision;
				}
				
				if (_session.DataSource.Provider.SetDbParameterScale) 
				{
					((IDbDataParameter)dataParameter).Scale = property.Scale;
				}
				
				// Set as direction parameter
				dataParameter.Direction = property.Direction;

				dataParameter.ParameterName = sqlParamName;

				_preparedStatement.DbParametersName.Add( property.PropertyName );
				_preparedStatement.DbParameters.Add( dataParameter );	

				if ( _session.DataSource.Provider.UsePositionalParameters == false)
				{
					_propertyDbParameterMap.Add(property, dataParameter);
				}
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
						// TODO Refactor?
						if (_parameterPrefix.Equals(":"))
						{
							// ODP.NET uses positional parameters by default
							// but uses ":0" or ":1" instead of "?"
							sqlParamName = ":" + index;	
						}
						else 
						{
							// OLEDB/OBDC doesn't support named parameters !!!
							sqlParamName = "?";
						}
						
					}
					else
					{
						dataParameter = (IDataParameter) _propertyDbParameterMap[property];
						
						// 5 May 2004
						// Need to check UseParameterPrefixInParameter here 
						// since CreateParametersForStatementText now does
						// a check for UseParameterPrefixInParameter before 
						// creating the parameter name!
						if (_session.DataSource.Provider.UseParameterPrefixInParameter) 
						{
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
						else
						{
							sqlParamName = _parameterPrefix+dataParameter.ParameterName;
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
