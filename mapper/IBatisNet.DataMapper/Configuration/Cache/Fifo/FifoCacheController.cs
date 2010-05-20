
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
using System.Collections;
using System.Collections.Specialized;

using IBatisNet.DataMapper.Configuration.Cache;
#endregion

namespace IBatisNet.DataMapper.Configuration.Cache.Fifo
{
	/// <summary>
	/// Summary description for FifoCacheController.
	/// </summary>
	public class FifoCacheController : ICacheController
	{
		#region Fields 
		private int _cacheSize = 0;
		private Hashtable _cache = null;
		private IList _keyList = null;
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// 
		/// </summary>
		public FifoCacheController() 
		{
			_cacheSize = 100;
			_cache = new Hashtable();
			_keyList = new ArrayList();
		}
		#endregion

		#region ICacheController Members

		/// <summary>
		/// Clears all elements from the cache.
		/// </summary>
		public void Flush()
		{
			lock(this) 
			{
				_cache.Clear();
				_keyList.Clear();
			}		
		}


		/// <summary>
		/// Adds an item with the specified key and value into cached data.
		/// Gets a cached object with the specified key.
		/// </summary>
		/// <value>The cached object or <c>null</c></value>
		public object this [object key] 
		{
			get
			{
				lock (this) 
				{
					return _cache[key];
				}
			}
			set
			{
				lock (this) 
				{
					_cache.Add(key, value);
					_keyList.Add(key);
					if (_keyList.Count > _cacheSize) 
					{
						object oldestKey = _keyList[0];
						_keyList.Remove(0);
						_cache.Remove(oldestKey);
					}		
				}
			}
		}


		/// <summary>
		/// Configures the cache
		/// </summary>
		public void Configure(HybridDictionary properties)
		{
			string size = (string)properties["CacheSize"];;
			if (size != null) 
			{
				_cacheSize = System.Convert.ToInt32(size);		
			}
		}
		
		#endregion

	}
}
