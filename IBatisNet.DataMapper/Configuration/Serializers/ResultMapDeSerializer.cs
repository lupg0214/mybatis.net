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

using System.Collections.Specialized;
using System.Xml;
using IBatisNet.Common.Xml;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Scope;
#endregion 

namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for ResultMapDeSerializer.
	/// </summary>
	public class ResultMapDeSerializer
	{
		/// <summary>
		/// Deserialize a ResultMap object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static ResultMap Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			ResultMap resultMap = new ResultMap();

			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
			resultMap.ClassName = prop["class"];
			resultMap.ExtendMap = prop["extends"];
			resultMap.Id = prop["id"];

			resultMap.SqlMapNameSpace = configScope.SqlMapNamespace;

			configScope.ErrorContext.MoreInfo = "initialize ResultMap";

			resultMap.Initialize( configScope );

			return resultMap;
		}
	}
}
