
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
using System.Reflection;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Exceptions;
#endregion 



namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Summary description for EnumTypeHandler.
	/// </summary>
	internal class EnumTypeHandler : BaseTypeHandler
	{

		/// <summary>
		///  Sets a parameter on a IDbCommand
		/// </summary>
		/// <param name="dataParameter">the parameter</param>
		/// <param name="parameterValue">the parameter value</param>
		/// <param name="dbType">the dbType of the parameter</param>
		public override void SetParameter(IDataParameter dataParameter, object parameterValue, string dbType)
		{
			dataParameter.Value =  Convert.ChangeType( parameterValue, Enum.GetUnderlyingType( parameterValue.GetType() ) );;
		}

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
				return Enum.Parse(mapping.PropertyInfo.PropertyType, dataReader.GetValue(index).ToString());
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
				return Enum.Parse(mapping.PropertyInfo.PropertyType, dataReader.GetValue(mapping.ColumnIndex).ToString());
			}
		}


		public override object ValueOf(Type type, string s)
		{
			return Enum.Parse(type, s);
		}

		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			return Enum.Parse(parameterType, outputValue.ToString());
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
