
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
using System.Globalization;

using IBatisNet.DataMapper.Configuration.ResultMapping;
#endregion 



namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Description résumée de TimespanTypeHandler.
	/// </summary>
	internal class TimeSpanTypeHandler : BaseTypeHandler
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public override object GetValueByName(ResultProperty mapping, IDataReader dataReader)
		{
			//return GetValueByIndex(dataReader.GetOrdinal(columnName), dataReader);
			return "ToDo";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public override object GetValueByIndex(ResultProperty mapping, IDataReader dataReader) 
		{
			// dépendra du dbType spécifié ds le ResultProperty (idem pour le StringTypeHandler
			// des fois en DateTime, d'autre fois en TimeSpan
			return "To do";
				//dataReader.GetTimeSpan(columnIndex);
		}

		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			return  new TimeSpan(Convert.ToInt64(outputValue));
		}

		public override object ValueOf(Type type, string s)
		{
			return new TimeSpan(Convert.ToInt64(s));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override bool IsSimpleType
		{
			get
			{
				return true;
			}
		}
	}
}
