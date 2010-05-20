
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
using System.Text;
using System.Threading;
using System.Xml;
using IBatisNet.Common;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.TypeHandlers;

#endregion

namespace IBatisNet.DataMapper
{
	/// <summary>
	/// Summary description for SqlMap.
	/// </summary>
	public class SqlMapper
	{
		/// <summary>
		/// A delegate called once per row in the QueryWithRowDelegate method
		/// </summary>
		/// <param name="obj">The object currently being processed.</param>
		/// <param name="parameterObject">The optional parameter object passed into the QueryWithRowDelegate method.</param>
		/// <param name="list">The IList that will be returned to the caller.</param>
		public delegate void RowDelegate(object obj, object parameterObject, IList list);

		/// <summary>
		/// A delegate called once per row in the QueryForMapWithRowDelegate method
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="parameterObject">The optional parameter object passed into the QueryForMapWithRowDelegate method.</param>
		/// <param name="dictionary">The IDictionary that will be returned to the caller.</param>
		public delegate void DictionaryRowDelegate(object key, object value, object parameterObject, IDictionary dictionary);

		#region Fields
		//(MappedStatement Name, MappedStatement)
		private HybridDictionary _mappedStatements = new HybridDictionary();
		//(ResultMap name, ResultMap)
		private HybridDictionary _resultMaps = new HybridDictionary();
		//(ParameterMap name, ParameterMap)
		private HybridDictionary _parameterMaps = new HybridDictionary();
		// DataSource
		private DataSource _dataSource = null;
		//(CacheModel name, cache))
		private HybridDictionary _cacheMaps = new HybridDictionary();
		private TypeHandlerFactory _typeHandlerFactory = null; 

		private bool _cacheModelsEnabled = false;
		private bool _useEmbedStatementParams = false;
		// An identifiant 
		private string _id = string.Empty;

		/// <summary>
		/// Container session unique for each thread. 
		/// </summary>
		private SessionHolder _sessionHolder = null;

		#endregion

		#region Properties

		/// <summary>
		///  Returns the DalSession instance 
		///  currently being used by the SqlMap.
		/// </summary>
		public IDalSession LocalSession
		{
			get { return _sessionHolder.LocalSession; }
		}


		/// <summary>
		/// Tell if the session is started.
		/// </summary>
		/// <returns></returns>
		public bool IsSessionStarted
		{
			get { return (_sessionHolder.LocalSession != null); }
		}

		/// <summary>
		/// A flag that determines whether cache models were enabled 
		/// when this SqlMap was built.
		/// </summary>
		public bool IsCacheModelsEnabled
		{
			get { return _cacheModelsEnabled; }
		}

		/// <summary>
		/// A flag that determines whether statements use
		/// embedded parameters.
		/// </summary>
		public bool UseEmbedStatementParams
		{
			get { return _useEmbedStatementParams; }
		}

		/// <summary>
		/// The TypeHandlerFactory
		/// </summary>
		internal TypeHandlerFactory TypeHandlerFactory
		{
			get { return _typeHandlerFactory; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Create a new SqlMap
		/// </summary>
		internal SqlMapper(TypeHandlerFactory typeHandlerFactory) 
		{
			_typeHandlerFactory = typeHandlerFactory;
			_id = HashCodeProvider.GetIdentityHashCode(this).ToString();
			_sessionHolder = new SessionHolder(_id);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Set the falg to tell us if cache models were enabled
		/// or not.
		/// </summary>
		internal void SetCacheModelsEnabled(bool value)
		{
			_cacheModelsEnabled = value;
		}

		/// <summary>
		/// Sets the flag indicating if statements should used embedded
		/// parameters
		/// </summary>
		internal void SetUseEmbedStatementParams(bool value)
		{
			_useEmbedStatementParams = value;
		}

		#region Configure

		/// <summary>
		/// Configure an SqlMap.
		/// </summary>
		/// <param name="document">An xml sql map configuration document.</param>
		/// <returns>the SqlMap</returns>
		[Obsolete("This method will be remove in next version, use DomSqlMapBuilder.Configure.", false)]
		static public SqlMapper Configure( XmlDocument document )
		{
			return new DomSqlMapBuilder().Build( document, false );
		}

		/// <summary>
		/// Configure an SqlMap from default resource file named SqlMap.config.
		/// </summary>
		/// <returns>An SqlMap</returns>
		/// <remarks>The file path is relative to the application root.</remarks>
		[Obsolete("This method will be remove in future version, use DomSqlMapBuilder.Configure.", false)]
		static public SqlMapper Configure()
		{
			return Configure( Resources.GetConfigAsXmlDocument(DomSqlMapBuilder.DEFAULT_FILE_CONFIG_NAME) );
		}


		/// <summary>
		/// Configure an SqlMap from via a relative ressource path.
		/// </summary>
		/// <param name="resource">
		/// A relative ressource path from your Application root 
		/// or an absolue file path file:\\c:\dir\a.config
		/// </param>
		/// <returns>An SqlMap</returns>
		[Obsolete("This method will be remove in future version, use DomSqlMapBuilder.Configure.", false)]
		public static SqlMapper Configure(string resource)
		{
			XmlDocument document = null;
			if (resource.StartsWith("file://"))
			{
				document = Resources.GetUrlAsXmlDocument( resource.Remove(0, 7) );	
			}
			else
			{
				document = Resources.GetResourceAsXmlDocument( resource );	
			}
			return new DomSqlMapBuilder().Build( document, false);
		}


		/// <summary>
		/// Configure and monitor the default configuration file for modifications 
		/// and automatically reconfigure SqlMap. 
		/// </summary>
		/// <returns>An SqlMap</returns>
		[Obsolete("This method will be remove in future version, use DomSqlMapBuilder.Configure.", false)]
		public static SqlMapper ConfigureAndWatch(ConfigureHandler configureDelegate)
		{
			return ConfigureAndWatch( DomSqlMapBuilder.DEFAULT_FILE_CONFIG_NAME, configureDelegate ) ;
		}


		/// <summary>
		/// Configure and monitor the configuration file for modifications 
		/// and automatically reconfigure SqlMap. 
		/// </summary>
		/// <param name="resource">
		/// A relative ressource path from your Application root 
		/// or a absolue file path file:\\c:\dir\a.config
		/// </param>
		///<param name="configureDelegate">
		/// Delegate called when the file has changed, to rebuild the dal.
		/// </param>
		/// <returns>An SqlMap</returns>
		[Obsolete("This method will be remove in future version, use DomSqlMapBuilder.Configure.", false)]
		public static SqlMapper ConfigureAndWatch( string resource, ConfigureHandler configureDelegate )
		{
			XmlDocument document = null;
			if (resource.StartsWith("file://"))
			{
				document = Resources.GetUrlAsXmlDocument( resource.Remove(0, 7) );	
			}
			else
			{
				document = Resources.GetResourceAsXmlDocument( resource );	
			}

			ConfigWatcherHandler.ClearFilesMonitored();
			ConfigWatcherHandler.AddFileToWatch( Resources.GetFileInfo( resource ) );

			TimerCallback callBakDelegate = new TimerCallback( DomSqlMapBuilder.OnConfigFileChange );

			StateConfig state = new StateConfig();
			state.FileName = resource;
			state.ConfigureHandler = configureDelegate;

			new ConfigWatcherHandler( callBakDelegate, state );

			return new DomSqlMapBuilder().Build( document, true );
		}


		#endregion

		#region Manage Connection, Transaction
		
		/// <summary>
		/// Open a connection
		/// </summary>
		/// <returns></returns>
		public IDalSession OpenConnection() 
		{
			if (_sessionHolder.LocalSession != null) 
			{
				throw new DataMapperException("SqlMap could not invoke OpenConnection(). A connection is already started. Call CloseConnection first.");
			}
			SqlMapSession session = new SqlMapSession(this);
			_sessionHolder.Store(session);
			session.OpenConnection();
			return session;
		}

		/// <summary>
		/// Open a connection, on the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string</param>
		public IDalSession OpenConnection(string connectionString)
		{
			if (_sessionHolder.LocalSession != null) 
			{
				throw new DataMapperException("SqlMap could not invoke OpenConnection(). A connection is already started. Call CloseConnection first.");
			}
			SqlMapSession session = new SqlMapSession(this);
			_sessionHolder.Store(session);
			session.OpenConnection(connectionString);
			return session;
		}

		/// <summary>
		/// Open a connection
		/// </summary>
		public void CloseConnection()
		{
			if (_sessionHolder.LocalSession == null) 
			{
				throw new DataMapperException("SqlMap could not invoke CloseConnection(). No connection was started. Call OpenConnection() first.");
			}
			try
			{
				IDalSession session = _sessionHolder.LocalSession;
				session.CloseConnection();			
			} 
			catch(Exception ex)
			{
				throw new DataMapperException("SqlMapper could not CloseConnection(). Cause :"+ex.Message, ex);
			}
			finally 
			{
				_sessionHolder.Dispose();
			}
		}


		/// <summary>
		/// Begins a database transaction.
		/// </summary>
		public IDalSession BeginTransaction() 
		{
			if (_sessionHolder.LocalSession != null) 
			{
				throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A Transaction is already started. Call CommitTransaction() or RollbackTransaction first.");
			}
			SqlMapSession session = new SqlMapSession(this);
			_sessionHolder.Store(session);
			session.BeginTransaction();
			return session ;
		}

		/// <summary>
		/// Open a connection and begin a transaction on the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string</param>
		public IDalSession BeginTransaction(string connectionString)
		{
			if (_sessionHolder.LocalSession != null) 
			{
				throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A Transaction is already started. Call CommitTransaction() or RollbackTransaction first.");
			}
			SqlMapSession session = new SqlMapSession(this);
			_sessionHolder.Store(session);
			session.BeginTransaction( connectionString );
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
				session = _sessionHolder.LocalSession;
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
			if (_sessionHolder.LocalSession != null) 
			{
				throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A Transaction is already started. Call CommitTransaction() or RollbackTransaction first.");
			}
			SqlMapSession session = new SqlMapSession(this);
			_sessionHolder.Store(session);
			session.BeginTransaction(isolationLevel);
			return session;
		}

		/// <summary>
		/// Open a connection and begin a transaction on the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string</param>
		/// <param name="isolationLevel">The transaction isolation level for this connection.</param>
		public IDalSession BeginTransaction(string connectionString, IsolationLevel isolationLevel)
		{
			if (_sessionHolder.LocalSession != null) 
			{
				throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A Transaction is already started. Call CommitTransaction() or RollbackTransaction first.");
			}
			SqlMapSession session = new SqlMapSession(this);
			_sessionHolder.Store(session);
			session.BeginTransaction( connectionString, isolationLevel);
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
				session = _sessionHolder.LocalSession;
				if (session == null) 
				{
					throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A session must be Open. Call OpenConnection() first.");
				}
				session.BeginTransaction(openConnection, isolationLevel);
			}
			return session;
		}

		/// <summary>
		/// Begins a transaction on the current connection
		/// with the specified IsolationLevel value.
		/// </summary>
		/// <param name="isolationLevel">The transaction isolation level for this connection.</param>
		/// <param name="connectionString">The connection string</param>
		/// <param name="openConnection">Open a connection.</param>
		public IDalSession BeginTransaction(string connectionString, bool openConnection, IsolationLevel isolationLevel)
		{
			IDalSession session = null;

			if (openConnection)
			{
				session = this.BeginTransaction(connectionString, isolationLevel);
			}
			else
			{
				session = _sessionHolder.LocalSession;
				if (session == null) 
				{
					throw new DataMapperException("SqlMap could not invoke BeginTransaction(). A session must be Open. Call OpenConnection() first.");
				}
				session.BeginTransaction(connectionString, openConnection, isolationLevel);
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
			if (_sessionHolder.LocalSession == null) 
			{
				throw new DataMapperException("SqlMap could not invoke CommitTransaction(). No Transaction was started. Call BeginTransaction() first.");
			}
			try
			{
				IDalSession session = _sessionHolder.LocalSession;
				session.CommitTransaction();
			} 
			finally 
			{
				_sessionHolder.Dispose();
			}
		}

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		/// <param name="closeConnection">Close the connection</param>
		public void CommitTransaction(bool closeConnection)
		{
			if (_sessionHolder.LocalSession == null) 
			{
				throw new DataMapperException("SqlMap could not invoke CommitTransaction(). No Transaction was started. Call BeginTransaction() first.");
			}
			try
			{
				IDalSession session = _sessionHolder.LocalSession;
				session.CommitTransaction(closeConnection);
			} 
			finally 
			{
				if (closeConnection)
				{
					_sessionHolder.Dispose();
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
			if (_sessionHolder.LocalSession == null) 
			{
				throw new DataMapperException("SqlMap could not invoke RollBackTransaction(). No Transaction was started. Call BeginTransaction() first.");
			}
			try
			{
				IDalSession session = _sessionHolder.LocalSession;
				session.RollBackTransaction();			
			} 
			finally 
			{
				_sessionHolder.Dispose();
			}
		}

		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		/// <param name="closeConnection">Close the connection</param>
		public void RollBackTransaction(bool closeConnection)
		{
			if (_sessionHolder.LocalSession == null) 
			{
				throw new DataMapperException("SqlMap could not invoke RollBackTransaction(). No Transaction was started. Call BeginTransaction() first.");
			}
			try
			{
				IDalSession session = _sessionHolder.LocalSession;
				session.RollBackTransaction(closeConnection);			
			} 
			finally 
			{
				if (closeConnection)
				{
					_sessionHolder.Dispose();
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
			IDalSession session = _sessionHolder.LocalSession;
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
			IDalSession session = _sessionHolder.LocalSession;
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
			IDalSession session = _sessionHolder.LocalSession;
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
				throw;
			}
			finally
			{
				if ( isSessionLocal )
				{
					session.CloseConnection();
				}
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
			IDalSession session = _sessionHolder.LocalSession;
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
			IDalSession session = _sessionHolder.LocalSession;
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
			IDalSession session = _sessionHolder.LocalSession;
 
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
		/// Runs a query for list with a custom object that gets a chance to deal 
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
			IDalSession session = _sessionHolder.LocalSession;
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


		/// <summary>
		/// Runs a query with a custom object that gets a chance to deal 
		/// with each row as it is processed.
		/// <p/>
		///  The parameter object is generally used to supply the input
		/// data for the WHERE clause parameter(s) of the SELECT statement.
		/// </summary>
		/// <param name="statementName">The name of the sql statement to execute.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="keyProperty">The property of the result object to be used as the key.</param>
		/// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
		/// <param name="rowDelegate"></param>
		/// <returns>A IDictionary (Hashtable) of object containing the rows keyed by keyProperty.</returns>
		///<exception cref="DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
		public IDictionary QueryForMapWithRowDelegate(string statementName, object parameterObject, string keyProperty, string valueProperty, DictionaryRowDelegate rowDelegate)
		{
			bool isSessionLocal = false;
			IDalSession session = _sessionHolder.LocalSession;
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
				map = statement.ExecuteQueryForMapWithRowDelegate(session, parameterObject, keyProperty, valueProperty, rowDelegate);
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

			return map;
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
			IDalSession session = _sessionHolder.LocalSession;
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
			IDalSession session = _sessionHolder.LocalSession;
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
			IDalSession session = _sessionHolder.LocalSession;
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
		public IMappedStatement GetMappedStatement(string name) 
		{
			if (_mappedStatements.Contains(name) == false) 
			{
				throw new DataMapperException("This SQL map does not contain a MappedStatement named " + name);
			}
			return (IMappedStatement) _mappedStatements[name];
		}

		/// <summary>
		/// Adds a (named) MappedStatement.
		/// </summary>
		/// <param name="key"> The key name</param>
		/// <param name="mappedStatement">The statement to add</param>
		internal void AddMappedStatement(string key, IMappedStatement mappedStatement) 
		{
			if (_mappedStatements.Contains(key) == true) 
			{
				throw new DataMapperException("This SQL map already contains a MappedStatement named " + mappedStatement.Id);
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
		/// The DataSource
		/// </summary>
		public DataSource DataSource
		{
			get { return  _dataSource; }
			set { _dataSource = value; }
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
			StringBuilder buffer = new StringBuilder();
			buffer.Append(Environment.NewLine);
			buffer.Append("Cache Data Statistics");
			buffer.Append(Environment.NewLine);
			buffer.Append("=====================");
			buffer.Append(Environment.NewLine);

			IDictionaryEnumerator enumerator = _mappedStatements.GetEnumerator();
			while (enumerator.MoveNext()) 
			{
				IMappedStatement mappedStatement = (IMappedStatement)enumerator.Value;

				buffer.Append(mappedStatement.Id);
				buffer.Append(": ");

				if (mappedStatement is CachingStatement)
				{
					double hitRatio = ((CachingStatement)mappedStatement).GetDataCacheHitRatio();
					if (hitRatio != -1) 
					{
						buffer.Append(Math.Round(hitRatio * 100));
						buffer.Append("%");
					} 
					else 
					{
						// this statement has a cache but it hasn't been accessed yet
						// buffer.Append("Cache has not been accessed."); ???
						buffer.Append("No Cache.");
					}
				}
				else
				{
					buffer.Append("No Cache.");
				}

				buffer.Append(Environment.NewLine);
			}

			return buffer.ToString();
		}

		#endregion

		#endregion
	}
}
