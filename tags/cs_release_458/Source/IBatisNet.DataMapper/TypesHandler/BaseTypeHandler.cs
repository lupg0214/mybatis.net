
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
using System.Collections;

using IBatisNet.DataMapper.Configuration.ResultMapping;

using IBatisNet.Common.Utilities;

namespace IBatisNet.DataMapper.TypesHandler
{
	/// <summary>
	/// Summary description for BaseTypeHandler.
	/// </summary>
	internal abstract class BaseTypeHandler : ITypeHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public object GetDataBaseValue(ResultProperty mapping, IDataReader dataReader)
		{
			object value = null;

			if (mapping.ColumnIndex == ResultProperty.UNKNOWN_COLUMN_INDEX)  
			{
				value = GetValueByName(mapping, dataReader);
			} 
			else 
			{
				value = GetValueByIndex(mapping, dataReader);
			}

			bool wasNull = (value == System.DBNull.Value);
			if (wasNull)
			{
				if (mapping.HasNullValue) 
				{
					value = GetNullValue(mapping);
				}
				else
				{
					value = null;
				}			
			}

			return value;
		}


		protected abstract object GetValueByName(ResultProperty mapping, IDataReader dataReader);

		protected abstract object GetValueByIndex(ResultProperty mapping, IDataReader dataReader);

		protected abstract object GetNullValue(ResultProperty mapping);

		public abstract object GetDataBaseValue(object outputValue, Type parameterType );

		public abstract bool IsEqualToNullValue(string nullValue, object realValue);

		public abstract bool IsSimpleType();

//		public abstract object GetDataParameter(ParameterProperty mapping, object source);
	}
}
