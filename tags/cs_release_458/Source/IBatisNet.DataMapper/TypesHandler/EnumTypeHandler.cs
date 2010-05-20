
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
using System.Globalization;

using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Exceptions;

namespace IBatisNet.DataMapper.TypesHandler
{
	/// <summary>
	/// Summary description for EnumTypeHandler.
	/// </summary>
	internal class EnumTypeHandler : BaseTypeHandler
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		protected override object GetValueByName(ResultProperty mapping, IDataReader dataReader)
		{
			int index = dataReader.GetOrdinal(mapping.ColumnName);

			if (dataReader.IsDBNull(index) == true)
			{
				return System.DBNull.Value;
			}
			else
			{  
				return Enum.Parse(mapping.PropertyInfo.PropertyType, dataReader.GetValue(index).ToString());
			}
		}

		protected override object GetValueByIndex(ResultProperty mapping, IDataReader dataReader) 
		{
			if (dataReader.IsDBNull(mapping.ColumnIndex) == true)
			{
				return System.DBNull.Value;
			}
			else
			{
				return Enum.Parse(mapping.PropertyInfo.PropertyType, dataReader.GetValue(mapping.ColumnIndex).ToString());
			}
		}

		protected override object GetNullValue(ResultProperty mapping) 
		{
			return Enum.Parse(mapping.PropertyInfo.PropertyType, mapping.NullValue);
		}

		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			return Enum.Parse(parameterType, outputValue.ToString());
		}

		public override bool IsEqualToNullValue(string nullValue, object realValue) 
		{
			return Enum.Parse(realValue.GetType(), nullValue).Equals(realValue);
		}

		public override bool IsSimpleType() 
		{
			return true;
		}
	}
}
