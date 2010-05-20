
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
using System.Text;
using System.Reflection;

using IBatisNet.Common;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Utilities.Objects;

using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Commands;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.Sql.SimpleDynamic;
using IBatisNet.DataMapper.Configuration.Sql.Static;
using IBatisNet.DataMapper.TypeHandlers;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.Scope;

using log4net;
#endregion

namespace IBatisNet.DataMapper.MappedStatements
{

	/// <summary>
	/// Summary description for MappedStatement.
	/// </summary>
	public class MappedStatement : IMappedStatement
	{
		/// <summary>
		/// 
		/// </summary>
		public event ExecuteEventHandler Execute;

		/// <summary>
		/// Enumeration of the ExecuteQuery method.
		/// </summary>
		private enum ExecuteMethod : int
		{
			ExecuteQueryForObject =0,
			ExecuteQueryForIList,
			ExecuteQueryForArrayList,
			ExecuteQueryForStrongTypedIList
		}


		/// <summary>
		/// All data tor retrieve 'select' result property
		/// </summary>
		/// <remarks>
		/// As ADO.NET allows to open DataReader per connection at once, we keep
		/// all th data to make the open the 'whish' DataReader after having closed the current. 
		/// </remarks>
		private class PostBindind
		{
			#region Fields
			private MappedStatement _statement = null;
			private ResultProperty _property = null;
			private object _target = null;
			private object _keys = null;
			private ExecuteMethod _method = ExecuteMethod.ExecuteQueryForIList;
			#endregion

			#region Properties
			/// <summary>
			/// 
			/// </summary>
			public MappedStatement Statement
			{
				set { _statement = value; }
				get { return _statement; }
			}

			/// <summary>
			/// 
			/// </summary>
			public ResultProperty ResultProperty
			{
				set { _property = value; }
				get { return _property; }
			}

			/// <summary>
			/// 
			/// </summary>
			public object Target
			{
				set { _target = value; }
				get { return _target; }
			}


			/// <summary>
			/// 
			/// </summary>
			public object Keys
			{
				set { _keys = value; }
				get { return _keys; }
			}

			/// <summary>
			/// 
			/// </summary>
			public ExecuteMethod Method
			{
				set { _method = value; }
				get { return _method; }
			}
			#endregion

		}


		#region Fields 

		// Magic number used to set the the maximum number of rows returned to 'all'. 
		private const int NO_MAXIMUM_RESULTS = -1;
		// Magic number used to set the the number of rows skipped to 'none'. 
		private const int NO_SKIPPED_RESULTS = -1;

		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private IStatement _statement = null;

		private SqlMapper _sqlMap = null;

		private IPreparedCommand _preparedCommand = null;

		#endregion

		#region Properties

		/// <summary>
		/// The IPreparedCommand to use
		/// </summary>
		public IPreparedCommand PreparedCommand
		{
			get { return _preparedCommand; }
		}

		/// <summary>
		/// Name used to identify the MappedStatement amongst the others.
		/// This the name of the SQL statement by default.
		/// </summary>
		public string Name
		{
			get { return _statement.Id; }
		}

		/// <summary>
		/// The SQL statment used by this MappedStatement
		/// </summary>
		public IStatement Statement
		{
			get { return _statement; }
		}

		/// <summary>
		/// The SqlMap used by this MappedStatement
		/// </summary>
		public SqlMapper SqlMap
		{
			get { return _sqlMap; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sqlMap">An SqlMap</param>
		/// <param name="statement">An SQL statement</param>
		internal MappedStatement( SqlMapper sqlMap, IStatement statement )
		{
			_sqlMap = sqlMap;
			_statement = statement;
			_preparedCommand = PreparedCommandFactory.GetPreparedCommand(sqlMap.UseEmbedStatementParams);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets a percentage of successful cache hits achieved
		/// </summary>
		/// <returns>The percentage of hits (0-1), or -1 if cache is disabled.</returns>
		public double GetDataCacheHitRatio() 
		{
			if (_statement.CacheModel != null) 
			{
				return _statement.CacheModel.HitRatio;
			} 
			else 
			{
				return -1;
			}
		}
		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="reader"></param>
		/// <param name="resultMap"></param>
		/// <param name="resultObject"></param>
		private void FillObjectWithReaderAndResultMap(RequestScope request,IDataReader reader, ResultMap resultMap, object resultObject)
		{
			// For each Property in the ResultMap, set the property in the object 
			foreach(DictionaryEntry entry in resultMap.ColumnsToPropertiesMap)
			{
				ResultProperty property = (ResultProperty)entry.Value;
				SetObjectProperty(request, resultMap, property, ref resultObject, reader);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="reader"></param>
		/// <param name="resultObject"></param>
		/// <returns></returns>
		private object ApplyResultMap(RequestScope request, IDataReader reader, object resultObject)
		{
			object outObject = resultObject; 

			// If there's an ResultMap, use it
			if (request.ResultMap != null) 
			{
				ResultMap resultMap = request.GetResultMap(reader);

				if (outObject == null) 
				{
					outObject = resultMap.CreateInstanceOfResult();
				}

				// For each Property in the ResultMap, set the property in the object 
				foreach(DictionaryEntry entry in resultMap.ColumnsToPropertiesMap)
				{
					ResultProperty property = (ResultProperty)entry.Value;
					SetObjectProperty(request, resultMap, property, ref outObject, reader);
				}
			} 
			else // else try to use a ResultClass
			{
				if (_statement.ResultClass != null) 
				{
					if (outObject == null) 
					{
						outObject = _statement.CreateInstanceOfResultClass();
					}

					// Check if the ResultClass is a 'primitive' Type
					if (_sqlMap.TypeHandlerFactory.IsSimpleType(_statement.ResultClass))
					{
						// Create a ResultMap
						ResultMap resultMap = new ResultMap();

						// Create a ResultProperty
						ResultProperty property = new ResultProperty();
						property.PropertyName = "value";
						property.ColumnIndex = 0;
						property.TypeHandler = _sqlMap.TypeHandlerFactory.GetTypeHandler(outObject.GetType());

						resultMap.AddResultPropery(property);

						SetObjectProperty(request, request.ResultMap, property, ref outObject, reader);
					}
					else if (outObject is Hashtable) 
					{
						for (int i = 0; i < reader.FieldCount; i++) 
						{
							string columnName = reader.GetName(i);
							((Hashtable) outObject).Add(columnName, reader.GetValue(i));
						}
					}
					else
					{
						AutoMapReader( reader, ref outObject);
					}
				}
			}

			return outObject;
		}		

		
		/// <summary>
		/// Retrieve the output parameter and map them on the result object.
		/// This routine is only use is you specified a ParameterMap and some output attribute
		/// or if you use a store procedure with output parameter...
		/// </summary>
		/// <param name="request"></param>
		/// <param name="session">The current session.</param>
		/// <param name="result">The result object.</param>
		/// <param name="command">The command sql.</param>
		private void RetrieveOutputParameters(RequestScope request, IDalSession session, IDbCommand command, object result)
		{
			if (request.ParameterMap != null)
			{
				for(int i=0; i<request.ParameterMap.PropertiesList.Count; i++)
				{
					ParameterProperty  mapping = request.ParameterMap.GetProperty(i);
					if (mapping.Direction == ParameterDirection.Output || 
						mapping.Direction == ParameterDirection.InputOutput) 
					{
						string parameterName = string.Empty;
						if (session.DataSource.Provider.UseParameterPrefixInParameter == false)
						{
							parameterName =  mapping.ColumnName;
						}
						else
						{
							parameterName = session.DataSource.Provider.ParameterPrefix + 
								mapping.ColumnName;
						}
						
						if (mapping.TypeHandler == null) // Find the TypeHandler
						{
							lock(mapping) 
							{
								if (mapping.TypeHandler == null)
								{
									Type propertyType =ObjectProbe.GetPropertyTypeForGetter(result,mapping.PropertyName);

									mapping.TypeHandler = _sqlMap.TypeHandlerFactory.GetTypeHandler(propertyType);
								}
							}					
						}
						
						object dataBaseValue = mapping.TypeHandler.GetDataBaseValue( ((IDataParameter)command.Parameters[parameterName]).Value, result.GetType() );

						ObjectProbe.SetPropertyValue(result, mapping.PropertyName, dataBaseValue);
					}
				}
			}
		}

		
		#region ExecuteForObject

		/// <summary>
		/// Executes an SQL statement that returns a single row as an Object.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <returns>The object</returns>
		public virtual object ExecuteQueryForObject( IDalSession session, object parameterObject )
		{
			return ExecuteQueryForObject(session, parameterObject, null);
		}


		/// <summary>
		/// Executes an SQL statement that returns a single row as an Object of the type of
		/// the resultObject passed in as a parameter.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="resultObject">The result object.</param>
		/// <returns>The object</returns>
		public virtual object ExecuteQueryForObject(IDalSession session, object parameterObject, object resultObject )
		{
			object obj = null;
			RequestScope request = _statement.Sql.GetRequestScope(parameterObject, session);;

			if (_statement.CacheModel == null) 
			{
				obj = RunQueryForObject(request, session, parameterObject, resultObject);
			}
			else
			{
				CacheKey key = null;
				if (_statement.ParameterMap != null) 
				{
					key = new CacheKey(_sqlMap.TypeHandlerFactory, this.Name, 
						request.PreparedStatement.PreparedSql,
						parameterObject, 
						request.ParameterMap.GetPropertyNameArray(), 
						NO_SKIPPED_RESULTS, 
						NO_MAXIMUM_RESULTS, 
						CacheKeyType.Object);
				} 
				else 
				{
					key = new CacheKey(_sqlMap.TypeHandlerFactory, this.Name, 
						request.PreparedStatement.PreparedSql,
						parameterObject, 
						new string[0], 
						NO_SKIPPED_RESULTS, 
						NO_MAXIMUM_RESULTS, 
						CacheKeyType.Object);
				}

				obj = _statement.CacheModel[key];
				if (obj == null) 
				{
					obj = RunQueryForObject(request, session, parameterObject, resultObject);
					_statement.CacheModel[key] = obj;
				}
			}

			return obj;
		}

		
		/// <summary>
		/// Executes an SQL statement that returns a single row as an Object of the type of
		/// the resultObject passed in as a parameter.
		/// </summary>
		/// <param name="request">The request scope.</param>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="resultObject">The result object.</param>
		/// <returns>The object</returns>
		private object RunQueryForObject(RequestScope request, IDalSession session, object parameterObject, object resultObject )
		{
			object result = resultObject;
			
			//using ( IDbCommand command = CreatePreparedCommand(request, session, parameterObject ))
			using ( IDbCommand command = _preparedCommand.Create( request, session, this.Statement, parameterObject ) )
			{
				using ( IDataReader reader = command.ExecuteReader() )
				{				
					if ( reader.Read() )
					{
						result = ApplyResultMap(request, reader, resultObject);		
					}
				}

				ExecutePostSelect( session, request);

				#region remark
				// If you are using the OleDb data provider (as you are), you need to close the
				// DataReader before output parameters are visible.
				#endregion

				RetrieveOutputParameters(request, session, command, parameterObject);
			}

			return result;
		}

		#endregion

		#region ExecuteQueryForList

		/// <summary>
		/// Runs a query with a custom object that gets a chance 
		/// to deal with each row as it is processed.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="rowDelegate"></param>
		public virtual IList ExecuteQueryForRowDelegate( IDalSession session, object parameterObject, SqlMapper.RowDelegate rowDelegate )
		{
			RequestScope request = _statement.Sql.GetRequestScope(parameterObject, session);;

			if (rowDelegate == null) 
			{
				throw new DataMapperException("A null RowDelegate was passed to QueryForRowDelegate.");
			}
			
			return RunQueryForList(request, session, parameterObject, NO_SKIPPED_RESULTS, NO_MAXIMUM_RESULTS, rowDelegate);
		}

		
		/// <summary>
		/// Executes the SQL and retuns all rows selected. This is exactly the same as
		/// calling ExecuteQueryForList(session, parameterObject, NO_SKIPPED_RESULTS, NO_MAXIMUM_RESULTS).
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <returns>A List of result objects.</returns>
		public virtual IList ExecuteQueryForList( IDalSession session, object parameterObject )
		{
			return ExecuteQueryForList( session, parameterObject, NO_SKIPPED_RESULTS, NO_MAXIMUM_RESULTS);
		}


		/// <summary>
		/// Executes the SQL and retuns a subset of the rows selected.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="skipResults">The number of rows to skip over.</param>
		/// <param name="maxResults">The maximum number of rows to return.</param>
		/// <returns>A List of result objects.</returns>
		public virtual IList ExecuteQueryForList( IDalSession session, object parameterObject, int skipResults, int maxResults )
		{
			IList list = null;
			RequestScope request = _statement.Sql.GetRequestScope(parameterObject, session);;

			if (_statement.CacheModel == null) 
			{
				list = RunQueryForList(request, session, parameterObject, skipResults, maxResults, null);
			}
			else
			{
				CacheKey key = null;
				if (_statement.ParameterMap != null) 
				{
					key = new CacheKey(_sqlMap.TypeHandlerFactory, this.Name, 
						request.PreparedStatement.PreparedSql, 
						parameterObject, 
						request.ParameterMap.GetPropertyNameArray(), 
						skipResults, 
						maxResults, 
						CacheKeyType.List);
				} 
				else 
				{
					key = new CacheKey(_sqlMap.TypeHandlerFactory, this.Name, 
						request.PreparedStatement.PreparedSql,  
						parameterObject, 
						new string[0], 
						skipResults, 
						maxResults, 
						CacheKeyType.List);
				}

				list = (IList)_statement.CacheModel[key];
				if (list == null) 
				{
					list = RunQueryForList(request, session, parameterObject, skipResults, maxResults, null);
					_statement.CacheModel[key] = list;
				}
			}

			return list;
		}

		
		/// <summary>
		/// Executes the SQL and retuns a List of result objects.
		/// </summary>
		/// <param name="request">The request scope.</param>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="skipResults">The number of rows to skip over.</param>
		/// <param name="maxResults">The maximum number of rows to return.</param>
		/// <param name="rowDelegate"></param>
		/// <returns>A List of result objects.</returns>
		private IList RunQueryForList(RequestScope request, IDalSession session, object parameterObject, int skipResults, int maxResults,  SqlMapper.RowDelegate rowDelegate)
		{
			IList list = null;
			
			//using ( IDbCommand command = CreatePreparedCommand(request, session, parameterObject ))
			using ( IDbCommand command = _preparedCommand.Create( request, session, this.Statement, parameterObject ) )
			{
				if (_statement.ListClass == null)
				{
					list = new ArrayList();
				}
				else
				{
					list = _statement.CreateInstanceOfListClass();
				}

				using ( IDataReader reader = command.ExecuteReader() )
				{			
					// skip results
					for (int i = 0; i < skipResults; i++) 
					{
						if (!reader.Read()) 
						{
							break;
						}
					}

					int n = 0;

					if (rowDelegate == null) 
					{
						while ( (maxResults == NO_MAXIMUM_RESULTS || n < maxResults) 
							&& reader.Read() )
						{
							object obj = ApplyResultMap(request, reader, null);
						
							list.Add( obj );
							n++;
						}
					}
					else
					{
						while ( (maxResults == NO_MAXIMUM_RESULTS || n < maxResults) 
							&& reader.Read() )
						{
							object obj = ApplyResultMap(request, reader, null);

							rowDelegate(obj, list);
							n++;
						}
					}
				}

				ExecutePostSelect( session, request);

				RetrieveOutputParameters(request, session, command, parameterObject);
			}

			return list;
		}
		
		
		/// <summary>
		/// Executes the SQL and and fill a strongly typed collection.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="resultObject">A strongly typed collection of result objects.</param>
		public virtual void ExecuteQueryForList(IDalSession session, object parameterObject, IList resultObject )
		{
			RequestScope request = _statement.Sql.GetRequestScope(parameterObject, session);;

			//using ( IDbCommand command = CreatePreparedCommand(request, session, parameterObject ) )
			using ( IDbCommand command = _preparedCommand.Create( request, session, this.Statement, parameterObject ) )
			{
				using ( IDataReader reader = command.ExecuteReader() )
				{			
					while ( reader.Read() )
					{
						object obj = ApplyResultMap(request, reader, null);
				
						resultObject.Add( obj );
					}
				}

				ExecutePostSelect( session, request);

				RetrieveOutputParameters(request, session, command, parameterObject);
			}
		}

		
		#endregion

		#region ExecuteUpdate, ExecuteInsert

		/// <summary>
		/// Execute an update statement. Also used for delete statement.
		/// Return the number of row effected.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <returns>The number of row effected.</returns>
		public virtual int ExecuteUpdate(IDalSession session, object parameterObject )
		{
			int rows = 0; // the number of rows affected
			RequestScope request = _statement.Sql.GetRequestScope(parameterObject, session);;

			//using (IDbCommand command = CreatePreparedCommand(request, session, parameterObject ))		
			using ( IDbCommand command = _preparedCommand.Create( request, session, this.Statement, parameterObject ) )
			{
				rows = command.ExecuteNonQuery();

				ExecutePostSelect( session, request);

				RetrieveOutputParameters(request, session, command, parameterObject);
			}

			RaiseExecuteEvent();

			return rows;
		}

		
		/// <summary>
		/// Execute an insert statement. Fill the parameter object with 
		/// the ouput parameters if any, also could return the insert generated key
		/// </summary>
		/// <param name="session">The session</param>
		/// <param name="parameterObject">The parameter object used to fill the statement.</param>
		/// <returns>Can return the insert generated key.</returns>
		public virtual object ExecuteInsert(IDalSession session, object parameterObject )
		{
			object generatedKey = null;
			SelectKey selectKeyStatement = null;
			RequestScope request = _statement.Sql.GetRequestScope(parameterObject, session);;

			if (_statement is Insert)
			{
				selectKeyStatement = ((Insert)_statement).SelectKey;
			}

			if (selectKeyStatement != null && !selectKeyStatement.isAfter)
			{
				MappedStatement mappedStatement = _sqlMap.GetMappedStatement( selectKeyStatement.Id );
				generatedKey = mappedStatement.ExecuteQueryForObject(session, parameterObject);

				ObjectProbe.SetPropertyValue(parameterObject, selectKeyStatement.PropertyName, generatedKey);
			}

			//using (IDbCommand command = CreatePreparedCommand(request, session, parameterObject ))
			using ( IDbCommand command = _preparedCommand.Create( request, session, this.Statement, parameterObject ) )
			{
				if (_statement is Insert)
				{
					command.ExecuteNonQuery();
				}
				else
				{
					generatedKey = command.ExecuteScalar();
					if ( (_statement.ResultClass!=null) && 
						_sqlMap.TypeHandlerFactory.IsSimpleType(_statement.ResultClass) )
					{
						ITypeHandler typeHandler = _sqlMap.TypeHandlerFactory.GetTypeHandler(_statement.ResultClass);
						generatedKey = typeHandler.GetDataBaseValue(generatedKey, _statement.ResultClass);
					}
				}
			
				if (selectKeyStatement != null && selectKeyStatement.isAfter)
				{
					MappedStatement mappedStatement = _sqlMap.GetMappedStatement( selectKeyStatement.Id );
					generatedKey = mappedStatement.ExecuteQueryForObject(session, parameterObject);

					ObjectProbe.SetPropertyValue(parameterObject, selectKeyStatement.PropertyName, generatedKey);
				}

				ExecutePostSelect( session, request);

				RetrieveOutputParameters(request, session, command, parameterObject);
			}

			RaiseExecuteEvent();

			return generatedKey;
		}

		#endregion

		#region ExecuteQueryForMap
	
		/// <summary>
		/// Executes the SQL and retuns all rows selected in a map that is keyed on the property named
		/// in the keyProperty parameter.  The value at each key will be the value of the property specified
		/// in the valueProperty parameter.  If valueProperty is null, the entire result object will be entered.
		/// </summary>
		/// <param name="session">
		/// The session used to execute the statement
		/// </param>
		/// <param name="parameterObject">
		/// The object used to set the parameters in the SQL.
		/// </param>
		/// <param name="keyProperty">
		/// The property of the result object to be used as the key.
		/// </param>
		/// <param name="valueProperty">
		/// The property of the result object to be used as the value (or null)
		/// </param>
		/// <returns>A hashtable of object containing the rows keyed by keyProperty.</returns>
		///<exception cref="DataMapperException">
		///If a transaction is not in progress, or the database throws an exception.
		///</exception>
		public virtual IDictionary ExecuteQueryForMap( IDalSession session, object parameterObject, string keyProperty, string valueProperty )
		{
			IDictionary map = new Hashtable();
			RequestScope request = _statement.Sql.GetRequestScope(parameterObject, session);;

			if (_statement.CacheModel == null) 
			{
				map = RunQueryForMap(request, session, parameterObject, keyProperty, valueProperty );
			}
			else
			{
				CacheKey key = null;
				if (_statement.ParameterMap != null) 
				{
					key = new CacheKey(_sqlMap.TypeHandlerFactory, this.Name, 
						request.PreparedStatement.PreparedSql, 
						parameterObject, 
						request.ParameterMap.GetPropertyNameArray(), 
						NO_SKIPPED_RESULTS, 
						NO_MAXIMUM_RESULTS, 
						CacheKeyType.Map);
				} 
				else 
				{
					key = new CacheKey(_sqlMap.TypeHandlerFactory, this.Name, 
						request.PreparedStatement.PreparedSql,  
						parameterObject, 
						new string[0], 
						NO_SKIPPED_RESULTS, 
						NO_MAXIMUM_RESULTS, 
						CacheKeyType.Map);
				}

				map = (IDictionary)_statement.CacheModel[key];
				if (map == null) 
				{
					map = RunQueryForMap( request, session, parameterObject, keyProperty, valueProperty );
					_statement.CacheModel[key] = map;
				}
			}

			return map;
		}

		
		/// <summary>
		/// Executes the SQL and retuns all rows selected in a map that is keyed on the property named
		/// in the keyProperty parameter.  The value at each key will be the value of the property specified
		/// in the valueProperty parameter.  If valueProperty is null, the entire result object will be entered.
		/// </summary>
		/// <param name="request">The request scope.</param>
		/// <param name="session">The session used to execute the statement</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="keyProperty">The property of the result object to be used as the key.</param>
		/// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
		/// <returns>A hashtable of object containing the rows keyed by keyProperty.</returns>
		///<exception cref="DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
		private IDictionary RunQueryForMap( RequestScope request, IDalSession session, 
			object parameterObject, string keyProperty, string valueProperty )
		{
			IDictionary map = new Hashtable();

			IList list = ExecuteQueryForList(session, parameterObject);

			for(int i =0; i<list.Count; i++)
			{
				object obj = list[i];
				if (obj != null)
				{
					object key = ObjectProbe.GetPropertyValue(obj, keyProperty);

					object value = obj;
					if (valueProperty != null) 
					{
						value = ObjectProbe.GetPropertyValue(obj, valueProperty);
					}
					map.Add(key, value);
				}
			}

			return map;
		}

		
		#endregion

		/// <summary>
		/// Process 'select' result properties
		/// </summary>
		/// <param name="request"></param>
		/// <param name="session"></param>
		private void ExecutePostSelect(IDalSession session, RequestScope request)
		{
			while (request.QueueSelect.Count>0)
			{
				PostBindind postSelect = request.QueueSelect.Dequeue() as PostBindind;

				if (postSelect.Method == ExecuteMethod.ExecuteQueryForIList)
				{
					object values = postSelect.Statement.ExecuteQueryForList(session, postSelect.Keys); 
					ObjectProbe.SetPropertyValue( postSelect.Target, postSelect.ResultProperty.PropertyName, values);
				}
				else if (postSelect.Method == ExecuteMethod.ExecuteQueryForStrongTypedIList)
				{
					object values = Activator.CreateInstance(postSelect.ResultProperty.PropertyInfo.PropertyType);
					postSelect.Statement.ExecuteQueryForList(session, postSelect.Keys, (IList)values);
					ObjectProbe.SetPropertyValue( postSelect.Target, postSelect.ResultProperty.PropertyName, values);
				}
				if (postSelect.Method == ExecuteMethod.ExecuteQueryForArrayList)
				{
					IList values = postSelect.Statement.ExecuteQueryForList(session, postSelect.Keys); 
					Type elementType = postSelect.ResultProperty.PropertyInfo.PropertyType.GetElementType();

					Array array = Array.CreateInstance(elementType, values.Count);
					for(int i=0;i<values.Count;i++)
					{
						array.SetValue(values[i],i);
					}

					postSelect.ResultProperty.PropertyInfo.SetValue(postSelect.Target, array, null);
				}
				else if (postSelect.Method == ExecuteMethod.ExecuteQueryForObject)
				{
					object value = postSelect.Statement.ExecuteQueryForObject(session, postSelect.Keys); 
					ObjectProbe.SetPropertyValue( postSelect.Target, postSelect.ResultProperty.PropertyName, value);
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="resultMap"></param>
		/// <param name="mapping"></param>
		/// <param name="target"></param>
		/// <param name="reader"></param>
		private void SetObjectProperty(RequestScope request, ResultMap resultMap, 
			ResultProperty mapping, ref object target, IDataReader reader)
		{
			string selectStatement = mapping.Select;

			if (selectStatement.Length == 0 && mapping.NestedResultMap == null)
			{
				// If the property is not a 'select' ResultProperty 
				//                     or a 'resultMap' ResultProperty
				// We have a 'normal' ResultMap

				#region Not a select statement
				if (mapping.TypeHandler == null) // Find the TypeHandler
				{
					lock(mapping) 
					{
						if (mapping.TypeHandler == null)
						{
							int columnIndex = 0;
							if (mapping.ColumnIndex == ResultProperty.UNKNOWN_COLUMN_INDEX) 
							{
								columnIndex = reader.GetOrdinal(mapping.ColumnName);
							} 
							else 
							{
								columnIndex = mapping.ColumnIndex;
							}
							Type systemType =((IDataRecord)reader).GetFieldType(columnIndex);

							mapping.TypeHandler = _sqlMap.TypeHandlerFactory.GetTypeHandler(systemType);
						}
					}					
				}

				object dataBaseValue = mapping.GetDataBaseValue( reader );

				if (resultMap != null) 
				{
					resultMap.SetValueOfProperty( ref target, mapping, dataBaseValue );
				}
				else
				{
					MappedStatement.SetValueOfProperty( ref target, mapping, dataBaseValue );
				}
				#endregion
			}
			else if (mapping.NestedResultMap != null) // 'resultMap' ResultProperty
			{
				object obj = null;

				obj = mapping.NestedResultMap.CreateInstanceOfResult();
				FillObjectWithReaderAndResultMap(request, reader, mapping.NestedResultMap, obj);
				MappedStatement.SetValueOfProperty( ref target, mapping, obj );
			}
			else //'select' ResultProperty 
			{
				// Get the select statement
				MappedStatement queryStatement = _sqlMap.GetMappedStatement(selectStatement);
				string paramString = mapping.ColumnName;
				object keys = null;
				bool wasNull = false;

				#region Find Key(s)
				if (paramString.IndexOf(',')>0 || paramString.IndexOf('=')>0) // composite parameters key
				{
					IDictionary keyMap = new Hashtable();
					keys = keyMap;
					// define which character is seperating fields
					char[] splitter  = {'=',','};

					string[] paramTab = paramString.Split(splitter);
					if (paramTab.Length % 2 != 0) 
					{
						throw new DataMapperException("Invalid composite key string format in '"+mapping.PropertyName+". It must be: property1=column1,property2=column2,..."); 
					}
					IEnumerator enumerator = paramTab.GetEnumerator();
					while (!wasNull && enumerator.MoveNext()) 
					{
						string hashKey = ((string)enumerator.Current).Trim();
						enumerator.MoveNext();
						object hashValue = reader.GetValue( reader.GetOrdinal(((string)enumerator.Current).Trim()) );

						keyMap.Add(hashKey, hashValue );
						wasNull = (hashValue == System.DBNull.Value);
					}
				} 
				else // single parameter key
				{
					keys = reader.GetValue(reader.GetOrdinal(paramString));
					wasNull = reader.IsDBNull(reader.GetOrdinal(paramString));
				}
				#endregion

				if (wasNull) 
				{
					// set the value of an object property to null
					ObjectProbe.SetPropertyValue(target, mapping.PropertyName, null);
				} 
				else // Collection object or .Net object
				{
					PostBindind postSelect = new PostBindind();
					postSelect.Statement = queryStatement;
					postSelect.Keys = keys;
					postSelect.Target = target;
					postSelect.ResultProperty = mapping;

					#region Collection object or .NET object
					// Check if the object to Map implement 'IList' or is IList type
					// If yes the ResultProperty is map to a IList object
					if ( (mapping.PropertyInfo.PropertyType.GetInterface("IList") != null) || 
						(mapping.PropertyInfo.PropertyType == typeof(IList)))
					{
						object values = null;

						if (mapping.IsLazyLoad)
						{
							values = LazyLoadList.NewInstance(_sqlMap.DataSource, queryStatement, keys, target, mapping.PropertyName);
							ObjectProbe.SetPropertyValue( target, mapping.PropertyName, values);
						}
						else
						{
							if (mapping.PropertyInfo.PropertyType == typeof(IList))
							{
								postSelect.Method = ExecuteMethod.ExecuteQueryForIList;
							}
							else
							{
								postSelect.Method = ExecuteMethod.ExecuteQueryForStrongTypedIList;
							}
						}
					} 
					else if (mapping.PropertyInfo.PropertyType.IsArray)
					{
						postSelect.Method = ExecuteMethod.ExecuteQueryForArrayList;
					}
					else // The ResultProperty is map to a .Net object
					{
						postSelect.Method = ExecuteMethod.ExecuteQueryForObject;
					}
					#endregion

					if (!mapping.IsLazyLoad)
					{
						request.QueueSelect.Enqueue(postSelect);
					}
				}
			}
		}
	

		private static void SetValueOfProperty( ref object target, ResultProperty property, object dataBaseValue )
		{
			if (target is Hashtable)
			{
				((Hashtable) target).Add(property.PropertyName, dataBaseValue);
			}
			else
			{
				if (property.PropertyName == "value")
				{
					target = dataBaseValue;
				}
				else
				{
					if (dataBaseValue == null)
					{
						if (property.PropertyInfo != null)
						{
							property.PropertyInfo.SetValue( target, null, null );
						}
						else
						{
							ObjectProbe.SetPropertyValue( target, property.PropertyName, null);
						}					
					}
					else
					{
						if (property.PropertyInfo != null)
						{
							property.PropertyInfo.SetValue( target, dataBaseValue, null );
						}
						else
						{
							ObjectProbe.SetPropertyValue( target, property.PropertyName, dataBaseValue);
						}					
					}
				}
			}
		}

			
		/// <summary>
		/// Raise an event ExecuteEventArgs
		/// (Used when a query is executed)
		/// </summary>
		private void RaiseExecuteEvent()
		{
			ExecuteEventArgs e = new ExecuteEventArgs();
			e.StatementName = _statement.Id;
			if (Execute != null)
			{
				Execute(this, e);
			}
		}

		/// <summary>
		/// ToString implementation.
		/// </summary>
		/// <returns>A string that describes the MappedStatement</returns>
		public override string ToString() 
		{
			StringBuilder buffer = new StringBuilder();
			buffer.Append("\tMappedStatement: " + this.Name + "\n");
			if (_statement.ParameterMap != null) buffer.Append(_statement.ParameterMap.Id);
			if (_statement.ResultMap != null) buffer.Append(_statement.ResultMap.Id);

			return buffer.ToString();
		}
	

		private ReaderAutoMapper _readerAutoMapper = null;

		private void AutoMapReader( IDataReader reader,ref object resultObject) 
		{
			if (_readerAutoMapper == null)
			{
				lock (this) 
				{
					if (_readerAutoMapper == null) 
					{
						_readerAutoMapper = new ReaderAutoMapper(_sqlMap.TypeHandlerFactory, reader, ref resultObject);
					}
				}
			}

			_readerAutoMapper.AutoMapReader( reader, ref resultObject );
		}
		#endregion

		private class ReaderAutoMapper 
		{

//			private IList _mappings = new ArrayList();
			private ResultMap _resultMap = new ResultMap();

			/// <summary>
			/// 
			/// </summary>
			/// <param name="reader"></param>
			/// <param name="resultObject"></param>
			/// <param name="typeHandlerFactory"></param>
			public ReaderAutoMapper(TypeHandlerFactory typeHandlerFactory, IDataReader reader,ref object resultObject) 
			{
				try 
				{
					// Get all PropertyInfo from the resultObject properties
					ReflectionInfo reflectionInfo = ReflectionInfo.GetInstance(resultObject.GetType());
					string[] propertiesName = reflectionInfo.GetWriteablePropertyNames();

					Hashtable propertyMap = new Hashtable();
					for (int i = 0; i < propertiesName.Length; i++) 
					{
						propertyMap.Add( propertiesName[i].ToUpper(), reflectionInfo.GetSetter(propertiesName[i]) );
					}

					// Get all column Name from the reader
					// and build a resultMap from with the help of the PropertyInfo[].
					DataTable dataColumn = reader.GetSchemaTable();
					for (int i = 0; i < dataColumn.Rows.Count; i++) 
					{
						string columnName = dataColumn.Rows[i][0].ToString();
						PropertyInfo matchedPropertyInfo = propertyMap[columnName.ToUpper()] as PropertyInfo;

						ResultProperty property = new ResultProperty();
						property.ColumnName = columnName;

						if (matchedPropertyInfo != null ) 
						{
							property.PropertyName = matchedPropertyInfo.Name;
							property.Initialize(typeHandlerFactory, matchedPropertyInfo );
							_resultMap.AddResultPropery(property);
						}
						else if (resultObject is Hashtable) 
						{
							property.PropertyName = columnName;
							_resultMap.AddResultPropery(property);
						}

						// Fix for IBATISNET-73 (JIRA-73) from Ron Grabowski
						if (property.PropertyName != null && property.PropertyName.Length > 0)
						{
							// Set TypeHandler
							Type propertyType = reflectionInfo.GetSetterType(property.PropertyName);
							property.TypeHandler = typeHandlerFactory.GetTypeHandler( propertyType );
						}
						else
						{
							if (_logger.IsDebugEnabled)
							{
								_logger.Debug("The column [" + columnName + "] could not be auto mapped to a property on [" + resultObject.ToString() + "]");
							}
						}
					}
				} 
				catch (Exception e) 
				{
					throw new DataMapperException("Error automapping columns. Cause: " + e.Message, e);
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="reader"></param>
			/// <param name="resultObject"></param>
			public void AutoMapReader(IDataReader reader, ref object resultObject)
			{
				foreach (string key in _resultMap.ColumnsToPropertiesMap.Keys) 
				{
					ResultProperty property = (ResultProperty) _resultMap.ColumnsToPropertiesMap[key];
					MappedStatement.SetValueOfProperty( ref resultObject, property, 
						property.GetDataBaseValue( reader ));
				}
			}

		}
	}
}
