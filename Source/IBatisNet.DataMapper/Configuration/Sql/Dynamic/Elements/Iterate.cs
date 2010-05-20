
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
using System.Xml.Serialization;

using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Handlers;

namespace IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements
{
	/// <summary>
	/// Represent an iterate sql tag element.
	/// </summary>
	[Serializable]
	[XmlRoot("iterate")]
	public class Iterate : BaseTag
	{

		#region Fields
		
		[NonSerialized]
		private string _open = string.Empty;
		[NonSerialized]
		private string _close = string.Empty;
		[NonSerialized]
		private string _conjunction = string.Empty;

		#endregion


		/// <summary>
		/// Conjonction attribute
		/// </summary>
		[XmlAttribute("conjunction")]
		public string Conjunction
		{
			get
			{
				return _conjunction;
			}
			set
			{
				_conjunction = value;
			}
		}


		/// <summary>
		/// Close attribute
		/// </summary>
		[XmlAttribute("close")]
		public string Close
		{
			get
			{
				return _close;
			}
			set
			{
				_close = value;
			}
		}


		/// <summary>
		/// Open attribute
		/// </summary>
		[XmlAttribute("open")]
		public string Open
		{
			get
			{
				return _open;
			}
			set
			{
				_open = value;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public Iterate()
		{
			this.Handler = new IterateTagHandler();
		}

	}
}
