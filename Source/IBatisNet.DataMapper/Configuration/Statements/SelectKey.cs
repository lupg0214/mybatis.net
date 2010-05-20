
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
using System.Xml.Serialization;

using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.Exceptions;
#endregion

namespace IBatisNet.DataMapper.Configuration.Statements
{
	/// <summary>
	/// Represent a SelectKey tag element.
	/// </summary>
	[Serializable]
	[XmlRoot("selectKey")]
	public class SelectKey : Statement 
	{

		#region Fields

		[NonSerialized]
		private SelectKeyType _selectKeyType = SelectKeyType.post;
		[NonSerialized]
		private string _property = string.Empty;

		#endregion

		#region Properties
		/// <summary>
		/// Extend statement attribute
		/// </summary>
		[XmlIgnoreAttribute]
		public override string ExtendSatement
		{
			get { return string.Empty;  }
			set {  }
		}

		/// <summary>
		/// The property name object to fill with the key.
		/// </summary>
		[XmlAttribute("property")]
		public string PropertyName
		{
			get { return _property; }
			set { _property = value; }
		}

		/// <summary>
		/// The type of the selectKey tag : 'Pre' or 'Post'
		/// </summary>
		[XmlAttribute("type")]
		public SelectKeyType SelectKeyType
		{
			get { return _selectKeyType; }
			set { _selectKeyType = value; }
		}


		/// <summary>
		/// True if it is a post-generated key.
		/// </summary>
		[XmlIgnoreAttribute]
		public bool isAfter
		{
			get { return _selectKeyType == SelectKeyType.post; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		[Obsolete("This public constructor with no parameter is not really obsolete, but is reserved for serialization.", false)]
		public SelectKey():base()
		{
		}
		#endregion

	}
}
