
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
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.Xml;

using IBatisNet.Common;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.SessionContainer;

using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.MappedStatements;

#endregion

namespace IBatisNet.DataMapper
{
	/// <summary>
	/// Summary description for SqlMap.
	/// </summary>
	public class SqlMapper
	{

		/// <summary>
		/// 
		/// </summary>
		public delegate void RowDelegate(object obj, IList list);

		#region Contants
		private const string DEFAULT_FILE_CONFIG_NAME = "sqlmap.config";
		#endregion

		#region Fields
		//(MappedStatement Name, MappedStatement)
		private HybridDictionary _mappedStatements = new HybridDictionary();
		//(ResultMap name, ResultMap)
		private HybridDictionary _resultMaps = new HybridDictionary();
		//(ParameterMap name, ParameterMap)
		private HybridDictionary _parameterMaps = new HybridDictionary();
		// DataSource
		private DataSource _dataSource = null;
		//(typeAlias name, type alias)
		private HybridDictionary _typeAliasMaps = new HybridDictionary();
		//(CacheModel name, cache))
		private HybridDictionary _cacheMaps = new HybridDictionary();

		private bool _cacheModelsEnabled = false;

		/// <summary>
		/// Container session unique for each thread. 
		/// </summary>
		private ISessionContainer _sessionContainer = null;

		#endregion

		#region Properties

		/// <summary>
		///  Returns the DalSession instance 
		///  currently being used by the SqlMap.
		/// </summary>
		public IDalSession LocalSession
		{
			get { return _sessionContainer.LocalSession; }
		}


		/// <summary>
		/// Tell if the session is started.
		/// </summary>
		/// <returns></returns>
		public bool IsSessionStarted
		{
			get { return (_sessionContainer.LocalSession != null); }
		}

		/// <summary>
		/// A flag that determines whether cache models were enabled 
		/// when this SqlMap was built.
		/// </summary>
		public bool IsCacheModelsEnabled
		{
			get { return _cacheModelsEnabled; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Create a new SqlMap
		/// </summary>
		internal SqlMapper() 
		{
			_sessionContainer = SessionContainerFactory.GetSessionContainer(IBatisNet.Common.Utilities.HashCodeProvider.GetIdentityHashCode(this).ToString());
		}
		#endregion

		#region Methods
		/// <summary>
		/// Set the faleg to tell us if cache models were enabled
		/// or not.
		/// </summary>
		internal void SetCacheModelsEnabled(bool value)
		{
			_cacheModelsEnabled = value;
		}

		#region Configure

		/// <summary>
		/// Configure an SqlMap.
		/// </summary>
		/// <param name="document">An xml sql map onfiguration document.</param>
		/// <returns>the SqlMap</returns>
		static public SqlMapper Configure( XmlDocument document )
		{
			return new DomSqlMapBuilder().Build( document, false );
		}

		/// <summary>
		/// Configure an SqlMap from
		/// default file named SqlMap.config.
		/// </summary>
		/// <returns>An SqlMap</returns>
		static public SqlMapper Configure()
		{
			return Configure( Resources.GetConfigAsXmlDocument(DEFAULT_FILE_CONFIG_NAME) );
		}


		/// <summary>
		/// Configure an SqlMap from via a file.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <returns>An SqlMap</returns>
		public static SqlMapper Configure(string fileName)
		{
			XmlDocument document = Resources.GetConfigAsXmlDocument(fileName);
			return new DomSqlMapBuilder().Build( document, false);
		}


		/// <summary>
		/// Configure and monitor the default configuration file for modifications 
		/// and automatically reconfigure SqlMap. 
		/// </summary>
		/// <returns>An SqlMap</returns>
		public static SqlMapper ConfigureAndWatch(ConfigureHandler configureDelegate)
		{
			return ConfigureAndWatch( DEFAULT_FILE_CONFIG_NAME, configureDelegate ) ;
		}


		/// <summary>
		/// Configure and monitor the configuration file for modifications 
		/// and automatically reconfigure SqlMap. 
		/// </summary>
		/// <param name="fileName">File name.</param>
		///<param name="configureDelegate">
		/// Delegate called when the file has changed, to rebuild the dal.
		/// </param>
		/// <returns>An SqlMap</returns>
		public static SqlMapper ConfigureAndWatch( string fileName, ConfigureHandler configureDelegate )
		{
			XmlDocument document = Resources.GetConfigAsXmlDocument( fileName );

			ConfigWatcherHandler.ClearFilesMonitored();
			ConfigWatcherHandler.AddFileToWatch( Resources.GetFileInfo( fileName ) );

			TimerCallback callBakDelegate = new TimerCallback( SqlMapper.OnConfigFileChange );

			StateConfig state = new StateConfig();
			state.fileName = fileName;
			state.configureHandler = configureDelegate;

			new ConfigWatcherHandler( callBakDelegate, state );

			return new DomSqlMapBuilder().Build( document, true );
		}

		/// <summary>
		/// Callback called when the SqlMap.config changed.
		/// </summary>
		/// <param name="obj">The state config.</param>
		public static void OnConfigFileChange(object obj)
		{
			StateConfig state = (StateConfig)obj;

			//SqlMap sqlMap = ConfigureAndWatch( state.fileName, state.configureHandler );
			//state.configureHandler( sqlMap );
			state.configureHandler( null );
		}

		#endregion

		#region Manage Connection, Transaction
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IDalSession OpenConnection() 
		{
			if (_sessionContainer.LocalSession != null) 
			{
				throw new DataMapperException("SqlMap could not invoke OpenConnection(). A connection is already started. Call CloseConnection first.");
			}
			SqlMapSession session = new SqlMapSession(this);
			_sessionContainer.Store(session);
			session.OpenConnection();
			return session;
		}

		/// <summary>
		/// Open a connection
		/// </summary>
		public void CloseConnection()
		{
			if (_sessionContainer.LocalSession == null) 
			{
				throw new DataMapperException("SqlMap could not invoke CloseConnection(). No connection was started. Call OpenConnection() first.");
			}
			try
			{
				IDalSession session = _sessionContainer.LocalSession;
				session.CloseConnection();			
			} 
			catch(Exception ex)
			{
				throw new DataMapperException("SqlMapper could not CloseConnection(). Cause :"+ex.Message, ex);
			}
			finally 
			{
				_sessionContainer.Dispose();
			}
		}


		/// <summary>
		/// Begins a database transaction.
		/// </summary>
		public IDalSession BeginTransaction() 
		{
			if (_sessionContainer.LocalSession != null) 
			{
				throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A Transaction is already started. Call CommitTransaction() or RollbackTransaction first.");
			}
			SqlMapSession session = new SqlMapSession(this);
			_sessionContainer.Store(session);
			session.BeginTransaction();
			return session ;
		}

		/// <summary>
		/// Begins a database transaction on the currect session
		/// </summary>
		/// <param name="openConnection">Open a connection.</param>
		public IDalSession BeginTransaction(bool openConnection)
		{
			IDalSession session = null;

			if (openConnection)
			{
				session = this.BeginTransaction();
			}
			else
			{
				session = _sessionContainer.LocalSession;
				if (session == null) 
				{
					throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A session must be Open. Call OpenConnection() first.");
				}
				session.BeginTransaction(openConnection);
			}

			return session;
		}

		/// <summary>
		/// Begins a database transaction with the specified isolation level.
		/// </summary>
		/// <param name="isolationLevel">
		/// The isolation level under which the transaction should run.
		/// </param>
		public IDalSession BeginTransaction(IsolationLevel isolationLevel)
		{
			if (_sessionContainer.LocalSession != null) 
			{
				throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A Transaction is already started. Call CommitTransaction() or RollbackTransaction first.");
			}
			SqlMapSession session = new SqlMapSession(this);
			_sessionContainer.Store(session);
			session.BeginTransaction(isolationLevel);
			return session;
		}

		/// <summary>
		/// Start a database transaction on the current session
		/// with the specified isolation level.
		/// </summary>
		/// <param name="openConnection">Open a connection.</param>
		/// <param name="isolationLevel">
		/// The isolation level under which the transaction should run.
		/// </param>
		public IDalSession BeginTransaction(bool openConnection, IsolationLevel isolationLevel)
		{
			IDalSession session = null;

			if (openConnection)
			{
				session = this.BeginTransaction(isolationLevel);
			}
			else
			{
				session = _sessionContainer.LocalSession;
				if (session == null) 
				{
					throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A session must be Open. Call OpenConnection() first.");
				}
				session.BeginTransaction(openConnection, isolationLevel);
			}
			return session;
		}

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		/// <remarks>
		/// Will close the connection.
		/// </remarks>
		public void CommitTransaction()
		{
			if (_sessionContainer.LocalSession == null) 
			{
				throw new DataMapperException("SqlMap could not invoke CommitTransaction(). No Transaction was started. Call BeginTransaction() first.");
			}
			try
			{
				IDalSession session = _sessionContainer.LocalSession;
				session.CommitTransaction();
			} 
			finally 
			{
				_sessionContainer.Dispose();
			}
		}

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		/// <param name="closeConnection">Close the connection</param>
		public void CommitTransaction(bool closeConnection)
		{
			if (_sessionContainer.LocalSession == null) 
			{
				throw new DataMapperException("SqlMap could not invoke CommitTransaction(). No Transaction was started. Call BeginTransaction() first.");
			}
			try
			{
				IDalSession session = _sessionContainer.LocalSession;
				session.CommitTransaction(closeConnection);
			} 
			finally 
			{
				if (closeConnection)
				{
					_sessionContainer.Dispose();
				}
			}
		}

		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		/// <remarks>
		/// Will close the connection.
		/// </remarks>
		public void RollBackTransaction()
		{
			if (_sessionContainer.LocalSession == null) 
			{
				throw new DataMapperException("SqlMap could not invoke RollBackTransaction(). No Transaction was started. Call BeginTransaction() first.");
			}
			try
			{
				IDalSession session = _sessionContainer.LocalSession;
				session.RollBackTransaction();			
			} 
			finally 
			{
				_sessionContainer.Dispose();
			}
		}

		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		/// <param name="closeConnection">Close the connection</param>
		public void RollBackTransaction(bool closeConnection)
		{
			if (_sessionContainer.LocalSession == null) 
			{
				throw new DataMapperException("SqlMap could not invoke RollBackTransaction(). No Transaction was started. Call BeginTransaction() first.");
			}
			try
			{
				IDalSession session = _sessionContainer.LocalSession;
				session.RollBackTransaction(closeConnection);			
			} 
			finally 
			{
				if (closeConnection)
				{
					_sessionContainer.Dispose();
				}
			}
		}

		#endregion
		
		#region QueryForObject

		/// <summary>
		/// Executes a Sql SELECT statement that returns that returns data 
		/// to populate a single object instance.
		/// <p/>
		/// The parameter object is generally used to supply the input
		/// data for the WHERE clause parameter(s) of the SELECT statement.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <returns> The single result object populated with the result set data.</returns>
		public object QueryForObject(string statementName, object parameterObject)
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionContainer.LocalSession;
			object result;
 
			if (session == null) 
			{
				session = new SqlMapSession(this.DataSource);
				session.OpenConnection();
				isSessionLocal = true;
			}

			IMappedStatement statement = GetMappedStatement(statementName);

			try 
			{
				result = statement.ExecuteQueryForObject(session, parameterObject);
			} 
			catch
			{
				throw;
			}
			finally
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
			}

			return result;
		}


		/// <summary>
		/// Executes a Sql SELECT statement that returns a single object of the type of the
		/// resultObject parameter.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="resultObject">An object of the type to be returned.</param>
		/// <returns>The single result object populated with the result set data.</returns>
		public object QueryForObject(string statementName, object parameterObject, object resultObject)
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionContainer.LocalSession;
			object result = null;
 
			if (session == null) 
			{
				session = new SqlMapSession(this.DataSource);
				session.OpenConnection();
				isSessionLocal = true;
			}

			IMappedStatement statement = GetMappedStatement(statementName);

			try 
			{
				result = statement.ExecuteQueryForObject(session, parameterObject, resultObject);
			} 
			catch
			{
				throw;
			}
			finally
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
			}

			return result;
		}

	
		#endregion

		#region QueryForMap, QueryForDictionary

		/// <summary>
		///  Alias to QueryForMap, .NET spirit.
		///  Feature idea by Ted Husted.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="keyProperty">The property of the result object to be used as the key.</param>
		/// <returns>A IDictionary (Hashtable) of object containing the rows keyed by keyProperty.</returns>
		public IDictionary QueryForDictionary(string statementName, object parameterObject, string keyProperty)
		{
			return QueryForMap( statementName, parameterObject, keyProperty);
		}

		/// <summary>
		/// Alias to QueryForMap, .NET spirit.
		///  Feature idea by Ted Husted.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="keyProperty">The property of the result object to be used as the key.</param>
		/// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
		/// <returns>A IDictionary (Hashtable) of object containing the rows keyed by keyProperty.</returns>
		///<exception cref="DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
		public IDictionary QueryForDictionary(string statementName, object parameterObject, string keyProperty, string valueProperty)
		{
			return QueryForMap( statementName, parameterObject, keyProperty, valueProperty);
		}

		/// <summary>
		///  Executes the SQL and retuns all rows selected in a map that is keyed on the property named
		///  in the keyProperty parameter.  The value at each key will be the entire result object.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="keyProperty">The property of the result object to be used as the key.</param>
		/// <returns>A IDictionary (Hashtable) of object containing the rows keyed by keyProperty.</returns>
		public IDictionary QueryForMap(string statementName, object parameterObject, string keyProperty)
		{
			return QueryForMap(statementName, parameterObject, keyProperty, null);
		}


		/// <summary>
		/// Executes the SQL and retuns all rows selected in a map that is keyed on the property named
		/// in the keyProperty parameter.  The value at each key will be the value of the property specified
		/// in the valueProperty parameter.  If valueProperty is null, the entire result object will be entered.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="keyProperty">The property of the result object to be used as the key.</param>
		/// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
		/// <returns>A IDictionary (Hashtable) of object containing the rows keyed by keyProperty.</returns>
		///<exception cref="DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
		public IDictionary QueryForMap(string statementName, object parameterObject, string keyProperty, string valueProperty)
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionContainer.LocalSession;
			IDictionary map = null;
 
			if (session == null) 
			{
				session = new SqlMapSession(this.DataSource);
				session.OpenConnection();
				isSessionLocal = true;
			}

			IMappedStatement statement = GetMappedStatement(statementName);

			try 
			{
				map = statement.ExecuteQueryForMap(session, parameterObject, keyProperty, valueProperty);
			} 
			catch
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
				throw;
			}

			return map;
		}

		
		#endregion

		#region QueryForList

		/// <summary>
		/// Executes a Sql SELECT statement that returns data to populate
		/// a number of result objects.
		/// <p/>
		///  The parameter object is generally used to supply the input
		/// data for the WHERE clause parameter(s) of the SELECT statement.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <returns>A List of result objects.</returns>
		public IList QueryForList(string statementName, object parameterObject)
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionContainer.LocalSession;
			IList list;
 
			if (session == null) 
			{
				session = new SqlMapSession(this.DataSource);
				session.OpenConnection();
				isSessionLocal = true;
			}

			IMappedStatement statement = GetMappedStatement(statementName);

			try 
			{
				list = statement.ExecuteQueryForList(session, parameterObject);				
			} 
			catch
			{
				throw;
			}
			finally
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
			}

			return list;
		}
		
		
		/// <summary>
		/// Executes the SQL and retuns all rows selected.
		/// <p/>
		///  The parameter object is generally used to supply the input
		/// data for the WHERE clause parameter(s) of the SELECT statement.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="skipResults">The number of rows to skip over.</param>
		/// <param name="maxResults">The maximum number of rows to return.</param>
		/// <returns>A List of result objects.</returns>
		public IList QueryForList(string statementName, object parameterObject, int skipResults, int maxResults)	
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionContainer.LocalSession;
			IList list;
 
			if (session == null) 
			{
				session = new SqlMapSession(this.DataSource);
				session.OpenConnection();
				isSessionLocal = true;
			}

			IMappedStatement statement = GetMappedStatement(statementName);

			try 
			{
				list = statement.ExecuteQueryForList(session, parameterObject, skipResults, maxResults);
			} 
			catch
			{
				throw;
			}
			finally
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
			}

			return list;
		}

		
		/// <summary>
		/// Executes a Sql SELECT statement that returns data to populate
		/// a number of result objects.
		/// <p/>
		///  The parameter object is generally used to supply the input
		/// data for the WHERE clause parameter(s) of the SELECT statement.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="resultObject">An Ilist object used to hold the objects.</param>
		/// <returns>A List of result objects.</returns>
		public void QueryForList(string statementName, object parameterObject, IList resultObject)
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionContainer.LocalSession;
 
			if (resultObject == null)
			{
				throw new DataMapperException("resultObject parameter must be instantiated before being passed to SqlMapper.QueryForList");
			}

			if (session == null) 
			{
				session = new SqlMapSession(this.DataSource);
				session.OpenConnection();
				isSessionLocal = true;
			}

			IMappedStatement statement = GetMappedStatement(statementName);

			try 
			{
				statement.ExecuteQueryForList(session, parameterObject, resultObject);
			} 
			catch
			{
				throw;
			}
			finally
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
			}
		}
		
		
		#endregion

		#region QueryForPaginatedList
		/// <summary>
		/// Executes the SQL and retuns a subset of the results in a dynamic PaginatedList that can be used to
		/// automatically scroll through results from a database table.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL</param>
		/// <param name="pageSize">The maximum number of objects to store in each page</param>
		/// <returns>A PaginatedList of beans containing the rows</returns>
		public PaginatedList QueryForPaginatedList(String statementName, object parameterObject, int pageSize)
		{
			IMappedStatement statement = GetMappedStatement(statementName);
			return new PaginatedList(statement, parameterObject, pageSize);
		}
		#endregion

		#region QueryWithRowDelegate

		/// <summary>
		/// Runs a query with a custom object that gets a chance to deal 
		/// with each row as it is processed.
		/// <p/>
		///  The parameter object is generally used to supply the input
		/// data for the WHERE clause parameter(s) of the SELECT statement.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="rowDelegate"></param>
		/// <returns>A List of result objects.</returns>
		public IList QueryWithRowDelegate(string statementName, object parameterObject, RowDelegate rowDelegate)
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionContainer.LocalSession;
			IList list = null;
 
			if (session == null) 
			{
				session = new SqlMapSession(this.DataSource);
				session.OpenConnection();
				isSessionLocal = true;
			}

			IMappedStatement statement = GetMappedStatement(statementName);

			try 
			{
				list = statement.ExecuteQueryForRowDelegate(session, parameterObject, rowDelegate);
			} 
			catch
			{
				throw;
			}
			finally
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
			}

			return list;
		}

		
		#endregion

		#region Query Insert, Update, Delete

		/// <summary>
		/// Executes a Sql INSERT statement.
		/// Insert is a bit different from other update methods, as it
		/// provides facilities for returning the primary key of the
		/// newly inserted row (rather than the effected rows).  This
		/// functionality is of course optional.
		/// <p/>
		/// The parameter object is generally used to supply the input
		/// data for the INSERT values.
		/// </summary>
		/// <param name="statementName">The name of the statement to execute.</param>
		/// <param name="parameterObject">The parameter object.</param>
		/// <returns> The primary key of the newly inserted row.  
		/// This might be automatically generated by the RDBMS, 
		/// or selected from a sequence table or other source.
		/// </returns>
		public object Insert(string statementName, object parameterObject)
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionContainer.LocalSession;
			object generatedKey = null;
 
			if (session == null) 
			{
				session = new SqlMapSession(this.DataSource);
				session.OpenConnection();
				isSessionLocal = true;
			}

			IMappedStatement statement = GetMappedStatement(statementName);

			try 
			{
				generatedKey = statement.ExecuteInsert(session, parameterObject);
			} 
			catch
			{
				throw;
			}
			finally
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
			}

			return generatedKey;
		}


		/// <summary>
		/// Executes a Sql UPDATE statement.
		/// Update can also be used for any other update statement type,
		/// such as inserts and deletes.  Update returns the number of
		/// rows effected.
		/// <p/>
		/// The parameter object is generally used to supply the input
		/// data for the UPDATE values as well as the WHERE clause parameter(s).
		/// </summary>
		/// <param name="statementName">The name of the statement to execute.</param>
		/// <param name="parameterObject">The parameter object.</param>
		/// <returns>The number of rows effected.</returns>
//		/// <exception cref="IBatisNet.Common.Exceptions.DalConcurrentException">
//		/// If no rows are effected throw this exception.
//		/// </exception>
		public int Update(string statementName, object parameterObject)
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionContainer.LocalSession;
			int rows = 0; // the number of rows affected

			if (session == null) 
			{
				session = new SqlMapSession(this.DataSource);
				session.OpenConnection();
				isSessionLocal = true;
			}

			IMappedStatement statement = GetMappedStatement(statementName);

			try 
			{
				rows = statement.ExecuteUpdate(session, parameterObject);
			} 
			catch
			{
				throw;
			}
			finally
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
			}

//			// check that statement affected a row
//			if( rows == 0 )
//			{
//				// throw concurrency error if no record was affected
//				throw new ConcurrentException();
//			}

			return rows;
		}


		/// <summary>
		///  Executes a Sql DELETE statement.
		///  Delete returns the number of rows effected.
		/// </summary>
		/// <param name="statementName">The name of the statement to execute.</param>
		/// <param name="parameterObject">The parameter object.</param>
		/// <returns>The number of rows effected.</returns>
		public int Delete(string statementName, object parameterObject)
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionContainer.LocalSession;
			int rows = 0; // the number of rows affected

			if (session == null) 
			{
				session = new SqlMapSession(this.DataSource);
				session.OpenConnection();
				isSessionLocal = true;
			}

			IMappedStatement statement = GetMappedStatement(statementName);
			
			try 
			{
				rows = statement.ExecuteUpdate(session, parameterObject);
			} 
			catch
			{
				throw;
			}
			finally
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
			}

			return rows;
		}


		#endregion

		#region Get/Add ParemeterMap, ResultMap, MappedStatement, TypeAlias, DataSource, CacheModel

		/// <summary>
		/// Gets a MappedStatement by name
		/// </summary>
		/// <param name="name"> The name of the statement</param>
		/// <returns> The MappedStatement</returns>
		public MappedStatement GetMappedStatement(string name) 
		{
			if (_mappedStatements.Contains(name) == false) 
			{
				throw new DataMapperException("This SQL map does not contain a MappedStatement named " + name);
			}
			return (MappedStatement) _mappedStatements[name];
		}

		/// <summary>
		/// Adds a (named) MappedStatement.
		/// </summary>
		/// <param name="key"> The key name</param>
		/// <param name="mappedStatement">The statement to add</param>
		internal void AddMappedStatement(string key, MappedStatement mappedStatement) 
		{
			if (_mappedStatements.Contains(key) == true) 
			{
				throw new DataMapperException("This SQL map already contains a MappedStatement named " + mappedStatement.Name);
			}
			_mappedStatements.Add(key, mappedStatement);
		}

		/// <summary>
		/// The MappedStatements collection
		/// </summary>
		internal HybridDictionary MappedStatements
		{
			get { return _mappedStatements; }
		}


		/// <summary>
		/// Get a ParameterMap by name
		/// </summary>
		/// <param name="name">The name of the ParameterMap</param>
		/// <returns>The ParameterMap</returns>
		internal ParameterMap GetParameterMap(string name) 
		{
			if (!_parameterMaps.Contains(name)) 
			{
				throw new DataMapperException("This SQL map does not contain an ParameterMap named " + name + ".  ");
			}
			return (ParameterMap) _parameterMaps[name];
		}


		/// <summary>
		/// Adds a (named) ParameterMap.
		/// </summary>
		/// <param name="parameterMap">the ParameterMap to add</param>
		internal void AddParameterMap(ParameterMap parameterMap) 
		{
			if (_parameterMaps.Contains(parameterMap.Id) == true) 
			{
				throw new DataMapperException("This SQL map already contains an ParameterMap named " + parameterMap.Id);
			}
			_parameterMaps.Add(parameterMap.Id, parameterMap);
		}


		/// <summary>
		/// Gets a ResultMap by name
		/// </summary>
		/// <param name="name">The name of the result map</param>
		/// <returns>The ResultMap</returns>
		internal ResultMap GetResultMap(string name) 
		{
			if (_resultMaps.Contains(name) == false) 
			{
				throw new DataMapperException("This SQL map does not contain an ResultMap named " + name);
			}
			return (ResultMap) _resultMaps[name];
		}

		/// <summary>
		/// Adds a (named) ResultMap
		/// </summary>
		/// <param name="resultMap">The ResultMap to add</param>
		internal void AddResultMap(ResultMap resultMap) 
		{
			if (_resultMaps.Contains(resultMap.Id) == true) 
			{
				throw new DataMapperException("This SQL map already contains an ResultMap named " + resultMap.Id);
			}
			_resultMaps.Add(resultMap.Id, resultMap);
		}

		/// <summary>
		/// The ParameterMap collection
		/// </summary>
		internal HybridDictionary ParameterMaps
		{
			get { return _parameterMaps; }
		}

		/// <summary>
		/// The ResultMap collection
		/// </summary>
		internal HybridDictionary ResultMaps
		{
			get { return _resultMaps; }
		}


		/// <summary>
		/// Get the DataSource
		/// </summary>
		public DataSource DataSource
		{
			get { return  _dataSource; }
			set { _dataSource = value; }
		}

		/// <summary>
		/// Gets a named TypeAlias from the list of available TypeAlias
		/// </summary>
		/// <param name="name">The name of the TypeAlias.</param>
		/// <returns>The TypeAlias.</returns>
		internal TypeAlias GetTypeAlias(string name) 
		{
			if (_typeAliasMaps.Contains(name) == true) 
			{
				return (TypeAlias) _typeAliasMaps[name];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Adds a named TypeAlias to the list of available TypeAlias.
		/// </summary>
		/// <param name="key">The key name.</param>
		/// <param name="typeAlias"> The TypeAlias.</param>
		internal void AddTypeAlias(string key, TypeAlias typeAlias) 
		{
			if (_typeAliasMaps.Contains(key) == true) 
			{
				throw new DataMapperException(" Alias name conflict occurred.  The type alias '" + key + "' is already mapped to the value '"+typeAlias.ClassName+"'.");
			}
			_typeAliasMaps.Add(key, typeAlias);
		}


		/// <summary>
		/// Gets the type object from the specific class name.
		/// </summary>
		/// <param name="className">The supplied class name.</param>
		/// <returns>The correpsonding type.
		/// </returns>
		internal Type GetType(string className) 
		{
			Type type = null;
			TypeAlias typeAlias = this.GetTypeAlias(className) as TypeAlias;

			if (typeAlias != null)
			{
				type = typeAlias.Class;
			}
			else
			{
				type = Resources.TypeForName(className);
			}

			return type;
		}

		/// <summary>
		/// Flushes all cached objects that belong to this SqlMap
		/// </summary>
		public void FlushCaches() 
		{
			IDictionaryEnumerator enumerator = _cacheMaps.GetEnumerator();
			while (enumerator.MoveNext())
			{
				((CacheModel)enumerator.Value).Flush();
			}
		}

		/// <summary>
		/// Adds a (named) cache.
		/// </summary>
		/// <param name="cache">The cache to add</param>
		internal void AddCache(CacheModel cache) 
		{
			if (_cacheMaps.Contains(cache.Id)) 
			{
				throw new DataMapperException("This SQL map already contains an Cache named " + cache.Id);
			}
			_cacheMaps.Add(cache.Id, cache);
		}

		/// <summary>
		/// Gets a cache by name
		/// </summary>
		/// <param name="name">The name of the cache to get</param>
		/// <returns>The cache object</returns>
		internal CacheModel GetCache(string name) 
		{
			if (!_cacheMaps.Contains(name)) 
			{
				throw new DataMapperException("This SQL map does not contain an Cache named " + name);
			}
			return (CacheModel) _cacheMaps[name];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetDataCacheStats() 
		{
			StringBuilder buffer = new StringBuilder("\nCache Data Statistiques");
			buffer.Append("\n=======================\n");
			IDictionaryEnumerator enumerator = _mappedStatements.GetEnumerator();
			while (enumerator.MoveNext()) 
			{
				MappedStatement statement = (MappedStatement) enumerator.Value;
				buffer.Append(statement.Name);
				buffer.Append(": ");
				double hitRatio = statement.GetDataCacheHitRatio();
				if (hitRatio != -1) 
				{
					buffer.Append(Math.Round(hitRatio * 100));
					buffer.Append("%");
				} 
				else 
				{
					buffer.Append("No Cache.");
				}
				buffer.Append("\n");
			}
			return buffer.ToString();
		}

		#endregion

		#endregion
	}
}
