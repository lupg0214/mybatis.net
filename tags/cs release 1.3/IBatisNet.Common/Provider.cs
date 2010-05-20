
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
using System.Data;
using System.Reflection;
using System.Xml.Serialization;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities.TypesResolver;

#endregion

namespace IBatisNet.Common
{
	/// <summary>
	/// Information about a data provider.
	/// </summary>
	[Serializable]
	[XmlRoot("provider", Namespace="http://ibatis.apache.org/providers")]
	public class Provider
	{
		#region Fields
		[NonSerialized]
		private string _assemblyName = string.Empty;
		[NonSerialized]
		private string _connectionClass = string.Empty;
		[NonSerialized]
		private string _commandClass = string.Empty;
		[NonSerialized]
		private string _dataParameter = string.Empty;
		[NonSerialized]
		private Type _dataParameterType = null;

		[NonSerialized]
		private string _parameterDbTypeClass = string.Empty;
		[NonSerialized]
		private Type _parameterDbType = null;

		[NonSerialized]
		private string _parameterDbTypeProperty = string.Empty;
		[NonSerialized]
		private string _dataAdapterClass = string.Empty;
		[NonSerialized]
		private string _commandBuilderClass = string.Empty;

		[NonSerialized]
		private string _name = string.Empty;
		[NonSerialized]
		private string _description = string.Empty;
		[NonSerialized]
		private bool _isDefault = false;
		[NonSerialized]
		private bool _isEnabled = true;
		[NonSerialized]
		private IDbConnection _templateConnection = null;
		[NonSerialized]
		private IDbCommand _templateCommand= null;
		[NonSerialized]
		private IDbDataAdapter _templateDataAdapter= null;
		[NonSerialized]
		private Type _commandBuilderType = null;
		[NonSerialized]
		private string _parameterPrefix = string.Empty;
		[NonSerialized]
		private bool _useParameterPrefixInSql = true;
		[NonSerialized]
		private bool _useParameterPrefixInParameter = true;
		[NonSerialized]
		private bool _usePositionalParameters = false;
		[NonSerialized]
		private bool _templateConnectionIsICloneable = false;
		[NonSerialized]
		private bool _templateCommandIsICloneable = false;
		[NonSerialized]
		private bool _templateDataAdapterIsICloneable = false;
		[NonSerialized]
		private bool _setDbParameterSize = true;
		[NonSerialized]
		private bool _setDbParameterPrecision = true;
		[NonSerialized]
		private bool _setDbParameterScale = true;
		[NonSerialized]
		private bool _useDeriveParameters = true;
		
//		private static readonly ILog _connectionLogger = LogManager.GetLogger("System.Data.IDbConnection");

		#endregion
		
		#region Properties
		/// <summary>
		/// The name of the assembly which conatins the definition of the provider.
		/// </summary>
		/// <example>Examples : "System.Data", "Microsoft.Data.Odbc"</example>
		[XmlAttribute("assemblyName")]
		public string AssemblyName
		{
			get { return _assemblyName; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("AssemblyName");
				_assemblyName = value;
			}
		}


		/// <summary>
		/// Tell us if it is the default data source.
		/// Default false.
		/// </summary>
		[XmlAttribute("default")]
		public bool IsDefault
		{             
			get { return _isDefault; }
			set {_isDefault = value;}
		}
		

		/// <summary>
		/// Tell us if this provider is enabled.
		/// Default true.
		/// </summary>
		[XmlAttribute("enabled")]
		public bool IsEnabled
		{             
			get { return _isEnabled; }
			set {_isEnabled = value;}
		}

	
		/// <summary>
		/// The connection class name to use.
		/// </summary>
		/// <example>
		/// "System.Data.OleDb.OleDbConnection", 
		/// "System.Data.SqlClient.SqlConnection", 
		/// "Microsoft.Data.Odbc.OdbcConnection"
		/// </example>
		[XmlAttribute("connectionClass")]
		public string ConnectionClass
		{             
			get { return _connectionClass; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The connectionClass attribute is mandatory in the provider " + _name);
				_connectionClass = value;
			}
		}

		/// <summary>
		/// Does this ConnectionProvider require the use of a Named Prefix in the SQL 
		/// statement. 
		/// </summary>
		/// <remarks>
		/// The OLE DB/ODBC .NET Provider does not support named parameters for 
		/// passing parameters to an SQL Statement or a stored procedure called 
		/// by an IDbCommand when CommandType is set to Text.
		/// 
		/// For example, SqlClient requires select * from simple where simple_id = @simple_id
		/// If this is false, like with the OleDb or Obdc provider, then it is assumed that 
		/// the ? can be a placeholder for the parameter in the SQL statement when CommandType 
		/// is set to Text.		
		/// </remarks>
		[XmlAttribute("useParameterPrefixInSql")]
		public bool UseParameterPrefixInSql 
		{
			get { return _useParameterPrefixInSql; }
			set { _useParameterPrefixInSql = value;}
		}
		
		/// <summary>
		/// Does this ConnectionProvider require the use of the Named Prefix when trying
		/// to reference the Parameter in the Command's Parameter collection. 
		/// </summary>
		/// <remarks>
		/// This is really only useful when the UseParameterPrefixInSql = true. 
		/// When this is true the code will look like IDbParameter param = cmd.Parameters["@paramName"], 
		/// if this is false the code will be IDbParameter param = cmd.Parameters["paramName"] - ie - Oracle.
		/// </remarks>
		[XmlAttribute("useParameterPrefixInParameter")]
		public bool UseParameterPrefixInParameter  
		{
			get { return _useParameterPrefixInParameter; }
			set { _useParameterPrefixInParameter = value; }		
		}

		/// <summary>
		/// The OLE DB/OBDC .NET Provider uses positional parameters that are marked with a 
		/// question mark (?) instead of named parameters.
		/// </summary>
		[XmlAttribute("usePositionalParameters")]
		public bool UsePositionalParameters  
		{
			get { return _usePositionalParameters; }
			set { _usePositionalParameters = value; }		
		}

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports parameter size.
		/// </summary>
		/// <remarks>
		/// See JIRA-49 about SQLite.Net provider not supporting parameter size.
		/// </remarks>
		[XmlAttribute("setDbParameterSize")]
		public bool SetDbParameterSize
		{
			get { return _setDbParameterSize; }
			set { _setDbParameterSize = value; }		
		}

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports parameter precision.
		/// </summary>
		/// <remarks>
		/// See JIRA-49 about SQLite.Net provider not supporting parameter precision.
		/// </remarks>
		[XmlAttribute("setDbParameterPrecision")]
		public bool SetDbParameterPrecision
		{
			get { return _setDbParameterPrecision; }
			set { _setDbParameterPrecision = value; }		
		}

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports a parameter scale.
		/// </summary>
		/// <remarks>
		/// See JIRA-49 about SQLite.Net provider not supporting parameter scale.
		/// </remarks>
		[XmlAttribute("setDbParameterScale")]
		public bool SetDbParameterScale
		{
			get { return _setDbParameterScale; }
			set { _setDbParameterScale = value; }		
		}

		/// <summary>
		/// Used to indicate whether or not the provider 
		/// supports DeriveParameters method for procedure.
		/// </summary>
		[XmlAttribute("useDeriveParameters")]
		public bool UseDeriveParameters
		{
			get { return _useDeriveParameters; }
			set { _useDeriveParameters = value; }		
		}

		/// <summary>
		/// The command class name to use.
		/// </summary>
		/// <example>
		/// "System.Data.SqlClient.SqlCommand"
		/// </example>
		[XmlAttribute("commandClass")]
		public string CommandClass
		{             
			get { return _commandClass; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The commandClass attribute is mandatory in the provider " + _name);
				_commandClass = value;
			}
		}

	
		/// <summary>
		/// The parameter class name to use.
		/// </summary>
		/// <example>
		/// "System.Data.SqlClient.SqlParameter"
		/// </example>
		[XmlAttribute("parameterClass")]
		public string ParameterClass
		{             
			get { return _dataParameter; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The parameterClass attribute is mandatory in the provider " + _name);
				_dataParameter = value;
			}
		}


		/// <summary>
		/// The ParameterDbType class name to use.
		/// </summary>			
		/// <example>
		/// "System.Data.SqlDbType"
		/// </example>
		[XmlAttribute("parameterDbTypeClass")]
		public string ParameterDbTypeClass
		{             
			get { return _parameterDbTypeClass; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The parameterDbTypeClass attribute is mandatory in the provider " + _name);
				_parameterDbTypeClass = value;
			}
		}


		/// <summary>
		/// The ParameterDbTypeProperty class name to use.
		/// </summary>
		/// <example >
		/// SqlDbType in SqlParamater.SqlDbType, 
		/// OracleType in OracleParameter.OracleType.
		/// </example>
		[XmlAttribute("parameterDbTypeProperty")]
		public string ParameterDbTypeProperty
		{             
			get { return _parameterDbTypeProperty; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The parameterDbTypeProperty attribute is mandatory in the provider " + _name);
				_parameterDbTypeProperty = value;
			}
		}

		/// <summary>
		/// The dataAdapter class name to use.
		/// </summary>
		/// <example >
		/// "System.Data.SqlDbType"
		/// </example>
		[XmlAttribute("dataAdapterClass")]
		public string DataAdapterClass
		{             
			get { return _dataAdapterClass; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The dataAdapterClass attribute is mandatory in the provider " + _name);
				_dataAdapterClass = value;
			}
		}

		/// <summary>
		/// The commandBuilder class name to use.
		/// </summary>
		/// <example >
		/// "System.Data.OleDb.OleDbCommandBuilder", 
		/// "System.Data.SqlClient.SqlCommandBuilder", 
		/// "Microsoft.Data.Odbc.OdbcCommandBuilder"
		/// </example>
		[XmlAttribute("commandBuilderClass")]
		public string CommandBuilderClass
		{             
			get { return _commandBuilderClass; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The commandBuilderClass attribute is mandatory in the provider " + _name);
				_commandBuilderClass = value;
			}
		}


		/// <summary>
		/// Name used to identify the provider amongst the others.
		/// </summary>
		[XmlAttribute("name")]
		public string Name
		{
			get { return _name; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The name attribute is mandatory in the provider.");

				_name = value; 			
			}
		}

		/// <summary>
		/// Description.
		/// </summary>
		[XmlAttribute("description")]
		public string Description
		{
			get { return _description; }
			set { _description = value;}
		}
		
		/// <summary>
		/// Parameter prefix use in store procedure.
		/// </summary>
		/// <example> @ for Sql Server.</example>
		[XmlAttribute("parameterPrefix")]
		public string ParameterPrefix
		{
			get { return _parameterPrefix; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
				{
					_parameterPrefix = ""; 
					//throw new ArgumentNullException("The parameterPrefix attribute is mandatory in the provider " + _name);
				}
				else
				{
					_parameterPrefix = value; 
				}
			}
		}

		/// <summary>
		/// Check if this provider is Odbc ?
		/// </summary>
		[XmlIgnore]
		public bool IsObdc
		{
			get
			{
				return (_connectionClass.IndexOf(".Odbc.")>0);
			}
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public Provider()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Init the provider.
		/// </summary>
		public void Initialize()
		{
			Assembly assembly;
			Type type;
			CachedTypeResolver cachedTypeResolver = new CachedTypeResolver();

			try
			{
				assembly = Assembly.Load(_assemblyName);

				// Build the Command template 
				type = assembly.GetType(_commandClass, true);
				_templateCommand = (IDbCommand)type.GetConstructor(Type.EmptyTypes).Invoke(null);
				// Build the DataAdapter template 
				type = assembly.GetType(_dataAdapterClass, true);
				_templateDataAdapter = (IDbDataAdapter)type.GetConstructor(Type.EmptyTypes).Invoke(null);
				// Build the connection template 
				type = assembly.GetType(_connectionClass, true);
				_templateConnection = (IDbConnection)type.GetConstructor(Type.EmptyTypes).Invoke(null);
				// Get the IDataParameter type
				_dataParameterType = assembly.GetType(_dataParameter, true);
				// Get the CommandBuilder Type
				_commandBuilderType = assembly.GetType(_commandBuilderClass, true);
				if (_parameterDbTypeClass.IndexOf(',')>0)
				{
					_parameterDbType = cachedTypeResolver.Resolve(_parameterDbTypeClass);
				}
				else
				{
					_parameterDbType = assembly.GetType(_parameterDbTypeClass, true);
				}

				_templateConnectionIsICloneable = _templateConnection is ICloneable;
				_templateCommandIsICloneable = _templateCommand is ICloneable;
				_templateDataAdapterIsICloneable = _templateDataAdapter is ICloneable;
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Could not configure providers. Unable to load provider named \"{0}\" not found, failed. Cause: {1}", _name, e.Message), e
					);
			}
		}

		/// <summary>
		/// Get a connection object for this provider.
		/// </summary>
		/// <returns>An 'IDbConnection' object.</returns>
		public IDbConnection GetConnection()
		{
			// Cannot do that because on 
			// IDbCommand.Connection = cmdConnection
			// .NET cast the cmdConnection to the real type (as SqlConnection)
			// and we pass a proxy --> exception invalid cast !
//			if (_connectionLogger.IsDebugEnabled)
//			{
//				connection = (IDbConnection)IDbConnectionProxy.NewInstance(connection, this);
//			}
			if (_templateConnectionIsICloneable)
			{
				return (IDbConnection) ((ICloneable)_templateConnection).Clone();
			}
			else
			{
				return (IDbConnection) Activator.CreateInstance(_templateConnection.GetType());
			}
		}

		/// <summary>
		/// Get a command object for this provider.
		/// </summary>
		/// <returns>An 'IDbCommand' object.</returns>
		public IDbCommand GetCommand()
		{
			if (_templateCommandIsICloneable)
			{
				return (IDbCommand) ((ICloneable)_templateCommand).Clone();
			}
			else
			{
				return (IDbCommand) Activator.CreateInstance(_templateCommand.GetType());
			}
		}

		/// <summary>
		/// Get a dataAdapter object for this provider.
		/// </summary>
		/// <returns>An 'IDbDataAdapter' object.</returns>
		public IDbDataAdapter GetDataAdapter()
		{
			if (_templateDataAdapterIsICloneable)
			{
				return (IDbDataAdapter) ((ICloneable)_templateDataAdapter).Clone();
			}
			else
			{
				return (IDbDataAdapter) Activator.CreateInstance(_templateDataAdapter.GetType());
			}
		}

		/// <summary>
		/// Get a IDataParameter object for this provider.
		/// </summary>
		/// <returns>An 'IDataParameter' object.</returns>
		public IDataParameter GetDataParameter()
		{
			return (IDataParameter) Activator.CreateInstance(_dataParameterType);
		}

		/// <summary>
		/// Get the CommandBuilder Type for this provider.
		/// </summary>
		/// <returns>An object.</returns>
		public Type GetCommandBuilderType()
		{
			return _commandBuilderType;
		}

		/// <summary>
		/// Get the ParameterDb Type for this provider.
		/// </summary>
		/// <returns>An object.</returns>
		public Type ParameterDbType
		{
			get { return _parameterDbType; }
		}

		/// <summary>
		/// Equals implemantation.
		/// </summary>
		/// <param name="obj">The test object.</param>
		/// <returns>A boolean.</returns>
		public override bool Equals(object obj)
		{
			if ((obj != null) && (obj is Provider))
			{
				Provider that = (Provider) obj;
				return ((this._name == that._name) && 
					(this._assemblyName == that._assemblyName) &&
					(this._connectionClass == that._connectionClass));
			}
			return false;
		}

		/// <summary>
		/// A hashcode for the provider.
		/// </summary>
		/// <returns>An integer.</returns>
		public override int GetHashCode()
		{
			return (_name.GetHashCode() ^ _assemblyName.GetHashCode() ^ _connectionClass.GetHashCode());
		}

		/// <summary>
		/// ToString implementation.
		/// </summary>
		/// <returns>A string that describes the provider.</returns>
		public override string ToString()
		{
			return "Provider " + _name;
		}
		#endregion

	}
}
