
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
using System.Xml.Serialization;

#endregion

namespace IBatisNet.Common
{
	/// <summary>
	/// Information about a data source.
	/// </summary>
	[Serializable]
	[XmlRoot("dataSource", Namespace="http://ibatis.apache.org/dataMapper")]
	public class DataSource
	{

		#region Fields
		[NonSerialized]
		private string _connectionString = string.Empty;
		[NonSerialized]
		private Provider _provider;
		[NonSerialized]
		private string _name = string.Empty;
		#endregion

		#region Properties
		/// <summary>
		/// The connection string.
		/// </summary>
		[XmlAttribute("connectionString")]
		public string ConnectionString
		{
			get { return _connectionString; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The connectionString attribute is mandatory in the data source " + _name);

				_connectionString = value;
			}
		}

		/// <summary>
		/// Name used to identify the provider amongst the others.
		/// </summary>
		[XmlAttribute("name")]
		public string Name
		{
			get { return _name; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The name attribute is mandatory in the data source.");

				_name = value; 
			}
		}

		/// <summary>
		/// The provider to use for this data source.
		/// </summary>
		[XmlIgnore]
		public Provider Provider
		{
			get { return _provider; }
			set
			{
				_provider = value;
			}
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Constructor
		/// </summary>
		public DataSource()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// ToString implementation.
		/// </summary>
		/// <returns>A string that describes the data source</returns>
		public override string ToString()
		{
			return "Source: ConnectionString : "+ConnectionString;
		}
		#endregion

	}
}
