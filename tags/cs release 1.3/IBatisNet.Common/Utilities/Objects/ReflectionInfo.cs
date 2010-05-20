
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

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

using IBatisNet.Common.Exceptions;

namespace IBatisNet.Common.Utilities.Objects
{
	/// <summary>
	/// This class represents a cached set of class definition information that
	/// allows for easy mapping between property names and get/set methods.
	/// </summary>
	public class ReflectionInfo
	{
		/// <summary>
		/// 
		/// </summary>
		public static BindingFlags BINDING_FLAGS_GET
			= BindingFlags.Public 
			| BindingFlags.GetProperty
			| BindingFlags.Instance 
			| BindingFlags.GetField
			;

		/// <summary>
		/// 
		/// </summary>
		public static BindingFlags BINDING_FLAGS_SET
			= BindingFlags.Public 
			| BindingFlags.SetProperty
			| BindingFlags.Instance 
			| BindingFlags.SetField
			;

		private static readonly string[] _emptyStringArray = new string[0];
		private static ArrayList _simleTypeMap = new ArrayList();
		private static Hashtable _reflectionInfoMap = Hashtable.Synchronized(new Hashtable());

		private string _className = string.Empty;
		private string[] _readablePropertyNames = _emptyStringArray;
		private string[] _writeablePropertyNames = _emptyStringArray;
		// (propertyName, property)
		private Hashtable _setProperties = new Hashtable();
		// (propertyName, property)
		private Hashtable _getProperties = new Hashtable();
		// (propertyName, property type)
		private Hashtable _setTypes = new Hashtable();
		// (propertyName, property type)
		private Hashtable _getTypes = new Hashtable();

		/// <summary>
		/// 
		/// </summary>
		public string ClassName 
		{
			get
			{
				return _className;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		static ReflectionInfo()
		{
			_simleTypeMap.Add(typeof(string));
			_simleTypeMap.Add(typeof(byte));
			_simleTypeMap.Add(typeof(char));
			_simleTypeMap.Add(typeof(bool));
			_simleTypeMap.Add(typeof(Guid));
			_simleTypeMap.Add(typeof(Int16));
			_simleTypeMap.Add(typeof(Int32));
			_simleTypeMap.Add(typeof(Int64));
			_simleTypeMap.Add(typeof(Single));
			_simleTypeMap.Add(typeof(Double));
			_simleTypeMap.Add(typeof(Decimal));
			_simleTypeMap.Add(typeof(DateTime));
			_simleTypeMap.Add(typeof(TimeSpan));
			_simleTypeMap.Add(typeof(Hashtable));
			_simleTypeMap.Add(typeof(SortedList));
			_simleTypeMap.Add(typeof(ListDictionary));
			_simleTypeMap.Add(typeof(HybridDictionary));


			//			_simleTypeMap.Add(Class.class);
			//			_simleTypeMap.Add(Collection.class);
			//			_simleTypeMap.Add(HashMap.class);
			//			_simleTypeMap.Add(TreeMap.class);
			_simleTypeMap.Add(typeof(ArrayList));
			//			_simleTypeMap.Add(HashSet.class);
			//			_simleTypeMap.Add(TreeSet.class);
			//			_simleTypeMap.Add(Vector.class);
			_simleTypeMap.Add(typeof(IEnumerator));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		private ReflectionInfo(Type type) 
		{
			_className = type.Name;
			AddPropertiess(type);

			string[] getArray = new string[_getProperties.Count];
			_getProperties.Keys.CopyTo(getArray,0);
			_readablePropertyNames = getArray;

			string[] setArray = new string[_setProperties.Count];
			_setProperties.Keys.CopyTo(setArray,0);
			_writeablePropertyNames = setArray;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		private void AddPropertiess(Type type) 
		{
			PropertyInfo[] properties = type.GetProperties(BINDING_FLAGS_SET);
			for (int i = 0; i < properties.Length; i++) 
			{
				string name = properties[i].Name;
				_setProperties.Add(name, properties[i]);
				_setTypes.Add(name, properties[i].PropertyType);
			}

			properties = type.GetProperties(BINDING_FLAGS_GET);
			for (int i = 0; i < properties.Length; i++) 
			{
				string name = properties[i].Name;
				_getProperties.Add(name, properties[i]);
				_getTypes.Add(name, properties[i].PropertyType);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public PropertyInfo GetSetter(string propertyName) 
		{
			PropertyInfo propertyInfo = (PropertyInfo) _setProperties[propertyName];

			if (propertyInfo == null) 
			{
				throw new ProbeException("There is no Set property named '" + propertyName + "' in class '" + _className + "'");
			}				

			return propertyInfo;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public PropertyInfo GetGetter(string propertyName) 
		{
			PropertyInfo propertyInfo = (PropertyInfo) _getProperties[propertyName];
			if (propertyInfo == null) 
			{
				throw new ProbeException("There is no Get property named '" + propertyName + "' in class '" + _className + "'");
			}
			return propertyInfo;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public Type GetSetterType(string propertyName) 
		{
			Type type = (Type) _setTypes[propertyName];
			if (type == null) 
			{
				throw new ProbeException("There is no Set property named '" + propertyName + "' in class '" + _className + "'");
			}
			return type;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public Type GetGetterType(string propertyName) 
		{
			Type type = (Type) _getTypes[propertyName];
			if (type == null) 
			{
				throw new ProbeException("There is no Get property named '" + propertyName + "' in class '" + _className + "'");
			}
			return type;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string[] GetReadablePropertyNames() 
		{
			return _readablePropertyNames;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string[] GetWriteablePropertyNames() 
		{
			return _writeablePropertyNames;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public bool HasWritableProperty(string propertyName) 
		{
			return _setProperties.ContainsKey(propertyName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public bool HasReadableProperty(string propertyName) 
		{
			return _getProperties.ContainsKey(propertyName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsKnownType(Type type) 
		{
			if (_simleTypeMap.Contains(type)) 
			{
				return true;
			} 
			else if (typeof(IList).IsAssignableFrom(type)) 
			{
				return true;
			} 
			else if (typeof(IDictionary).IsAssignableFrom(type)) 
			{
				return true;
			} 
			else if (typeof(IEnumerator).IsAssignableFrom(type)) 
			{
				return true;
			} 
			else 
			{
				return false;
			}
		}

//		/// <summary>
//		///  Returns the type that the get expects to receive as a parameter when
//		///  setting a property value.
//		/// </summary>
//		/// <param name="type">The type to check</param>
//		/// <param name="propertyName">The name of the property</param>
//		/// <returns>The type of the property</returns>
//		public static ReflectionInfo GetReflectionInfoForGetter(Type type, string propertyName) 
//		{
//			ReflectionInfo reflectionInfo = null;
//			if (propertyName.IndexOf('.') > -1) 
//			{
//				StringTokenizer parser = new StringTokenizer(propertyName, ".");
//				IEnumerator enumerator = parser.GetEnumerator();
//
//				while (enumerator.MoveNext()) 
//				{
//					propertyName = (string)enumerator.Current;
//					type = ReflectionInfo.GetInstance(type).GetGetterType(propertyName);
//				}
//			} 
//			else 
//			{
//				reflectionInfo = ReflectionInfo.GetInstance(type);
//			}
//
//			return type;
//		}

		/// <summary>
		/// Gets an instance of ReflectionInfo for the specified type.
		/// </summary>summary>
		/// <param name="type">The type for which to lookup the method cache.</param>
		/// <returns>The properties cache for the type</returns>
		public static ReflectionInfo GetInstance(Type type) 
		{
			lock (type) 
			{
				ReflectionInfo cache = (ReflectionInfo) _reflectionInfoMap[type];
				if (cache == null) 
				{
					cache = new ReflectionInfo(type);
					_reflectionInfoMap.Add(type, cache);
				}
				return cache;
			}
		}

																									
	}
	
}
