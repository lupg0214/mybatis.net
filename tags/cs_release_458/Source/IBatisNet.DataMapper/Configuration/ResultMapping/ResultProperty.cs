
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
using System.Reflection;
using System.Xml.Serialization;

using IBatisNet.DataMapper.TypesHandler;
using IBatisNet.Common.Utilities;
#endregion

namespace IBatisNet.DataMapper.Configuration.ResultMapping
{
	/// <summary>
	/// Summary description for ResultProperty.
	/// </summary>
	[Serializable]
	[XmlRoot("result")]
	public class ResultProperty
	{
		/// <summary>
		/// 
		/// </summary>
		public const int UNKNOWN_COLUMN_INDEX = -999999;

		#region Fields
		[NonSerialized]
		private PropertyInfo _propertyInfo;
		[NonSerialized]
		private string _nullValue = string.Empty;
		[NonSerialized]
		private string _property = string.Empty;
		[NonSerialized]
		private string _columnName = string.Empty;
		[NonSerialized]
		private int _columnIndex = UNKNOWN_COLUMN_INDEX;
		[NonSerialized]
		private string _select = string.Empty;
		[NonSerialized]
		private string _resulMapName = string.Empty;
		[NonSerialized]
		private ResultMap _resulMap = null;
		[NonSerialized]
		private string _dbType = string.Empty;
		[NonSerialized]
		private string _clrType = string.Empty;
		[NonSerialized]
		private bool _isLazyLoad = false;
		[NonSerialized]
		private ITypeHandler _typeHandler = null;
		#endregion

		#region Properties
		/// <summary>
		/// Tell us if we must lazy load this property..
		/// </summary>
		[XmlAttribute("lazyLoad")]
		public bool IsLazyLoad
		{
			get { return _isLazyLoad; }
			set { _isLazyLoad = value; }
		}

		/// <summary>
		/// The typeHandler used to work with the result property.
		/// </summary>
		[XmlIgnoreAttribute]
		public ITypeHandler TypeHandler
		{
			get { return _typeHandler; }
			set { _typeHandler = value; }
		}

		/// <summary>
		/// Give an entry in the 'DbType' enumeration
		/// </summary>
		/// <example >
		/// For Sql Server, give an entry of SqlDbType : Bit, Decimal, Money...
		/// <br/>
		/// For Oracle, give an OracleType Enumeration : Byte, Int16, Number...
		/// </example>
		[XmlAttribute("dbType")]
		public string DbType
		{
			get { return _dbType; }
			set { _dbType = value; }
		}

		
		/// <summary>
		/// Specify the CLR type of the result.
		/// </summary>
		/// <remarks>
		/// The type attribute is used to explicitly specify the property type of the property to be set.
		/// Normally this can be derived from a property through reflection, but certain mappings such as
		/// HashTable cannot provide the type to the framework.
		/// </remarks>
		[XmlAttribute("type")]
		public string CLRType
		{
			get { return _clrType; }
			set { _clrType = value; }
		}

		/// <summary>
		/// Column Index
		/// </summary>
		[XmlAttribute("columnIndex")]
		public int ColumnIndex
		{
			get { return _columnIndex; }
			set { _columnIndex = value; }
		}

		/// <summary>
		/// The name of the statement to retrieve the property
		/// </summary>
		[XmlAttribute("select")]
		public string Select
		{
			get { return _select; }
			set { _select = value; }
		}

		/// <summary>
		/// The name of the ResultMap to set the property
		/// </summary>
		[XmlAttribute("resultMapping")]
		public string ResulMapName
		{
			get { return _resulMapName; }
			set { _resulMapName = value; }
		}

		/// <summary>
		/// Column Name
		/// </summary>
		[XmlAttribute("column")]
		public string ColumnName
		{
			get { return _columnName; }
			set { _columnName = value; }
		}

		/// <summary>
		/// The property name used to identify the property amongst the others.
		/// </summary>
		[XmlAttribute("property")]
		public string PropertyName
		{
			get { return _property; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The property attribut is mandatory in a result property.");

				_property = value; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[XmlIgnoreAttribute]
		public PropertyInfo PropertyInfo
		{
			get { return _propertyInfo; }
		}

		/// <summary>
		/// Tell if a nullValue is defined.
		/// </summary>
		[XmlIgnoreAttribute]
		public bool HasNullValue
		{
			get { return (_nullValue.Length>0); }
		}

		/// <summary>
		/// Null value replacement.
		/// </summary>
		/// <example>"no_email@provided.com"</example>
		[XmlAttribute("nullValue")]
		public string NullValue
		{
			get { return _nullValue; }
			set { _nullValue = value; }
		}

		/// <summary>
		/// The resultMap use to set the object
		/// </summary>
		[XmlIgnoreAttribute]
		public ResultMap ResultMap
		{
			get { return _resulMap; }
			set { _resulMap = value; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public ResultProperty()
		{
		}
		#endregion

		#region Methods

		/// <summary>
		/// Initialize the PropertyInfo of the result property.
		/// </summary>
		/// <param name="propertyInfo">A PropertyInfoot.</param>
		public void Initialize( PropertyInfo propertyInfo )
		{
			_propertyInfo = propertyInfo;

			if ( propertyInfo != null)
			{
				_typeHandler =  TypeHandlerFactory.GetTypeHandler(propertyInfo.PropertyType);
			}
			// If we specify a type, it can overrride 
			if (this.CLRType != string.Empty)
			{
				_typeHandler = TypeHandlerFactory.GetTypeHandler(Resources.TypeForName(this.CLRType));
			}
		}
		#endregion

	}
}
