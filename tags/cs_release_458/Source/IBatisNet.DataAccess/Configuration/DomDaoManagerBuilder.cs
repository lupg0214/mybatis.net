
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
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;

using IBatisNet.Common;
using IBatisNet.Common.Exceptions;    
using IBatisNet.Common.Utilities;
using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Exceptions;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess.DaoSessionHandlers;
#endregion

namespace IBatisNet.DataAccess.Configuration
{
	/// <summary>
	/// Summary description for DomDaoManagerBuilder.
	/// </summary>
	public class DomDaoManagerBuilder
	{
		#region Constants
		private const string DEFAULT_PROVIDER_NAME = "_DEFAULT_PROVIDER_NAME";
		private const string DEFAULT_DAOSESSIONHANDLER_NAME = "DEFAULT_DAOSESSIONHANDLER_NAME";
		/// <summary>
		/// Token for providers config file name.
		/// </summary>
		private const string PROVIDERS_FILE_NAME = "providers.config";
		#endregion

		#region Fields
		private HybridDictionary _providers = new HybridDictionary();
		private HybridDictionary _daoSectionHandlers = new HybridDictionary();
		private NameValueCollection  _properties = new NameValueCollection();
		private bool _useConfigFileWatcher = false;

		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public HybridDictionary Providers
		{
			get { return _providers; }
		}

		/// <summary>
		/// 
		/// </summary>
		public HybridDictionary DaoSectionHandlers
		{
			get { return _daoSectionHandlers; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Constructor.
		/// </summary>
		public DomDaoManagerBuilder()
		{
			Reset();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Build DaoManagers from config document.
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void BuildDaoManagers(XmlDocument document, bool useConfigFileWatcher)
		{

			_daoSectionHandlers.Add(DEFAULT_DAOSESSIONHANDLER_NAME, DaoSessionHandlerFactory.GetDaoSessionHandler("ADONET"));
			_daoSectionHandlers.Add("ADONET", DaoSessionHandlerFactory.GetDaoSessionHandler("ADONET"));
			_daoSectionHandlers.Add("SqlMap", DaoSessionHandlerFactory.GetDaoSessionHandler("SqlMap"));

			_useConfigFileWatcher = useConfigFileWatcher;

			GetConfig(document);
		}


		/// <summary>
		/// Reset globals variables
		/// </summary>
		internal void Reset()
		{
			_providers.Clear();
			_daoSectionHandlers.Clear();
		}


		private void GetConfig(XmlDocument daoConfig)
		{
			XmlNode section = null;

			section = daoConfig.SelectSingleNode("daoConfig");

			GetProviders(Resources.GetConfigAsXmlDocument(PROVIDERS_FILE_NAME));

			GetDaoSessionHandlers(section.SelectSingleNode("daoSessionHandlers"));
			GetContexts(section);
		}

		
		/// <summary>
		/// Load and initialize providers from specified file.
		/// </summary>
		/// <param name="xmlProviders"></param>
		private void GetProviders(XmlDocument xmlProviders)
		{
			XmlSerializer serializer = null;
			Provider provider = null;
			string directoryName = string.Empty;

			serializer = new XmlSerializer(typeof(Provider));

			foreach (XmlNode node in xmlProviders.SelectNodes("/providers/provider"))
			{
				provider = (Provider) serializer.Deserialize(new XmlNodeReader(node));

				if (provider.IsEnabled == true)
				{
					provider.Initialisation();
					_providers.Add(provider.Name,provider);
					if (provider.IsDefault == true)
					{
						if (_providers[DEFAULT_PROVIDER_NAME] == null) 
						{
							_providers.Add(DEFAULT_PROVIDER_NAME,provider);
						} 
						else 
						{
							throw new ConfigurationException(
								string.Format("Error while configuring the Provider named \"{0}\" There can be only one default Provider.",provider.Name));
						}
					}
				}
			}
		}


		private void GetDaoSessionHandlers(XmlNode daoSessionHandlersNode)
		{
			XmlSerializer serializer = null;

			serializer = new XmlSerializer(typeof(DaoSessionHandler));

			if (daoSessionHandlersNode != null)
			{
				foreach (XmlNode node in daoSessionHandlersNode.SelectNodes("handler"))
				{
					DaoSessionHandler daoSessionHandler =(DaoSessionHandler) serializer.Deserialize(new XmlNodeReader(node));
				
					IDaoSessionHandler sessionHandler = daoSessionHandler.GetIDaoSessionHandler();

					_daoSectionHandlers[daoSessionHandler.Name] = sessionHandler;

					if (daoSessionHandler.IsDefault == true)
					{
						_daoSectionHandlers[DEFAULT_DAOSESSIONHANDLER_NAME] = sessionHandler;
					}
				}
			}
		}


		private void GetContexts(XmlNode section)
		{
			DaoManager daoManager;
			XmlAttribute attribute;

			// Init
			DaoManager.Reset();

			// Build one daoManager for each context
			foreach (XmlNode node in section.SelectNodes("context"))
			{
				daoManager = DaoManager.NewInstance();

				// name
				attribute = node.Attributes["id"];
				daoManager.Name = attribute.Value;

				// default
				attribute = node.Attributes["default"];
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

				#region Properties
				ParseGlobalProperties(node);
				#endregion

				#region provider
				daoManager.Provider = ParseProvider(node);
				#endregion

				#region DataSource 
				daoManager.DataSource = ParseDataSource(node);
				daoManager.DataSource.Provider = daoManager.Provider;
				#endregion

				#region DaoSessionHandler

				XmlNode nodeSessionHandler = node.SelectSingleNode("daoSessionHandler");
				IDictionary properties = new Hashtable();
				// By default, add the DataSource
				properties.Add( "DataSource", daoManager.DataSource);
				// By default, add the useConfigFileWatcher
				properties.Add( "UseConfigFileWatcher", _useConfigFileWatcher);

				IDaoSessionHandler sessionHandler = null;

				if (nodeSessionHandler!= null)
				{
					//daoSessionHandler = (DaoSessionHandler)DomDaoManagerBuilder.DaoSectionHandlers[nodeSessionHandler.Attributes["name"].Value];
					
					sessionHandler = (IDaoSessionHandler)_daoSectionHandlers[nodeSessionHandler.Attributes["id"].Value];

					// Parse property node
					foreach(XmlNode nodeProperty in nodeSessionHandler.SelectNodes("property"))
					{
						properties.Add(nodeProperty.Attributes["name"].Value, 
							Resources.ParsePropertyTokens(nodeProperty.Attributes["value"].Value, _properties));
					}
				}
				else
				{
					//daoSessionHandler = (DaoSessionHandler)DomDaoManagerBuilder.DaoSectionHandlers[DEFAULT_DAOSESSIONHANDLER_NAME];
					sessionHandler = (IDaoSessionHandler)_daoSectionHandlers[DEFAULT_DAOSESSIONHANDLER_NAME];
				}

				//IDaoSessionHandler sessionHandler = daoSessionHandler.GetIDaoSessionHandler();
				// Configure the sessionHandler
				sessionHandler.Configure(properties);

				daoManager.DaoSessionHandler = sessionHandler;

				#endregion

				#region Daos
				ParseDaoFactory(node,daoManager);
				#endregion

				DaoManager.RegisterDaoManager(daoManager.Name, daoManager);

			}
		}

		/// <summary>
		/// Initialize the list of variables defined in the
		/// properties file.
		/// </summary>
		/// <param name="xmlContext">The current context being analysed.</param>
		private void ParseGlobalProperties(XmlNode xmlContext)
		{
			XmlNode nodeProperties = xmlContext.SelectSingleNode("properties");

			if (nodeProperties != null)
			{
				// Load the file defined by the resource attribut
				XmlDocument propertiesConfig = Resources.GetAsXmlDocument(nodeProperties); 

				foreach (XmlNode node in propertiesConfig.SelectNodes("/settings/add"))
				{
					_properties[node.Attributes["key"].Value] = node.Attributes["value"].Value;
				}
			}
		}


		private Provider ParseProvider(XmlNode xmlContext)
		{
			XmlAttribute attribute = null;
			XmlNode node = xmlContext.SelectSingleNode("database/provider");

			if (node != null)
			{
				// name
				attribute = node.Attributes["name"];

				if (_providers.Contains(attribute.Value) == true)
				{
					return (Provider)_providers[attribute.Value];
				}
				else
				{
					throw new ConfigurationException(
						string.Format("Error while configuring the Provider named \"{0}\" in the Context named \"{1}\".",attribute.Value,xmlContext.Attributes["name"].Value));
				}
			}
			else
			{
				if(_providers.Contains(DEFAULT_PROVIDER_NAME) == true)
				{
					return (Provider)_providers[DEFAULT_PROVIDER_NAME];
				}
				else
				{
					throw new ConfigurationException(
						string.Format("Error while configuring the Context named \"{0}\". There is no default provider.",xmlContext.Attributes["name"].Value));
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


		private DataSource ParseDataSource(System.Xml.XmlNode xmlContext)
		{
			XmlSerializer serializer = null;
			DataSource dataSource = null;
			XmlNode node = xmlContext.SelectSingleNode("database/dataSource");

			serializer = new XmlSerializer(typeof(DataSource));

			dataSource = (DataSource)serializer.Deserialize(new XmlNodeReader(node));

			dataSource.ConnectionString = Resources.ParsePropertyTokens(dataSource.ConnectionString, _properties);
			return dataSource;
		}


		private void ParseDaoFactory(System.Xml.XmlNode xmlContext, DaoManager daoManager)
		{
			XmlSerializer serializer = null;
			Dao dao = null;
			XmlNode xmlDaoFactory = null;

			xmlDaoFactory = xmlContext.SelectSingleNode("daoFactory");

			string currentDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().CodeBase.Replace(@"file:///",""));

			serializer = new XmlSerializer(typeof(Dao));
			
			foreach (XmlNode node in xmlDaoFactory.SelectNodes("dao"))
			{
				dao = (Dao) serializer.Deserialize(new XmlNodeReader(node));
				try
				{
					dao.Initialize(daoManager);
					daoManager.RegisterDao(dao);
				}
				catch(Exception e)
				{
					throw new ConfigurationException(string.Format("DaoManager could not configure DaoFactory. Cause: {1}", e.Message), e);
				}
			}
		}
		#endregion

	}
}
