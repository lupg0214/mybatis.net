
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
using System.Xml.Serialization;
using IBatisNet.Common;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.Sql;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements;
using IBatisNet.DataMapper.Configuration.Sql.SimpleDynamic;
using IBatisNet.DataMapper.Configuration.Sql.Static;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.TypeHandlers;
using log4net;

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
		private const string XML_CONFIG_ROOT = "sqlMapConfig";

		/// <summary>
		/// Token for xml path to SqlMapConfig settings element.
		/// </summary>
		private const string XML_CONFIG_SETTINGS = "/sqlMapConfig/settings/setting";

		/// <summary>
		/// Token for providers config file name.
		/// </summary>
		private const string PROVIDERS_FILE_NAME = "providers.config";

		/// <summary>
		/// Token for xml path to SqlMapConfig providers element.
		/// </summary>
		private static string XML_CONFIG_PROVIDERS = "/sqlMapConfig/providers";

		// TODO: Other XML paths.

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

		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private ConfigurationScope _configScope = null;
		private InlineParameterMapParser _paramParser = null;

		#endregion 		
		
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public DomSqlMapBuilder()
		{
			_configScope = new ConfigurationScope();
			_paramParser = new InlineParameterMapParser(_configScope.ErrorContext);
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
		private SqlMapper Build(XmlDocument document, 
		                        DataSource dataSource, 
			bool useConfigFileWatcher, bool isCallFromDao)
		{
			_configScope.SqlMapConfigDocument = document;
			_configScope.DataSource = dataSource;
			_configScope.IsCallFromDao = isCallFromDao;
			_configScope.UseConfigFileWatcher = useConfigFileWatcher;
			
			try
			{
				ValidateSchema( document.ChildNodes[1], "SqlMapConfig.xsd" );
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
			StreamReader streamReader = null; 

			_configScope.ErrorContext.Activity = "Validate SqlMap config";
			try
			{
				//Validate the document using a schema
				validatingReader = new XmlValidatingReader( new XmlTextReader( new StringReader( section.OuterXml ) ) );
				validatingReader.ValidationType = ValidationType.Schema;

				xsdFile = GetStream( schemaFileName ); 
				streamReader = new StreamReader( xsdFile ); 

				validatingReader.Schemas.Add( XmlSchema.Read( new XmlTextReader( streamReader ), new ValidationEventHandler(ValidationCallBack) ) );

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
				if( streamReader != null ) streamReader.Close();
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
		/// Intilaize an SqlMap.
		/// </summary>
		private void Initialize()
		{
			Reset();
			
			_configScope.SqlMapper = new SqlMapper( new TypeHandlerFactory() );

			#region Load Global Properties
			if (_configScope.IsCallFromDao == false)
			{
				_configScope.NodeContext = _configScope.SqlMapConfigDocument.SelectSingleNode(XML_CONFIG_ROOT);
				ParseGlobalProperties();
			}
			#endregion

			#region Load settings

			_configScope.ErrorContext.Activity = "loading global settings";

			XmlNodeList settings = _configScope.SqlMapConfigDocument.SelectNodes(XML_CONFIG_SETTINGS);

			if (settings!=null)
			{
				foreach (XmlNode setting in settings)
				{
					if (setting.Attributes[ATR_USE_STATEMENT_NAMESPACES] != null )
					{	
						string value = Resources.ParsePropertyTokens(setting.Attributes[ATR_USE_STATEMENT_NAMESPACES].Value, _configScope.Properties);
						_configScope.UseStatementNamespaces =  Convert.ToBoolean( value ); 
					}
					if (setting.Attributes[ATR_CACHE_MODELS_ENABLED] != null )
					{		
						string value = Resources.ParsePropertyTokens(setting.Attributes[ATR_CACHE_MODELS_ENABLED].Value, _configScope.Properties);
						_configScope.IsCacheModelsEnabled =  Convert.ToBoolean( value ); 
					}
					if (setting.Attributes[ATR_EMBED_STATEMENT_PARAMS] != null )
					{		
						string value = Resources.ParsePropertyTokens(setting.Attributes[ATR_EMBED_STATEMENT_PARAMS].Value, _configScope.Properties);
						_configScope.UseEmbedStatementParams =  Convert.ToBoolean( value ); 
					}

					if (setting.Attributes[ATR_VALIDATE_SQLMAP] != null )
					{		
						string value = Resources.ParsePropertyTokens(setting.Attributes[ATR_VALIDATE_SQLMAP].Value, _configScope.Properties);
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
			XmlNode nodeDataSource = _configScope.SqlMapConfigDocument.SelectSingleNode("/sqlMapConfig/database/dataSource");
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
					XmlSerializer serializer = null;
					serializer = new XmlSerializer(typeof(DataSource));
					DataSource dataSource = (DataSource) serializer.Deserialize(new XmlNodeReader(nodeDataSource));

					dataSource.Provider = provider;
					// Use Global Properties if any
					dataSource.ConnectionString = Resources.ParsePropertyTokens(dataSource.ConnectionString, _configScope.Properties);

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
			foreach (XmlNode xmlNode in _configScope.SqlMapConfigDocument.SelectNodes("/sqlMapConfig/alias/typeAlias"))
			{
				_configScope.ErrorContext.Activity = "loading global Type alias";
				TypeAlias typeAlias = null;
				XmlSerializer serializer = new XmlSerializer(typeof(TypeAlias));

				typeAlias = (TypeAlias) serializer.Deserialize(new XmlNodeReader(xmlNode));
				_configScope.ErrorContext.ObjectId = typeAlias.ClassName;
				_configScope.ErrorContext.MoreInfo = "initialize type alias";
				typeAlias.Initialize();

				_configScope.SqlMapper.AddTypeAlias( typeAlias.Name, typeAlias );
			}
			_configScope.ErrorContext.Reset();
			#endregion

			#region Load TypeHandlers
			foreach (XmlNode xmlNode in _configScope.SqlMapConfigDocument.SelectNodes("/sqlMapConfig/typeHandlers/typeHandler"))
			{
				try
				{
					_configScope.ErrorContext.Activity = "loading typeHandler";
					TypeHandler handler = null;
					XmlSerializer serializer = new XmlSerializer(typeof(TypeHandler));

					handler = (TypeHandler) serializer.Deserialize(new XmlNodeReader(xmlNode));
					_configScope.ErrorContext.ObjectId = handler.CallBackName;
					_configScope.ErrorContext.MoreInfo = "initialize typeHandler";
					handler.Initialize();


					_configScope.ErrorContext.MoreInfo = "Check the callback attribute '" + handler.CallBackName + "' (must be a classname).";
					ITypeHandler typeHandler = null;
					Type type = _configScope.SqlMapper.GetType(handler.CallBackName);
					object impl = Activator.CreateInstance( type );
					if (impl is ITypeHandlerCallback) 
					{
						typeHandler = new CustomTypeHandler((ITypeHandlerCallback) impl);
					} 
					else if (impl is ITypeHandler) 
					{
						typeHandler = (ITypeHandler) impl;
					} 
					else 
					{
						throw new ConfigurationException("The callBack type is not a valid implementation of ITypeHandler or ITypeHandlerCallback");
					}

					_configScope.ErrorContext.MoreInfo = "Check the type attribute '" + handler.ClassName + "' (must be a class name) or the dbType '" + handler.DbType + "' (must be a DbType type name).";
					if (handler.DbType!= null && handler.DbType.Length > 0) 
					{
						_configScope.TypeHandlerFactory.Register(Resources.TypeForName(handler.ClassName), handler.DbType, typeHandler);
					} 
					else 
					{
						_configScope.TypeHandlerFactory.Register(Resources.TypeForName(handler.ClassName), typeHandler);
					}
				} 
				catch (Exception e) 
				{
					throw new ConfigurationException(
						String.Format("Error registering TypeHandler class \"{0}\" for handling .Net type \"{1}\" and dbType \"{2}\". Cause: {3}", 
						xmlNode.Attributes["callback"].Value,
						xmlNode.Attributes["type"].Value,
						xmlNode.Attributes["dbType"].Value,
						e.Message), e);
				}
			}
			_configScope.ErrorContext.Reset();
			#endregion

			#region Load sqlMap mapping files
			
			foreach (XmlNode xmlNode in _configScope.SqlMapConfigDocument.SelectNodes("/sqlMapConfig/sqlMaps/sqlMap"))
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

					MappedStatement mappedStatement = (MappedStatement)entry.Value;
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
			XmlSerializer serializer = null;
			Provider provider = null;
			XmlDocument xmlProviders = null;

			_configScope.ErrorContext.Activity = "loading Providers";

			XmlNode providersNode = null;
			providersNode = _configScope.SqlMapConfigDocument.SelectSingleNode(XML_CONFIG_PROVIDERS);
			if (providersNode != null )
			{
				xmlProviders = Resources.GetAsXmlDocument( providersNode, _configScope.Properties );
			}
			else
			{
				xmlProviders = Resources.GetConfigAsXmlDocument(PROVIDERS_FILE_NAME);
			}

			serializer = new XmlSerializer(typeof(Provider));

			foreach (XmlNode node in xmlProviders.SelectNodes("/providers/provider"))
			{
				_configScope.ErrorContext.Resource = node.InnerXml.ToString();

				provider = (Provider) serializer.Deserialize(new XmlNodeReader(node));

				if (provider.IsEnabled == true)
				{
					_configScope.ErrorContext.ObjectId = provider.Name;
					_configScope.ErrorContext.MoreInfo = "initialize provider";

					provider.Initialisation();
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
			XmlNode node = _configScope.SqlMapConfigDocument.SelectSingleNode("//database/provider");

			if (node != null)
			{
				_configScope.ErrorContext.Resource = node.OuterXml.ToString();
				// name
				string providerName = Resources.ParsePropertyTokens(node.Attributes["name"].Value, _configScope.Properties);

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
			XmlSerializer serializer = null;
			XmlNode sqlMapNode = _configScope.NodeContext;

			_configScope.ErrorContext.Activity = "loading SqlMap ";
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

			_configScope.SqlMapNamespace = _configScope.SqlMapDocument.SelectSingleNode("sqlMap").Attributes["namespace"].Value;

			#region Load TypeAlias

			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes("/sqlMap/alias/typeAlias"))
			{
				_configScope.ErrorContext.MoreInfo = "loading type alias";
				TypeAlias typeAlias = null;
				serializer = new XmlSerializer(typeof(TypeAlias));
				typeAlias = (TypeAlias) serializer.Deserialize(new XmlNodeReader(xmlNode));
				_configScope.ErrorContext.ObjectId = typeAlias.ClassName;
				_configScope.ErrorContext.MoreInfo = "initialize type alias";
				typeAlias.Initialize();

				_configScope.SqlMapper.AddTypeAlias( typeAlias.Name, typeAlias );
			}
			_configScope.ErrorContext.MoreInfo = string.Empty;
			_configScope.ErrorContext.ObjectId = string.Empty;

			#endregion

			#region Load resultMap

			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes("/sqlMap/resultMaps/resultMap"))
			{
				_configScope.ErrorContext.MoreInfo = "loading ResultMap tag";
				_configScope.NodeContext = xmlNode; // A ResultMap node

				BuildResultMap();
			}

			#endregion

			#region Load parameterMaps

			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes("/sqlMap/parameterMaps/parameterMap"))
			{
				_configScope.ErrorContext.MoreInfo = "loading ParameterMap tag";
				_configScope.NodeContext = xmlNode; // A ParameterMap node

				BuildParameterMap();
			}

			#endregion
		
			#region Load statements

			#region Statement tag
			Statement statement = null;
			serializer = new XmlSerializer(typeof(Statement));
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes("/sqlMap/statements/statement"))
			{
				_configScope.ErrorContext.MoreInfo = "loading statement tag";
				_configScope.NodeContext = xmlNode; // A statement tag

				MappedStatement mappedStatement = null;

				statement = (Statement) serializer.Deserialize(new XmlNodeReader(xmlNode));
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
					//config, sqlMapName, sqlMap, xmlNode, statement);

				// Build MappedStatement
				mappedStatement = new MappedStatement( _configScope.SqlMapper, statement);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Name, mappedStatement);
			}
			#endregion

			#region Select tag
			Select select = null;
			serializer = new XmlSerializer(typeof(Select));
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes("/sqlMap/statements/select"))
			{
				_configScope.ErrorContext.MoreInfo = "loading select tag";
				_configScope.NodeContext = xmlNode; // A select node

				MappedStatement mappedStatement = null;

				select = (Select) serializer.Deserialize(new XmlNodeReader(xmlNode));
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
						//config, sqlMapName, sqlMap, xmlNode, select);
				}

				// Build MappedStatement
				mappedStatement = new SelectMappedStatement( _configScope.SqlMapper, select);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Name, mappedStatement);
			}
			#endregion

			#region Insert tag
			Insert insert = null;
			serializer = new XmlSerializer(typeof(Insert));
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes("/sqlMap/statements/insert"))
			{
				_configScope.ErrorContext.MoreInfo = "loading insert tag";
				_configScope.NodeContext = xmlNode; // A insert tag

				MappedStatement mappedStatement = null;

				insert = (Insert) serializer.Deserialize(new XmlNodeReader(xmlNode));
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
						//config, sqlMapName, sqlMap, xmlNode, insert);
				}

				// Build MappedStatement
				mappedStatement = new InsertMappedStatement( _configScope.SqlMapper, insert);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Name, mappedStatement);

				#region statement SelectKey
				// Set sql statement SelectKey 
				if (insert.SelectKey != null)
				{
					insert.SelectKey.Initialize( _configScope );
					insert.SelectKey.Id = insert.Id + DOT + "SelectKey";
					string commandText = xmlNode.SelectSingleNode("selectKey").FirstChild.InnerText.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ').Trim();
					commandText = Resources.ParsePropertyTokens(commandText, _configScope.Properties);
					StaticSql sql = new StaticSql(insert.SelectKey);
					IDalSession session = new SqlMapSession( _configScope.SqlMapper.DataSource );
					sql.BuildPreparedStatement( session, commandText );
					insert.SelectKey.Sql = sql;					
					
					// Build MappedStatement
					mappedStatement = new MappedStatement( _configScope.SqlMapper, insert.SelectKey);

					_configScope.SqlMapper.AddMappedStatement(mappedStatement.Name, mappedStatement);
				}
				#endregion
			}
			#endregion

			#region Update tag
			Update update = null;
			serializer = new XmlSerializer(typeof(Update));
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes("/sqlMap/statements/update"))
			{
				_configScope.ErrorContext.MoreInfo = "loading update tag";
				_configScope.NodeContext = xmlNode; // A update tag

				MappedStatement mappedStatement = null;

				update = (Update) serializer.Deserialize(new XmlNodeReader(xmlNode));
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
						//config, sqlMapName, sqlMap, xmlNode, update);
				}	

				// Build MappedStatement
				mappedStatement = new UpdateMappedStatement( _configScope.SqlMapper, update);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Name, mappedStatement);
			}
			#endregion

			#region Delete tag
			Delete delete = null;
			serializer = new XmlSerializer(typeof(Delete));
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes("/sqlMap/statements/delete"))
			{
				_configScope.ErrorContext.MoreInfo = "loading delete tag";
				_configScope.NodeContext = xmlNode; // A delete tag
				MappedStatement mappedStatement = null;

				delete = (Delete) serializer.Deserialize(new XmlNodeReader(xmlNode));
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
						//config, sqlMapName, sqlMap, xmlNode, delete);
				}	

				// Build MappedStatement
				mappedStatement = new DeleteMappedStatement( _configScope.SqlMapper, delete);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Name, mappedStatement);
			}
			#endregion

			#region Procedure tag
			Procedure procedure = null;
			serializer = new XmlSerializer(typeof(Procedure));
			foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes("/sqlMap/statements/procedure"))
			{
				_configScope.ErrorContext.MoreInfo = "loading procedure tag";
				_configScope.NodeContext = xmlNode; // A procedure tag

				MappedStatement mappedStatement = null;

				procedure = (Procedure)serializer.Deserialize(new XmlNodeReader(xmlNode));
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
					//config, sqlMapName, sqlMap, xmlNode, procedure);

				// Build MappedStatement
				mappedStatement = new MappedStatement( _configScope.SqlMapper, procedure);

				_configScope.SqlMapper.AddMappedStatement(mappedStatement.Name, mappedStatement);
			}
			#endregion

			#endregion

			#region Load CacheModels

			if (_configScope.IsCacheModelsEnabled == true)
			{
				CacheModel cacheModel = null;
				serializer = new XmlSerializer(typeof(CacheModel));
				foreach (XmlNode xmlNode in _configScope.SqlMapDocument.SelectNodes("/sqlMap/cacheModels/cacheModel"))
				{
					cacheModel = (CacheModel) serializer.Deserialize(new XmlNodeReader(xmlNode));
					cacheModel.Id = ApplyNamespace( cacheModel.Id );

					// Attach ExecuteEventHandler
					foreach(XmlNode flushOn in xmlNode.SelectNodes("flushOnExecute"))
					{
						string statementName = flushOn.Attributes["statement"].Value;
						if (_configScope.UseStatementNamespaces == true)
						{
							statementName = _configScope.SqlMapNamespace + DOT + statementName;
						}

						MappedStatement mappedStatement = _configScope.SqlMapper.GetMappedStatement(statementName);

						cacheModel.RegisterTriggerStatement(mappedStatement);
					}

					// Get Properties
					foreach(XmlNode propertie in xmlNode.SelectNodes("property"))
					{
						string name = propertie.Attributes["name"].Value;
						string value = propertie.Attributes["value"].Value;
					
						cacheModel.AddPropertie(name, value);
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
			if (statement.ExtendSatement.Length >0)
			{
				// Find 'super' statement
				XmlNode supperStatementNode = _configScope.SqlMapDocument.SelectSingleNode("/sqlMap/statements/child::*[@id='"+statement.ExtendSatement+"']");
				if (supperStatementNode!=null)
				{
					commandTextNode.InnerXml = supperStatementNode.InnerXml + commandTextNode.InnerXml;
				}
				else
				{
					throw new ConfigurationException("Unable to find extend statement named '"+statement.ExtendSatement+"' on statement '"+statement.Id+"'.'");
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
					string data = child.InnerText.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ').Trim(); //??

					data = Resources.ParsePropertyTokens(data, _configScope.Properties);

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
					XmlSerializer serializer = SerializerFactory.GetSerializer(nodeName);

					if (serializer != null) 
					{
						isDynamic = true;
						SqlTag tag = null;

						tag = (SqlTag) serializer.Deserialize(new XmlNodeReader(child));

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
			XmlNode nodeProperties = _configScope.NodeContext.SelectSingleNode("properties");
			_configScope.ErrorContext.Activity = "loading global properties";
			
			if (nodeProperties != null)
			{
				if (nodeProperties.HasChildNodes)
				{
					foreach (XmlNode propertyNode in nodeProperties.SelectNodes("property"))
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
							
							foreach (XmlNode node in propertiesConfig.SelectNodes("/settings/add"))
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

					foreach (XmlNode node in propertiesConfig.SelectNodes("/settings/add"))
					{
						_configScope.Properties[node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value] = node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value;
						_logger.Info( string.Format("Add property \"{0}\" value \"{1}\"",node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value,node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value) );
					}					
				}
			}
			_configScope.ErrorContext.Reset();;
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

			XmlSerializer serializer = new XmlSerializer(typeof(ParameterMap));

			_configScope.ErrorContext.MoreInfo = "build ParameterMap";

			// Get the parameterMap id
			string id = ApplyNamespace( ((XmlAttribute)parameterMapNode.Attributes.GetNamedItem("id")).Value );
			_configScope.ErrorContext.ObjectId = id;

			// Did we already process it ?
			if (_configScope.SqlMapper.ParameterMaps.Contains( id ) == false)
			{
				parameterMap = (ParameterMap) serializer.Deserialize(new XmlNodeReader(parameterMapNode));
				
				_configScope.ErrorContext.MoreInfo = "Initialize ParameterMap";
				_configScope.NodeContext = parameterMapNode;
				parameterMap.Initialize( _configScope );

				parameterMap.Id = ApplyNamespace( parameterMap.Id );
				string attributeExtendMap = parameterMap.ExtendMap;
				parameterMap.ExtendMap = ApplyNamespace( parameterMap.ExtendMap );

				if (parameterMap.ExtendMap.Length >0)
				{
					ParameterMap superMap = null;
					// Did we already build Extend ParameterMap ?
					if (_configScope.SqlMapper.ParameterMaps.Contains( parameterMap.ExtendMap ) == false)
					{
						XmlNode superNode = _configScope.SqlMapConfigDocument.SelectSingleNode("/sqlMap/parameterMaps/parameterMap[@id='"+ attributeExtendMap +"']");

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

			XmlSerializer serializer = new XmlSerializer(typeof(ResultMap));

			_configScope.ErrorContext.MoreInfo = "build ResultMap";

			string id = ApplyNamespace(  ((XmlAttribute)resultMapNode.Attributes.GetNamedItem("id")).Value );
			_configScope.ErrorContext.ObjectId = id;

			// Did we alredy process it
			if (_configScope.SqlMapper.ResultMaps.Contains( id ) == false)
			{
				resultMap = (ResultMap) serializer.Deserialize(new XmlNodeReader(resultMapNode));
				
				resultMap.SqlMapNameSpace = _configScope.SqlMapNamespace;

				_configScope.ErrorContext.MoreInfo = "initialize ResultMap";
				resultMap.Initialize( _configScope );

				resultMap.Id = ApplyNamespace( resultMap.Id );
				string attributeExtendMap = resultMap.ExtendMap;
				resultMap.ExtendMap = ApplyNamespace( resultMap.ExtendMap );

				if (resultMap.ExtendMap.Length >0)
				{
					ResultMap superMap = null;
					// Did we already build Extend ResultMap?
					if (_configScope.SqlMapper.ResultMaps.Contains( resultMap.ExtendMap ) == false)
					{
						XmlNode superNode = _configScope.SqlMapDocument.SelectSingleNode("/sqlMap/resultMaps/resultMap[@id='"+ attributeExtendMap +"']");

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
