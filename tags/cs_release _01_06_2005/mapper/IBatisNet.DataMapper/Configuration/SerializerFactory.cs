
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
using System.Xml.Serialization;

using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements;
#endregion

namespace IBatisNet.DataMapper.Configuration
{
	/// <summary>
	/// Summary description for SerializerFactory.
	/// </summary>
	public class SerializerFactory
	{
		private static Hashtable _serializerMap = new Hashtable();

		/// <summary>
		/// 
		/// </summary>
		static SerializerFactory()
		{
			_serializerMap.Add("dynamic", new XmlSerializer(typeof(Dynamic)));
			_serializerMap.Add("isEqual", new XmlSerializer(typeof(IsEqual)));
			_serializerMap.Add("isNotEqual", new XmlSerializer(typeof(IsNotEqual)));
			_serializerMap.Add("isGreaterEqual", new XmlSerializer(typeof(IsGreaterEqual)));
			_serializerMap.Add("isGreaterThan", new XmlSerializer(typeof(IsGreaterThan)));
			_serializerMap.Add("isLessEqual", new XmlSerializer(typeof(IsLessEqual)));
			_serializerMap.Add("isLessThan", new XmlSerializer(typeof(IsLessThan)));
			_serializerMap.Add("isNotEmpty", new XmlSerializer(typeof(IsNotEmpty)));
			_serializerMap.Add("isEmpty", new XmlSerializer(typeof(IsEmpty)));
			_serializerMap.Add("isNotNull", new XmlSerializer(typeof(IsNotNull)));
			_serializerMap.Add("isNotParameterPresent", new XmlSerializer(typeof(IsNotParameterPresent)));
			_serializerMap.Add("isNotPropertyAvailable", new XmlSerializer(typeof(IsNotPropertyAvailable)));
			_serializerMap.Add("isNull", new XmlSerializer(typeof(IsNull)));
			_serializerMap.Add("isParameterPresent", new XmlSerializer(typeof(IsParameterPresent)));
			_serializerMap.Add("isPropertyAvailable", new XmlSerializer(typeof(IsPropertyAvailable)));
			_serializerMap.Add("iterate", new XmlSerializer(typeof(Iterate)));

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static XmlSerializer GetSerializer(string name) 
		{
			return (XmlSerializer) _serializerMap[name];
		}
	}
}
