
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

using IBatisNet.Common;
#endregion

namespace IBatisNet.DataMapper.SessionContainer
{
	/// <summary>
	/// Definition for session container.
	/// </summary>
	public interface ISessionContainer
	{
		#region Doc ThreadStatic
		//http://www.joecheng.com/blog/entries/Thread-localstorageinJava.html
		//http://www.hanselman.com/blog/PermaLink.aspx?guid=320
		//http://www.stud.ntnu.no/~wathne/files/JavaDotNetThreading.pdf
		//http://weblogs.asp.net/yreynhout/posts/4061.aspx
		//http://discuss.develop.com/archives/wa.exe?A2=ind0107C&L=DOTNET&P=R6756
		/*
		 * Don't slap a [ThreadStatic] attribute on a static member when you're operating 
		 * within ASP.NET as chances are you don't control the thread life...you inherit a worker thread. 
		 * ThreadStatic gives you thread local storage, not HttpContext local storage! 
		 * If you need to store something away to be used later in the same HTTP request, 
		 * think about my favorite ASP.NET class, the little-known and not-used-enough 
		 * System.Web.HttpContext.Current.Items (aside: great article by Susan Warren on Context). 
		 * In ASP.NET your code is run on a WorkerThread from the 25 or so threads 
		 * in the default ASP.NET worker thread pool and the variable that you think 
		 * is "personal private to your thread" is personal private...to you and every 
		 * other request that this worker thread has been with. Under load you may 
		 * well find your variable modified.[Scott Hanselman]

		 * 
		 */ 
		#endregion

		#region Properties
		/// <summary>
		/// Get the local session
		/// </summary>
		SqlMapSession LocalSession
		{
			get; 
		}
		#endregion

		#region Methods
		/// <summary>
		/// Store the local session on the container.
		/// Ensure that the session is unique for each thread.
		/// </summary>
		/// <param name="session">The session to store</param>
		void Store(SqlMapSession session);

		/// <summary>
		/// Remove the local session from the container.
		/// </summary>
		void Dispose();
		#endregion

	}
}
