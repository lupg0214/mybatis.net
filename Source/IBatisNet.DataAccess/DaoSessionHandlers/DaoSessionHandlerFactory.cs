
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

using IBatisNet.Common.Exceptions;
using IBatisNet.DataAccess.Interfaces;
#endregion

namespace IBatisNet.DataAccess.DaoSessionHandlers
{
	/// <summary>
	/// Summary description for DaoSessionHandlerFactory.
	/// </summary>
	public class DaoSessionHandlerFactory
	{
		#region Fields
		private static HybridDictionary _daoSessionHandlerMap = new HybridDictionary();
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		static public int Count
		{
			get
			{
				return _daoSessionHandlerMap.Count;
			}
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Static constructor
		/// </summary>
		static DaoSessionHandlerFactory()
		{
			_daoSessionHandlerMap.Add("ADONET", new SimpleDaoSessionHandler());
			_daoSessionHandlerMap.Add("SqlMap", new SqlMapDaoSessionHandler());
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		static public IDaoSessionHandler GetDaoSessionHandler(string name)
		{
			if (_daoSessionHandlerMap.Contains(name) == true)
			{
				return (IDaoSessionHandler)_daoSessionHandlerMap[name];
			}
			else
			{
				throw new ConfigurationException("There is no DaoSessionHandler named " + name);
			}
		}
		#endregion

	}
}
