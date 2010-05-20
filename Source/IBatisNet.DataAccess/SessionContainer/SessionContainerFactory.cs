
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: $
 * $Date: $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
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
using System.Web;
#endregion

namespace IBatisNet.DataAccess.SessionContainer
{
	/// <summary>
	/// Build a session container for a Windows or Web context.
	/// When running in the context of a web application the session object is 
	/// stored in HttpContext items and has 'per request' lifetime.
	/// When running in the context of a windows application the session object is 
	/// stored in a with a TLS (ThreadLocalStorage).
	/// </summary>
	public class SessionContainerFactory
	{
		#region Methods
		/// <summary>
		/// Get a session container for a Windows or Web context.
		/// </summary>
		/// <param name="daoManagerName">The DaoManager name.</param>
		/// <returns></returns>
		static public ISessionContainer GetSessionContainer(string daoManagerName)
		{
			if (System.Web.HttpContext.Current == null)
			{
				return new WindowSessionContainer();
			}
			else
			{
				return new WebSessionContainer(daoManagerName);
			}
		}
		#endregion
	}
}
