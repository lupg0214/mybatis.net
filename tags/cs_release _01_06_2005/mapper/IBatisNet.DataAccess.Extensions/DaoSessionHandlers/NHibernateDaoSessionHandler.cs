
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
using IBatisNet.Common;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;

using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Exceptions;    
using IBatisNet.DataAccess.Interfaces;

using NHibernate.Cfg;
using NHibernate;
#endregion

namespace IBatisNet.DataAccess.Extensions.DaoSessionHandlers
{
	/// <summary>
	/// Summary description for NHibernateDaoSessionHandler.
	/// </summary>
	public class NHibernateDaoSessionHandler : IDaoSessionHandler										 
	{
		#region Constants
		private const string CONNECTION_STRING = "hibernate.connection.connection_string";
		#endregion

		#region Fields
		private ISessionFactory _factory = null;
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public ISessionFactory Factory
		{
			get { return _factory; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// 
		/// </summary>
		public NHibernateDaoSessionHandler()
		{

		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="properties"></param>
		/// <param name="resources"></param>
		public void Configure(NameValueCollection properties,IDictionary resources)
		{
			string mapping = string.Empty;

			try
			{
				NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration();

				// Set the connection string retrieve on the datasource
				config.SetProperty( CONNECTION_STRING, (resources["DataSource"] as DataSource).ConnectionString );

				foreach(DictionaryEntry entry in resources)
				{
					if ((entry.Key.ToString()).StartsWith("class.")) 
					{
						config.AddClass( Resources.TypeForName( entry.Value.ToString() ) );
					}
					if ((entry.Key.ToString())=="mapping") 
					{
						mapping = entry.Value.ToString();
					}

					config.SetProperty( entry.Key.ToString(), entry.Value.ToString() );
				}

				if (mapping.Length>0)
				{
					config.AddAssembly(mapping);
				}
				_factory = config.BuildSessionFactory();
			}
			catch(Exception e)
			{
				throw new ConfigurationException(string.Format("DaoManager could not configure NHibernateDaoSessionHandler. Cause: {0}", e.Message), e);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="daoManager"></param>
		/// <returns></returns>
		public DaoSession GetDaoSession(DaoManager daoManager)
		{
			return (new NHibernateDaoSession(daoManager, _factory));
		}
		#endregion

	}
}
