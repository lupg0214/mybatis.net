
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
using System.Runtime.Remoting.Messaging;

using IBatisNet.Common;
#endregion

namespace IBatisNet.DataAccess.SessionContainer
{
	/// <summary>
	/// Session container for web application.
	/// </summary>
	public class WebSessionContainer : ISessionContainer
	{

		#region Constants

		/// <summary>
		/// Token for SqlMapConfig xml root.
		/// </summary>
		private const string LOCAL_SESSION = "_IBATIS_LOCAL_DAOSESSION_";

		#endregion

		#region Fields
		private string _sessionName = string.Empty;
		#endregion

		#region Constructor (s) / Destructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="daoManagerName">The DaoManager name.</param>
		public WebSessionContainer(string daoManagerName)
		{
			_sessionName = LOCAL_SESSION+daoManagerName;
			CallContext.SetData(_sessionName, null);
		}
		#endregion

		#region ISessionContainer Members

		#region Properties
		/// <summary>
		/// Get the local session
		/// </summary>
		public IDalSession LocalSession
		{
			get
			{
				return CallContext.GetData(_sessionName) as IDalSession;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Store the local session on the container.
		/// Ensure that the session is unique for each thread.
		/// </summary>
		/// <param name="session">The session to store</param>
		public void Store(IDalSession session)
		{
			CallContext.SetData(_sessionName, session);
		}

		/// <summary>
		/// Remove the local session from the container.
		/// </summary>
		public void Dispose()
		{
			CallContext.SetData(_sessionName, null);
		}

		#endregion

		#endregion
	}
}
