
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
using System.Data;
using System.Reflection;
using System.Xml.Serialization;

using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.TypesHandler;
#endregion

namespace IBatisNet.DataMapper.Configuration.ParameterMapping
{
	/// <summary>
	/// Summary description for ParameterProperty.
	/// </summary>
	[Serializable]
	[XmlRoot("parameter")]
	public class ParameterProperty
	{

		#region Fields
		[NonSerialized]
		private string _nullValue = string.Empty;
		[NonSerialized]
		private string _property = string.Empty;
		[NonSerialized]
		private ParameterDirection _direction = ParameterDirection.Input;
		[NonSerialized]
		private string _directionAttribut = string.Empty;
		[NonSerialized]
		private string _dbType = string.Empty;
		[NonSerialized]
		private int _size = -1;
		[NonSerialized]
		private byte _scale= 0;
		[NonSerialized]
		private byte _precision = 0;
		[NonSerialized]
		private string _columnName = string.Empty; // used only for store procedure
		[NonSerialized]
		private ITypeHandler _typeHandler = null;
		[NonSerialized]
		private string _clrType = string.Empty;
		#endregion

		#region Properties
		/// <summary>
		/// Specify the CLR type of the result.
		/// </summary>
		/// <remarks>
		/// The type attribute is used to explicitly specify the property type to be read.
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
		/// The typeHandler used to work with the parameter.
		/// </summary>
		[XmlIgnoreAttribute]
		public ITypeHandler TypeHandler
		{
			get { return _typeHandler; }
			set { _typeHandler = value; }
		}

		/// <summary>
		/// Column Name for output parameter 
		/// in store proccedure.
		/// </summary>
		[XmlAttribute("column")]
		public string ColumnName
		{
			get { return _columnName; }
			set { _columnName = value; }
		}

		/// <summary>
		/// Column size.
		/// </summary>
		[XmlAttribute("size")]
		public int Size
		{
			get { return _size; }
			set { _size = value; }
		}

		/// <summary>
		/// Column Scale.
		/// </summary>
		[XmlAttribute("scale")]
		public byte Scale
		{
			get { return _scale; }
			set { _scale = value; }
		}

		/// <summary>
		/// Column Precision.
		/// </summary>
		[XmlAttribute("precision")]
		public byte Precision
		{
			get { return _precision; }
			set { _precision = value; }
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
		/// The direction attribut of the XML parameter.
		/// </summary>
		/// <example> Input, Output, InputOutput</example>
		[XmlAttribute("direction")]
		public string DirectionAttribut
		{
			get { return _directionAttribut; }
			set { _directionAttribut = value; }
		}

		/// <summary>
		/// Indicate the direction of the parameter.
		/// </summary>
		/// <example> Input, Output, InputOutput</example>
		[XmlIgnoreAttribute]
		public ParameterDirection Direction
		{
			get { return _direction; }
			set 
			{ 
				_direction = value;
				_directionAttribut = _direction.ToString();
			}
		}

		/// <summary>
		/// Property name used to identify the property amongst the others.
		/// </summary>
		/// <example>EmailAddress</example>
		[XmlAttribute("property")]
		public string PropertyName
		{
			get { return _property; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The property attribut is mandatory in a paremeter property.");

				_property = value; 
			}
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
		#endregion

		#region Constructor (s) / Destructor

		/// <summary>
		/// Constructor
		/// </summary>
		public ParameterProperty()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		public void Initialize()
		{
			if(_directionAttribut.Length >0)
			{
				_direction = (ParameterDirection)Enum.Parse( typeof(ParameterDirection), _directionAttribut, true );
			}
			// If we specify a type, it can be set 
			if (this.CLRType != string.Empty)
			{
				_typeHandler = TypeHandlerFactory.GetTypeHandler(Resources.TypeForName(this.CLRType));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj) 
		{
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			ParameterProperty p = (ParameterProperty)obj;
			return (this.PropertyName == p.PropertyName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() 
		{
			return _property.GetHashCode();
		}
		#endregion

	}
}
