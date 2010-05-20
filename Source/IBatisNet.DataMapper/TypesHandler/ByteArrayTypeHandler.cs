
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
using System.Data;
using System.Globalization;
using System.IO;

using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Exceptions;
#endregion


namespace IBatisNet.DataMapper.TypesHandler
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
		protected override object GetValueByName(ResultProperty mapping, IDataReader dataReader)
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

		protected override object GetValueByIndex(ResultProperty mapping, IDataReader dataReader) 
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
			int bufferSize = 100;                  // Size of the BLOB buffer.
			byte[] buffer = new byte[bufferSize];  // The BLOB byte[] buffer to be filled by GetBytes.
			long size = bufferSize;                // The bytes returned from GetBytes.
			long startIndex = 0;                   //  The data position in the BLOB output.
			MemoryStream stream = null;                   // Writes the BLOB to a memory stream.

			// Create a memory stream to hold the output.
			stream = new MemoryStream();

			// Reset the starting byte for the new BLOB.
			startIndex  = 0;

			// Read the bytes into outbyte[] and retain the number of bytes returned.
			size  = dataReader.GetBytes(columnIndex, startIndex , buffer, 0, bufferSize);

			// Continue reading and writing while there are bytes beyond the size of the buffer.
			while (size  == bufferSize)
			{
				stream.Write(buffer, 0, (int)size);
				stream.Flush();

				// Reposition the start index to the end of the last buffer and fill the buffer.
				startIndex += bufferSize;
				size  = dataReader.GetBytes(columnIndex, startIndex, buffer, 0, bufferSize);
			}

			// Write the remaining buffer.
			stream.Write(buffer, 0, (int)size);
			stream.Flush();

			return stream.ToArray();
		}

		protected override object GetNullValue(ResultProperty mapping) 
		{
			return null;
		}


		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			throw new DataMapperException("NotSupportedException");
		}

		public override bool IsEqualToNullValue(string nullValue, Object realValue) 
		{
			return false;
		}

		public override bool IsSimpleType() 
		{
			return true;
		}
	}
}
