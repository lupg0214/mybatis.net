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
#endregion 

namespace IBatisNet.Common
{
	/// <summary>
	/// Summary description for ProviderDeSerializer.
	/// </summary>
	public class ProviderDeSerializer
	{
		/// <summary>
		/// Deserialize a Provider object
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static Provider Deserialize(XmlNode node)
		{
			Provider provider = new Provider();
			NameValueCollection prop = NodeUtils.ParseAttributes(node);

			provider.AssemblyName = prop["assemblyName"];
			provider.CommandBuilderClass = prop["commandBuilderClass"];
			provider.CommandClass = prop["commandClass"];
			provider.ConnectionClass = prop["connectionClass"];
			provider.DataAdapterClass = prop["dataAdapterClass"];
			provider.Description = prop["description"];
			provider.IsDefault = NodeUtils.GetBooleanAttribute(prop, "default", false);
			provider.IsEnabled = NodeUtils.GetBooleanAttribute(prop, "enabled", true);
			provider.Name = prop["name"];
			provider.ParameterClass = prop["parameterClass"];
			provider.ParameterDbTypeClass = prop["parameterDbTypeClass"];
			provider.ParameterDbTypeProperty = prop["parameterDbTypeProperty"];
			provider.ParameterPrefix = prop["parameterPrefix"];
			provider.SetDbParameterPrecision = NodeUtils.GetBooleanAttribute(prop, "setDbParameterPrecision", true);
			provider.SetDbParameterScale = NodeUtils.GetBooleanAttribute(prop, "setDbParameterScale", true);
			provider.SetDbParameterSize = NodeUtils.GetBooleanAttribute(prop, "setDbParameterSize", true);
			provider.UseDeriveParameters = NodeUtils.GetBooleanAttribute(prop, "useDeriveParameters", true);
			provider.UseParameterPrefixInParameter = NodeUtils.GetBooleanAttribute(prop, "useParameterPrefixInParameter", true);
			provider.UseParameterPrefixInSql = NodeUtils.GetBooleanAttribute(prop, "useParameterPrefixInSql", true);
			provider.UsePositionalParameters = NodeUtils.GetBooleanAttribute(prop, "usePositionalParameters", false);

			return provider;
		}
	}
}
