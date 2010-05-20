
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
using System.Xml;
using System.Xml.Serialization;

using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities.TypesResolver;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.TypesHandler;

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
		private Type _class;
		//(columnName, property)
		[NonSerialized]
		private Hashtable _columnsToPropertiesMap = new Hashtable();	
		#endregion

		#region Properties
		/// <summary>
		/// The collection of result properties.
		/// </summary>
		[XmlIgnoreAttribute]
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
					throw new ArgumentNullException("The id attribut is mandatory in a ResultMap tag.");

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
		[XmlIgnoreAttribute]
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
					throw new ArgumentNullException("The class attribut is mandatory in a ResultMap tag.");

				_className = value; 
			}
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		[Obsolete("This public constructor with no parameter is not really obsolete, but is reserved for serialization.", false)]
		public ResultMap()
		{
		}
		#endregion

		#region Methods

		#region Configuration
		/// <summary>
		/// Initialize the resultMap from an xmlNode..
		/// </summary>
		/// <param name="sqlMap">The sqlMap.</param>
		/// <param name="node">An XmlNode.</param>
		public void Initialize( SqlMapper sqlMap, XmlNode node)
		{
			try
			{
				_class = sqlMap.GetType(_className);

				// Load the properties
				GetProperties(node);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Could not configure ResulMap. ResulMap named \"{0}\" not found, failed. \n Cause: {1}", _id, e.Message)
					);
			}
		}


		/// <summary>
		/// Get the result properties from the xmNode.
		/// </summary>
		/// <param name="node">An xmlNode.</param>
		private void GetProperties(XmlNode node)
		{
			XmlSerializer serializer = null;
			ResultProperty property = null;

			serializer = new XmlSerializer(typeof(ResultProperty));
			foreach ( XmlNode resultNode in node.SelectNodes("result") )
			{
				property = (ResultProperty) serializer.Deserialize(new XmlNodeReader(resultNode));
					
				PropertyInfo propertyInfo = null;

				if ( property.PropertyName != "value" && !typeof(IDictionary).IsAssignableFrom(_class) )
				{
					propertyInfo = ReflectionInfo.GetInstance(_class).GetSetter( property.PropertyName );
				}
				property.Initialize( propertyInfo );

				this.AddResultPropery( property  );
			}
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
		#endregion
	}
}
