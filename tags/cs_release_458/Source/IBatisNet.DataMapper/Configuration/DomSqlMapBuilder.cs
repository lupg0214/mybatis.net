
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
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using IBatisNet.Common;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;

using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper;

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
using IBatisNet.DataMapper.TypesHandler;
using IBatisNet.DataMapper.MappedStatements;
#endregion

namespace IBatisNet.DataMapper.Configuration
{
	/// <summary>
	/// Builds SqlMapClient instances from a supplied resource (e.g. XML configuration file)
	/// </summary>
	public class DomSqlMapBuilder
	{
		#region Constant
		private const string DEFAULT_PROVIDER_NAME = "_DEFAULT_PROVIDER_NAME";
		private const string DOT = ".";
		private const string PARAMETER_TOKEN = "#";

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
		/// Token for xml path to SqlMapConfig assembly element.
		/// </summary>
		private const string XML_CONFIG_ASSEMBLY = "/sqlMapConfig/assembly";

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
		/// Token for assemblyResource attribute.
		/// </summary>
		private const string ATR_ASSEMBLY_RESOURCE = "assemblyResource";

		#endregion

		#region Fields 

		// These variables maintain the state of the build process.
		private XmlDocument _sqlMapConfig = null;
		private bool _isCallFromDao = false;
		private bool _useConfigFileWatcher = false;
		private DataSource _dataSource = null;
		private bool _useStatementNamespaces = false;
		private bool _cacheModelsEnabled = false;
		private string _assemblyResource = string.Empty;

		private HybridDictionary _providers = new HybridDictionary();
		private NameValueCollection  _properties = new NameValueCollection();
		private SqlMapper _sqlMap = null;

		// static private Assembly _assembly = null;

		#endregion 
		 
		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public HybridDictionary Providers
		{
			get { return _providers; }
		}

		#endregion

		#region Methods
		/// <summary>
		/// Load statement, parameters, resultmap.
		/// Used by Dao
		/// </summary>
		public SqlMapper Build(XmlDocument document, DataSource dataSource, bool useConfigFileWatcher)
		{
			_sqlMapConfig = document;
			_dataSource = dataSource;
			_isCallFromDao = true;
			_useConfigFileWatcher = useConfigFileWatcher;
			_sqlMap = null;

			// Load sqlMap statement.
			return Initialize();
		}


		/// <summary>
		/// Load SqlMap configuration from
		/// from the XmlDocument passed in parameter.
		/// </summary>
		/// <param name="document">The xml sql map configuration.</param>
		/// <param name="useConfigFileWatcher"></param>
		public SqlMapper Build(XmlDocument document, bool useConfigFileWatcher)
		{
			_sqlMapConfig= document;
			_dataSource = null;
			_isCallFromDao = false;
			_useConfigFileWatcher = useConfigFileWatcher;
			_sqlMap = null;

			// Load DatAsource, provider & sqlMaps statement.
			return Initialize();
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
		private SqlMapper Initialize()
		{
			Reset();

			SqlMapper sqlMap = new SqlMapper();
			_sqlMap = sqlMap;

			#region Load settings

			XmlNodeList settings = _sqlMapConfig.SelectNodes(XML_CONFIG_SETTINGS);

			if (settings!=null)
			{
				foreach (XmlNode setting in settings)
				{
					if (setting.Attributes[ATR_USE_STATEMENT_NAMESPACES] != null )
					{				
						_useStatementNamespaces =  System.Convert.ToBoolean(setting.Attributes[ATR_USE_STATEMENT_NAMESPACES].Value); 
					}
					if (setting.Attributes[ATR_CACHE_MODELS_ENABLED] != null )
					{				
						_cacheModelsEnabled =  System.Convert.ToBoolean(setting.Attributes[ATR_CACHE_MODELS_ENABLED].Value); 
					}
					#region Embedded resource

					// Which files must we allow to be used as Embedded Resources ?
					// - slqMap.config [No]
					// - providers.config [No]
					// - sqlMap files [yes]
					// - properties file (like Database.config) [Yes]
					// see contribution, NHibernate usage,
					// see http://www.codeproject.com/csharp/EmbeddedResourceStrings.asp
					// see http://www.devhood.com/tutorials/tutorial_details.aspx?tutorial_id=75
					if (setting.Attributes[ATR_ASSEMBLY_RESOURCE] != null )
					{
						_assemblyResource = setting.Attributes[ATR_ASSEMBLY_RESOURCE].Value;
					}
					#endregion
				}
			}

			sqlMap.SetCacheModelsEnabled(_cacheModelsEnabled);

			#endregion

			#region Load the assembly

			// Which files must we allow to be used as Embedded Resources ?
			// - slqMap.config [No]
			// - providers.config [No]
			// - sqlMap files [yes]
			// - properties file (like Database.config) [Yes]
			// see contribution, NHibernate usage,
			// see http://www.codeproject.com/csharp/EmbeddedResourceStrings.asp
			// see http://www.devhood.com/tutorials/tutorial_details.aspx?tutorial_id=75
			XmlNode assemblyNode = _sqlMapConfig.SelectSingleNode(XML_CONFIG_ASSEMBLY);
			if (assemblyNode != null)
			{
				try
				{
					Assembly.Load(assemblyNode.InnerText);
				}
				catch (Exception e)
				{
					throw new ConfigurationException(
						string.Format("Unable to load assembly \"{0}\". Cause : ", e.Message  ) ,e);
				}
			}

			#endregion

			#region Load Global Properties
			if (_isCallFromDao == false)
			{
				ParseGlobalProperties(_sqlMapConfig.SelectSingleNode(XML_CONFIG_ROOT));
			}
			#endregion

			#region Load providers
			if (_isCallFromDao == false)
			{
				GetProviders(Resources.GetConfigAsXmlDocument(PROVIDERS_FILE_NAME));
			}
			#endregion

			#region Load DataBase
			#region Choose the  provider
			Provider provider = null;
			if ( _isCallFromDao==false )
			{
				XmlNode providerNode = null;

				providerNode = _sqlMapConfig.SelectSingleNode("/sqlMapConfig/database/provider");

				provider = ParseProvider(providerNode);
			}
			#endregion

			#region Load the DataSources

			XmlNode nodeDataSource = _sqlMapConfig.SelectSingleNode("/sqlMapConfig/database/dataSource");
			if (nodeDataSource == null)
			{
				if (_isCallFromDao == false)
				{
					throw new ConfigurationException("There's no dataSource tag in SqlMap.config.");
				}
				else  // patch from Luke Yang
				{
					sqlMap.DataSource = _dataSource;
				}
			}
			else
			{
				if (_isCallFromDao == false)
				{
					XmlSerializer serializer = null;
					serializer = new XmlSerializer(typeof(DataSource));
					DataSource dataSource = (DataSource) serializer.Deserialize(new XmlNodeReader(nodeDataSource));

					dataSource.Provider = provider;
					// Use Global Properties if any
					dataSource.ConnectionString = Resources.ParsePropertyTokens(dataSource.ConnectionString, _properties);

					sqlMap.DataSource = dataSource;
				}
				else
				{
					sqlMap.DataSource = _dataSource;
				}
			}
			#endregion
			#endregion

			#region Load Global TypeAlias
			foreach (XmlNode xmlNode in _sqlMapConfig.SelectNodes("/sqlMapConfig/alias/typeAlias"))
			{
				TypeAlias typeAlias = null;
				XmlSerializer serializer = new XmlSerializer(typeof(TypeAlias));

				typeAlias = (TypeAlias) serializer.Deserialize(new XmlNodeReader(xmlNode));
				typeAlias.Initialize();

				sqlMap.AddTypeAlias( typeAlias.Name, typeAlias );
			}
			#endregion

			#region Load the mapping files
			
			foreach (XmlNode xmlNode in _sqlMapConfig.SelectNodes("/sqlMapConfig/sqlMaps/sqlMap"))
			{
				ConfigureSqlMap(sqlMap, xmlNode );
			}

			#endregion

			#region Resolve "resulMap" attribut on ResultProperty

			foreach(DictionaryEntry entry in sqlMap.ResultMaps)
			{
				ResultMap resultMap = (ResultMap)entry.Value;
				foreach(DictionaryEntry item in resultMap.ColumnsToPropertiesMap)
				{
					ResultProperty property = (ResultProperty)item.Value;
					if(property.ResulMapName.Length >0)
					{
						property.ResultMap = sqlMap.GetResultMap(property.ResulMapName);
					}
				}
			}

			#endregion

			return sqlMap;
		}

		/// <summary>
		/// Load and initialize providers from specified file.
		/// </summary>
		/// <param name="xmlProviders"></param>
		private void GetProviders(XmlDocument xmlProviders)
		{
			XmlSerializer serializer = null;
			Provider provider = null;

			_providers.Clear();

			serializer = new XmlSerializer(typeof(Provider));

			foreach (XmlNode node in xmlProviders.SelectNodes("/providers/provider"))
			{
				provider = (Provider) serializer.Deserialize(new XmlNodeReader(node));

				if (provider.IsEnabled == true)
				{
					provider.Initialisation();
					_providers.Add(provider.Name, provider);

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


		/// <summary>
		/// Parse the provider tag.
		/// </summary>
		/// <remarks>
		/// Could be specified via Global propertie in properties file (see properties tag).
		/// </remarks>
		/// <param name="node">An xml provider node</param>
		/// <returns>A provider object.</returns>
		private Provider ParseProvider(System.Xml.XmlNode node)
		{
			if (node != null)
			{
				// name
				string providerName = Resources.ParsePropertyTokens(node.Attributes["name"].Value, _properties);

				if (_providers.Contains(providerName) == true)
				{
					return (Provider) _providers[providerName];
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
				if (_providers.Contains(DEFAULT_PROVIDER_NAME) == true)
				{
					return (Provider) _providers[DEFAULT_PROVIDER_NAME];
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
		/// <param name="sqlMap">An SqlMap</param>
		/// <param name="sqlMapNode"></param>
		private void ConfigureSqlMap(SqlMapper sqlMap, XmlNode sqlMapNode)
		{
			XmlSerializer serializer = null;
			string sqlMapName = string.Empty;

			if (_useConfigFileWatcher == true)
			{
				ConfigWatcherHandler.AddFileToWatch( Resources.GetFileInfo( Resources.GetValueOfNodeResourceUrl(sqlMapNode) ) );
			}

			// Load the file 
			XmlDocument config = Resources.GetAsXmlDocument(sqlMapNode);

			sqlMapName = config.SelectSingleNode("sqlMap").Attributes["namespace"].Value;

			#region Load TypeAlias

			foreach (XmlNode xmlNode in config.SelectNodes("/sqlMap/alias/typeAlias"))
			{
				TypeAlias typeAlias = null;
				serializer = new XmlSerializer(typeof(TypeAlias));
				typeAlias = (TypeAlias) serializer.Deserialize(new XmlNodeReader(xmlNode));
				typeAlias.Initialize();

				sqlMap.AddTypeAlias( typeAlias.Name, typeAlias );
			}

			#endregion

			#region Load resultMap

//			ResultMap resultMap = null;
//			serializer = new XmlSerializer(typeof(ResultMap));
			foreach (XmlNode xmlNode in config.SelectNodes("/sqlMap/resultMaps/resultMap"))
			{
				BuildResultMap( config, xmlNode, sqlMapName, sqlMap );
//				resultMap = (ResultMap) serializer.Deserialize(new XmlNodeReader(xmlNode));
//				resultMap.Initialize( sqlMap, xmlNode);
//
//				resultMap.Id = sqlMapName + DOT + resultMap.Id;
//				if (resultMap.ExtendMap.Length >0)
//				{
//					resultMap.ExtendMap = sqlMapName + DOT + resultMap.ExtendMap;
//				}
//
//				sqlMap.AddResultMap( resultMap );
			}

			#endregion

			#region Load parameterMaps

			foreach (XmlNode xmlNode in config.SelectNodes("/sqlMap/parameterMaps/parameterMap"))
			{
				BuildParameterMap(config, xmlNode, sqlMapName, sqlMap );
			}

			#endregion
		
			#region Load statements

			#region Statement tag
			Statement statement = null;
			serializer = new XmlSerializer(typeof(Statement));
			foreach (XmlNode xmlNode in config.SelectNodes("/sqlMap/statements/statement"))
			{
				MappedStatement mappedStatement = null;

				statement = (Statement) serializer.Deserialize(new XmlNodeReader(xmlNode));
				if (_useStatementNamespaces == true)
				{
					statement.Id = ApplyNamespace(statement.Id, sqlMapName);
				}
				statement.Initialize( sqlMapName, sqlMap );

				// Build ISql (analyse sql statement)		
				ProcessSqlStatement(config, sqlMapName, sqlMap, xmlNode, statement);

				// Build MappedStatement
				mappedStatement = new MappedStatement( sqlMap, statement);

				sqlMap.AddMappedStatement(mappedStatement.Name, mappedStatement);
			}
			#endregion

			#region Select tag
			Select select = null;
			serializer = new XmlSerializer(typeof(Select));
			foreach (XmlNode xmlNode in config.SelectNodes("/sqlMap/statements/select"))
			{
				MappedStatement mappedStatement = null;

				select = (Select) serializer.Deserialize(new XmlNodeReader(xmlNode));
				if (_useStatementNamespaces == true)
				{
					select.Id = ApplyNamespace(select.Id, sqlMapName);
				}
				select.Initialize( sqlMapName, sqlMap );

				if (select.Generate != null)
				{
					GenerateCommandText(sqlMap, select);
				}
				else
				{
					// Build ISql (analyse sql statement)		
					ProcessSqlStatement(config, sqlMapName, sqlMap, xmlNode, select);
				}

				// Build MappedStatement
				mappedStatement = new SelectMappedStatement( sqlMap, select);

				sqlMap.AddMappedStatement(mappedStatement.Name, mappedStatement);
			}
			#endregion

			#region Insert tag
			Insert insert = null;
			serializer = new XmlSerializer(typeof(Insert));
			foreach (XmlNode xmlNode in config.SelectNodes("/sqlMap/statements/insert"))
			{
				MappedStatement mappedStatement = null;

				insert = (Insert) serializer.Deserialize(new XmlNodeReader(xmlNode));
				if (_useStatementNamespaces == true)
				{
					insert.Id = ApplyNamespace(insert.Id, sqlMapName);
				}

				insert.Initialize( sqlMapName, sqlMap );

				// Build ISql (analyse sql command text)
				if (insert.Generate != null)
				{
					GenerateCommandText(sqlMap, insert);
				}
				else
				{
					ProcessSqlStatement(config, sqlMapName, sqlMap, xmlNode, insert);
				}

				// Build MappedStatement
				mappedStatement = new InsertMappedStatement( sqlMap, insert);

				sqlMap.AddMappedStatement(mappedStatement.Name, mappedStatement);

				#region statement SelectKey
				// Set sql statement SelectKey 
				if (insert.SelectKey != null)
				{
					insert.SelectKey.Initialize( sqlMapName, sqlMap );
					insert.SelectKey.Id = insert.Id + DOT + "SelectKey";
					string commandText = xmlNode.SelectSingleNode("selectKey").FirstChild.InnerText.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ').Trim();
					StaticSql sql = new StaticSql(insert.SelectKey);
					IDalSession session = new SqlMapSession(sqlMap.DataSource);
					sql.BuildPreparedStatement( session, commandText );
					insert.SelectKey.Sql = sql;					
					
					// Build MappedStatement
					mappedStatement = new MappedStatement( sqlMap, insert.SelectKey);

					sqlMap.AddMappedStatement(mappedStatement.Name, mappedStatement);
				}
				#endregion
			}
			#endregion

			#region Update tag
			Update update = null;
			serializer = new XmlSerializer(typeof(Update));
			foreach (XmlNode xmlNode in config.SelectNodes("/sqlMap/statements/update"))
			{
				MappedStatement mappedStatement = null;

				update = (Update) serializer.Deserialize(new XmlNodeReader(xmlNode));
				if (_useStatementNamespaces == true)
				{
					update.Id = ApplyNamespace(update.Id, sqlMapName);
				}
				update.Initialize( sqlMapName, sqlMap );

				// Build ISql (analyse sql statement)	
				if (update.Generate != null)
				{
					GenerateCommandText(sqlMap, update);
				}
				else
				{
					// Build ISql (analyse sql statement)		
					ProcessSqlStatement(config, sqlMapName, sqlMap, xmlNode, update);
				}	

				// Build MappedStatement
				mappedStatement = new UpdateMappedStatement( sqlMap, update);

				sqlMap.AddMappedStatement(mappedStatement.Name, mappedStatement);
			}
			#endregion

			#region Delete tag
			Delete delete = null;
			serializer = new XmlSerializer(typeof(Delete));
			foreach (XmlNode xmlNode in config.SelectNodes("/sqlMap/statements/delete"))
			{
				MappedStatement mappedStatement = null;

				delete = (Delete) serializer.Deserialize(new XmlNodeReader(xmlNode));
				if (_useStatementNamespaces == true)
				{
					delete.Id = ApplyNamespace(delete.Id, sqlMapName);
				}
				delete.Initialize( sqlMapName, sqlMap );

				// Build ISql (analyse sql statement)
				if (delete.Generate != null)
				{
					GenerateCommandText(sqlMap, delete);
				}
				else
				{
					// Build ISql (analyse sql statement)		
					ProcessSqlStatement(config, sqlMapName, sqlMap, xmlNode, delete);
				}	

				// Build MappedStatement
				mappedStatement = new DeleteMappedStatement( sqlMap, delete);

				sqlMap.AddMappedStatement(mappedStatement.Name, mappedStatement);
			}
			#endregion

			#region Procedure tag
			Procedure procedure = null;
			serializer = new XmlSerializer(typeof(Procedure));
			foreach (XmlNode xmlNode in config.SelectNodes("/sqlMap/statements/procedure"))
			{
				MappedStatement mappedStatement = null;

				procedure = (Procedure)serializer.Deserialize(new XmlNodeReader(xmlNode));
				if (_useStatementNamespaces == true)
				{
					procedure.Id = ApplyNamespace(procedure.Id, sqlMapName);
				}
				procedure.Initialize( sqlMapName, sqlMap );

				// Build ISql (analyse sql command text)
				ProcessSqlStatement(config, sqlMapName, sqlMap, xmlNode, procedure);

				// Build MappedStatement
				mappedStatement = new MappedStatement( sqlMap, procedure);

				sqlMap.AddMappedStatement(mappedStatement.Name, mappedStatement);
			}
			#endregion

			#endregion

			#region Load CacheModels

			if (sqlMap.IsCacheModelsEnabled == true)
			{
				CacheModel cacheModel = null;
				serializer = new XmlSerializer(typeof(CacheModel));
				foreach (XmlNode xmlNode in config.SelectNodes("/sqlMap/cacheModels/cacheModel"))
				{
					cacheModel = (CacheModel) serializer.Deserialize(new XmlNodeReader(xmlNode));

					// Attach ExecuteEventHandler
					foreach(XmlNode flushOn in xmlNode.SelectNodes("flushOnExecute"))
					{
						string statementName = flushOn.Attributes["statement"].Value;
						if (_useStatementNamespaces == true)
						{
							statementName = sqlMapName + DOT + statementName;
						}

						MappedStatement mappedStatement = sqlMap.GetMappedStatement(statementName);

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

					sqlMap.AddCache( cacheModel );
				}

				// Attach CacheModel to statement
				foreach(DictionaryEntry entry in sqlMap.MappedStatements)
				{
					MappedStatement mappedStatement = (MappedStatement)entry.Value;
					if (mappedStatement.Statement.CacheModelName.Length >0)
					{
						mappedStatement.Statement.CacheModel = sqlMap.GetCache(mappedStatement.Statement.CacheModelName);
					}
				}
			}

			#endregion

		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="config"></param>
		/// <param name="sqlMapName"></param>
		/// <param name="sqlMap"></param>
		/// <param name="commandTextNode"></param>
		/// <param name="statement"></param>
		private void ProcessSqlStatement(XmlDocument config, string sqlMapName, SqlMapper sqlMap, XmlNode commandTextNode, IStatement statement) 
		{
			bool isDynamic = false;
			DynamicSql dynamic = new DynamicSql(statement);
			StringBuilder sqlBuffer = new StringBuilder();

			// Resolve "extend" attribute on Statement
			if (statement.ExtendSatement.Length >0)
			{
				// Find 'super' statement
				XmlNode supperStatementNode = config.SelectSingleNode("/sqlMap/statements/child::*[@id='"+statement.ExtendSatement+"']");
				if (supperStatementNode!=null)
				{
					commandTextNode.InnerXml = supperStatementNode.InnerXml + commandTextNode.InnerXml;
				}
				else
				{
					throw new ConfigurationException("Unable to find extend statement named '"+statement.ExtendSatement+"' on statement '"+statement.Id+"'.'");
				}
			}

			isDynamic = ParseDynamicTags(commandTextNode, dynamic, sqlBuffer, isDynamic, false);

			if (isDynamic) 
			{
				statement.Sql = dynamic;
			} 
			else 
			{	
				string sqlText = sqlBuffer.ToString();
				ApplyInlineParemeterMap(sqlMap, statement, sqlText);
			}
		}

				
		/// <summary>
		/// 
		/// </summary>
		/// <param name="commandTextNode"></param>
		/// <param name="dynamic"></param>
		/// <param name="sqlBuffer"></param>
		/// <param name="isDynamic"></param>
		/// <param name="postParseRequired"></param>
		/// <returns></returns>
		private bool ParseDynamicTags(XmlNode commandTextNode, IDynamicParent dynamic, 
			StringBuilder sqlBuffer, bool isDynamic, bool postParseRequired) 
		{
			XmlNodeList children = commandTextNode.ChildNodes;
			for (int i = 0; i < children.Count; i++) 
			{
				XmlNode child = children[i];
				if ( (child.NodeType == XmlNodeType.CDATA) || (child.NodeType == XmlNodeType.Text) )
				{
					string data = child.InnerText.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ').Trim(); //??
					//data = ParsePropertyTokens(data);

					SqlText sqlText = null;
					if ( postParseRequired == true ) 
					{
						sqlText = new SqlText();
						sqlText.Text = data.ToString();
					} 
					else 
					{
						sqlText = ParseInlineParameterMap(data);
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
		/// 
		/// </summary>
		/// <param name="sqlMap"></param>
		/// <param name="statement"></param>
		/// <param name="sqlStatement"></param>
		private void ApplyInlineParemeterMap(SqlMapper sqlMap, IStatement statement, string sqlStatement)
		{
			string newSql = sqlStatement;

			// Check the inline parameter
			if (statement.ParameterMap == null)
			{
				// Construit Une parameter Map avec les inline Parameter
				// si il existe, et vire les infos 'inline' du sqlText
				// ParseInlineParameter devrait retourner une ParameterMap ou null
				
				SqlText sqlText = ParseInlineParameterMap( statement, newSql );

				if (sqlText.Parameters.Length > 0)
				{
					ParameterMap map = new ParameterMap();
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
				sql = new SimpleDynamicSql(newSql, statement);
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
					IDalSession session = new SqlMapSession(sqlMap.DataSource);

					((StaticSql)sql).BuildPreparedStatement( session, newSql );
				}					
			}
			statement.Sql = sql;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sqlStatement"></param>
		/// <returns></returns>
		public static SqlText ParseInlineParameterMap(string sqlStatement) 
		{
			return ParseInlineParameterMap(null, sqlStatement);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="statement"></param>
		/// <param name="sqlStatement"></param>
		/// <returns></returns>
		private static SqlText ParseInlineParameterMap(IStatement statement, string sqlStatement )
		{
			SqlText sqlText = new SqlText();
			string newSql = sqlStatement;
			Type parameterClass = null;

			if (statement != null)
			{
				parameterClass = statement.ParameterClass;
			}

			ArrayList mappingList = new ArrayList();

			StringTokenizer parser = new StringTokenizer(sqlStatement, PARAMETER_TOKEN, true);
			StringBuilder newSqlBuffer = new StringBuilder();

			string token = null;
			string lastToken = null;

			IEnumerator enumerator = parser.GetEnumerator();

			while (enumerator.MoveNext()) 
			{
				token = (string)enumerator.Current;

				if (PARAMETER_TOKEN.Equals(lastToken)) 
				{
					if (PARAMETER_TOKEN.Equals(token)) 
					{
						newSqlBuffer.Append(PARAMETER_TOKEN);
						token = null;
					} 
					else 
					{
						if (token.IndexOf(':') > -1) 
						{
							StringTokenizer paramParser = new StringTokenizer(token, ":", true);
							IEnumerator enumeratorParam = paramParser.GetEnumerator();

							int n1 = paramParser.TokenNumber;
							if (n1 == 3) 
							{
								enumeratorParam.MoveNext();
								string propertyName = ((string)enumeratorParam.Current).Trim();
								enumeratorParam.MoveNext();
								enumeratorParam.MoveNext(); //ignore ":"
								string dBType = ((string)enumeratorParam.Current).Trim();
								ParameterProperty mapping = new ParameterProperty();
								mapping.PropertyName = propertyName;
								mapping.DbType = dBType;
								ITypeHandler handler = null;
								if (parameterClass == null) 
								{
									handler = null; //TypeHandlerFactory.getUnkownTypeHandler();
								} 
								else 
								{
									handler = ResolveTypeHandler(parameterClass, propertyName, null, null);
										//TypeHandlerFactory.GetTypeHandler(parameterClass);
										//
								}
								mapping.TypeHandler = handler;
								mapping.Initialize();
								mappingList.Add(mapping);
							} 
							else if (n1 >= 5) 
							{
								enumeratorParam.MoveNext();
								string propertyName = ((string)enumeratorParam.Current).Trim();
								enumeratorParam.MoveNext();
								enumeratorParam.MoveNext(); //ignore ":"
								string dBType = ((string)enumeratorParam.Current).Trim();
								enumeratorParam.MoveNext();
								enumeratorParam.MoveNext(); //ignore ":"
								string nullValue = ((string)enumeratorParam.Current).Trim();
								while (enumeratorParam.MoveNext()) 
								{
									nullValue = nullValue + ((string)enumeratorParam.Current).Trim();
								}
								ParameterProperty mapping = new ParameterProperty();
								mapping.PropertyName = propertyName;
								mapping.DbType = dBType;
								mapping.NullValue = nullValue;
								ITypeHandler handler;
								if (parameterClass == null) 
								{
									handler = null;//TypeHandlerFactory.getUnkownTypeHandler();
								} 
								else 
								{
									handler = ResolveTypeHandler(parameterClass, propertyName, null, null);
										//TypeHandlerFactory.GetTypeHandler(parameterClass);
										//
								}
								mapping.TypeHandler = handler;
								mapping.Initialize();
								mappingList.Add(mapping);
							} 
							else 
							{
								throw new ConfigurationException("Incorrect inline parameter map format: " + token);
							}
						} 
						else 
						{
							ParameterProperty mapping = new ParameterProperty();
							mapping.PropertyName = token;
							ITypeHandler handler;
							if (parameterClass == null) 
							{
								handler = null;
									//TypeHandlerFactory.getUnkownTypeHandler();
							} 
							else 
							{
								handler = ResolveTypeHandler(parameterClass, token, null, null);
									//TypeHandlerFactory.GetTypeHandler(parameterClass);
									//
							}
							mapping.TypeHandler = handler;
							mapping.Initialize();
							mappingList.Add(mapping);
						}
						newSqlBuffer.Append("? ");
						
						enumerator.MoveNext();
						token = ((string)enumerator.Current).Trim();
						if (!PARAMETER_TOKEN.Equals(token)) 
						{
							throw new ConfigurationException("Unterminated inline parameter in mapped statement (" + statement.Id + ").");
						}
						token = null;
					}
				} 
				else 
				{
					if (!PARAMETER_TOKEN.Equals(token)) 
					{
						newSqlBuffer.Append(token);
					}
				}

				lastToken = token;
			}

			newSql = newSqlBuffer.ToString();
			sqlText.Text = newSql;

			ParameterProperty[] mappingArray = (ParameterProperty[]) mappingList.ToArray(typeof(ParameterProperty));
			sqlText.Parameters = mappingArray;

			return sqlText;
		}
		

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyName"></param>
		/// <param name="propertyType"></param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		private static ITypeHandler ResolveTypeHandler(Type type, string propertyName, 
			string propertyType, string dbType) 
		{
			ITypeHandler handler = null;

			if (type == null) 
			{
				handler = null;//TypeHandlerFactory.getUnkownTypeHandler();
			} 
			else if (typeof(IDictionary).IsAssignableFrom(type))//java.util.Map.class.isAssignableFrom(clazz)) 
			{
				if (propertyType == null) 
				{
					handler = TypeHandlerFactory.GetTypeHandler(typeof(object), dbType);
				} 
				else 
				{
					try 
					{
						Type typeClass = Resources.TypeForName( propertyType );
						handler = TypeHandlerFactory.GetTypeHandler(typeClass, dbType);
					} 
					catch (Exception e) 
					{
						throw new ConfigurationException("Error. Could not set TypeHandler.  Cause: " + e, e);
					}
				}
			} 
			else if (TypeHandlerFactory.GetTypeHandler(type, dbType) != null) 
			{
				handler = TypeHandlerFactory.GetTypeHandler(type, dbType);
			} 
			else 
			{
				Type typeClass = ObjectProbe.GetPropertyTypeForGetter(type, propertyName);
				handler = TypeHandlerFactory.GetTypeHandler(typeClass, dbType);
			}

			return handler;
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

//			if (assemblyNode != null)
//			{
//				try
//				{
//					Assembly.Load(assemblyNode.InnerText);
//				}
//				catch (Exception e)
//				{
//					throw new ConfigurationException(
//						string.Format("Unable to load assembly \"{0}\". Cause : ", e.Message  ) ,e);
//				}
//			}
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
		/// 
		/// </summary>
		/// <param name="config"></param>
		/// <param name="parameterMapNode"></param>
		/// <param name="sqlMapName"></param>
		/// <param name="sqlMap"></param>
		private void BuildParameterMap(XmlDocument config, XmlNode parameterMapNode, string sqlMapName, SqlMapper sqlMap)
		 {
			ParameterMap parameterMap = null;
			XmlSerializer serializer = new XmlSerializer(typeof(ParameterMap));

			string id = ((XmlAttribute)parameterMapNode.Attributes.GetNamedItem("id")).Value;
		
			// Did we already process it ?
			if (sqlMap.ParameterMaps.Contains( sqlMapName + DOT + id ) == false)
			{
				parameterMap = (ParameterMap) serializer.Deserialize(new XmlNodeReader(parameterMapNode));
				parameterMap.Initialize(parameterMapNode);

				parameterMap.Id = sqlMapName + DOT + parameterMap.Id;

				if (parameterMap.ExtendMap.Length >0)
				{
					ParameterMap superMap = null;
					// Did we already build Extend ParameterMap ?
					if (sqlMap.ParameterMaps.Contains(sqlMapName + DOT + parameterMap.ExtendMap) == false)
					{
						XmlNode superNode = config.SelectSingleNode("/sqlMap/parameterMaps/parameterMap[@id='"+ parameterMap.ExtendMap +"']");

						if (superNode != null)
						{
							BuildParameterMap( config, superNode, sqlMapName, sqlMap);
							superMap = sqlMap.GetParameterMap(sqlMapName + DOT + parameterMap.ExtendMap);
						}
						else
						{
							throw new ConfigurationException("In mapping file '"+sqlMapName+"' the parameterMap '"+parameterMap.Id+"' can not resolve extends attribut '"+parameterMap.ExtendMap+"'");
						}
					}
					else
					{
						superMap = sqlMap.GetParameterMap(sqlMapName + DOT + parameterMap.ExtendMap);
					}
					// Do extends
					int index = 0;

					// Add parent property
					foreach(string propertyName in superMap.GetPropertyNameArray())
					{
						parameterMap.InsertParameterProperty( index, superMap.GetProperty(propertyName) );
						index++;
					}
				}
				sqlMap.AddParameterMap( parameterMap );
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="config"></param>
		/// <param name="resultMapNode"></param>
		/// <param name="sqlMapName"></param>
		/// <param name="sqlMap"></param>
		private void BuildResultMap(XmlDocument config, XmlNode resultMapNode, string sqlMapName, SqlMapper sqlMap)
		 {
			ResultMap resultMap = null;

			XmlSerializer serializer = new XmlSerializer(typeof(ResultMap));

			string id = ((XmlAttribute)resultMapNode.Attributes.GetNamedItem("id")).Value;

			// Did we alredy process it
			if (sqlMap.ResultMaps.Contains( sqlMapName + DOT + id ) == false)
			{
				resultMap = (ResultMap) serializer.Deserialize(new XmlNodeReader(resultMapNode));
				resultMap.Initialize( sqlMap, resultMapNode);

				resultMap.Id = sqlMapName + DOT + resultMap.Id;

				if (resultMap.ExtendMap.Length >0)
				{
					ResultMap superMap = null;
					// Did we already buid it ?
					if (sqlMap.ResultMaps.Contains(sqlMapName + DOT + resultMap.ExtendMap) == false)
					{
						XmlNode superNode = config.SelectSingleNode("/sqlMap/resultMaps/resultMap[@id='"+ resultMap.ExtendMap +"']");

						if (superNode != null)
						{
							BuildResultMap( config, superNode, sqlMapName, sqlMap);
							superMap = sqlMap.GetResultMap(sqlMapName + DOT + resultMap.ExtendMap);
						}
						else
						{
							throw new ConfigurationException("In mapping file '"+sqlMapName+"' the resultMap '"+resultMap.Id+"' can not resolve extends attribut '"+resultMap.ExtendMap+"'" );
						}
					}
					else
					{
						superMap = sqlMap.GetResultMap(sqlMapName + DOT + resultMap.ExtendMap);
					}

					// Add parent property
					foreach(DictionaryEntry dicoEntry in superMap.ColumnsToPropertiesMap)
					{
						ResultProperty property = (ResultProperty)dicoEntry.Value;
						resultMap.AddResultPropery(property);
					}
				}
				sqlMap.AddResultMap( resultMap );
			}
		 }


		/// <summary>
		/// Register under Statement Name or Fully Qualified Statement Name
		/// </summary>
		/// <param name="id">An Identity</param>
		/// <param name="currentNamespace">The current Namespace of the sql mapping file</param>
		/// <returns>The new Identity</returns>
		private string ApplyNamespace(string id, string currentNamespace) 
		{
			string newId = id;

			if (currentNamespace != null && currentNamespace.Length > 0 && id != null && id.IndexOf(".") < 0) 
			{
				newId = currentNamespace + "." + id;
			}
			return newId;
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
