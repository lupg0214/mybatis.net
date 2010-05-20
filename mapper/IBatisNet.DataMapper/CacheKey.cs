
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
using System.Collections;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.TypeHandlers;

#endregion


namespace IBatisNet.DataMapper
{
	/// <summary>
	/// Summary description for FlushInterval.
	/// </summary>
	[Serializable]
	internal class CacheKey
	{
		#region Fields
		private string[] _properties = null;
		private object _parameter = null;
		private string _sql = string.Empty;
		private string _statementName = string.Empty;
		private int _maxResults = 0 ;
		private int _skipRecords = 0;
		private CacheKeyType _type = CacheKeyType.Object;
		private TypeHandlerFactory _typeHandlerFactory = null;

		private string _hashCodeString = string.Empty;
		private int _hashCode = 0;
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="statementName"></param>
		/// <param name="sql"></param>
		/// <param name="parameter"></param>
		/// <param name="properties"></param>
		/// <param name="skipRecords"></param>
		/// <param name="maxResults"></param>
		/// <param name="type"></param>
		/// <param name="typeHandlerFactory"></param>
		internal CacheKey(TypeHandlerFactory typeHandlerFactory, string statementName, string sql, object parameter, string[] properties, 
			int skipRecords, int maxResults, CacheKeyType type)
		{
			_typeHandlerFactory = typeHandlerFactory;
			_statementName = statementName;
			_sql = sql;
			_parameter = parameter;
			_properties = properties;
			_skipRecords = skipRecords;
			_maxResults = maxResults;
			_type = type;
			_hashCode = GenerateHashCode();
			_hashCodeString = Convert.ToString(_hashCode);
		}


		// name.GetHashCode() ^  age.GetHashCode(); 
		// hash algorithms 


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private int GenerateHashCode() 
		{
			int result = 0;

			if (_parameter is Hashtable) 
			{
				result = (_parameter != null ? _parameter.GetHashCode() : 0);
			} 
			else if ( _parameter != null && _typeHandlerFactory.IsSimpleType(_parameter.GetType()) ) 
			{
				result = (_parameter != null ? _parameter.GetHashCode() : 0);
			} 
			else 
			{
				result = (_parameter != null ? ObjectProbe.ObjectHashCode(_parameter, _properties) : 0);
			}
			result = 29 * result + (_statementName != null ? _statementName.GetHashCode() : 0);
			result = 29 * result + (_sql != null ? _sql.GetHashCode() : 0);
			result = 29 * result + _maxResults;
			result = 29 * result + _skipRecords;
			result = 29 * result + (int)_type;
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj) 
		{
			//-----------------------------------
			if (this == obj) return true;
			if (!(obj is CacheKey)) return false;

			CacheKey cacheKey = (CacheKey)obj;

			if (_maxResults != cacheKey._maxResults) return false;
			if (_skipRecords != cacheKey._skipRecords) return false;
			if (_type != cacheKey._type) return false;
			if (_parameter is Hashtable) 
			{
				if (_hashCode != cacheKey._hashCode) return false;
				if (!_parameter.Equals(cacheKey._parameter)) return false;
			} 
			else if (_parameter != null && _typeHandlerFactory.IsSimpleType(_parameter.GetType())) 
			{
				if (_parameter != null ? !_parameter.Equals(cacheKey._parameter) : cacheKey._parameter != null) return false;
			} 
			else 
			{
				if (_hashCode != cacheKey._hashCode) return false;
			}
			if (_sql != null ? !_sql.Equals(cacheKey._sql) : cacheKey._sql != null) return false;

			return true;
		}


		/// <summary>
		/// Get the HashCode for this CacheKey
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() 
		{
			return _hashCode;
		}

		/// <summary>
		/// ToString implementation.
		/// </summary>
		/// <returns>A string that give the CacheKey HashCode.</returns>
		public override string ToString() 
		{
			return _hashCodeString;
		}

	}
}
