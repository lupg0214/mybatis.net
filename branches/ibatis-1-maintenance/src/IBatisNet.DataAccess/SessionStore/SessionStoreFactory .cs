#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 378715 $
 * $Date: 2007-03-06 15:23:03 -0700 (Tue, 06 Mar 2007) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006 - Apache Fondation
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


namespace IBatisNet.DataAccess.SessionStore
{
	/// <summary>
	/// Build a session container for a Windows or Web context.
	/// When running in the context of a web application the session object is 
	/// stored in HttpContext items and has 'per request' lifetime.
	/// When running in the context of a windows application the session object is 
	/// stored via CallContext.
	/// </summary>
	public sealed class SessionStoreFactory
	{

		/// <summary>
		/// Get a session container for a Windows or Web context.
		/// </summary>
		/// <param name="daoManagerName">The DaoManager name.</param>
		/// <returns></returns>
		static public ISessionStore GetSessionStore(string daoManagerName)
		{
			if (System.Web.HttpContext.Current == null)
			{
				return new CallContextSessionStore(daoManagerName);
			}
			else
			{
				return new WebSessionStore(daoManagerName);
			}
		}

	}
}

