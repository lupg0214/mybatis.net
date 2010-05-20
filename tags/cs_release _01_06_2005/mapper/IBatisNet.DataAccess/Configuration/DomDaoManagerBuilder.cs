
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
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using IBatisNet.Common;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess.Scope;
using log4net;

#endregion

namespace IBatisNet.DataAccess.Configuration
{
	/// <summary>
	/// Summary description for DomDaoManagerBuilder.
	/// </summary>
	public class DomDaoManagerBuilder
	{
		#region Constants

		
		private const string PROPERTY_ELEMENT_KEY_ATTRIB = "key";
		private const string PROPERTY_ELEMENT_VALUE_ATTRIB = "value";

		/// <summary>
		/// Key for default config name
		/// </summary>
		public const string DEFAULT_FILE_CONFIG_NAME = "dao.config";
		/// <summary>
		/// Key for default provider name
		/// </summary>
		public const string DEFAULT_PROVIDER_NAME = "_DEFAULT_PROVIDER_NAME";
		/// <summary>
		/// Key for default dao session handler name
		/// </summary>
		public const string DEFAULT_DAOSESSIONHANDLER_NAME = "DEFAULT_DAOSESSIONHANDLER_NAME";

		/// <summary>
		/// Token for xml path to DaoConfig providers element.
		/// </summary>
		private static string XML_CONFIG_PROVIDERS = "/daoConfig/providers";

		/// <summary>
		/// Token for providers config file name.
		/// </summary>
		private const string PROVIDERS_FILE_NAME = "providers.config";
		#endregion

		#region Fields

		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		#endregion 

		#region Constructor (s) / Destructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public DomDaoManagerBuilder(){ }

		#endregion

		#region Configure

		/// <summary>
		/// Configure DaoManagers from via the default file config.
		/// (accesd as relative ressource path from your Application root)
		/// </summary>
		public void Configure()
		{
			Configure( DomDaoManagerBuilder.DEFAULT_FILE_CONFIG_NAME );
		}


		/// <summary>
		/// Configure DaoManagers from a file path.
		/// </summary>
		/// <param name="resource">
		/// A relative ressource path from your Application root 
		/// or a absolue file path file:\\c:\dir\a.config
		/// </param>
		public void Configure(string resource)
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
			BuildDaoManagers( document, false );
		}


		/// <summary>
		///  Configure DaoManagers from a stream.
		/// </summary>
		/// <param name="resource">A stream resource</param>
		public void Configure(Stream resource)
		{
			XmlDocument document = Resources.GetStreamAsXmlDocument( resource );
			BuildDaoManagers( document, false );
		}

		/// <summary>
		///  Configure DaoManagers from a FileInfo.
		/// </summary>
		/// <param name="resource">A FileInfo resource</param>
		/// <returns>An SqlMap</returns>
		public void Configure(FileInfo resource)
		{
			XmlDocument document = Resources.GetFileInfoAsXmlDocument( resource );
			BuildDaoManagers( document, false );
		}

		/// <summary>
		///  Configure DaoManagers from an Uri.
		/// </summary>
		/// <param name="resource">A Uri resource</param>
		/// <returns></returns>
		public void Configure(Uri resource)
		{
			XmlDocument document = Resources.GetUriAsXmlDocument( resource );
			BuildDaoManagers( document, false );
		}

		/// <summary>
		/// Configure and monitor the configuration file for modifications and 
		/// automatically reconfigure  
		/// </summary>
		/// <param name="configureDelegate">
		/// Delegate called when a file is changed to rebuild the 
		/// </param>
		public void ConfigureAndWatch(ConfigureHandler configureDelegate)
		{
			ConfigureAndWatch( DomDaoManagerBuilder.DEFAULT_FILE_CONFIG_NAME, configureDelegate );
		}


		/// <summary>
		/// Configure and monitor the configuration file for modifications and 
		/// automatically reconfigure  
		/// </summary>
		/// <param name="resource">
		/// A relative ressource path from your Application root 
		/// or an absolue file path file:\\c:\dir\a.config
		/// </param>
		///<param name="configureDelegate">
		/// Delegate called when the file has changed, to rebuild the dal.
		/// </param>
		public void ConfigureAndWatch(string resource, ConfigureHandler configureDelegate)
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

			BuildDaoManagers( document, true );

			TimerCallback callBakDelegate = new TimerCallback( DomDaoManagerBuilder.OnConfigFileChange );

			StateConfig state = new StateConfig();
			state.FileName = resource;
			state.ConfigureHandler = configureDelegate;

			new ConfigWatcherHandler( callBakDelegate, state );
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
		public void ConfigureAndWatch( FileInfo resource, ConfigureHandler configureDelegate )
		{
			XmlDocument document = Resources.GetFileInfoAsXmlDocument(resource);

			ConfigWatcherHandler.ClearFilesMonitored();
			ConfigWatcherHandler.AddFileToWatch( resource );

			BuildDaoManagers( document, true );

			TimerCallback callBakDelegate = new TimerCallback( DomDaoManagerBuilder.OnConfigFileChange );

			StateConfig state = new StateConfig();
			state.FileName = resource.FullName;
			state.ConfigureHandler = configureDelegate;

			new ConfigWatcherHandler( callBakDelegate, state );
		}

		/// <summary>
		/// Called when the configuration has been updated. 
		/// </summary>
		/// <param name="obj">The state config.</param>
		public static void OnConfigFileChange(object obj)
		{
			StateConfig state = (StateConfig)obj;
			state.ConfigureHandler(null);
		}


		#endregion

		#region Methods

		/// <summary>
		/// Build DaoManagers from config document.
		/// </summary>
//		[MethodImpl(MethodImplOptions.Synchronized)]
		public void BuildDaoManagers(XmlDocument document, bool useConfigFileWatcher)
		{
			ConfigurationScope configurationScope = new ConfigurationScope();

			configurationScope.UseConfigFileWatcher = useConfigFileWatcher;
			configurationScope.DaoConfigDocument = document;

			try
			{
				GetConfig( configurationScope );
			}
			catch(Exception ex)
			{
				throw new ConfigurationException( configurationScope.ErrorContext.ToString(), ex);
			}
		}

		/// <summary>
		/// Load and build the dao managers.
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		private void GetConfig(ConfigurationScope configurationScope)
		{
			GetProviders( configurationScope );

			GetDaoSessionHandlers( configurationScope );
			GetContexts( configurationScope );
		}

		
		/// <summary>
		/// Load and initialize providers from specified file.
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		private void GetProviders(ConfigurationScope configurationScope)
		{
			XmlSerializer serializer = null;
			Provider provider = null;
			XmlDocument xmlProviders = null;

			configurationScope.ErrorContext.Activity = "load DataBase Provider";

			XmlNode providersNode = null;
			providersNode = configurationScope.DaoConfigDocument.SelectSingleNode(XML_CONFIG_PROVIDERS);
			if (providersNode != null )
			{
				xmlProviders = Resources.GetAsXmlDocument( providersNode, configurationScope.Properties );
			}
			else
			{
				xmlProviders = Resources.GetConfigAsXmlDocument(PROVIDERS_FILE_NAME);
			}

			serializer = new XmlSerializer(typeof(Provider));

			foreach (XmlNode node in xmlProviders.SelectNodes("/providers/provider"))
			{
				configurationScope.ErrorContext.Resource = node.InnerXml.ToString();

				provider = (Provider) serializer.Deserialize(new XmlNodeReader(node));

				if (provider.IsEnabled == true)
				{
					configurationScope.ErrorContext.ObjectId = provider.Name;
					configurationScope.ErrorContext.MoreInfo = "initialize provider";

					provider.Initialisation();
					configurationScope.Providers.Add(provider.Name, provider);
					if (provider.IsDefault == true)
					{
						if (configurationScope.Providers[DEFAULT_PROVIDER_NAME] == null) 
						{
							configurationScope.Providers.Add(DEFAULT_PROVIDER_NAME, provider);
						} 
						else 
						{
							throw new ConfigurationException(
								string.Format("Error while configuring the Provider named \"{0}\" There can be only one default Provider.",provider.Name));
						}
					}
				}
			}

			configurationScope.ErrorContext.Reset();
		}


		/// <summary>
		/// Load and initialize custom DaoSession Handlers.
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		private void GetDaoSessionHandlers(ConfigurationScope configurationScope)
		{
			XmlSerializer serializer = null;
			XmlNode daoSessionHandlersNode = null;

			configurationScope.ErrorContext.Activity = "load custom DaoSession Handlers";

			serializer = new XmlSerializer(typeof(DaoSessionHandler));
			daoSessionHandlersNode = configurationScope.DaoConfigDocument.SelectSingleNode("daoConfig").SelectSingleNode("daoSessionHandlers");

			if (daoSessionHandlersNode != null)
			{
				foreach (XmlNode node in daoSessionHandlersNode.SelectNodes("handler"))
				{
					configurationScope.ErrorContext.Resource = node.InnerXml.ToString();

					DaoSessionHandler daoSessionHandler =(DaoSessionHandler) serializer.Deserialize(new XmlNodeReader(node));
				
					configurationScope.ErrorContext.ObjectId = daoSessionHandler.Name;
					configurationScope.ErrorContext.MoreInfo = "build daoSession handler";

					IDaoSessionHandler sessionHandler = daoSessionHandler.GetIDaoSessionHandler();

					configurationScope.DaoSectionHandlers[daoSessionHandler.Name] = sessionHandler;

					if (daoSessionHandler.IsDefault == true)
					{
						configurationScope.DaoSectionHandlers[DEFAULT_DAOSESSIONHANDLER_NAME] = sessionHandler;
					}
				}
			}

			configurationScope.ErrorContext.Reset();
		}


		/// <summary>
		/// Build dao contexts
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		private void GetContexts(ConfigurationScope configurationScope)
		{
			DaoManager daoManager = null;
			XmlAttribute attribute = null;
			XmlNode section = null;

			// Init
			DaoManager.Reset();

			section = configurationScope.DaoConfigDocument.SelectSingleNode("daoConfig");

			// Build one daoManager for each context
			foreach (XmlNode contextNode in section.SelectNodes("context"))
			{
				configurationScope.ErrorContext.Activity = "build daoManager";
				configurationScope.NodeContext = contextNode;

				#region Configure a new DaoManager

				daoManager = DaoManager.NewInstance();

				// name
				attribute = contextNode.Attributes["id"];
				daoManager.Name = attribute.Value;

				configurationScope.ErrorContext.Activity += daoManager.Name;

				// default
				attribute = contextNode.Attributes["default"];
				if (attribute != null)
				{
					if (attribute.Value=="true")
					{
						daoManager.IsDefault = true;
					}
					else
					{
						daoManager.IsDefault= false;
					}
				}
				else
				{
					daoManager.IsDefault= false;
				}
				#endregion 

				#region Properties
				ParseGlobalProperties( configurationScope );
				#endregion

				#region provider
				daoManager.Provider = ParseProvider( configurationScope );

				configurationScope.ErrorContext.Resource = string.Empty;
				configurationScope.ErrorContext.MoreInfo = string.Empty;
				configurationScope.ErrorContext.ObjectId = string.Empty;
				#endregion

				#region DataSource 
				daoManager.DataSource = ParseDataSource( configurationScope );
				daoManager.DataSource.Provider = daoManager.Provider;
				#endregion

				#region DaoSessionHandler

				XmlNode nodeSessionHandler = contextNode.SelectSingleNode("daoSessionHandler");

				configurationScope.ErrorContext.MoreInfo = "configure DaoSessionHandler";

				// The resources use to initialize the SessionHandler 
				IDictionary resources = new Hashtable();
				// By default, add the DataSource
				resources.Add( "DataSource", daoManager.DataSource);
				// By default, add the useConfigFileWatcher
				resources.Add( "UseConfigFileWatcher", configurationScope.UseConfigFileWatcher);

				IDaoSessionHandler sessionHandler = null;

				if (nodeSessionHandler!= null)
				{
					configurationScope.ErrorContext.Resource = nodeSessionHandler.InnerXml.ToString();
					
					sessionHandler = (IDaoSessionHandler)configurationScope.DaoSectionHandlers[nodeSessionHandler.Attributes["id"].Value];

					// Parse property node
					foreach(XmlNode nodeProperty in nodeSessionHandler.SelectNodes("property"))
					{
						resources.Add(nodeProperty.Attributes["name"].Value, 
							Resources.ParsePropertyTokens(nodeProperty.Attributes["value"].Value, configurationScope.Properties));
					}
				}
				else
				{
					sessionHandler = (IDaoSessionHandler)configurationScope.DaoSectionHandlers[DEFAULT_DAOSESSIONHANDLER_NAME];
				}

				// Configure the sessionHandler
				configurationScope.ErrorContext.ObjectId = sessionHandler.GetType().FullName;

				sessionHandler.Configure(configurationScope.Properties,  resources );

				daoManager.DaoSessionHandler = sessionHandler;

				configurationScope.ErrorContext.Resource = string.Empty;
				configurationScope.ErrorContext.MoreInfo = string.Empty;
				configurationScope.ErrorContext.ObjectId = string.Empty;
				#endregion

				#region Build Daos
				ParseDaoFactory(configurationScope, daoManager);
				#endregion

				#region Register DaoManager

				configurationScope.ErrorContext.MoreInfo = "register DaoManager";
				configurationScope.ErrorContext.ObjectId = daoManager.Name;

				DaoManager.RegisterDaoManager(daoManager.Name, daoManager);

				configurationScope.ErrorContext.Resource = string.Empty;
				configurationScope.ErrorContext.MoreInfo = string.Empty;
				configurationScope.ErrorContext.ObjectId = string.Empty;
				#endregion 
			}
		}

		/// <summary>
		/// Initialize the list of variables defined in the
		/// properties file.
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		private void ParseGlobalProperties(ConfigurationScope configurationScope)
		{
			XmlNode nodeProperties = configurationScope.NodeContext.SelectSingleNode("properties");

			configurationScope.ErrorContext.Resource = nodeProperties.InnerXml.ToString();
			configurationScope.ErrorContext.MoreInfo = "add global properties";

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
							configurationScope.Properties.Add( keyAttrib.Value,  valueAttrib.Value);
							_logger.Info( string.Format("Add property \"{0}\" value \"{1}\"",keyAttrib.Value,valueAttrib.Value) );
						}
						else
						{
							// Load the file defined by the attribute
							XmlDocument propertiesConfig = Resources.GetAsXmlDocument(propertyNode, configurationScope.Properties); 
							
							foreach (XmlNode node in propertiesConfig.SelectNodes("/settings/add"))
							{
								configurationScope.Properties[node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value] = node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value;
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

					configurationScope.ErrorContext.Resource = nodeProperties.OuterXml.ToString();

					// Load the file defined by the attribute
					XmlDocument propertiesConfig = Resources.GetAsXmlDocument(nodeProperties, configurationScope.Properties); 

					foreach (XmlNode node in propertiesConfig.SelectNodes("/settings/add"))
					{
						configurationScope.Properties[node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value] = node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value;
						_logger.Info( string.Format("Add property \"{0}\" value \"{1}\"",node.Attributes[PROPERTY_ELEMENT_KEY_ATTRIB].Value,node.Attributes[PROPERTY_ELEMENT_VALUE_ATTRIB].Value) );
					}					
				}
//				// Load the file defined by the resource attribut
//				XmlDocument propertiesConfig = Resources.GetAsXmlDocument(nodeProperties, configurationScope.Properties); 
//
//				foreach (XmlNode node in propertiesConfig.SelectNodes("/settings/add"))
//				{
//					configurationScope.Properties[node.Attributes["key"].Value] = node.Attributes["value"].Value;
//				}
			}

			configurationScope.ErrorContext.Resource = string.Empty;
			configurationScope.ErrorContext.MoreInfo = string.Empty;
		}


		/// <summary>
		/// Initialize the provider
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		/// <returns>A provider</returns>
		private Provider ParseProvider(ConfigurationScope configurationScope)
		{
			XmlAttribute attribute = null;
			XmlNode node = configurationScope.NodeContext.SelectSingleNode("//database/provider");

			configurationScope.ErrorContext.MoreInfo = "configure provider";

			if (node != null)
			{
				configurationScope.ErrorContext.Resource = node.OuterXml.ToString();
				// name
				attribute = node.Attributes["name"];

				configurationScope.ErrorContext.ObjectId = attribute.Value;

				if (configurationScope.Providers.Contains(attribute.Value) == true)
				{
					return (Provider)configurationScope.Providers[attribute.Value];
				}
				else
				{
					throw new ConfigurationException(
						string.Format("Error while configuring the Provider named \"{0}\" in the Context named \"{1}\".",
						attribute.Value, configurationScope.NodeContext.Attributes["name"].Value));
				}
			}
			else
			{
				if(configurationScope.Providers.Contains(DEFAULT_PROVIDER_NAME) == true)
				{
					return (Provider) configurationScope.Providers[DEFAULT_PROVIDER_NAME];
				}
				else
				{
					throw new ConfigurationException(
						string.Format("Error while configuring the Context named \"{0}\". There is no default provider.",
						configurationScope.NodeContext.Attributes["name"].Value));
				}
			}
		}

//		/// <summary>
//		/// Build a provider
//		/// </summary>
//		/// <param name="node"></param>
//		/// <returns></returns>
//		/// <remarks>
//		/// Not use, I use it to test if it faster than serializer.
//		/// But the tests are not concluant...
//		/// </remarks>
//		private static Provider BuildProvider(XmlNode node)
//		{
//			XmlAttribute attribute = null;
//			Provider provider = new Provider();
//
//			attribute = node.Attributes["assemblyName"];
//			provider.AssemblyName = attribute.Value;
//			attribute = node.Attributes["default"];
//			if (attribute != null)
//			{
//				provider.IsDefault = Convert.ToBoolean( attribute.Value );
//			}
//			attribute = node.Attributes["enabled"];
//			if (attribute != null)
//			{
//				provider.IsEnabled = Convert.ToBoolean( attribute.Value );
//			}
//			attribute = node.Attributes["connectionClass"];
//			provider.ConnectionClass = attribute.Value;
//			attribute = node.Attributes["UseParameterPrefixInSql"];
//			if (attribute != null)
//			{
//				provider.UseParameterPrefixInSql = Convert.ToBoolean( attribute.Value );
//			}
//			attribute = node.Attributes["useParameterPrefixInParameter"];
//			if (attribute != null)
//			{
//				provider.UseParameterPrefixInParameter = Convert.ToBoolean( attribute.Value );
//			}
//			attribute = node.Attributes["usePositionalParameters"];
//			if (attribute != null)
//			{
//				provider.UsePositionalParameters = Convert.ToBoolean( attribute.Value );
//			}
//			attribute = node.Attributes["commandClass"];
//			provider.CommandClass = attribute.Value;
//			attribute = node.Attributes["parameterClass"];
//			provider.ParameterClass = attribute.Value;
//			attribute = node.Attributes["parameterDbTypeClass"];
//			provider.ParameterDbTypeClass = attribute.Value;
//			attribute = node.Attributes["parameterDbTypeProperty"];
//			provider.ParameterDbTypeProperty = attribute.Value;
//			attribute = node.Attributes["dataAdapterClass"];
//			provider.DataAdapterClass = attribute.Value;
//			attribute = node.Attributes["commandBuilderClass"];
//			provider.CommandBuilderClass = attribute.Value;
//			attribute = node.Attributes["commandBuilderClass"];
//			provider.CommandBuilderClass = attribute.Value;
//			attribute = node.Attributes["name"];
//			provider.Name = attribute.Value;
//			attribute = node.Attributes["parameterPrefix"];
//			provider.ParameterPrefix = attribute.Value;
//
//			return provider;
//		}


		/// <summary>
		/// Build the data source object
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		/// <returns>A DataSource</returns>
		private DataSource ParseDataSource(ConfigurationScope configurationScope)
		{
			XmlSerializer serializer = null;
			DataSource dataSource = null;
			XmlNode node = configurationScope.NodeContext.SelectSingleNode("database/dataSource");

			configurationScope.ErrorContext.Resource = node.InnerXml.ToString();
			configurationScope.ErrorContext.MoreInfo = "configure data source";

			serializer = new XmlSerializer(typeof(DataSource));

			dataSource = (DataSource)serializer.Deserialize(new XmlNodeReader(node));

			dataSource.ConnectionString = Resources.ParsePropertyTokens(dataSource.ConnectionString, configurationScope.Properties);
			
			configurationScope.ErrorContext.Resource = string.Empty;
			configurationScope.ErrorContext.MoreInfo = string.Empty;
			
			return dataSource;
		}


		/// <summary>
		/// Parse dao factory tag
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		/// <param name="daoManager"></param>
		private void ParseDaoFactory(ConfigurationScope configurationScope, DaoManager daoManager)
		{
			XmlSerializer serializer = null;
			Dao dao = null;
			XmlNode xmlDaoFactory = null;

			xmlDaoFactory = configurationScope.NodeContext.SelectSingleNode("daoFactory");

			configurationScope.ErrorContext.Resource = xmlDaoFactory.InnerXml.ToString();
			configurationScope.ErrorContext.MoreInfo = "configure dao";

			serializer = new XmlSerializer(typeof(Dao));
			
			foreach (XmlNode node in xmlDaoFactory.SelectNodes("dao"))
			{
				dao = (Dao) serializer.Deserialize(new XmlNodeReader(node));
				
				configurationScope.ErrorContext.ObjectId = dao.Implementation;

				dao.Initialize(daoManager);
				daoManager.RegisterDao(dao);
			}

			configurationScope.ErrorContext.Resource = string.Empty;
			configurationScope.ErrorContext.MoreInfo = string.Empty;
			configurationScope.ErrorContext.ObjectId = string.Empty;
		}
		#endregion

	}
}
