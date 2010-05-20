
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
using System.Configuration;
using System.Xml;
using ConfigurationException = IBatisNet.Common.Exceptions.ConfigurationException;

namespace IBatisNet.Common.Logging
{
	/// <summary>
	/// Summary description for ConfigurationSectionHandler.
	/// </summary>
	public class ConfigurationSectionHandler: IConfigurationSectionHandler
	{

		#region Fields

		private static readonly string LOGFACTORYADAPTER_ELEMENT = "logFactoryAdapter";
		private static readonly string LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB = "type";
		private static readonly string ARGUMENT_ELEMENT = "arg";
		private static readonly string ARGUMENT_ELEMENT_KEY_ATTRIB = "key";
		private static readonly string ARGUMENT_ELEMENT_VALUE_ATTRIB = "value";

		#endregion 

		/// <summary>
		/// Constructor
		/// </summary>
		public ConfigurationSectionHandler()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		private LogSetting ReadConfiguration( XmlNode section )
		{
			XmlNode logFactoryElement = section.SelectSingleNode( LOGFACTORYADAPTER_ELEMENT );
			
			string factoryTypeString = string.Empty;
			if ( logFactoryElement.Attributes[LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB] != null )
				factoryTypeString = logFactoryElement.Attributes[LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB].Value;
            
			if ( factoryTypeString == string.Empty )
				throw new ConfigurationException
					( "Required Attribute '" 
					+ LOGFACTORYADAPTER_ELEMENT_TYPE_ATTRIB 
					+ "' not found in element '"
					+ LOGFACTORYADAPTER_ELEMENT
					+ "'"
					);


			Type factoryType = null;
			try
			{
				factoryType = Type.GetType( factoryTypeString, true, false );
			}
			catch ( Exception e )
			{
				throw new ConfigurationException
					( "Unable to create type '" + factoryTypeString + "'"
					, e
					);
			}
			
			XmlNodeList propertyNodes = logFactoryElement.SelectNodes( ARGUMENT_ELEMENT );
			
			NameValueCollection properties = new NameValueCollection( null, new CaseInsensitiveComparer() );

			foreach ( XmlNode propertyNode in propertyNodes )
			{
				string key = string.Empty;
				string itsValue = string.Empty;

				XmlAttribute keyAttrib = propertyNode.Attributes[ARGUMENT_ELEMENT_KEY_ATTRIB];
				XmlAttribute valueAttrib = propertyNode.Attributes[ARGUMENT_ELEMENT_VALUE_ATTRIB];

				if ( keyAttrib == null )
				{
					throw new ConfigurationException
						( "Required Attribute '" 
						+ ARGUMENT_ELEMENT_KEY_ATTRIB 
						+ "' not found in element '"
						+ ARGUMENT_ELEMENT
						+ "'"
						);
				}
				else
				{
					key = keyAttrib.Value;
				}

				if ( valueAttrib != null )
				{
					itsValue = valueAttrib.Value;
				}

				properties.Add( key, itsValue );
			}

			return new LogSetting( factoryType, properties );
		}

		#region IConfigurationSectionHandler Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			int logFactoryElementsCount = section.SelectNodes( LOGFACTORYADAPTER_ELEMENT ).Count;
			
			if ( logFactoryElementsCount > 1 )
			{
				throw new ConfigurationException( "Only one <logFactoryAdapter> element allowed" );
			}
			else if ( logFactoryElementsCount == 1 )
			{
				return ReadConfiguration( section );
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}

