
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

#region using
using System;
using System.Data;
using System.Globalization;
using System.IO;

using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Exceptions;
#endregion


namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Description résumée de ByteArrayTypeHandler.
	/// </summary>
	internal class ByteArrayTypeHandler : BaseTypeHandler
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
				return System.DBNull.Value;
			}
			else
			{
				return GetValueByIndex(index, dataReader);
			}
		}

		public override object GetValueByIndex(ResultProperty mapping, IDataReader dataReader) 
		{
			if (dataReader.IsDBNull(mapping.ColumnIndex) == true)
			{
				return System.DBNull.Value;
			}
			else
			{
				return GetValueByIndex(mapping.ColumnIndex, dataReader);
			}
		}


		private byte[] GetValueByIndex(int columnIndex, IDataReader dataReader) 
		{
			// determine the buffer size
			int bufferLength = (int) dataReader.GetBytes(columnIndex, 0, null, 0, 0);

			// initialize it
			byte[] byteArray = new byte[bufferLength];

			// fill it
			dataReader.GetBytes(columnIndex, 0, byteArray, 0, bufferLength);

			return byteArray;
		}

		
		public override object ValueOf(Type type, string s)
		{
			return System.Text.Encoding.Default.GetBytes(s);
		}

		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			throw new DataMapperException("NotSupportedException");
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
