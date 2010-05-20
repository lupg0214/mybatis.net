
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
using System.Data;
using System.Xml.Serialization;

using IBatisNet.Common.Exceptions;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.Scope;
#endregion

namespace IBatisNet.DataMapper.Configuration.Statements
{
	/// <summary>
	/// Represent a store Procedure.
	/// </summary>
	[Serializable]
	[XmlRoot("procedure")]
	public class Procedure : Statement
	{

		#region Properties
		/// <summary>
		/// The type of the statement StoredProcedure.
		/// </summary>
		[XmlIgnoreAttribute]
		public override CommandType CommandType
		{
			get { return CommandType.StoredProcedure; }
		}

		/// <summary>
		/// Extend statement attribute
		/// </summary>
		[XmlIgnoreAttribute]
		public override string ExtendSatement
		{
			get { return string.Empty;  }
			set {  }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		[Obsolete("This public constructor with no parameter is not really obsolete, but is reserved for serialization.", false)]
		public Procedure():base()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		override internal void Initialize(ConfigurationScope configurationScope)
		{
			base.Initialize( configurationScope );
			if (this.ParameterMap == null)
			{
				throw new ConfigurationException("The parameterMap attribute is required in the procedure tag named '"+ this.Id +"'.");
			}
		}
		#endregion

	}
}
