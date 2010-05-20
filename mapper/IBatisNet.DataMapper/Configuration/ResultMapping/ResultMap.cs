
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
using System.Data;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.Common.Utilities.TypesResolver;
using IBatisNet.DataMapper.Scope;

#endregion

namespace IBatisNet.DataMapper.Configuration.ResultMapping
{
	/// <summary>
	/// Summary description for ResultMap.
	/// </summary>
	[Serializable]
	[XmlRoot("resultMap")]
	public class ResultMap
	{
		#region Fields
		[NonSerialized]
		private string _id = string.Empty;
		[NonSerialized]
		private string _className = string.Empty;
		[NonSerialized]
		private string _extendMap = string.Empty;
		[NonSerialized]
		private Type _class = null;
		//(columnName, property)
		[NonSerialized]
		private Hashtable _columnsToPropertiesMap = new Hashtable();
		[NonSerialized]
		private Discriminator _discriminator = null;
		[NonSerialized]
		private string _sqlMapNameSpace = string.Empty;
		#endregion

		#region Properties

		/// <summary>
		/// The sqlMap namespace
		/// </summary>
		[XmlIgnore]
		public string SqlMapNameSpace
		{
			get
			{
				return _sqlMapNameSpace;
			}	
			set
			{
				_sqlMapNameSpace = value;
			}	
		}

		/// <summary>
		/// The discriminator used to choose the good SubMap
		/// </summary>
		[XmlIgnore]
		public Discriminator Discriminator
		{
			get
			{
				return _discriminator;
			}	
			set
			{
				_discriminator = value;
			}	
		}

		/// <summary>
		/// The collection of result properties.
		/// </summary>
		[XmlIgnore]
		public Hashtable ColumnsToPropertiesMap
		{
			get { return _columnsToPropertiesMap; }
		}

		/// <summary>
		/// Identifier used to identify the resultMap amongst the others.
		/// </summary>
		/// <example>GetProduct</example>
		[XmlAttribute("id")]
		public string Id
		{
			get { return _id; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The id attribute is mandatory in a ResultMap tag.");

				_id = value; 
			}
		}

		/// <summary>
		/// Extend ResultMap attribute
		/// </summary>
		[XmlAttribute("extends")]
		public string ExtendMap
		{
			get { return _extendMap; }
			set { _extendMap = value; }
		}

		/// <summary>
		/// The output type class of the resultMap.
		/// </summary>
		[XmlIgnore]
		public Type Class
		{
			get { return _class; }
		}


		/// <summary>
		/// The output class name of the resultMap.
		/// </summary>
		/// <example>Com.Site.Domain.Product</example>
		[XmlAttribute("class")]
		public string ClassName
		{
			get { return _className; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The class attribute is mandatory in a ResultMap tag.");

				_className = value; 
			}
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public ResultMap()
		{
		}
		#endregion

		#region Methods

		#region Configuration
		/// <summary>
		/// Initialize the resultMap from an xmlNode..
		/// </summary>
		/// <param name="configScope"></param>
		public void Initialize( ConfigurationScope configScope )
		{
			try
			{
				_class = configScope.SqlMapper.GetType(_className);

				// Load the child node
				GetChildNode(configScope);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Could not configure ResultMap. ResultMap named \"{0}\" not found, failed. \n Cause: {1}", _id, e.Message)
					);
			}
		}


		/// <summary>
		/// Get the result properties and the subMap properties.
		/// </summary>
		/// <param name="configScope"></param>
		private void GetChildNode(ConfigurationScope configScope)
		{
			XmlSerializer serializer = null;
			ResultProperty mapping = null;
			SubMap subMap = null;

			#region Load the Result Properties

			serializer = new XmlSerializer(typeof(ResultProperty));
			foreach ( XmlNode resultNode in configScope.NodeContext.SelectNodes("result") )
			{
				mapping = (ResultProperty) serializer.Deserialize(new XmlNodeReader(resultNode));
					
				configScope.ErrorContext.MoreInfo = "initialize result property :"+mapping.PropertyName;
//
//				PropertyInfo propertyInfo = null;
//
//				if ( mapping.PropertyName != "value" && !typeof(IDictionary).IsAssignableFrom(_class) )
//				{
//					propertyInfo = ReflectionInfo.GetInstance(_class).GetSetter( mapping.PropertyName );
//				}
				mapping.Initialize( configScope, _class );

				this.AddResultPropery( mapping  );
			}
			#endregion 

			#region Load the Discriminator Property

			serializer = new XmlSerializer(typeof(Discriminator));
			XmlNode discriminatorNode = configScope.NodeContext.SelectSingleNode("discriminator");
			if (discriminatorNode != null)
			{
				configScope.ErrorContext.MoreInfo = "initialize discriminator";

				this.Discriminator = (Discriminator) serializer.Deserialize(new XmlNodeReader(discriminatorNode));

				this.Discriminator.SetMapping( configScope, _class );
			}
			#endregion 

			#region Load the SubMap Properties

			serializer = new XmlSerializer(typeof(SubMap));
			if (configScope.NodeContext.SelectNodes("subMap").Count>0 && this.Discriminator==null)
			{
				throw new ConfigurationException("The discriminator is null, but somehow a subMap was reached.  This is a bug.");
			}
			foreach ( XmlNode resultNode in configScope.NodeContext.SelectNodes("subMap") )
			{
				configScope.ErrorContext.MoreInfo = "initialize subMap";
				subMap = (SubMap) serializer.Deserialize(new XmlNodeReader(resultNode));
				subMap.ResultMapName = this.SqlMapNameSpace + DomSqlMapBuilder.DOT + subMap.ResultMapName;
				this.Discriminator.Add( subMap );
			}
			#endregion 
		}

		#endregion

		/// <summary>
		/// Create an instance Of result.
		/// </summary>
		/// <returns>An object.</returns>
		public object CreateInstanceOfResult()
		{
			TypeCode typeCode = Type.GetTypeCode(_class);

			if (typeCode == TypeCode.Object)
			{
				return Activator.CreateInstance(_class);
			}
			else
			{
				return TypeAliasResolver.InstantiatePrimitiveType(typeCode);
			}
		}

		/// <summary>
		/// Add a ResultProperty to the list of ResultProperty.
		/// </summary>
		/// <param name="property">The property to add.</param>
		public void AddResultPropery(ResultProperty property)
		{
			_columnsToPropertiesMap.Add( property.PropertyName, property  );
		}

		/// <summary>
		/// Set the value of an object property.
		/// </summary>
		/// <param name="target">The object to set the property.</param>
		/// <param name="property">The result property to use.</param>
		/// <param name="dataBaseValue">The database value to set.</param>
		public void SetValueOfProperty( ref object target, ResultProperty property, object dataBaseValue )
		{
			if (target is Hashtable)
			{
				((Hashtable) target).Add(property.PropertyName, dataBaseValue);
			}
			else
			{
				if ( target.GetType() != _class )
				{
					throw new ArgumentException( "Could not set value of type '"+ target.GetType() +"' in property '"+property.PropertyName+"' of type '"+_class+"'" );
				}

				if ( property.PropertyInfo != null )
				{
					property.PropertyInfo.SetValue( target, dataBaseValue, null );
				}
				else // Primitive type ('value')
				{
					target = dataBaseValue;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public ResultMap ResolveSubMap(IDataReader dataReader)
		{
			 ResultMap subMap = this;
			if (_discriminator != null)
			{	
				ResultProperty mapping = _discriminator.ResultProperty;
				object dataBaseValue = mapping.GetDataBaseValue( dataReader );
				subMap = _discriminator.GetSubMap( dataBaseValue.ToString() );

				if (subMap == null) 
				{
					subMap = this;
				} 
				else if (subMap != this) 
				{
					subMap = subMap.ResolveSubMap(dataReader);
				}
			}
			return subMap;
		}

		
		#endregion
	}
}
