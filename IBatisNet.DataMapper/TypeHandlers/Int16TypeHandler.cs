
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
using IBatisNet.DataMapper.Configuration.ResultMapping;

#endregion 



namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Description résumée de Int16TypeHandler.
	/// </summary>
	internal class Int16TypeHandler : BaseTypeHandler
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public override object GetValueByName(ResultProperty mapping, IDataReader dataReader)
		{
			int index = dataReader.GetOrdinal(mapping.ColumnName);

			if (dataReader.IsDBNull(index) == true)
			{
				return DBNull.Value;
			}
			else
			{
				// Don't used dataReader.GetInt32 to fix oracle who alwray return decimal type
				return Convert.ToInt16(dataReader.GetValue(index));
			}
		}

		public override object GetValueByIndex(ResultProperty mapping, IDataReader dataReader) 
		{
			if (dataReader.IsDBNull(mapping.ColumnIndex) == true)
			{
				return DBNull.Value;
			}
			else
			{
				// Don't used dataReader.GetInt32 to fix oracle who alwray return decimal type
				return Convert.ToInt16(dataReader.GetValue(mapping.ColumnIndex));
			}
		}

		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			return Convert.ToInt16(outputValue);
		}

		public override object ValueOf(Type type, string s)
		{
			return Convert.ToInt16(s);
		}

		public override bool IsSimpleType
		{
			get
			{
				return true;
			}
		}
	}
}
