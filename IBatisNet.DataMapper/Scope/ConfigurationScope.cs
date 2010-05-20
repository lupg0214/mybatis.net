
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
using System.Configuration;
using System.Xml;
using IBatisNet.Common;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.TypeHandlers;

#endregion

namespace IBatisNet.DataMapper.Scope
{
	/// <summary>
	/// The ConfigurationScope maintains the state of the build process.
	/// </summary>
	public class ConfigurationScope
	{
		#region Fields
		
		private ErrorContext _errorContext = null;
		private HybridDictionary _providers = new HybridDictionary();
		private NameValueCollection _properties = new NameValueCollection();

		private XmlDocument _sqlMapConfigDocument = null;
		private XmlDocument _sqlMapDocument = null;
		private XmlNode _nodeContext = null;

		private bool _useConfigFileWatcher = false;
		private bool _useStatementNamespaces = false;
		private bool _isCacheModelsEnabled = false;
		private bool _useEmbedStatementParams = false;
		private bool _validateSqlMap = false;
		private bool _validateSqlMapConfig = true;
		private bool _isCallFromDao = false;

		private SqlMapper _sqlMapper = null;
		private string _sqlMapNamespace = null;
		private DataSource _dataSource = null;
		private bool _isXmlValid = true;
		private XmlNamespaceManager _nsmgr = null;
		private HybridDictionary _cacheModelFlushOnExecuteStatements = new HybridDictionary();

		#endregion
	
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public ConfigurationScope()
		{
			_errorContext = new ErrorContext();

			_providers.Clear();
		}
		#endregion 

		#region Properties

		/// <summary>
		/// XmlNamespaceManager
		/// </summary>
		public XmlNamespaceManager XmlNamespaceManager
		{
			set { _nsmgr = value; }
			get { return _nsmgr; }
		}

		/// <summary>
		/// Indicates whether or not to validate the configuration document
		/// </summary>
		public bool ValidateSqlMapConfig
		{
			set { _validateSqlMapConfig = value; }
			get { return _validateSqlMapConfig; }
		}
		
		/// <summary>
		/// Set if theparser should validate the sqlMap documents
		/// </summary>
		public bool ValidateSqlMap
		{
			set { _validateSqlMap = value; }
			get { return _validateSqlMap; }
		}

		/// <summary>
		/// Tells us if the xml configuration file validate the schema 
		/// </summary>
		public bool IsXmlValid
		{
			set { _isXmlValid = value; }
			get { return _isXmlValid; }
		}


		/// <summary>
		/// The current SqlMap namespace.
		/// </summary>
		public string SqlMapNamespace
		{
			set { _sqlMapNamespace = value; }
			get { return _sqlMapNamespace; }
		}

		/// <summary>
		/// The SqlMapper we are building.
		/// </summary>
		public SqlMapper SqlMapper
		{
			set { _sqlMapper = value; }
			get { return _sqlMapper; }
		}

		/// <summary>
		/// The current TypeHandlerFactory
		/// </summary>
		internal TypeHandlerFactory TypeHandlerFactory
		{
			get { return _sqlMapper.TypeHandlerFactory; }
		}

		/// <summary>
		/// Tell us if we are in a DataAccess context.
		/// </summary>
		public bool IsCallFromDao
		{
			set { _isCallFromDao = value; }
			get { return _isCallFromDao; }
		}

		/// <summary>
		/// Tell us if we cache model is enabled.
		/// </summary>
		public bool IsCacheModelsEnabled
		{
			set { _isCacheModelsEnabled = value; }
			get { return _isCacheModelsEnabled; }
		}

		/// <summary>
		/// External data source
		/// </summary>
		public DataSource DataSource
		{
			set { _dataSource = value; }
			get { return _dataSource; }
		}

		/// <summary>
		/// The current context node we are analizing
		/// </summary>
		public XmlNode NodeContext
		{
			set { _nodeContext = value; }
			get { return _nodeContext; }
		}

		/// <summary>
		/// The XML SqlMap config file
		/// </summary>
		public XmlDocument SqlMapConfigDocument
		{
			set { _sqlMapConfigDocument = value; }
			get { return _sqlMapConfigDocument; }
		}

		/// <summary>
		/// A XML SqlMap file
		/// </summary>
		public XmlDocument SqlMapDocument
		{
			set { _sqlMapDocument = value; }
			get { return _sqlMapDocument; }
		}

		/// <summary>
		/// Tell us if we use Configuration File Watcher
		/// </summary>
		public bool UseConfigFileWatcher
		{
			set { _useConfigFileWatcher = value; }
			get { return _useConfigFileWatcher; }
		}

		/// <summary>
		/// Tell us if we use statements namespaces
		/// </summary>
		public bool UseStatementNamespaces
		{
			set { _useStatementNamespaces = value; }
			get { return _useStatementNamespaces; }
		}
		
		/// <summary>
		///  Get the request's error context
		/// </summary>
		public ErrorContext ErrorContext
		{
			get { return _errorContext; }
		}

		/// <summary>
		///  List of providers
		/// </summary>
		public HybridDictionary Providers
		{
			get { return _providers; }
		}

		/// <summary>
		///  List of global properties
		/// </summary>
		public NameValueCollection Properties
		{
			get { return _properties; }
		}

		/// <summary>
		/// Indicates if parameters should be embedded in the sql statement.
		/// </summary>
		public bool UseEmbedStatementParams
		{
			get { return _useEmbedStatementParams; }
			set { _useEmbedStatementParams = value; }
		}

		/// <summary>
		/// Temporary storage for mapping cache model ids (key is System.String) to statements (value is IList which contains IMappedStatements).
		/// </summary>
		public HybridDictionary CacheModelFlushOnExecuteStatements
		{
			get { return _cacheModelFlushOnExecuteStatements; }
			set { _cacheModelFlushOnExecuteStatements = value; }
		}

		#endregion 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clazz">Type of the ResultMap</param>
		/// <param name="propertyName">Property name to map</param>
		/// <param name="clrType"></param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public ITypeHandler ResolveTypeHandler(Type clazz, string propertyName, string clrType, string dbType)
		{
			ITypeHandler handler = null;
			if (clazz==null)
			{
				handler = this.TypeHandlerFactory.GetUnkownTypeHandler();
			}
			else if (typeof(IDictionary).IsAssignableFrom(clazz)) 
			{
				// IDictionary
				if (clrType ==null ||clrType.Length == 0) 
				{
					handler = this.TypeHandlerFactory.GetUnkownTypeHandler(); 
				} 
				else 
				{
					try 
					{
						Type type = Resources.TypeForName(clrType);
						handler = this.TypeHandlerFactory.GetTypeHandler(type, dbType);
					} 
					catch (Exception e) 
					{
						throw new ConfigurationException("Error. Could not set TypeHandler.  Cause: " + e.Message, e);
					}
				}
			}
			else if (this.TypeHandlerFactory.GetTypeHandler(clazz, dbType) != null) 
			{
				// Primitive
				handler = this.TypeHandlerFactory.GetTypeHandler(clazz, dbType);
			}
			else 
			{
				// .NET object
				if (clrType ==null || clrType.Length == 0) 
				{
					Type type = ObjectProbe.GetPropertyTypeForGetter(clazz, propertyName);
					handler = this.TypeHandlerFactory.GetTypeHandler(type, dbType);
				} 
				else 
				{
					try 
					{
						Type type = Resources.TypeForName(clrType);
						handler = this.TypeHandlerFactory.GetTypeHandler(type, dbType);
					} 
					catch (Exception e) 
					{
						throw new ConfigurationException("Error. Could not set TypeHandler.  Cause: " + e.Message, e);
					}
				}
			}

			return handler;
		}

	}
}
