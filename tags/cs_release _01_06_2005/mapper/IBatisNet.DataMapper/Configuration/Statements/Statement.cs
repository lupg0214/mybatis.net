
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
using System.Collections;
using System.Xml.Serialization;
using System.Reflection;

using IBatisNet.Common.Exceptions; 
using IBatisNet.Common.Utilities.TypesResolver;

using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.TypeHandlers;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Configuration.Sql;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.Scope;
#endregion

namespace IBatisNet.DataMapper.Configuration.Statements
{
	/// <summary>
	/// Summary description for Statement.
	/// </summary>
	[Serializable]
	[XmlRoot("statement")]
	public class Statement : IStatement
	{
		#region Constants
		private const string DOT = ".";
		#endregion

		#region Fields

		[NonSerialized]
		private string _id = string.Empty;
		// ResultMap
		[NonSerialized]
		private string _resultMapName = string.Empty;
		[NonSerialized]
		private ResultMap _resultMap = null;
		// ParameterMap
		[NonSerialized]
		private string _parameterMapName = string.Empty;
		[NonSerialized]
		private ParameterMap _parameterMap = null;
		// Result Class
		[NonSerialized]
		private string _resultClassName = string.Empty;
		[NonSerialized]
		private Type _resultClass = null;
		// Parameter Class
		[NonSerialized]
		private string _parameterClassName = string.Empty;
		[NonSerialized]
		private Type _parameterClass = null;
		// List Class
		[NonSerialized]
		private string _listClassName = string.Empty;
		[NonSerialized]
		private Type _listClass = null;
		// CacheModel
		[NonSerialized]
		private string _cacheModelName = string.Empty;
		[NonSerialized]
		private CacheModel _cacheModel = null;
		[NonSerialized]
		private ISql _sql = null;
		[NonSerialized]
		private string _extendStatement = string.Empty;
		#endregion

		#region Properties

		/// <summary>
		/// Extend statement attribute
		/// </summary>
		[XmlAttribute("extends")]
		public virtual string ExtendSatement
		{
			get { return _extendStatement; }
			set { _extendStatement = value; }
		}

		/// <summary>
		/// The CacheModel name to use.
		/// </summary>
		[XmlAttribute("cacheModel")]
		public string CacheModelName
		{
			get { return _cacheModelName; }
			set { _cacheModelName = value; }
		}

		/// <summary>
		/// Tell us if a cacheModel is attached to this statement.
		/// </summary>
		[XmlIgnoreAttribute]
		public bool HasCacheModel
		{
			get{ return _cacheModelName.Length >0;}
		}

		/// <summary>
		/// The CacheModel used by this statement.
		/// </summary>
		[XmlIgnoreAttribute]
		public CacheModel CacheModel
		{
			get { return _cacheModel; }
			set { _cacheModel = value; }
		}

		/// <summary>
		/// The list class name to use for strongly typed collection.
		/// </summary>
		[XmlAttribute("listClass")]
		public string ListClassName
		{
			get { return _listClassName; }
			set { _listClassName = value; }
		}

		
		/// <summary>
		/// The list class type to use for strongly typed collection.
		/// </summary>
		[XmlIgnoreAttribute]
		public Type ListClass
		{
			get { return _listClass; }
		}

		/// <summary>
		/// The result class name to used.
		/// </summary>
		[XmlAttribute("resultClass")]
		public string ResultClassName
		{
			get { return _resultClassName; }
			set { _resultClassName = value; }
		}

		/// <summary>
		/// The result class type to used.
		/// </summary>
		[XmlIgnoreAttribute]
		public Type ResultClass
		{
			get { return _resultClass; }
		}

		/// <summary>
		/// The parameter class name to used.
		/// </summary>
		[XmlAttribute("parameterClass")]
		public string ParameterClassName
		{
			get { return _parameterClassName; }
			set { _parameterClassName = value; }
		}

		/// <summary>
		/// The parameter class type to used.
		/// </summary>
		[XmlIgnoreAttribute]
		public Type ParameterClass
		{
			get { return _parameterClass; }
		}

		/// <summary>
		/// Name used to identify the statement amongst the others.
		/// </summary>
		[XmlAttribute("id")]
		public string Id
		{
			get { return _id; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new DataMapperException("The id attribute is required in a statement tag.");

				_id= value; 
			}
		}

		
		/// <summary>
		/// The sql statement
		/// </summary>
		[XmlIgnoreAttribute]
		public ISql Sql 
		{
			get { return _sql; }
			set 
			{ 
				if (value == null)
					throw new DataMapperException("The sql statement query text is required in the statement tag "+_id);

				_sql = value; 
			}
		}

	
		/// <summary>
		/// The ResultMap name used by the statement.
		/// </summary>
		[XmlAttribute("resultMap")]
		public string ResultMapName
		{
			get { return _resultMapName; }
			set { _resultMapName = value; }
		}

		/// <summary>
		/// The ParameterMap name used by the statement.
		/// </summary>
		[XmlAttribute("parameterMap")]
		public string ParameterMapName
		{
			get { return _parameterMapName; }
			set { _parameterMapName = value; }
		}

		/// <summary>
		/// The ResultMap used by the statement.
		/// </summary>
		[XmlIgnoreAttribute]
		public ResultMap ResultMap
		{
			get { return _resultMap; }
		}

		/// <summary>
		/// The parameterMap used by the statement.
		/// </summary>
		[XmlIgnoreAttribute]
		public ParameterMap ParameterMap
		{
			get { return _parameterMap; }
			set { _parameterMap = value; }
		}

		
		/// <summary>
		/// The type of the statement (text or procedure)
		/// Default Text.
		/// </summary>
		/// <example>Text or StoredProcedure</example>
		[XmlIgnoreAttribute]
		public virtual CommandType CommandType
		{
			get { return CommandType.Text; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		[Obsolete("This public constructor with no parameter is not really obsolete, but is reserved for serialization.", false)]
		public Statement() {}
		#endregion

		#region Methods
		/// <summary>
		/// Initialize an statement for the sqlMap.
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		internal virtual void Initialize(ConfigurationScope configurationScope)
		{
			if (_resultMapName != string.Empty )
			{
				_resultMap = configurationScope.SqlMapper.GetResultMap( _resultMapName);
			}
			if (_parameterMapName != string.Empty )
			{
				_parameterMap = configurationScope.SqlMapper.GetParameterMap( _parameterMapName);
			}
			if (_resultClassName != string.Empty )
			{
				_resultClass = configurationScope.SqlMapper.GetType(_resultClassName);
			}
			if (_parameterClassName != string.Empty )
			{
				_parameterClass = configurationScope.SqlMapper.GetType(_parameterClassName);
			}
			if (_listClassName != string.Empty )
			{
				_listClass = configurationScope.SqlMapper.GetType(_listClassName);
			}
		}


		/// <summary>
		/// Create an instance of result class.
		/// </summary>
		/// <returns>An object.</returns>
		public object CreateInstanceOfResultClass()
		{
			if (_resultClass.IsPrimitive || _resultClass == typeof (string) )
			{
				TypeCode typeCode = Type.GetTypeCode(_resultClass);
				return TypeAliasResolver.InstantiatePrimitiveType(typeCode);
			}
			else
			{
				if (_resultClass == typeof (Guid))
				{
					return Guid.Empty;
				}
				else if (_resultClass == typeof (TimeSpan))
				{
					return new TimeSpan(0);
				}
				else
				{
					return Activator.CreateInstance(_resultClass);
				}
			}
		}


		/// <summary>
		/// Create an instance of 'IList' class.
		/// </summary>
		/// <returns>An object which implment IList.</returns>
		public IList CreateInstanceOfListClass()
		{
			return (IList)Activator.CreateInstance(_listClass);
		}
		#endregion

	}
}
