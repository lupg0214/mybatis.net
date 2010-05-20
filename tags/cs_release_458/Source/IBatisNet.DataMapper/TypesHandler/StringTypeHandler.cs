
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

using IBatisNet.DataMapper.Configuration.ResultMapping;

namespace IBatisNet.DataMapper.TypesHandler
{
	/// <summary>
	/// Description résumée de SByteTypeHandler.
	/// </summary>
	internal class StringTypeHandler : BaseTypeHandler
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
				return dataReader.GetString(index);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		protected override object GetValueByIndex(ResultProperty mapping, IDataReader dataReader) 
		{	
			if (dataReader.IsDBNull(mapping.ColumnIndex) == true)
			{
				return System.DBNull.Value;
			}
			else
			{
				return dataReader.GetString(mapping.ColumnIndex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		/// <returns></returns>
		protected override object GetNullValue(ResultProperty mapping) 
		{
			return Convert.ToString(mapping.NullValue);
		}

		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			return Convert.ToString(outputValue);;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nullValue"></param>
		/// <param name="realValue"></param>
		/// <returns></returns>
		public override bool IsEqualToNullValue(string nullValue, object realValue) 
		{
			return Convert.ToString(nullValue).Equals(realValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override bool IsSimpleType() 
		{
			return true;
		}
	}
}
