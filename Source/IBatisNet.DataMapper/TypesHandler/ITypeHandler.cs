
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
using System.Data;

using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper;

namespace IBatisNet.DataMapper.TypesHandler
{
	/// <summary>
	/// Summary description for ITypeHandler.
	/// </summary>
	public interface ITypeHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		object GetDataBaseValue(ResultProperty mapping, IDataReader dataReader);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nullValue"></param>
		/// <param name="realValue"></param>
		/// <returns></returns>
		bool IsEqualToNullValue(string nullValue, object realValue);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		bool IsSimpleType();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="outputValue"></param>
		/// <param name="parameterType"></param>
		/// <returns></returns>
		object GetDataBaseValue(object outputValue, Type parameterType);

//		/// <summary>
//		/// Get value to set a data parameter. 
//		/// </summary>
//		/// <param name="mapping">The mapping between data parameter and object property.</param>
//		/// <param name="source">The object from the value will be read.</param>
//		/// <returns>A value to set.</returns>
//		object GetDataParameter(ParameterProperty mapping, object source);

	}
}
