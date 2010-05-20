
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
using System.Collections;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.TypeHandlers;

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
		[NonSerialized]
		private bool _usePositionalParameters =false;


		#endregion

		#region Properties
		/// <summary>
		/// Identifier used to identify the ParameterMap amongst the others.
		/// </summary>
		[XmlAttribute("id")]
		public string Id
		{
			get { return _id; }
			set { _id = value; }
		}


		/// <summary>
		/// The collection of ParameterProperty
		/// </summary>
		[XmlIgnore]
		public ArrayList Properties
		{
			get
			{
				//				if (_usePositionalParameters) //obdc/oledb
				//				{
				//					return _properties;
				//				}
				//				else 
				//				{
				//					return _propertiesList;
				//				}
				return _properties;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[XmlIgnore]
		public ArrayList PropertiesList
		{
			get { return _propertiesList; }
		}

		/// <summary>
		/// Extend Parametermap attribute
		/// </summary>
		/// <remarks>The id of a ParameterMap</remarks>
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
		{}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="usePositionalParameters"></param>
		public ParameterMap(bool usePositionalParameters)
		{
			_usePositionalParameters = usePositionalParameters;

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
			if (_usePositionalParameters) //obdc/oledb
			{
				return (ParameterProperty)_properties[index];
			}
			else 
			{
				return (ParameterProperty)_propertiesList[index];
			}
			//return (ParameterProperty)_properties[index];
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
			idx = Convert.ToInt32(propertyName.Replace("[","").Replace("]",""));
			return idx;
		}
		

		/// <summary>
		/// Get all Parameter Property Name 
		/// </summary>
		/// <returns>A string array</returns>
		public string[] GetPropertyNameArray() 
		{
			string[] propertyNameArray = new string[_propertiesMap.Count];

			IEnumerator myEnumerator = _propertiesList.GetEnumerator();
			int index =0;
			while ( myEnumerator.MoveNext() )
			{
				propertyNameArray[index] = ((ParameterProperty)myEnumerator.Current).PropertyName;
				index++;
			}

			return (propertyNameArray); 
		}


		/// <summary>
		/// Set parameter value, replace the null value if any.
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataParameter"></param>
		/// <param name="parameterValue"></param>
		public void SetParameter(ParameterProperty mapping, IDataParameter dataParameter, object parameterValue)
		{
			object value = parameterValue;
			ITypeHandler typeHandler = mapping.TypeHandler;

			// "The primitive types are Boolean, Byte, SByte, Int16, UInt16, Int32,
			// UInt32, Int64, UInt64, Char, Double, and Single."

			if (parameterValue.GetType() != typeof(string) && 
				parameterValue.GetType() != typeof(Guid) &&
				parameterValue.GetType() != typeof(Decimal) &&
				parameterValue.GetType() != typeof(DateTime) &&
				!parameterValue.GetType().IsPrimitive)
			{
				value = ObjectProbe.GetPropertyValue(value, mapping.PropertyName);

				// This code is obsolete
				// if we realy need it we must put it in the SetParameter method 
				// of theByteArrayTypeHandler
//				if (value != null && value.GetType() == typeof(byte[]))
//				{
//					MemoryStream stream = new MemoryStream((byte[])value);
//
//					value = stream.ToArray();
//				}
			}

			// Apply Null Value
			if (mapping.HasNullValue) 
			{
				if (typeHandler.Equals(value, mapping.NullValue)) 
				{
					value = null;
				}
			}

			// Set Parameter
			if (value != null) 
			{
				typeHandler.SetParameter(dataParameter, value, mapping.DbType);
			}
			else if(typeHandler is CustomTypeHandler)
			{
				typeHandler.SetParameter(dataParameter, value, mapping.DbType);
			}
			else 
			{
				// When sending a null parameter value to the server,
				// the user must specify DBNull, not null. 
				dataParameter.Value = DBNull.Value;
			}
		}

		#region Configuration
		/// <summary>
		/// Initialize the parameter properties child.
		/// </summary>
		/// <param name="configScope"></param>
		public void Initialize(ConfigurationScope configScope)
		{
			_usePositionalParameters = configScope.DataSource.Provider.UsePositionalParameters;
			GetProperties( configScope );
		}


		/// <summary>
		///  Get the parameter properties child for the xmlNode parameter.
		/// </summary>
		/// <param name="configScope"></param>
		private void GetProperties(ConfigurationScope configScope)
		{
			XmlSerializer serializer = null;
			ParameterProperty property = null;

			serializer = new XmlSerializer(typeof(ParameterProperty));
			foreach ( XmlNode parameterNode in configScope.NodeContext.SelectNodes("parameter") )
			{
				property = (ParameterProperty) serializer.Deserialize(new XmlNodeReader(parameterNode));
				property.Initialize( configScope );
					
				AddParameterProperty(property);
			}
		}
		#endregion

		#endregion

	}
}
