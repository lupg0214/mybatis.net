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
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Scope;

#endregion 

namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for ProcedureDeSerializer.
	/// </summary>
	public class ProcedureDeSerializer
	{
		/// <summary>
		/// Deserialize a Procedure object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static Procedure Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			Procedure procedure = new Procedure();
			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
			
			procedure.CacheModelName = NodeUtils.GetStringAttribute(prop, "cacheModel");
			procedure.Id = NodeUtils.GetStringAttribute(prop, "id");
			procedure.ListClassName = NodeUtils.GetStringAttribute(prop, "listClass");
			procedure.ParameterClassName = NodeUtils.GetStringAttribute(prop, "parameterClass");
			procedure.ParameterMapName = NodeUtils.GetStringAttribute(prop, "parameterMap");
			procedure.ResultClassName = NodeUtils.GetStringAttribute(prop, "resultClass");
			procedure.ResultMapName = NodeUtils.GetStringAttribute(prop, "resultMap");
			procedure.ListClassName = NodeUtils.GetStringAttribute(prop, "listClass");

			return procedure;
		}
	}
}
