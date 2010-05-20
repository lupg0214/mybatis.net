
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
using System.Reflection;

using IBatisNet.Common;
using IBatisNet.Common.Exceptions;    
using IBatisNet.Common.Utilities;

using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess.Exceptions;
#endregion

namespace IBatisNet.DataAccess.Configuration
{
	/// <summary>
	/// Description résumée de DaoSessionHandler.
	/// </summary>
	[Serializable]
	[XmlRoot("handler")]
	public class DaoSessionHandler
	{
		#region Constants
		/// <summary>
		/// An empty object array.
		/// </summary>
		public static readonly object [] EmptyObjects = new object [] {};
		#endregion

		#region Fields
		[NonSerialized]
		private string _name = string.Empty;
		[NonSerialized]
		private string _implementation =string.Empty;
		[NonSerialized]
		private bool _isDefault = false;
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("default")]
		public bool IsDefault
		{             
			get { return _isDefault; }
			set {_isDefault = value;}
		}

	
		/// <summary>
		/// 
		/// </summary>
		/// <example>
		/// Examples of Type: "IBatisNet.DataAccess.DaoSessionHandlers.SimpleDaoSessionHandler"
		/// </example>
		[XmlAttribute("implementation")]
		public string Implementation
		{
			get { return _implementation; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("Implementation");
				_implementation = value;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("id")]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("Name");
				_name = value;
			}
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		[Obsolete("This public constructor with no parameter is not really obsolete, but is reserved for serialization.", false)]
		public DaoSessionHandler()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name"></param>
		/// <param name="implementation"></param>
		public DaoSessionHandler(string name, string implementation)
		{
			if ((implementation == null) || (implementation.Length < 1))
				throw new ArgumentNullException("implementation");
			if ((name == null) || (name.Length < 1))
				throw new ArgumentNullException("name");

			_implementation = implementation;
			_name = name;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Build an implementation of the IDaoSessionHandler
		/// </summary>
		/// <returns>An implementation</returns>
		public IDaoSessionHandler GetIDaoSessionHandler()
		{
			Type type = null;
			
			try
			{
				type = Resources.TypeForName(_implementation);

				return (IDaoSessionHandler)Activator.CreateInstance(type, EmptyObjects);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("DaoManager could not configure DaoSessionHandler. DaoSessionHandler named \"{0}\", failed. Cause: {1}", _name, e.Message)
					);
			}
		}
		#endregion


	}
}
