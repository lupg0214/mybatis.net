
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

using System;

namespace IBatisNet.Common.Logging.Impl
{
	/// <summary>
	/// Summary description for NoOpLogger.
	/// </summary>
	public sealed class NoOpLogger: ILog
	{
		/// <summary>
		/// A NOP implementation.
		/// </summary>
		public NoOpLogger()
		{ }

		#region Members of ILog

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Debug(object message)
		{
			// NOP
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Debug(object message, Exception e)
		{
			// NOP
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Error(object message)
		{
			// NOP
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Error(object message, Exception e)
		{
			// NOP
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Fatal(object message)
		{
			// NOP
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Fatal(object message, Exception e)
		{
			// NOP
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Info(object message)
		{
			// NOP
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Info(object message, Exception e)
		{
			// NOP
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Trace(object message)
		{
			// NOP
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Trace(object message, Exception e)
		{
			// NOP
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Warn(object message)
		{
			// NOP
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Warn(object message, Exception e)
		{
			// NOP
		}


		/// <summary>
		/// 
		/// </summary>
		public bool IsDebugEnabled
		{
			get { return false; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsErrorEnabled
		{
			get { return false; }

		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFatalEnabled
		{
			get { return false; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsInfoEnabled
		{
			get { return false; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsTraceEnabled
		{
			get { return false; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsWarnEnabled
		{
			get { return false; }
		}

		#endregion
	}
}

