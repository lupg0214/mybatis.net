
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
using System.Collections.Specialized;

namespace IBatisNet.Common.Logging
{
	/// <summary>
	/// Setting for a logger.
	/// </summary>
	internal class LogSetting
	{

		#region Fields

		private Type _factoryAdapterType= null;
		private NameValueCollection _properties = null;

		#endregion 
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="factoryAdapterType"></param>
		/// <param name="properties"></param>
		public LogSetting ( Type factoryAdapterType , NameValueCollection properties )
		{
			_factoryAdapterType = factoryAdapterType;
			_properties = properties;
		}
		
		/// <summary>
		/// 
		/// </summary>
		public Type FactoryAdapterType
		{
			get { return _factoryAdapterType; }
		}

		/// <summary>
		/// 
		/// </summary>
		public NameValueCollection Properties
		{
			get { return _properties; }
		}
	}
}
