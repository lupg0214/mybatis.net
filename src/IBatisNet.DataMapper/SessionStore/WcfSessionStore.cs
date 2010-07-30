#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 378715 $
 * $Date: 2006-11-19 09:07:45 -0700 (Sun, 19 Nov 2006) $
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


namespace IBatisNet.DataMapper.SessionStore
{

	/// <summary>
	/// Provides an implementation of <see cref="ISessionStore"/>
	/// which relies on <c>OperationContext</c>. Suitable for WCF based projects.
    /// This implementation will get the current session from the current 
    /// request.
	/// </summary>
	public class WcfSessionStore : AbstractSessionStore
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSessionStore"/> class.
        /// </summary>
        /// <param name="sqlMapperId">The SQL mapper id.</param>
        public WcfSessionStore(string sqlMapperId)
            : base(sqlMapperId)
		{}

		/// <summary>
		/// Get the local session
		/// </summary>
        public override ISqlMapSession LocalSession
		{
			get
			{
			    return WcfSessionItemsInstanceExtension.Current.Items.Find(sessionName) as SqlMapSession;
			}
		}

		/// <summary>
		/// Store the specified session.
		/// </summary>
		/// <param name="session">The session to store</param>
        public override void Store(ISqlMapSession session)
		{
            WcfSessionItemsInstanceExtension.Current.Items.Set(sessionName,session);
		}

		/// <summary>
		/// Remove the local session.
		/// </summary>
		public override void Dispose()
		{
            WcfSessionItemsInstanceExtension.Current.Items.Remove(sessionName);
		}

        ///<summary>
        /// Helper method used to short-cut for the Hybrid map
        ///</summary>
        ///<param name="sessionName"></param>
        ///<returns></returns>
        public static ISqlMapSession For(string sessionName)
        {
            return WcfSessionItemsInstanceExtension.Current.Items.Find(sessionName) as SqlMapSession;
        }
	}
}
