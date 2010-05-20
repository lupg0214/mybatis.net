
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
#endregion

namespace IBatisNet.DataMapper.SessionContainer
{
	/// <summary>
	/// Session container for windows/console application.
	/// </summary>
	public class WindowSessionContainer : ISessionContainer 
	{
		#region Fields
		[ThreadStatic] 
		private SqlMapSession _localSqlMapSession = null;
		#endregion

		#region ISessionContainer Members

		#region Properties
		/// <summary>
		/// Get the local session
		/// </summary>
		public SqlMapSession LocalSession
		{
			get
			{
				return _localSqlMapSession;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Store the local session on the container.
		/// Ensure that the session is unique for each thread.
		/// </summary>
		/// <param name="session">The session to store</param>
		public void Store(SqlMapSession session)
		{
			_localSqlMapSession = session;
		}

		/// <summary>
		/// Remove the local session from the container.
		/// </summary>
		public void Dispose()
		{
			_localSqlMapSession = null;
		}
		#endregion

		#endregion
	}
}
