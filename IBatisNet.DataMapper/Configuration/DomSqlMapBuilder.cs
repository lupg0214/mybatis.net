
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
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using IBatisNet.Common;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Logging;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Xml;
using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Configuration.Cache.Fifo;
using IBatisNet.DataMapper.Configuration.Cache.Lru;
using IBatisNet.DataMapper.Configuration.Cache.Memory;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.Serializers;
using IBatisNet.DataMapper.Configuration.Sql;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements;
using IBatisNet.DataMapper.Configuration.Sql.SimpleDynamic;
using IBatisNet.DataMapper.Configuration.Sql.Static;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.TypeHandlers;

#endregion

namespace IBatisNet.DataMapper.Configuration
{
	/// <summary>
	/// Builds SqlMapper instances from a supplied resource (e.g. XML configuration file)
	/// </summary>
	public class DomSqlMapBuilder
	{
		#region Embedded resource

		// Which files must we allow to be used as Embedded Resources ?
		// - slqMap.config [No]
		// - providers.config [No]
		// - sqlMap files [yes]
		// - properties file (like Database.config) [Yes]
		// see contribution, NHibernate usage,
		// see http://www.codeproject.com/csharp/EmbeddedResourceStrings.asp
		// see http://www.devhood.com/tutorials/tutorial_details.aspx?tutorial_id=75
		#endregion

		#region Constant

		private const string PROPERTY_ELEMENT_KEY_ATTRIB = "key";
		private const string PROPERTY_ELEMENT_VALUE_ATTRIB = "value";

		/// <summary>
		/// 
		/// </summary>
		private const string DATAMAPPER_NAMESPACE_PREFIX = "mapper";
		private const string PROVIDERS_NAMESPACE_PREFIX = "provider";
		private const string MAPPING_NAMESPACE_PREFIX = "mapping";
		private const string DATAMAPPER_XML_NAMESPACE = "http://ibatis.apache.org/dataMapper";
		private const string PROVIDER_XML_NAMESPACE = "http://ibatis.apache.org/providers";
		private const string MAPPING_XML_NAMESPACE = "http://ibatis.apache.org/mapping";

		/// <summary>
		/// Default congig name
		/// </summary>
		public const string DEFAULT_FILE_CONFIG_NAME = "sqlmap.config";

		/// <summary>
		/// Default provider name
		/// </summary>
		private const string DEFAULT_PROVIDER_NAME = "_DEFAULT_PROVIDER_NAME";

		/// <summary>
		/// Dot representation
		/// </summary>
		public const string DOT = ".";

		/// <summary>
		/// Token for SqlMapConfig xml root.
		/// </summary>
		private const string XML_DATAMAPPER_CONFIG_ROOT = "sqlMapConfig";

		/// <summary>
		/// Token for xml path to SqlMapConfig settings element.
		/// </summary>
		private const string XML_CONFIG_SETTINGS = "sqlMapConfig/settings/setting";

		/// <summary>
		/// Token for providers config file name.
		/// </summary>
		private const string PROVIDERS_FILE_NAME = "providers.config";

		/// <summary>
		/// Token for xml path to SqlMapConfig providers element.
		/// </summary>
		private const string XML_CONFIG_PROVIDERS = "sqlMapConfig/providers";

		/// <summary>
		/// Token for xml path to properties elements.
		/// </summary>
		private const string XML_PROPERTIES = "properties";

		/// <summary>
		/// Token for xml path to property elements.
		/// </summary>
		private const string XML_PROPERTY = "property";

		/// <summary>
		/// Token for xml path to settings add elements.
		/// </summary>
		private const string XML_SETTINGS_ADD = "/settings/add";

		/// <summary>
		/// Token for xml path to global properties elements.
		/// </summary>
		private const string XML_GLOBAL_PROPERTIES = "*/add";

		/// <summary>
		/// Token for xml path to provider elements.
		/// </summary>
		private const string XML_PROVIDER = "providers/provider";

		/// <summary>
		/// Token for xml path to database provider elements.
		/// </summary>
		private const string XML_DATABASE_PROVIDER = "sqlMapConfig/database/provider";

		/// <summary>
		/// Token for xml path to database source elements.
		/// </summary>
		private const string XML_DATABASE_DATASOURCE = "sqlMapConfig/database/dataSource";

		/// <summary>
		/// Token for xml path to global type alias elements.
		/// </summary>
		private const string XML_GLOBAL_TYPEALIAS = "sqlMapConfig/alias/typeAlias";

		/// <summary>
		/// Token for xml path to global type alias elements.
		/// </summary>
		private const string XML_GLOBAL_TYPEHANDLER = "sqlMapConfig/typeHandlers/typeHandler";

		/// <summary>
		/// Token for xml path to sqlMap elements.
		/// </summary>
		private const string XML_SQLMAP = "sqlMapConfig/sqlMaps/sqlMap";

		/// <summary>
		/// Token for mapping xml root.
		/// </summary>
		private const string XML_MAPPING_ROOT = "sqlMap";

		/// <summary>
		/// Token for xml path to type alias elements.
		/// </summary>
		private const string XML_TYPEALIAS = "sqlMap/alias/typeAlias";

		/// <summary>
		/// Token for xml path to resultMap elements.
		/// </summary>
		private const string XML_RESULTMAP = "sqlMap/resultMaps/resultMap";

		/// <summary>
		/// Token for xml path to parameterMap elements.
		/// </summary>
		private const string XML_PARAMETERMAP = "sqlMap/parameterMaps/parameterMap";

		/// <summary>
		/// Token for xml path to statement elements.
		/// </summary>
		private const string XML_STATEMENT = "sqlMap/statements/statement";

		/// <summary>
		/// Token for xml path to select elements.
		/// </summary>
		private const string XML_SELECT = "sqlMap/statements/select";

		/// <summary>
		/// Token for xml path to insert elements.
		/// </summary>
		private const string XML_INSERT = "sqlMap/statements/insert";

		/// <summary>
		/// Token for xml path to selectKey elements.
		/// </summary>
		private const string XML_SELECTKEY= "selectKey";

		/// <summary>
		/// Token for xml path to update elements.
		/// </summary>
		private const string XML_UPDATE ="sqlMap/statements/update";

		/// <summary>
		/// Token for xml path to delete elements.
		/// </summary>
		private const string XML_DELETE ="sqlMap/statements/delete";

		/// <summary>
		/// Token for xml path to procedure elements.
		/// </summary>
		private const string XML_PROCEDURE ="sqlMap/statements/procedure";

		/// <summary>
		/// Token for xml path to cacheModel elements.
		/// </summary>
		private const string XML_CACHE_MODEL = "sqlMap/cacheModels/cacheModel";

		/// <summary>
		/// Token for xml path to flushOnExecute elements.
		/// </summary>
		private const string XML_FLUSH_ON_EXECUTE = "flushOnExecute";

		/// <summary>
		/// Token for xml path to search statement elements.
		/// </summary>
		private const string XML_SEARCH_STATEMENT ="sqlMap/statements";

		/// <summary>
		/// Token for xml path to search parameterMap elements.
		/// </summary>
		private const string XML_SEARCH_PARAMETER ="sqlMap/parameterMaps/parameterMap[@id='";

		/// <summary>
		/// Token for xml path to search resultMap elements.
		/// </summary>
		private const string XML_SEARCH_RESULTMAP ="sqlMap/resultMaps/resultMap[@id='";

		/// <summary>
		/// Token for useStatementNamespaces attribute.
		/// </summary>
		private const string ATR_USE_STATEMENT_NAMESPACES = "useStatementNamespaces";
		/// <summary>
		/// Token for cacheModelsEnabled attribute.
		/// </summary>
		private const string ATR_CACHE_MODELS_ENABLED = "cacheModelsEnabled";

		/// <summary>
		/// Token for validateSqlMap attribute.
		/// </summary>
		private const string ATR_VALIDATE_SQLMAP = "validateSqlMap";
		/// <summary>
		/// Token for embedStatementParams attribute.
		/// </summary>
		private const string ATR_EMBED_STATEMENT_PARAMS = "useEmbedStatementParams";

		#endregion

		#region Fields

		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

		private ConfigurationScope _configScope = null;
		private DeSerializerFactory _deSerializerFactory = null; 
		private InlineParameterMapParser _paramParser = null;

		#endregion 		
		
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public DomSqlMapBuilder()
		{
			_configScope = new ConfigurationScope();
			_paramParser = new InlineParameterMapParser( _configScope.ErrorContext );
			_deSerializerFactory = new DeSerializerFactory(_configScope);
		}

		/// <summary>
		/// Constructs a DomSqlMapBuilder 
		/// with or without configuration document validation using the 
		/// SqlMapConfig schema.
		/// </summary>
		/// <param name="validateSqlMapConfig">
		/// Specify whether the configuration Xml document should be 
		/// validated with the SqlMapConfig schema.
		/// </param>
		public DomSqlMapBuilder(bool validateSqlMapConfig)
		{
			_configScope = new ConfigurationScope();
			_configScope.ValidateSqlMapConfig = validateSqlMapConfig;
			_deSerializerFactory = new DeSerializerFactory(_configScope);
			_paramParser = new InlineParameterMapParser( _configScope.ErrorContext );
		}		
		#endregion 

		#region Configure

		/// <summary>
		/// Configure a SqlMapper from default resource file named SqlMap.config.
		/// </summary>
		/// <returns>An SqlMap</returns>
		/// <remarks>The file path is relative to the application root.</remarks>
		public SqlMapper Configure()
		{
			return Configure( Resources.GetConfigAsXmlDocument(DomSqlMapBuilder.DEFAULT_FILE_CONFIG_NAME) );
		}


		/// <summary>
		/// Configure an SqlMap.
		/// </summary>
		/// <param name="document">An xml sql map configuration document.</param>
		/// <returns>the SqlMap</returns>
		public SqlMapper Configure( XmlDocument document )
		{
			return Build( document, false );
		}


		/// <summary>
		/// Configure a SqlMapper from a file path.
		/// </summary>
		/// <param name="resource">
		/// A relative ressource path from your Application root 
		/// or a absolue file path file:\\c:\dir\a.config
		/// </param>
		/// <returns>An SqlMap</returns>
		public SqlMapper Configure(string resource)
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
			return Build( document, false);
		}


		/// <summary>
		///  Configure a SqlMapper from a stream.
		/// </summary>
		/// <param name="resource">A stream resource</param>
		/// <returns>An SqlMap</returns>
		public SqlMapper Configure(Stream resource)
		{
			XmlDocument document = Resources.GetStreamAsXmlDocument( resource );
			return Build( document, false);
		}


		/// <summary>
		///  Configure a SqlMapper from a FileInfo.
		/// </summary>
		/// <param name="resource">A FileInfo resource</param>
		/// <returns>An SqlMap</returns>
		public SqlMapper Configure(FileInfo resource)
		{
			XmlDocument document = Resources.GetFileInfoAsXmlDocument( resource );
			return Build( document, false);
		}


		/// <summary>
		///  Configure a SqlMapper from an Uri.
		/// </summary>
		/// <param name="resource">A Uri resource</param>
		/// <returns></returns>
		public SqlMapper Configure(Uri resource)
		{
			XmlDocument document = Resources.GetUriAsXmlDocument( resource );
			return Build( document, false);
		}


		/// <summary>
		/// Configure and monitor the default configuration file for modifications 
		/// and automatically reconfigure SqlMap. 
		/// </summary>
		/// <returns>An SqlMap</returns>
		public SqlMapper ConfigureAndWatch(ConfigureHandler configureDelegate)
		{
			return ConfigureAndWatch( DEFAULT_FILE_CONFIG_NAME, configureDelegate ) ;
		}


		/// <summary>
		/// Configure and monitor the configuration file for modifications 
		/// and automatically reconfigure SqlMap. 
		/// </summary>
		/// <param name="resource">
		/// A relative ressource path from your Application root 
		/// or an absolue file path file:\\c:\dir\a.config
		/// </param>
		///<param name="configureDelegate">
		/// Delegate called when the file has changed, to rebuild the dal.
		/// </param>
		/// <returns>An SqlMap</returns>
		public SqlMapper ConfigureAndWatch( string resource, ConfigureHandler configureDelegate )
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

			return Build( document, true );
		}


		/// <summary>
		/// Configure and monitor the configuration file for modifications 
		/// and automatically reconfigure SqlMap. 
		/// </summary>
		/// <param name="resource">
		/// A FileInfo to your config file.
		/// </param>
		///<param name="configureDelegate">
		/// Delegate called when the file has changed, to rebuild the dal.
		/// </param>
		/// <returns>An SqlMap</returns>
		public SqlMapper ConfigureAndWatch( FileInfo resource, ConfigureHandler configureDelegate )
		{
			XmlDocument document = Resources.GetFileInfoAsXmlDocument(resource);

			ConfigWatcherHandler.ClearFilesMonitored();
			ConfigWatcherHandler.AddFileToWatch( resource );

			TimerCallback callBakDelegate = new TimerCallback( DomSqlMapBuilder.OnConfigFileChange );

			StateConfig state = new StateConfig();
			state.FileName = resource.FullName;
			state.ConfigureHandler = configureDelegate;

			new ConfigWatcherHandler( callBakDelegate, state );

			return Build( document, true );
		}


		/// <summary>
		/// Callback called when the SqlMap.config changed.
		/// </summary>
		/// <param name="obj">The state config.</param>
		public static void OnConfigFileChange(object obj)
		{
			StateConfig state = (StateConfig)obj;
			state.ConfigureHandler( null );
		}

		#endregion 

		#region Methods

		/// <summary>
		/// Build a SqlMapper instance
		/// </summary>
		/// <param name="document">An xml configuration document</param>
		/// <param name="dataSource">A data source</param>
		/// <param name="useConfigFileWatcher"></param>
		/// <param name="isCallFromDao"></param>
		/// <returns>return an a SqlMapper instance</returns>
		private SqlMapper Build(XmlDocument document,DataSource dataSource, 
			bool useConfigFileWatcher, bool isCallFromDao)
		{
			_configScope.SqlMapConfigDocument = document;
			_configScope.DataSource = dataSource;
			_configScope.IsCallFromDao = isCallFromDao;
			_configScope.UseConfigFileWatcher = useConfigFileWatcher;
			
			_configScope.XmlNamespaceManager = new XmlNamespaceManager(_configScope.SqlMapConfigDocument.NameTable);
			_configScope.XmlNamespaceManager.AddNamespace(DATAMAPPER_NAMESPACE_PREFIX, DATAMAPPER_XML_NAMESPACE);
			_configScope.XmlNamespaceManager.AddNamespace(PROVIDERS_NAMESPACE_PREFIX, PROVIDER_XML_NAMESPACE);
			_configScope.XmlNamespaceManager.AddNamespace(MAPPING_NAMESPACE_PREFIX, MAPPING_XML_NAMESPACE);

			try
			{
				if (_configScope.ValidateSqlMapConfig) 
				{
					ValidateSchema( document.ChildNodes[1], "SqlMapConfig.xsd" );
				}
				Initialize();
				return _configScope.SqlMapper;
			}
			catch(Exception e)
			{	
				throw new ConfigurationException(_configScope.ErrorContext.ToString(), e);
			}
		}

		/// <summary>
		/// validate againts schema
		/// </summary>
		/// <param name="section">The doc to validate</param>
		/// <param name="schemaFileName">schema File Name</param>
		private void ValidateSchema( XmlNode section, string schemaFileName )
		{
			XmlValidatingReader validatingReader = null;
			Stream xsdFile = null; 

			_configScope.ErrorContext.Activity = "Validate SqlMap config";
			try
			{
				//Validate the document using a schema
				validatingReader = new XmlValidatingReader( new XmlTextReader( new StringReader( section.OuterXml ) ) );
				validatingReader.ValidationType = ValidationType.Schema;

				xsdFile = GetStream( schemaFileName ); 

				if (xsdFile == null)
				{
					// TODO: avoid using hard-coded value "IBatisNet.DataMapper"
					throw new ConfigurationException( "Unable to locate embedded resource [IBatisNet.DataMapper."+schemaFileName+"]. If you are building from source, verfiy the file is marked as an embedded resource.");
				}
				
				XmlSchema xmlSchema = XmlSchema.Read( xsdFile, new ValidationEventHandler(ValidationCallBack) );

				validatingReader.Schemas.Add(xmlSchema);

				// Wire up the call back.  The ValidationEvent is fired when the
				// XmlValidatingReader hits an issue validating a section of the xml
				validatingReader.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

				// Validate the document
				while (validatingReader.Read()){}

				if(! _configScope.IsXmlValid )
				{
					throw new ConfigurationException( "Invalid SqlMap.config document. cause :"+_configScope.ErrorContext.Resource);
						//( Resource.ResourceManager.FormatMessage( Resource.MessageKeys.DocumentNotValidated, _schemaErrors )) );
				}
			}
			finally
			{
				if( validatingReader != null ) validatingReader.Close();
				if( xsdFile != null ) xsdFile.Close();
			}
		}


		private void ValidationCallBack( object sender, ValidationEventArgs args )
		{
			_configScope.IsXmlValid = false;
			_configScope.ErrorContext.Resource += args.Message + Environment.NewLine;
		}

		/// <summary>
		/// Load statement, parameters, resultmap.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="dataSource"></param>
		/// <param name="useConfigFileWatcher"></param>
		/// <param name="properties"></param>
		/// <returns></returns>
		/// <remarks>Used by Dao</remarks>
		public SqlMapper Build(XmlDocument document, DataSource dataSource, bool useConfigFileWatcher, NameValueCollection properties)
		{
			_configScope.Properties.Add(properties);
			return Build(document, dataSource, useConfigFileWatcher, true);
		}


		/// <summary>
		/// Load SqlMap configuration from
		/// from the XmlDocument passed in parameter.
		/// </summary>
		/// <param name="document">The xml sql map configuration.</param>
		/// <param name="useConfigFileWatcher"></param>
		public SqlMapper Build(XmlDocument document, bool useConfigFileWatcher)
		{
			return Build(document, null, useConfigFileWatcher, false);
		}


		/// <summary>
		/// Reset PreparedStatements cache
		/// </summary>
		private void Reset()
		{
		}

		/// <summary>
		/// Intialize an SqlMap.
		/// </summary>
		private void Initialize()
		{
			Reset();
			
			_configScope.SqlMapper = new SqlMapper( new TypeHandlerFactory() );

			#region Cache Alias

			TypeAlias cacheAlias = new TypeAlias(typeof(MemoryCacheControler));
			cacheAlias.Name = "MEMORY";
			_configScope.SqlMapper.TypeHandlerFactory.AddTypeAlias(cacheAlias.Name, cacheAlias);
			cacheAlias = new TypeAlias(typeof(LruCacheController));
			cacheAlias.Name = "LRU";
			_configScope.SqlMapper.TypeHandlerFactory.AddTypeAlias(cacheAlias.Name, cacheAlias);
			cacheAlias = new TypeAlias(typeof(FifoCacheController));
			cacheAlias.Name = "FIFO";
			_configScope.SqlMapper.TypeHandlerFactory.AddTypeAlias(cacheAlias.Name, cacheAlias);

			#endregion 

			#region Load Global Properties
			if (_configScope.IsCallFromDao == false)
			{
				_configScope.NodeContext = _configScope.SqlMapConfigDocument.SelectSingleNode( ApplyDataMapperNamespacePrefix(XML_DATAMAPPER_CONFIG_ROOT), _configScope.XmlNamespaceManager);

				ParseGlobalProperties();
			}
			#endregion

			#region Load settings

			_configScope.ErrorContext.Activity = "loading global settings";

			XmlNodeList settings = _configScope.SqlMapConfigDocument.SelectNodes( ApplyDataMapperNamespacePrefix(XML_CONFIG_SETTINGS), _configScope.XmlNamespaceManager);
				//XML_CONFIG_SETTINGS);

			if (settings!=null)
			{
				foreach (XmlNode setting in settings)
				{
					if (setting.Attributes[ATR_USE_STATEMENT_NAMESPACES] != null )
					{	
						string value = NodeUtils.ParsePropertyTokens(setting.Attributes[ATR_USE_STATEMENT_NAMESPACES].Value, _configScope.Properties);
						_configScope.UseStatementNamespaces =  Convert.ToBoolean( value ); 
					}
					if (setting.Attributes[ATR_CACHE_MODELS_ENABLED] != null )
					{		
						string value = NodeUtils.ParsePropertyTokens(setting.Attributes[ATR_CACHE_MODELS_ENABLED].Value, _configScope.Properties);
						_configScope.IsCacheModelsEnabled =  Convert.ToBoolean( value ); 
					}
					if (setting.Attributes[ATR_EMBED_STATEMENT_PARAMS] != null )
					{		
						string value = NodeUtils.ParsePropertyTokens(setting.Attributes[ATR_EMBED_STATEMENT_PARAMS].Value, _configScope.Properties);
						_configScope.UseEmbedStatementParams =  Convert.ToBoolean( value ); 
					}

					if (setting.Attributes[ATR_VALIDATE_SQLMAP] != null )
					{		
						string value = NodeUtils.ParsePropertyTokens(setting.Attributes[ATR_VALIDATE_SQLMAP].Value, _configScope.Properties);
						_configScope.ValidateSqlMap =  Convert.ToBoolean( value ); 
					}
				}
			}

			_configScope.SqlMapper.SetCacheModelsEnabled(_configScope.IsCacheModelsEnabled);
			_configScope.SqlMapper.SetUseEmbedStatementParams(_configScope.UseEmbedStatementParams);

			#endregion

			#region Load providers
			if (_configScope.IsCallFromDao == false)
			{
				GetProviders();
			}
			#endregion

			#region Load DataBase
			#region Choose the  provider
			Provider provider = null;
			if ( _configScope.IsCallFromDao==false )
			{
				provider = ParseProvider();
				_configScope.ErrorContext.Reset();
			}
			#endregion

			#region Load the DataSources

			_configScope.ErrorContext.Activity = "loading Database DataSource";
			XmlNode nodeDataSource = _configScope.SqlMapConfigDocument.SelectSingleNode( ApplyDataMapperNamespacePrefix(XML_DATABASE_DATASOURCE), _configScope.XmlNamespaceManager );
			
			if (nodeDataSource == null)
			{
				if (_configScope.IsCallFromDao == false)
				{
					throw new ConfigurationException("There's no dataSource tag in SqlMap.config.");
				}
				else  // patch from Luke Yang
				{
					_configScope.SqlMapper.DataSource = _configScope.DataSource;
				}
			}
			else
			{
				if (_configScope.IsCallFromDao == false)
				{
					_configScope.ErrorContext.Resource = nodeDataSource.OuterXml.ToString();
					_configScope.ErrorContext.MoreInfo = "parse DataSource";

					DataSource dataSource = DataSourceDeSerializer.Deserialize( nodeDataSource );

					dataSource.Provider = provider;
					dataSource.ConnectionString = NodeUtils.ParsePropertyTokens(dataSource.ConnectionString, _configScope.Properties);

					_configScope.DataSource = dataSource;
					_configScope.SqlMapper.DataSource = _configScope.DataSource;
				}
				else
				{
					_configScope.SqlMapper.DataSource = _configScope.DataSource;
				}
				_configScope.ErrorContext.Reset();
			}
			#endregion
			#endregion

			#region Load Global TypeAlias
			foreach (XmlNode xmlNode in _configScope.SqlMapConfigDocument.SelectNodes( ApplyDataMapperNamespacePrefix(XML_GLOBAL_TYPEALIAS), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.Activity = "loading global Type alias";
				TypeAliasDeSerializer.Deserialize(xmlNode, _configScope);
			}
			_configScope.ErrorContext.Reset();
			#endregion

			#region Load TypeHandlers
			foreach (XmlNode xmlNode in _configScope.SqlMapConfigDocument.SelectNodes( ApplyDataMapperNamespacePrefix(XML_GLOBAL_TYPEHANDLER), _configScope.XmlNamespaceManager))
			{
				try
				{
					_configScope.ErrorContext.Activity = "loading typeHandler";
					TypeHandlerDeSerializer.Deserialize( xmlNode, _configScope );
				} 
				catch (Exception e) 
				{
					NameValueCollection prop = NodeUtils.ParseAttributes(xmlNode, _configScope.Properties);

					throw new ConfigurationException(
						String.Format("Error registering TypeHandler class \"{0}\" for handling .Net type \"{1}\" and dbType \"{2}\". Cause: {3}", 
						NodeUtils.GetStringAttribute(prop, "callback"),
						NodeUtils.GetStringAttribute(prop, "type"),
						NodeUtils.GetStringAttribute(prop, "dbType"),
						e.Message), e);
				}
			}
			_configScope.ErrorContext.Reset();
			#endregion

			#region Load sqlMap mapping files
			
			foreach (XmlNode xmlNode in _configScope.SqlMapConfigDocument.SelectNodes( ApplyDataMapperNamespacePrefix(XML_SQLMAP), _configScope.XmlNamespaceManager))
			{
				_configScope.NodeContext = xmlNode;
				ConfigureSqlMap();
			}

			#endregion

			#region Attach CacheModel to statement

			if (_configScope.IsCacheModelsEnabled)
			{
				foreach(DictionaryEntry entry in _configScope.SqlMapper.MappedStatements)
				{
					_configScope.ErrorContext.Activity = "Set CacheModel to statement";

					IMappedStatement mappedStatement = (IMappedStatement)entry.Value;
					if (mappedStatement.Statement.CacheModelName.Length >0)
					{
						_configScope.ErrorContext.MoreInfo = "statement : "+mappedStatement.Statement.Id;
						_configScope.ErrorContext.Resource = "cacheModel : " +mappedStatement.Statement.CacheModelName;
						mappedStatement.Statement.CacheModel = _configScope.SqlMapper.GetCache(mappedStatement.Statement.CacheModelName);
					}
				}
			}
			_configScope.ErrorContext.Reset();
			#endregion 

			#region Register Trigger Statements for Cache Models
			foreach (DictionaryEntry entry in _configScope.CacheModelFlushOnExecuteStatements)
			{
				string cacheModelId = (string)entry.Key;
				IList statementsToRegister = (IList)entry.Value;

				if (statementsToRegister != null && statementsToRegister.Count > 0)
				{
					foreach (string statementName in statementsToRegister)
					{
						CacheModel cacheModel = _configScope.SqlMapper.GetCache(cacheModelId);
						IMappedStatement mappedStatement = (IMappedStatement)_configScope.SqlMapper.MappedStatements[statementName];

						if (_logger.IsDebugEnabled)
						{
							_logger.Debug("Registering trigger statement [" + mappedStatement.Id + "] to cache model [" + cacheModel.Id + "]");
						}

						cacheModel.RegisterTriggerStatement(mappedStatement);
					}
				}
			}
			#endregion

			#region Resolve "resultMap" attribute on Result Property + initialize Discriminator property 

			foreach(DictionaryEntry entry in _configScope.SqlMapper.ResultMaps)
			{
				_configScope.ErrorContext.Activity = "Resolve 'resultMap' attribute on Result Property";

				ResultMap resultMap = (ResultMap)entry.Value;
				foreach(DictionaryEntry item in resultMap.ColumnsToPropertiesMap)
				{
					ResultProperty result = (ResultProperty)item.Value;
					if(result.NestedResultMapName.Length >0)
					{
						result.NestedResultMap = _configScope.SqlMapper.GetResultMap(result.NestedResultMapName);
					}
				}
				if (resultMap.Discriminator != null)
				{
					resultMap.Discriminator.Initialize(_configScope);
				}
			}

			_configScope.ErrorContext.Reset();

			#endregion

		}

		/// <summary>
		/// Load and initialize providers from specified file.
		/// </summary>
		private void GetProviders()
		{
			Provider provider = null;
			XmlDocument xmlProviders = null;

			_configScope.ErrorContext.Activity = "loading Providers";

			XmlNode providersNode = null;
			providersNode = _configScope.SqlMapConfigDocument.SelectSingleNode( ApplyDataMapperNamespacePrefix(XML_CONFIG_PROVIDERS), _configScope.XmlNamespaceManager);

			if (providersNode != null )
			{
				xmlProviders = Resources.GetAsXmlDocument( providersNode, _configScope.Properties );
			}
			else
			{
				xmlProviders = Resources.GetConfigAsXmlDocument(PROVIDERS_FILE_NAME);
			}

			foreach (XmlNode node in xmlProviders.SelectNodes( ApplyProviderNamespacePrefix(XML_PROVIDER), _configScope.XmlNamespaceManager ) )
			{
				_configScope.ErrorContext.Resource = node.InnerXml.ToString();
				provider = ProviderDeSerializer.Deserialize(node);

				if (provider.IsEnabled == true)
				{
					_configScope.ErrorContext.ObjectId = provider.Name;
					_configScope.ErrorContext.MoreInfo = "initialize provider";

					provider.Initialize();
					_configScope.Providers.Add(provider.Name, provider);

					if (provider.IsDefault == true)
					{
						if (_configScope.Providers[DEFAULT_PROVIDER_NAME] == null) 
						{
							_configScope.Providers.Add(DEFAULT_PROVIDER_NAME,provider);
						} 
						else 
						{
							throw new ConfigurationException(
								string.Format("Error while configuring the Provider named \"{0}\" There can be only one default Provider.",provider.Name));
						}
					}
				}
			}
			_configScope.ErrorContext.Reset();
		}


		/// <summary>
		/// Parse the provider tag.
		/// </summary>
		/// <returns>A provider object.</returns>
		private Provider ParseProvider()
		{
			_configScope.ErrorContext.Activity = "load DataBase Provider";
			XmlNode node = _configScope.SqlMapConfigDocument.SelectSingleNode( ApplyDataMapperNamespacePrefix(XML_DATABASE_PROVIDER), _configScope.XmlNamespaceManager  );

			if (node != null)
			{
				_configScope.ErrorContext.Resource = node.OuterXml.ToString();
				string providerName = NodeUtils.ParsePropertyTokens(node.Attributes["name"].Value, _configScope.Properties);

				_configScope.ErrorContext.ObjectId = providerName;

				if (_configScope.Providers.Contains(providerName) == true)
				{
					return (Provider) _configScope.Providers[providerName];
				}
				else
				{
					throw new ConfigurationException(
						string.Format("Error while configuring the Provider named \"{0}\". Cause : The provider is not in 'providers.config'.",
							providerName));
				}
			}
			else
			{
				if (_configScope.Providers.Contains(DEFAULT_PROVIDER_NAME) == true)
				{
					return (Provider) _configScope.Providers[DEFAULT_PROVIDER_NAME];
				}
				else
				{
					throw new ConfigurationException(
						string.Format("Error while configuring the SqlMap. There is no provider marked default in 'providers.config' file."));
				}
			}
		}


		/// <summary>
		/// Load sqlMap statement.
		/// </summary>
		private void ConfigureSqlMap( )
		{
			XmlNode sqlMapNode = _configScope.NodeContext;

			_configScope.ErrorContext.Activity = "loading SqlMap";
			_configScope.ErrorContext.Resource = sqlMapNode.OuterXml.ToString();

			if (_configScope.UseConfigFileWatcher == true)
			{
				if (sqlMapNode.Attributes["resource"] != null || sqlMapNode.Attributes["url"] != null) 
				{ 
					ConfigWatcherHandler.AddFileToWatch( Resources.GetFileInfo( Resources.GetValueOfNodeResourceUrl(sqlMapNode, _configScope.Properties) ) );
				}
			}

			// Load the file 
			_configScope.SqlMapDocument = Resources.GetAsXmlDocument(sqlMapNode, _configScope.Properties);
			
			if (_configScope.ValidateSqlMap)
			{
				ValidateSchema( _configScope.SqlMapDocument.ChildNodes[1], "SqlMap.xsd" );
			}

			_configScope.SqlMapNamespace = _configScope.SqlMapDocument.SelectSingleNode( ApplyMappingNamespacePrefix(XML_MAPPING_ROOT), _configScope.XmlNamespaceManager ).Attributes["namespace"].Value;

			#region Load TypeAlias

			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_TYPEALIAS), _configScope.XmlNamespaceManager))
			{
				TypeAliasDeSerializer.Deserialize(xmlNode, _configScope);
			}
			_configScope.ErrorContext.MoreInfo = string.Empty;
			_configScope.ErrorContext.ObjectId = string.Empty;

			#endregion

			#region Load resultMap

			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_RESULTMAP), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading ResultMap tag";
				_configScope.NodeContext = xmlNode; // A ResultMap node

				BuildResultMap();
			}

			#endregion

			#region Load parameterMaps

			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_PARAMETERMAP), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading ParameterMap tag";
				_configScope.NodeContext = xmlNode; // A ParameterMap node

				BuildParameterMap();
			}

			#endregion
		
			#region Load statements

			#region Statement tag
			Statement statement = null;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_STATEMENT), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading statement tag";
				_configScope.NodeContext = xmlNode; // A statement tag

				MappedStatement mappedStatement = null;

				statement = StatementDeSerializer.Deserialize(xmlNode, _configScope);
				statement.CacheModelName = ApplyNamespace( statement.CacheModelName );
				statement.ParameterMapName = ApplyNamespace( statement.ParameterMapName );
				statement.ResultMapName = ApplyNamespace( statement.ResultMapName );

				if (_configScope.UseStatementNamespaces == true)
				{
					statement.Id = ApplyNamespace(statement.Id);
				}
				_configScope.ErrorContext.ObjectId = statement.Id;
				statement.Initialize( _configScope );

				// Build ISql (analyse sql statement)		
				ProcessSqlStatement( statement  );

				// Build MappedStatement
				mappedStatement = new MappedStatement( _configScope.SqlMapper, statement);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mappedStatement);
			}
			#endregion

			#region Select tag
			Select select = null;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_SELECT), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading select tag";
				_configScope.NodeContext = xmlNode; // A select node

				select = SelectDeSerializer.Deserialize(xmlNode, _configScope);
				select.CacheModelName = ApplyNamespace( select.CacheModelName );
				select.ParameterMapName = ApplyNamespace( select.ParameterMapName );
				select.ResultMapName = ApplyNamespace( select.ResultMapName );

				if (_configScope.UseStatementNamespaces == true)
				{
					select.Id = ApplyNamespace(select.Id);
				}
				_configScope.ErrorContext.ObjectId = select.Id;
				
				select.Initialize( _configScope );

				if (select.Generate != null)
				{
					GenerateCommandText(_configScope.SqlMapper, select);
				}
				else
				{
					// Build ISql (analyse sql statement)		
					ProcessSqlStatement( select);
				}

				// Build MappedStatement
				MappedStatement mappedStatement = new SelectMappedStatement( _configScope.SqlMapper, select);
				IMappedStatement mapStatement = mappedStatement;
				if (select.CacheModelName != null && select.CacheModelName.Length> 0 && _configScope.IsCacheModelsEnabled)
				{
					mapStatement = new CachingStatement( mappedStatement);
				}

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mapStatement);
			}
			#endregion

			#region Insert tag
			Insert insert = null;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_INSERT), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading insert tag";
				_configScope.NodeContext = xmlNode; // A insert tag

				MappedStatement mappedStatement = null;

				insert = InsertDeSerializer.Deserialize(xmlNode, _configScope);
				insert.CacheModelName = ApplyNamespace( insert.CacheModelName );
				insert.ParameterMapName = ApplyNamespace( insert.ParameterMapName );
				insert.ResultMapName = ApplyNamespace( insert.ResultMapName );

				if (_configScope.UseStatementNamespaces == true)
				{
					insert.Id = ApplyNamespace(insert.Id);
				}
				_configScope.ErrorContext.ObjectId = insert.Id;
				insert.Initialize( _configScope );

				// Build ISql (analyse sql command text)
				if (insert.Generate != null)
				{
					GenerateCommandText(_configScope.SqlMapper, insert);
				}
				else
				{
					ProcessSqlStatement( insert);
				}

				// Build MappedStatement
				mappedStatement = new InsertMappedStatement( _configScope.SqlMapper, insert);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mappedStatement);

				#region statement SelectKey
				// Set sql statement SelectKey 
				if (insert.SelectKey != null)
				{
					insert.SelectKey.Id = insert.Id;
					insert.SelectKey.Initialize( _configScope );
					insert.SelectKey.Id += DOT + "SelectKey";

					// Initialize can also use _configScope.ErrorContext.ObjectId to get the id
					// of the parent <select> node
					// insert.SelectKey.Initialize( _configScope );
					// insert.SelectKey.Id = insert.Id + DOT + "SelectKey";
					
					string commandText = xmlNode.SelectSingleNode( ApplyMappingNamespacePrefix(XML_SELECTKEY), _configScope.XmlNamespaceManager).FirstChild.InnerText.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ').Trim();
					commandText = NodeUtils.ParsePropertyTokens(commandText, _configScope.Properties);
					StaticSql sql = new StaticSql(insert.SelectKey);
					IDalSession session = new SqlMapSession( _configScope.SqlMapper.DataSource );
					sql.BuildPreparedStatement( session, commandText );
					insert.SelectKey.Sql = sql;					
					
					// Build MappedStatement
					mappedStatement = new MappedStatement( _configScope.SqlMapper, insert.SelectKey);

					_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mappedStatement);
				}
				#endregion
			}
			#endregion

			#region Update tag
			Update update = null;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_UPDATE), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading update tag";
				_configScope.NodeContext = xmlNode; // A update tag

				MappedStatement mappedStatement = null;

				update = UpdateDeSerializer.Deserialize(xmlNode, _configScope);
				update.CacheModelName = ApplyNamespace( update.CacheModelName );
				update.ParameterMapName = ApplyNamespace( update.ParameterMapName );
				update.ResultMapName = ApplyNamespace( update.ResultMapName );

				if (_configScope.UseStatementNamespaces == true)
				{
					update.Id = ApplyNamespace(update.Id);
				}
				_configScope.ErrorContext.ObjectId = update.Id;
				update.Initialize( _configScope );

				// Build ISql (analyse sql statement)	
				if (update.Generate != null)
				{
					GenerateCommandText(_configScope.SqlMapper, update);
				}
				else
				{
					// Build ISql (analyse sql statement)		
					ProcessSqlStatement(update);
				}	

				// Build MappedStatement
				mappedStatement = new UpdateMappedStatement( _configScope.SqlMapper, update);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mappedStatement);
			}
			#endregion

			#region Delete tag
			Delete delete = null;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_DELETE), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading delete tag";
				_configScope.NodeContext = xmlNode; // A delete tag
				MappedStatement mappedStatement = null;

				delete = DeleteDeSerializer.Deserialize(xmlNode, _configScope);
				delete.CacheModelName = ApplyNamespace( delete.CacheModelName );
				delete.ParameterMapName = ApplyNamespace( delete.ParameterMapName );
				delete.ResultMapName = ApplyNamespace( delete.ResultMapName );

				if (_configScope.UseStatementNamespaces == true)
				{
					delete.Id = ApplyNamespace(delete.Id);
				}
				_configScope.ErrorContext.ObjectId = delete.Id;
				delete.Initialize( _configScope );

				// Build ISql (analyse sql statement)
				if (delete.Generate != null)
				{
					GenerateCommandText(_configScope.SqlMapper, delete);
				}
				else
				{
					// Build ISql (analyse sql statement)		
					ProcessSqlStatement(delete);
				}	

				// Build MappedStatement
				mappedStatement = new DeleteMappedStatement( _configScope.SqlMapper, delete);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mappedStatement);
			}
			#endregion

			#region Procedure tag
			Procedure procedure = null;
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_PROCEDURE), _configScope.XmlNamespaceManager))
			{
				_configScope.ErrorContext.MoreInfo = "loading procedure tag";
				_configScope.NodeContext = xmlNode; // A procedure tag

				MappedStatement mappedStatement = null;

				procedure = ProcedureDeSerializer.Deserialize(xmlNode, _configScope);
				procedure.CacheModelName = ApplyNamespace( procedure.CacheModelName );
				procedure.ParameterMapName = ApplyNamespace( procedure.ParameterMapName );
				procedure.ResultMapName = ApplyNamespace( procedure.ResultMapName );

				if (_configScope.UseStatementNamespaces == true)
				{
					procedure.Id = ApplyNamespace(procedure.Id);
				}
				_configScope.ErrorContext.ObjectId = procedure.Id;
				procedure.Initialize( _configScope );

				// Build ISql (analyse sql command text)
				ProcessSqlStatement( procedure );

				// Build MappedStatement
				mappedStatement = new MappedStatement( _configScope.SqlMapper, procedure);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Id, mappedStatement);
			}
			#endregion

			#endregion

			#region Load CacheModels

			if (_configScope.IsCacheModelsEnabled == true)
			{
				CacheModel cacheModel = null;
				foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes( ApplyMappingNamespacePrefix(XML_CACHE_MODEL), _configScope.XmlNamespaceManager))
				{
					cacheModel = CacheModelDeSerializer.Deserialize(xmlNode, _configScope);
					cacheModel.Id = ApplyNamespace( cacheModel.Id );

					// Attach ExecuteEventHandler
					foreach(XmlNode flushOn in xmlNode.SelectNodes( ApplyMappingNamespacePrefix(XML_FLUSH_ON_EXECUTE), _configScope.XmlNamespaceManager  ))
					{
						string statementName = flushOn.Attributes["statement"].Value;
						if (_configScope.UseStatementNamespaces == true)
						{
							statementName = ApplyNamespace( statementName ); 
						}

						// delay registering statements to cache model until all sqlMap files have been processed
						IList statementNames = (IList)_configScope.CacheModelFlushOnExecuteStatements[cacheModel.Id];
						if (statementNames == null)
						{
							statementNames = new ArrayList();
						}
						statementNames.Add(statementName);
						_configScope.CacheModelFlushOnExecuteStatements[cacheModel.Id] = statementNames;
					}

					// Get Properties
					foreach(XmlNode propertie in xmlNode.SelectNodes( ApplyMappingNamespacePrefix(XML_PROPERTY), _configScope.XmlNamespaceManager))
					{
						string name = propertie.Attributes["name"].Value;
						string value = propertie.Attributes["value"].Value;
					
						cacheModel.AddProperty(name, value);
					}

					cacheModel.Initialize();

					_configScope.SqlMapper.AddCache( cacheModel );
				}
			}

			#endregion

			_configScope.ErrorContext.Reset();
		}


		/// <summary>
		/// Process the Sql Statement
		/// </summary>
		/// <param name="statement"></param>
		private void ProcessSqlStatement( IStatement statement )
		{
			bool isDynamic = false;
			XmlNode commandTextNode = _configScope.NodeContext;
			DynamicSql dynamic = new DynamicSql( _configScope,  statement );
			StringBuilder sqlBuffer = new StringBuilder();

			_configScope.ErrorContext.MoreInfo = "process the Sql statement";

			// Resolve "extend" attribute on Statement
			if (statement.ExtendStatement.Length >0)
			{
				// Find 'super' statement
				XmlNode supperStatementNode = _configScope.SqlMapDocument.SelectSingleNode( ApplyMappingNamespacePrefix(XML_SEARCH_STATEMENT)+"/child::*[@id='"+statement.ExtendStatement+"']",_configScope.XmlNamespaceManager );
				if (supperStatementNode!=null)
				{
					commandTextNode.InnerXml = supperStatementNode.InnerXml + commandTextNode.InnerXml;
				}
				else
				{
					throw new ConfigurationException("Unable to find extend statement named '"+statement.ExtendStatement+"' on statement '"+statement.Id+"'.'");
				}
			}

			_configScope.ErrorContext.MoreInfo = "parse dynamic tags on sql statement";

			isDynamic = ParseDynamicTags( commandTextNode, dynamic, sqlBuffer, isDynamic, false);

			if (isDynamic) 
			{
				statement.Sql = dynamic;
			} 
			else 
			{	
				string sqlText = sqlBuffer.ToString();
				ApplyInlineParemeterMap( statement, sqlText);
			}
		}

				
		/// <summary>
		/// Parse dynamic tags
		/// </summary>
		/// <param name="commandTextNode"></param>
		/// <param name="dynamic"></param>
		/// <param name="sqlBuffer"></param>
		/// <param name="isDynamic"></param>
		/// <param name="postParseRequired"></param>
		/// <returns></returns>
		private bool ParseDynamicTags( XmlNode commandTextNode, IDynamicParent dynamic, 
			StringBuilder sqlBuffer, bool isDynamic, bool postParseRequired) 
		{
			XmlNodeList children = commandTextNode.ChildNodes;
			for (int i = 0; i < children.Count; i++) 
			{
				XmlNode child = children[i];
				if ( (child.NodeType == XmlNodeType.CDATA) || (child.NodeType == XmlNodeType.Text) )
				{
					string data = child.InnerText.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' '); //??

					data = NodeUtils.ParsePropertyTokens(data, _configScope.Properties);

					SqlText sqlText = null;
					if ( postParseRequired == true ) 
					{
						sqlText = new SqlText();
						sqlText.Text = data.ToString();
					} 
					else 
					{
						sqlText = _paramParser.ParseInlineParameterMap(_configScope.TypeHandlerFactory, null, data );
					}

					dynamic.AddChild(sqlText);
					sqlBuffer.Append(data);
				} 
				else 
				{
					string nodeName = child.Name;
					IDeSerializer serializer = _deSerializerFactory.GetDeSerializer(nodeName);

					if (serializer != null) 
					{
						isDynamic = true;
						SqlTag tag = null;

						tag = serializer.Deserialize(child);

						dynamic.AddChild(tag);

						if (child.HasChildNodes == true) 
						{
							isDynamic = ParseDynamicTags( child, tag, sqlBuffer, isDynamic, tag.Handler.IsPostParseRequired );
						}
					}
				}
			}

			return isDynamic;
		}


		#region Inline Parameter parsing

		/// <summary>
		/// Apply inline paremeterMap
		/// </summary>
		/// <param name="statement"></param>
		/// <param name="sqlStatement"></param>
		private void ApplyInlineParemeterMap( IStatement statement, string sqlStatement)
		{
			string newSql = sqlStatement;

			_configScope.ErrorContext.MoreInfo = "apply inline parameterMap";

			// Check the inline parameter
			if (statement.ParameterMap == null)
			{
				// Build a Parametermap with the inline parameters.
				// if they exist. Then delete inline infos from sqltext.
				
				SqlText sqlText = _paramParser.ParseInlineParameterMap(_configScope.TypeHandlerFactory,  statement, newSql );

				if (sqlText.Parameters.Length > 0)
				{
					ParameterMap map = new ParameterMap(_configScope.DataSource.Provider.UsePositionalParameters);
					map.Id = statement.Id + "-InLineParameterMap";
					statement.ParameterMap = map;	
				
					for(int index=0;index<sqlText.Parameters.Length;index++)
					{
						map.AddParameterProperty( sqlText.Parameters[index] );
					}
				}
				newSql = sqlText.Text;
			}

			ISql sql = null;

			newSql = newSql.Trim();

			if (SimpleDynamicSql.IsSimpleDynamicSql(newSql)) 
			{
				sql = new SimpleDynamicSql(_configScope.TypeHandlerFactory, newSql, statement);
			} 
			else 
			{
				if (statement is Procedure)
				{
					sql = new ProcedureSql(newSql, statement);
					// Could not call BuildPreparedStatement for procedure because when NUnit Test
					// the database is not here (but in theory procedure must be prepared like statement)
					// It's even better as we can then switch DataSource.
				}
				else if (statement is Statement)
				{
					sql = new StaticSql(statement);
					IDalSession session = new SqlMapSession(_configScope.SqlMapper.DataSource);

					((StaticSql)sql).BuildPreparedStatement( session, newSql );
				}					
			}
			statement.Sql = sql;
		}

		#endregion

		
		/// <summary>
		/// Initialize the list of variables defined in the
		/// properties file.
		/// </summary>
		private void ParseGlobalProperties()
		{
			XmlNode nodeProperties = _configScope.NodeContext.SelectSingleNode( ApplyDataMapperNamespacePrefix(XML_PROPERTIES), _configScope.XmlNamespaceManager);
			_configScope.ErrorContext.Activity = "loading global properties";
			
			if (nodeProperties != null)
			{
				if (nodeProperties.HasChildNodes)
				{
					foreach (XmlNode propertyNode in nodeProperties.SelectNodes( ApplyDataMapperNamespacePrefix(XML_PROPERTY), _configScope.XmlNamespaceManager))
					{
						XmlAttribute keyAttrib = propertyNode.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB];
						XmlAttribute valueAttrib = propertyNode.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB];

						if ( keyAttrib != null && valueAttrib!=null)
						{
							_configScope.Properties.Add( keyAttrib.Value,  valueAttrib.Value);
							_logger.Info( string.Format("Add property \"{0}\" value \"{1}\"",keyAttrib.Value,valueAttrib.Value) );
						}
						else
						{
							// Load the file defined by the attribute
							XmlDocument propertiesConfig = Resources.GetAsXmlDocument(propertyNode, _configScope.Properties); 
							
							foreach (XmlNode node in propertiesConfig.SelectNodes( XML_GLOBAL_PROPERTIES, _configScope.XmlNamespaceManager))
							{
								_configScope.Properties[node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value] = node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value;
								_logger.Info( string.Format("Add property \"{0}\" value \"{1}\"",node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value,node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value) );
							}
						}
					}
				}
				else
				{
					// JIRA-38 Fix 
					// <properties> element's InnerXml is currently an empty string anyway
					// since <settings> are in properties file

					_configScope.ErrorContext.Resource = nodeProperties.OuterXml.ToString();

					// Load the file defined by the attribute
					XmlDocument propertiesConfig = Resources.GetAsXmlDocument(nodeProperties, _configScope.Properties); 

					foreach (XmlNode node in propertiesConfig.SelectNodes( XML_SETTINGS_ADD ) )
					{
						_configScope.Properties[node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value] = node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value;
						_logger.Info( string.Format("Add property \"{0}\" value \"{1}\"",node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value,node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value) );
					}					
				}
			}
			_configScope.ErrorContext.Reset();
		}



		/// <summary>
		/// Generate the command text for CRUD operation
		/// </summary>
		/// <param name="sqlMap"></param>
		/// <param name="statement"></param>
		private void GenerateCommandText(SqlMapper sqlMap, IStatement statement)
		{
			string generatedSQL = string.Empty;

			//------ Build SQL CommandText
			generatedSQL = SqlGenerator.BuildQuery(statement);

			ISql sql = new StaticSql(statement);
			IDalSession session = new SqlMapSession(sqlMap.DataSource);

			((StaticSql)sql).BuildPreparedStatement( session, generatedSQL );
			statement.Sql = sql;
		}

		
		/// <summary>
		/// Build a ParameterMap
		/// </summary>
		private void BuildParameterMap()
		 {
			ParameterMap parameterMap = null;
			XmlNode parameterMapNode = _configScope.NodeContext;

			_configScope.ErrorContext.MoreInfo = "build ParameterMap";

			// Get the parameterMap id
			string id = ApplyNamespace( ((XmlAttribute)parameterMapNode.Attributes.GetNamedItem("id")).Value );
			_configScope.ErrorContext.ObjectId = id;

			// Did we already process it ?
			if (_configScope.SqlMapper.ParameterMaps.Contains( id ) == false)
			{
				parameterMap = ParameterMapDeSerializer.Deserialize(parameterMapNode, _configScope);

				parameterMap.Id = ApplyNamespace( parameterMap.Id );
				string attributeExtendMap = parameterMap.ExtendMap;
				parameterMap.ExtendMap = ApplyNamespace( parameterMap.ExtendMap );

				if (parameterMap.ExtendMap.Length >0)
				{
					ParameterMap superMap = null;
					// Did we already build Extend ParameterMap ?
					if (_configScope.SqlMapper.ParameterMaps.Contains( parameterMap.ExtendMap ) == false)
					{
						XmlNode superNode = _configScope.SqlMapConfigDocument.SelectSingleNode(ApplyMappingNamespacePrefix(XML_SEARCH_PARAMETER)+ attributeExtendMap +"']", _configScope.XmlNamespaceManager );

						if (superNode != null)
						{
							_configScope.ErrorContext.MoreInfo = "Build parent ParameterMap";
							_configScope.NodeContext = superNode;
							BuildParameterMap();
							superMap = _configScope.SqlMapper.GetParameterMap( parameterMap.ExtendMap );
						}
						else
						{
							throw new ConfigurationException("In mapping file '"+ _configScope.SqlMapNamespace +"' the parameterMap '"+parameterMap.Id+"' can not resolve extends attribute '"+parameterMap.ExtendMap+"'");
						}
					}
					else
					{
						superMap = _configScope.SqlMapper.GetParameterMap( parameterMap.ExtendMap );
					}
					// Add extends property
					int index = 0;

					foreach(string propertyName in superMap.GetPropertyNameArray())
					{
						parameterMap.InsertParameterProperty( index, superMap.GetProperty(propertyName) );
						index++;
					}
				}
				_configScope.SqlMapper.AddParameterMap( parameterMap );
			}
		}


		/// <summary>
		/// Build a ResultMap
		/// </summary>
		private void BuildResultMap()
		 {
			ResultMap resultMap = null;
			XmlNode resultMapNode = _configScope.NodeContext;

			_configScope.ErrorContext.MoreInfo = "build ResultMap";

			string id = ApplyNamespace(  ((XmlAttribute)resultMapNode.Attributes.GetNamedItem("id")).Value );
			_configScope.ErrorContext.ObjectId = id;

			// Did we alredy process it
			if (_configScope.SqlMapper.ResultMaps.Contains( id ) == false)
			{
				resultMap =  ResultMapDeSerializer.Deserialize( resultMapNode, _configScope );
				
				resultMap.Id = ApplyNamespace( resultMap.Id );
				string attributeExtendMap = resultMap.ExtendMap;
				resultMap.ExtendMap = ApplyNamespace( resultMap.ExtendMap );

				if (resultMap.ExtendMap!=null && resultMap.ExtendMap.Length >0)
				{
					ResultMap superMap = null;
					// Did we already build Extend ResultMap?
					if (_configScope.SqlMapper.ResultMaps.Contains( resultMap.ExtendMap ) == false)
					{
						XmlNode superNode = _configScope.SqlMapDocument.SelectSingleNode( ApplyMappingNamespacePrefix(XML_SEARCH_RESULTMAP)+ attributeExtendMap +"']", _configScope.XmlNamespaceManager);

						if (superNode != null)
						{
							_configScope.ErrorContext.MoreInfo = "Build parent ResultMap";
							_configScope.NodeContext = superNode;
							BuildResultMap();
							superMap = _configScope.SqlMapper.GetResultMap( resultMap.ExtendMap );
						}
						else
						{
							throw new ConfigurationException("In mapping file '"+_configScope.SqlMapNamespace+"' the resultMap '"+resultMap.Id+"' can not resolve extends attribute '"+resultMap.ExtendMap+"'" );
						}
					}
					else
					{
						superMap = _configScope.SqlMapper.GetResultMap( resultMap.ExtendMap );
					}

					// Add parent property
					foreach(DictionaryEntry dicoEntry in superMap.ColumnsToPropertiesMap)
					{
						ResultProperty property = (ResultProperty)dicoEntry.Value;
						resultMap.AddResultPropery(property);
					}
				}
				_configScope.SqlMapper.AddResultMap( resultMap );
			}
		 }


		/// <summary>
		/// Register under Statement Name or Fully Qualified Statement Name
		/// </summary>
		/// <param name="id">An Identity</param>
		/// <returns>The new Identity</returns>
		private string ApplyNamespace(string id) 
		{
			string newId = id;
			string currentNamespace = _configScope.SqlMapNamespace;

			if (currentNamespace != null && currentNamespace.Length > 0 
				&& id != null && id.Length>0 && id.IndexOf(".") < 0) 
			{
				newId = currentNamespace + DOT + id;
			}
			return newId;
		}



		/// <summary>
		/// Gets a resource stream.
		/// </summary>
		/// <param name="schemaResourceKey">The schema resource key.</param>
		/// <returns>A resource stream.</returns>
		public Stream GetStream( string schemaResourceKey )
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream("IBatisNet.DataMapper." + schemaResourceKey); 
		}


		/// <summary>
		/// Apply the dataMapper namespace prefix
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		public string ApplyDataMapperNamespacePrefix( string elementName )
		{
			return DATAMAPPER_NAMESPACE_PREFIX+ ":" + elementName.
				Replace("/","/"+DATAMAPPER_NAMESPACE_PREFIX+":");
		}

		/// <summary>
		/// Apply the provider namespace prefix
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		public string ApplyProviderNamespacePrefix( string elementName )
		{
			return PROVIDERS_NAMESPACE_PREFIX+ ":" + elementName.
				Replace("/","/"+PROVIDERS_NAMESPACE_PREFIX+":");
		}

		/// <summary>
		/// Apply the provider namespace prefix
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		public static string ApplyMappingNamespacePrefix( string elementName )
		{
			return MAPPING_NAMESPACE_PREFIX+ ":" + elementName.
				Replace("/","/"+MAPPING_NAMESPACE_PREFIX+":");
		}

//		private String ParsePropertyTokens(string str) 
//		{
//		const string  OPEN = "${";
//		const string CLOSE = "}";
//
//
//		string newString = str;
//		if (str != null && props != null) {
//      int start = newString.indexOf(OPEN);
//      int end = newString.indexOf(CLOSE);
//
//      while (start > -1 && end > start) {
//        String prepend = newString.substring(0, start);
//        String append = newString.substring(end + CLOSE.length());
//        String propName = newString.substring(start + OPEN.length(), end);
//        String propValue = props.getProperty(propName);
//        if (propValue == null) {
//          newString = prepend + propName + append;
//        } else {
//          newString = prepend + propValue + append;
//        }
//        start = newString.indexOf(OPEN);
//        end = newString.indexOf(CLOSE);
//      }
//    }
//    return newString;
//  }

		#endregion
	}
}
