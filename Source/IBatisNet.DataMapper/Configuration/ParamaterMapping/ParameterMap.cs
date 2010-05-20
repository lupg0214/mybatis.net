
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
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Globalization;

using IBatisNet.Common.Utilities.Objects;
#endregion

namespace IBatisNet.DataMapper.Configuration.ParameterMapping
{
	/// <summary>
	/// Summary description for ParameterMap.
	/// </summary>
	[Serializable]
	[XmlRoot("parameterMap")]
	public class ParameterMap
	{

		#region private
		[NonSerialized]
		private string _id = string.Empty;
		[NonSerialized]
			// Properties list
		private ArrayList _properties = new ArrayList();
		// Same list as _properties but without doubled (Test UpdateAccountViaParameterMap2)
		[NonSerialized]
		private ArrayList _propertiesList = new ArrayList();		
		//(property Name, property)
		[NonSerialized]
		private Hashtable _propertiesMap = new Hashtable(); // Corrected ?? Support Request 1043181, move to HashTable
		[NonSerialized]
		private string _extendMap = string.Empty;

		#endregion

		#region Properties
		/// <summary>
		/// Identifier used to identify the ParameterMap amongst the others.
		/// </summary>
		[XmlAttribute("id")]
		public string Id
		{
			get { return _id; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The id attribut is mandatory in a ParameterMap tag.");

				_id = value; 
			}
		}


		/// <summary>
		/// The collection of ParameterProperty
		/// </summary>
		[XmlIgnoreAttribute]
		public ArrayList Properties
		{
			get { return _properties; }
		}

		/// <summary>
		/// 
		/// </summary>
		[XmlIgnoreAttribute]
		public ArrayList PropertiesList
		{
			get { return _propertiesList; }
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
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public ParameterMap()
		{
		}
		#endregion


		#region Methods
		/// <summary>
		/// Get the ParameterProperty at index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>A ParameterProperty</returns>
		public ParameterProperty GetProperty(int index)
		{
			return (ParameterProperty)_properties[index];
		}

		/// <summary>
		/// Get a ParameterProperty by his name.
		/// </summary>
		/// <param name="name">The name of the ParameterProperty</param>
		/// <returns>A ParameterProperty</returns>
		public ParameterProperty GetProperty(string name)
		{
			return (ParameterProperty)_propertiesMap[name];
		}


		/// <summary>
		/// Add a ParameterProperty to the ParameterProperty list.
		/// </summary>
		/// <param name="property"></param>
		public void AddParameterProperty(ParameterProperty property)
		{
			// These mappings will replace any mappings that this map 
			// had for any of the keys currently in the specified map. 
			_propertiesMap[property.PropertyName] = property;
			_properties.Add( property );
			
			if (_propertiesList.Contains(property) == false)
			{
				_propertiesList.Add( property );
			}
		}

		/// <summary>
		/// Insert a ParameterProperty ine the ParameterProperty list at the specified index..
		/// </summary>
		/// <param name="index">
		/// The zero-based index at which ParameterProperty should be inserted. 
		/// </param>
		/// <param name="property">The ParameterProperty to insert. </param>
		public void InsertParameterProperty(int index, ParameterProperty property)
		{
			// These mappings will replace any mappings that this map 
			// had for any of the keys currently in the specified map. 
			_propertiesMap[property.PropertyName] = property;
			_properties.Insert( index, property );
			
			if (_propertiesList.Contains(property) == false)
			{
				_propertiesList.Insert( index, property );
			}
		}

		/// <summary>
		/// Retrieve the index for array property
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public int GetParameterIndex(string propertyName) 
		{
			int idx = -1;
			//idx = (Integer) parameterMappingIndex.get(propertyName);
			idx = System.Convert.ToInt32(propertyName.Replace("[","").Replace("]",""));
			return idx;
		}
		

		/// <summary>
		/// Get all Parameter Property Name 
		/// </summary>
		/// <returns>A string array</returns>
		public string[] GetPropertyNameArray() 
		{
			string[] propertyNameArray = new string[_propertiesMap.Count];

			System.Collections.IEnumerator myEnumerator = _propertiesList.GetEnumerator();
			int index =0;
			while ( myEnumerator.MoveNext() )
			{
				propertyNameArray[index] = ((ParameterProperty)myEnumerator.Current).PropertyName;
				index++;
			}

			return (propertyNameArray); 
		}


		/// <summary>
		/// Get the value of a property form an object. Replace the null value if any.
		/// </summary>
		/// <param name="source">The object to get the property.</param>
		/// <param name="propertyName">The name of the property to read.</param>
		/// <returns>An object</returns>
		public object GetValueOfProperty(object source, string propertyName)
		{
			object propertyValue = null;

			if ( _propertiesMap.Contains( propertyName ) )
			{
				ParameterProperty property = (ParameterProperty)_propertiesMap[propertyName];

				// Get the property value
				propertyValue = ObjectProbe.GetPropertyValue(source, propertyName);

				if (propertyValue != null && propertyValue.GetType() == typeof(byte[]))
				{
					System.IO.MemoryStream stream = new System.IO.MemoryStream((byte[])propertyValue);

					propertyValue = stream.ToArray();
				}

				// Check null value
				// Case of Enum property
				if (propertyValue != null && propertyValue.GetType().IsEnum)
				{
					#region Enum case
					PropertyInfo propertyInfo =  ReflectionInfo.GetInstance(source.GetType()).GetGetter( propertyName );
				
					// check nullValue
					if ( property.HasNullValue == true )
					{
						object nullValue = null;

						nullValue = Enum.Parse(propertyInfo.PropertyType, property.NullValue);
						//Convert.ChangeType( property.NullValue, type );
						
						if ( object.Equals(propertyValue, nullValue) )
						{
							propertyValue = DBNull.Value; ;
						}
						else
						{
							// Convert enum value to numeric value
							propertyValue = Convert.ChangeType( propertyValue, Enum.GetUnderlyingType( propertyInfo.PropertyType ) );
						}
					}
					else
					{
						// Convert enum value to numeric value
						propertyValue = Convert.ChangeType( propertyValue, Enum.GetUnderlyingType( propertyInfo.PropertyType ) );
					}
					#endregion
				}
				else
				{
					// check nullValue
					if ( property.HasNullValue == true )
					{
						object nullValue = null;

						PropertyInfo propertyInfo =  ReflectionInfo.GetInstance(source.GetType()).GetGetter( propertyName );

						if (propertyInfo.PropertyType == typeof(System.Decimal))
						{
							#region Decimal
							CultureInfo culture = new CultureInfo( "en-US" );
							// nullValue decimal must be  ######.##
							nullValue = decimal.Parse( property.NullValue, culture);
							#endregion
						}
						else if (propertyInfo.PropertyType == typeof(System.Guid)) 
						{
							#region Guid
							nullValue = new Guid(property.NullValue); 
							#endregion
						}						
						else
						{
							nullValue = Convert.ChangeType( property.NullValue, propertyInfo.PropertyType );
						}

						if ( object.Equals(propertyValue, nullValue) )
						{
							propertyValue = DBNull.Value; ;
						}
					}
					else
					{
						if (propertyValue == null)
						{
							propertyValue = DBNull.Value; ;
						}
					}
				}
			}

			return propertyValue;
		}
	

		#region Configuration
		/// <summary>
		/// Initialize the parameter properties child.
		/// </summary>
		/// <param name="node"></param>
		public void Initialize(XmlNode node)
		{
			GetProperties( node);
		}


		/// <summary>
		///  Get the parameter properties child for the xmlNode parameter.
		/// </summary>
		/// <param name="node">An xmlNode.</param>
		private void GetProperties(XmlNode node)
		{
			XmlSerializer serializer = null;
			ParameterProperty property = null;

			serializer = new XmlSerializer(typeof(ParameterProperty));
			foreach ( XmlNode parameterNode in node.SelectNodes("parameter") )
			{
				property = (ParameterProperty) serializer.Deserialize(new XmlNodeReader(parameterNode));
				property.Initialize();
					
				AddParameterProperty(property);
			}
		}
		#endregion

		#endregion


}
}
