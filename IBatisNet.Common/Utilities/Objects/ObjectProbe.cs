
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
using System.Reflection;

using IBatisNet.Common.Exceptions;

namespace IBatisNet.Common.Utilities.Objects
{
	/// <summary>
	/// Description résumée de ObjectProbe.
	/// </summary>
	public class ObjectProbe
	{
		private static ArrayList _simpleTypeMap = new ArrayList();

		static ObjectProbe() 
		{
			_simpleTypeMap.Add(typeof(string));
			_simpleTypeMap.Add(typeof(Byte));
			_simpleTypeMap.Add(typeof(Int16));
			_simpleTypeMap.Add(typeof(char));
			_simpleTypeMap.Add(typeof(Int32));
			_simpleTypeMap.Add(typeof(Int64));
			_simpleTypeMap.Add(typeof(Single));
			_simpleTypeMap.Add(typeof(Double));
			_simpleTypeMap.Add(typeof(Boolean));
			_simpleTypeMap.Add(typeof(DateTime));
			_simpleTypeMap.Add(typeof(Decimal));

			//			_simpleTypeMap.Add(typeof(Hashtable));
			//			_simpleTypeMap.Add(typeof(SortedList));
			//			_simpleTypeMap.Add(typeof(ArrayList));
			//			_simpleTypeMap.Add(typeof(Array));

			//			simpleTypeMap.Add(LinkedList.class);
			//			simpleTypeMap.Add(HashSet.class);
			//			simpleTypeMap.Add(TreeSet.class);
			//			simpleTypeMap.Add(Vector.class);
			//			simpleTypeMap.Add(Hashtable.class);
			_simpleTypeMap.Add(typeof(SByte));
			_simpleTypeMap.Add(typeof(UInt16));
			_simpleTypeMap.Add(typeof(UInt32));
			_simpleTypeMap.Add(typeof(UInt64));
			_simpleTypeMap.Add(typeof(IEnumerator));
		}


		/// <summary>
		/// Returns an array of the readable properties names exposed by an object
		/// </summary>
		/// <param name="obj">The object</param>
		/// <returns>The properties name</returns>
		public static string[] GetReadablePropertyNames(object obj) 
		{
			return ReflectionInfo.GetInstance(obj.GetType()).GetReadablePropertyNames();
		}

	
		/// <summary>
		/// Returns an array of the writeable properties name exposed by a object
		/// </summary>
		/// <param name="obj">The object</param>
		/// <returns>The properties name</returns>
		public static string[] GetWriteablePropertyNames(object obj) 
		{
			return ReflectionInfo.GetInstance(obj.GetType()).GetWriteablePropertyNames();
		}

		/// <summary>
		///  Returns the type that the set expects to receive as a parameter when
		///  setting a property value.
		/// </summary>
		/// <param name="obj">The object to check</param>
		/// <param name="propertyName">The name of the property</param>
		/// <returns>The type of the property</returns>
		public static Type GetPropertyTypeForSetter(object obj, string propertyName) 
		{
			Type type = obj.GetType();

			if (obj is IDictionary) 
			{
				IDictionary map = (IDictionary) obj;
				object value = map[propertyName];
				if (value == null) 
				{
					type = typeof(object);
				} 
				else 
				{
					type = value.GetType();
				}
			} 
			else 
			{
				if (propertyName.IndexOf('.') > -1) 
				{
					StringTokenizer parser = new StringTokenizer(propertyName, ".");
					IEnumerator enumerator = parser.GetEnumerator();

					while (enumerator.MoveNext()) 
					{
						propertyName = (string)enumerator.Current;
						type = ReflectionInfo.GetInstance(type).GetSetterType(propertyName);
					}
				} 
				else 
				{
					type = ReflectionInfo.GetInstance(type).GetSetterType(propertyName);
				}
			}

			return type;
		}


		/// <summary>
		///  Returns the type that the set expects to receive as a parameter when
		///  setting a property value.
		/// </summary>
		/// <param name="type">The type to check</param>
		/// <param name="propertyName">The name of the property</param>
		/// <returns>The type of the property</returns>
		private static Type GetPropertyTypeForSetter(Type type, string propertyName) 
		{
			if (propertyName.IndexOf('.') > -1) 
			{
				StringTokenizer parser = new StringTokenizer(propertyName, ".");
				IEnumerator enumerator = parser.GetEnumerator();

				while (enumerator.MoveNext()) 
				{
					propertyName = (string)enumerator.Current;
					type = ReflectionInfo.GetInstance(type).GetSetterType(propertyName);
				}
			} 
			else 
			{
				type = ReflectionInfo.GetInstance(type).GetSetterType(propertyName);
			}

			return type;
		}


		/// <summary>
		///  Returns the type that the get expects to receive as a parameter when
		///  setting a property value.
		/// </summary>
		/// <param name="obj">The object to check</param>
		/// <param name="propertyName">The name of the property</param>
		/// <returns>The type of the property</returns>
		public static Type GetPropertyTypeForGetter(object obj, string propertyName) 
		{
			Type type = obj.GetType();

			if (obj is IDictionary) 
			{
				IDictionary map = (IDictionary) obj;
				object value = map[propertyName];
				if (value == null) 
				{
					type = typeof(object);
				} 
				else 
				{
					type = value.GetType();
				}
			} 
			else 
			{
				if (propertyName.IndexOf('.') > -1) 
				{
					StringTokenizer parser = new StringTokenizer(propertyName, ".");
					IEnumerator enumerator = parser.GetEnumerator();

					while (enumerator.MoveNext()) 
					{
						propertyName = (string)enumerator.Current;
						type = ReflectionInfo.GetInstance(type).GetGetterType(propertyName);
					}
				} 
				else 
				{
					type = ReflectionInfo.GetInstance(type).GetGetterType(propertyName);
				}
			}

			return type;
		}


		/// <summary>
		///  Returns the type that the get expects to receive as a parameter when
		///  setting a property value.
		/// </summary>
		/// <param name="type">The type to check</param>
		/// <param name="propertyName">The name of the property</param>
		/// <returns>The type of the property</returns>
		public static Type GetPropertyTypeForGetter(Type type, string propertyName) 
		{
			if (propertyName.IndexOf('.') > -1) 
			{
				StringTokenizer parser = new StringTokenizer(propertyName, ".");
				IEnumerator enumerator = parser.GetEnumerator();

				while (enumerator.MoveNext()) 
				{
					propertyName = (string)enumerator.Current;
					type = ReflectionInfo.GetInstance(type).GetGetterType(propertyName);
				}
			} 
			else 
			{
				type = ReflectionInfo.GetInstance(type).GetGetterType(propertyName);
			}

			return type;
		}

		/// <summary>
		///  Returns the PropertyInfo of the set property on the specified type.
		/// </summary>
		/// <param name="type">The type to check</param>
		/// <param name="propertyName">The name of the property</param>
		/// <returns>The type of the property</returns>
		public static PropertyInfo GetPropertyInfoForSetter(Type type, string propertyName) 
		{
			PropertyInfo propertyInfo =null;
			if (propertyName.IndexOf('.') > -1) 
			{
				StringTokenizer parser = new StringTokenizer(propertyName, ".");
				IEnumerator enumerator = parser.GetEnumerator();
				Type parentType = null;

				while (enumerator.MoveNext()) 
				{
					propertyName = (string)enumerator.Current;
					parentType = type;
					type = ReflectionInfo.GetInstance(type).GetSetterType(propertyName);
				}
				propertyInfo = ReflectionInfo.GetInstance(parentType).GetSetter(propertyName);
			} 
			else 
			{
				propertyInfo = ReflectionInfo.GetInstance(type).GetSetter(propertyName);
			}

			return propertyInfo;
		}

		private static object GetArrayProperty(object obj, string indexedName) 
		{
			object value = null;

			try 
			{
				int startIndex  = indexedName.IndexOf("[");
				int length = indexedName.IndexOf("]");
				string name = indexedName.Substring(0, startIndex);
				string index = indexedName.Substring( startIndex+1, length-(startIndex+1));
				int i = System.Convert.ToInt32(index);
				
				if (name.Length > 0)
				{
					value = GetProperty(obj, name);
				}
				else
				{
					value = obj;
				}

				if (value is IList) 
				{
					value = ((IList) value)[i];
				} 
				else 
				{
					throw new ProbeException("The '" + name + "' property of the " + obj.GetType().Name + " class is not a List or Array.");
				}
			}
			catch (ProbeException pe) 
			{
				throw pe;
			} 
			catch(Exception e)
			{		
				throw new ProbeException("Error getting ordinal value from .net object. Cause" + e.Message, e);
			}

			return value;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		protected static object GetProperty(object obj, string propertyName) 
		{
			ReflectionInfo reflectionCache = ReflectionInfo.GetInstance(obj.GetType());

			try 
			{
				object value = null;

				if (propertyName.IndexOf("[") > -1) 
				{
					value = GetArrayProperty(obj, propertyName);
				} 
				else 
				{
					if (obj is IDictionary) 
					{
						value = ((IDictionary) obj)[propertyName];
					} 
					else 
					{
						PropertyInfo propertyInfo = reflectionCache.GetGetter(propertyName);
						if (propertyInfo == null) 
						{
							throw new ProbeException("No Get method for property " + propertyName + " on instance of " + obj.GetType().Name);
						}
						try 
						{
							value = propertyInfo.GetValue(obj, null);
						} 
						catch (ArgumentException ae) 
						{
							throw new ProbeException(ae);
						}
						catch (TargetException t) 
						{
							throw new ProbeException(t);
						}
						catch (TargetParameterCountException tp) 
						{
							throw new ProbeException(tp);
						}
						catch (MethodAccessException ma) 
						{
							throw new ProbeException(ma);
						}						
					}
				}
				return value;
			} 
			catch (ProbeException pe) 
			{
				throw pe;
			} 
			catch(Exception e)
			{
				throw new ProbeException("Could not Set property '" + propertyName + "' for " + obj.GetType().Name + ".  Cause: " + e.Message, e);
			}
		}


		private static void SetArrayProperty(object obj, string indexedName, object value) 
		{
			try 
			{
				int startIndex  = indexedName.IndexOf("[");
				int length = indexedName.IndexOf("]");
				string name = indexedName.Substring(0, startIndex);
				string index = indexedName.Substring( startIndex+1, length-(startIndex+1));
				int i = System.Convert.ToInt32(index);
				
				object list = null;
				if (name.Length > 0)
				{
					list = GetProperty(obj, name);
				}
				else
				{
					list = obj;
				}

				if (list is IList) 
				{
					((IList) list)[i] = value;
				} 
				else 
				{
					throw new ProbeException("The '" + name + "' property of the " + obj.GetType().Name + " class is not a List or Array.");
				}
			} 
			catch (ProbeException pe) 
			{
				throw pe;
			} 
			catch (Exception e) 
			{
				throw new ProbeException("Error getting ordinal value from .net object. Cause" + e.Message, e);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="propertyName"></param>
		/// <param name="propertyValue"></param>
		protected static void SetProperty(object obj, string propertyName, object propertyValue)
		{
			ReflectionInfo reflectionCache = ReflectionInfo.GetInstance(obj.GetType());

			try 
			{
				if (propertyName.IndexOf("[") > -1) 
				{
					SetArrayProperty(obj, propertyName, propertyValue);
				} 
				else 
				{
					if (obj is IDictionary) 
					{
						((IDictionary) obj)[propertyName] = propertyValue;
					} 
					else 
					{
						PropertyInfo propertyInfo = reflectionCache.GetSetter(propertyName);
						
						if (propertyInfo == null) 
						{
							throw new ProbeException("No Set method for property " + propertyName + " on instance of " + obj.GetType().Name);
						}
						try 
						{
							propertyInfo.SetValue(obj, propertyValue, null);
						}
						catch (ArgumentException ae) 
						{
							throw new ProbeException(ae);
						}
						catch (TargetException t) 
						{
							throw new ProbeException(t);
						}
						catch (TargetParameterCountException tp) 
						{
							throw new ProbeException(tp);
						}
						catch (MethodAccessException ma) 
						{
							throw new ProbeException(ma);
						}						
					}
				}
			}
			catch (ProbeException pe) 
			{
				throw pe;
			} 
			catch (Exception e) 
			{
				throw new ProbeException("Could not Get property '" + propertyName + "' for " + obj.GetType().Name + ".  Cause: " + e.Message, e);
			}
		}


		/// <summary>
		/// Return the specified property on an object. 
		/// </summary>
		/// <param name="obj">The Object on which to invoke the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>An Object representing the return value of the invoked property.</returns>
		public static object GetPropertyValue(object obj, string propertyName) 
		{
			if (propertyName.IndexOf('.') > -1) 
			{
				StringTokenizer parser = new StringTokenizer(propertyName, ".");
				IEnumerator enumerator = parser.GetEnumerator();
				object value = obj;
				string token = null;

				while (enumerator.MoveNext()) 
				{
					token = (string)enumerator.Current;
					value = GetProperty(value, token);

					if (value == null) 
					{
						break;
					}
				}
				return value;
			} 
			else 
			{
				return GetProperty(obj, propertyName);
			}
		}


		/// <summary>
		/// Set the specified property on an object 
		/// </summary>
		/// <param name="obj">The Object on which to invoke the specified property.</param>
		/// <param name="propertyName">The name of the property to set.</param>
		/// <param name="propertyValue">The new value to set.</param>
		public static void SetPropertyValue(object obj, string propertyName, object propertyValue)
		{
			if (propertyName.IndexOf('.') > -1) 
			{
				StringTokenizer parser = new StringTokenizer(propertyName, ".");
				IEnumerator enumerator = parser.GetEnumerator();
				enumerator.MoveNext();

				string currentPropertyName = (string)enumerator.Current;
				object child = obj;
      
				while (enumerator.MoveNext()) 
				{
					Type type = GetPropertyTypeForSetter(child, currentPropertyName);
					object parent = child;
					child = GetProperty(parent, currentPropertyName);
					if (child == null) 
					{
						try 
						{
							child = Activator.CreateInstance(type);
							SetPropertyValue(parent, currentPropertyName, child);
						} 
						catch (Exception e) 
						{
							throw new ProbeException("Cannot set value of property '" + propertyName + "' because '" + currentPropertyName + "' is null and cannot be instantiated on instance of " + type.Name + ". Cause:" + e.Message, e);
						}
					}
					currentPropertyName = (string)enumerator.Current;
				}
				SetProperty(child, currentPropertyName, propertyValue);
			} 
			else 
			{
				SetProperty(obj, propertyName, propertyValue);
			}
		}


		/// <summary>
		/// Checks to see if a Object has a writable property/field be a given name
		/// </summary>
		/// <param name="obj"> The object to check</param>
		/// <param name="propertyName">The property to check for</param>
		/// <returns>True if the property exists and is writable</returns>
		public static bool HasWritableProperty(object obj, string propertyName) 
		{
			bool hasProperty = false;
			if (obj is IDictionary) 
			{
				hasProperty = ((IDictionary) obj).Contains(propertyName);
			} 
			else 
			{
				if (propertyName.IndexOf('.') > -1) 
				{
					StringTokenizer parser = new StringTokenizer(propertyName, ".");
					IEnumerator enumerator = parser.GetEnumerator();
					Type type = obj.GetType();

					while (enumerator.MoveNext()) 
					{ 
						propertyName = (string)enumerator.Current;
						type = ReflectionInfo.GetInstance(type).GetGetterType(propertyName);
						hasProperty = ReflectionInfo.GetInstance(type).HasWritableProperty(propertyName);
					}
				} 
				else 
				{
					hasProperty = ReflectionInfo.GetInstance(obj.GetType()).HasWritableProperty(propertyName);
				}
			}
			return hasProperty;
		}


		/// <summary>
		/// Checks to see if the Object have a property/field be a given name.
		/// </summary>
		/// <param name="obj">The Object on which to invoke the specified property.</param>
		/// <param name="propertyName">The name of the property to check for.</param>
		/// <returns>
		/// True or false if the property exists and is readable.
		/// </returns>
		public static bool HasReadableProperty(object obj, string propertyName) 
		{
			bool hasProperty = false;

			if (obj is IDictionary) 
			{
				hasProperty = ((IDictionary) obj).Contains(propertyName);
			} 
			else 
			{
				if (propertyName.IndexOf('.') > -1) 
				{
					StringTokenizer parser = new StringTokenizer(propertyName, ".");
					IEnumerator enumerator = parser.GetEnumerator();
					Type type = obj.GetType();

					while (enumerator.MoveNext()) 
					{ 
						propertyName = (string)enumerator.Current;
						type = ReflectionInfo.GetInstance(type).GetGetterType(propertyName);
						hasProperty = ReflectionInfo.GetInstance(type).HasReadableProperty(propertyName);
					}
				} 
				else 
				{
					hasProperty = ReflectionInfo.GetInstance(obj.GetType()).HasReadableProperty(propertyName);
				}
			}
			
			return hasProperty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsSimpleType(Type type) 
		{
			if (_simpleTypeMap.Contains(type)) 
			{
				return true;
			} 
			else if (type.IsSubclassOf(typeof(ICollection)))
			{
				return true;
			} 
			else if (type.IsSubclassOf(typeof(IDictionary))) 
			{
				return true;
			} 
			else if (type.IsSubclassOf(typeof(IList))) 
			{
				return true;
			} 
			else if (type.IsSubclassOf(typeof(IEnumerable))) 
			{
				return true;
			} 
			else
			{
				return false;
			}
		}


		/// <summary>
		///  Calculates a hash code for all readable properties of a object.
		/// </summary>
		/// <param name="obj">The object to calculate the hash code for.</param>
		/// <returns>The hash code.</returns>
		public static int ObjectHashCode(object obj) 
		{
			return ObjectHashCode(obj, GetReadablePropertyNames(obj));
		}


		/// <summary>
		/// Calculates a hash code for a subset of the readable properties of a object.
		/// </summary>
		/// <param name="obj">The object to calculate the hash code for.</param>
		/// <param name="properties">A list of the properties to hash.</param>
		/// <returns>The hash code.</returns>
		public static int ObjectHashCode(object obj, string[] properties ) 
		{
			ArrayList alreadyDigested = new ArrayList();

			int hashcode = obj.GetType().FullName.GetHashCode();
			for (int i = 0; i < properties.Length; i++) 
			{
				object value = GetProperty(obj, properties[i]);
				if (value != null) 
				{
					if (IsSimpleType(value.GetType())) 
					{
						hashcode += value.GetHashCode();
						hashcode += value.ToString().GetHashCode()*37;
					} 
					else 
					{
						// It's a Object 
						// Check to avoid endless loop (circular dependency)
						if (value != obj) 
						{
							if (!alreadyDigested.Contains(value)) 
							{
								alreadyDigested.Add(value);
								hashcode += ObjectHashCode(value);
							}
						}
					}
					hashcode *= 29;
				}
			}
			return hashcode;
		}

	}
}
