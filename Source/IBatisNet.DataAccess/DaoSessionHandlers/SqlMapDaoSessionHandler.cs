
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

using IBatisNet.Common;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;

using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Exceptions;    
using IBatisNet.DataAccess.Interfaces;

using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
#endregion

namespace IBatisNet.DataAccess.DaoSessionHandlers
{
	/// <summary>
	/// Summary description for SqlMapDaoSessionHandler.
	/// </summary>
	public class SqlMapDaoSessionHandler : IDaoSessionHandler
	{
		#region Fields
		private SqlMapper _sqlMap;
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public SqlMapper SqlMap
		{
			get { return _sqlMap; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// 
		/// </summary>
		public SqlMapDaoSessionHandler()
		{
		}
		#endregion
		
		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="properties"></param>
		public void Configure(IDictionary properties)
		{
			try
			{
				string fileName = (string) properties["sqlMapConfigFile"];
				DataSource dataSource = (DataSource) properties["DataSource"];
				bool useConfigFileWatcher = (bool) properties["UseConfigFileWatcher"];
				
				if (useConfigFileWatcher == true)
				{
					ConfigWatcherHandler.AddFileToWatch( Resources.GetFileInfo( fileName ) );
				}

				_sqlMap = new DomSqlMapBuilder().Build( Resources.GetConfigAsXmlDocument(fileName), dataSource, useConfigFileWatcher);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(string.Format("DaoManager could not configure SqlMapDaoSessionHandler.Cause: {0}", e.Message));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="daoManager"></param>
		/// <returns></returns>
		public DaoSession GetDaoSession(DaoManager daoManager)
		{
			return (new SqlMapDaoSession(daoManager, _sqlMap));
		}
		#endregion

	}
}
