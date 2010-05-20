
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
using System.Collections;

using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.Statements;
#endregion

namespace IBatisNet.DataMapper.Scope
{
	/// <summary>
	/// Hold data during the process of a mapped statement.
	/// </summary>
	public class RequestScope
	{
		#region Fields
		
		private ParameterMap _parameterMap = null;
		private ResultMap _resultMap = null;
		private PreparedStatement _preparedStatement = null;
		private Queue _properties = new Queue();

		#endregion
	
		#region Properties

		/// <summary>
		/// The 'select' result property to process after having process the main properties.
		/// </summary>
		public Queue QueueSelect
		{
			get { return _properties; }
			set { _properties = value; }
		}

		/// <summary>
		/// The ResultMap used by this request.
		/// </summary>
		public ResultMap ResultMap
		{
			get { return _resultMap; }
			set { _resultMap = value; }
		}

		/// <summary>
		/// The parameterMap used by this request.
		/// </summary>
		public ParameterMap ParameterMap
		{
			get { return _parameterMap; }
			set { _parameterMap = value; }
		}

		/// <summary>
		/// The PreparedStatement used by this request.
		/// </summary>
		public PreparedStatement PreparedStatement
		{
			get { return _preparedStatement; }
			set { _preparedStatement = value; }
		}
		#endregion

	}
}
