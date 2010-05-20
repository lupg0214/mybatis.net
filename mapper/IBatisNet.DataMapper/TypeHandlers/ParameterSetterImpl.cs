
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

using System.Data;

#endregion 

namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// A ParameterSetter implementation
	/// </summary>
	public class ParameterSetterImpl  : IParameterSetter	
	{
		#region Fields

		private IDataParameter _dataParameter = null;

		#endregion 

		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="dataParameter"></param>
		public ParameterSetterImpl(IDataParameter dataParameter) 
		{
			_dataParameter = dataParameter;
		}
		#endregion 

		#region IParameterSetter members

		/// <summary>
		/// Returns the underlying DataParameter
		/// </summary>
		public IDataParameter DataParameter
		{
			get
			{
				return _dataParameter;
			}
		}

		/// <summary>
		/// Set the parameter value
		/// </summary>
		public object Value
		{
			set
			{
				_dataParameter.Value = value;
			}
		}

		#endregion
	}
}
