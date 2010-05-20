
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
using System.Xml.Serialization;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.TypeHandlers;

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
		private string _directionAttribute = string.Empty;
		[NonSerialized]
		private string _dbType = null;
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
		[NonSerialized]
		private string _callBackName= string.Empty;
		#endregion

		#region Properties

		/// <summary>
		/// Specify the custom type handlers to used.
		/// </summary>
		/// <remarks>Will be an alias to a class wchic implement ITypeHandlerCallback</remarks>
		[XmlAttribute("typeHandler")]
		public string CallBackName
		{
			get { return _callBackName; }
			set { _callBackName = value; }
		}

		/// <summary>
		/// Specify the CLR type of the parameter.
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
		[XmlIgnore]
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
		/// The direction attribute of the XML parameter.
		/// </summary>
		/// <example> Input, Output, InputOutput</example>
		[XmlAttribute("direction")]
		public string DirectionAttribute
		{
			get { return _directionAttribute; }
			set { _directionAttribute = value; }
		}

		/// <summary>
		/// Indicate the direction of the parameter.
		/// </summary>
		/// <example> Input, Output, InputOutput</example>
		[XmlIgnore]
		public ParameterDirection Direction
		{
			get { return _direction; }
			set 
			{ 
				_direction = value;
				_directionAttribute = _direction.ToString();
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
					throw new ArgumentNullException("The property attribute is mandatory in a paremeter property.");

				_property = value; 
			}
		}

		/// <summary>
		/// Tell if a nullValue is defined.
		/// </summary>
		[XmlIgnore]
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
		/// <param name="configScope"></param>
		public void Initialize(ConfigurationScope configScope)
		{
			if(_directionAttribute.Length >0)
			{
				_direction = (ParameterDirection)Enum.Parse( typeof(ParameterDirection), _directionAttribute, true );
			}
			
			configScope.ErrorContext.MoreInfo = "Check the parameter mapping typeHandler attribute '" + this.CallBackName + "' (must be a ITypeHandlerCallback implementation).";
			if (this.CallBackName.Length >0)
			{
				try 
				{
					Type type = configScope.SqlMapper.GetType(this.CallBackName);
					ITypeHandlerCallback typeHandlerCallback = (ITypeHandlerCallback) Activator.CreateInstance( type );
					_typeHandler = new CustomTypeHandler(typeHandlerCallback);
				}
				catch (Exception e) 
				{
					throw new ConfigurationException("Error occurred during custom type handler configuration.  Cause: " + e.Message, e);
				}
			}
			else
			{
				if (this.CLRType.Length == 0 )  // Unknown
				{
					_typeHandler = configScope.TypeHandlerFactory.GetUnkownTypeHandler();
				}
				else // If we specify a CLR type, use it
				{ 
					Type type = Resources.TypeForName(this.CLRType);

					if (configScope.TypeHandlerFactory.IsSimpleType(type)) 
					{
						// Primitive
						_typeHandler = configScope.TypeHandlerFactory.GetTypeHandler(type, _dbType);
					}
					else
					{
						// .NET object
						type = ObjectProbe.GetPropertyTypeForGetter(type, this.PropertyName);
						_typeHandler = configScope.TypeHandlerFactory.GetTypeHandler(type, _dbType);
					}
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="errorContext"></param>
		/// <param name="typeHandlerFactory"></param>
		internal void Initialize(TypeHandlerFactory typeHandlerFactory, ErrorContext errorContext)
		{
			if(_directionAttribute.Length >0)
			{
				_direction = (ParameterDirection)Enum.Parse( typeof(ParameterDirection), _directionAttribute, true );
			}
			
			errorContext.MoreInfo = "Initialize an inline parameter property '" + this.PropertyName + "' .";
			if (this.CLRType.Length == 0 )  // Unknown
			{
				_typeHandler = typeHandlerFactory.GetUnkownTypeHandler();
			}
			else // If we specify a CLR type, use it
			{ 
				Type type = Resources.TypeForName(this.CLRType);

				if (typeHandlerFactory.IsSimpleType(type)) 
				{
					// Primitive
					_typeHandler = typeHandlerFactory.GetTypeHandler(type);
				}
				else
				{
					// .NET object
					type = ObjectProbe.GetPropertyTypeForGetter(type, this.PropertyName);
					_typeHandler = typeHandlerFactory.GetTypeHandler(type);
				}
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
