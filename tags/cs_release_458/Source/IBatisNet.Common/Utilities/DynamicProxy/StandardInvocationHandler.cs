
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

#region remark
	// Code from Apache Avalon.Net project
#endregion

namespace IBatisNet.Common.Utilities.DynamicProxy
{
	using System;

	/// <summary>
	/// Provides a standard implementation of <see cref="IInvocationHandler"/>.
	/// Methods PreInvoke, PostInvoke can be overrided to customize its behavior.
	/// </summary>
	public class StandardInvocationHandler : IInvocationHandler
	{
		private object m_target;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		public StandardInvocationHandler(object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			m_target = target;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="proxy"></param>
		/// <param name="method"></param>
		/// <param name="arguments"></param>
		protected virtual void PreInvoke(object proxy, System.Reflection.MethodInfo method, params object[] arguments)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="proxy"></param>
		/// <param name="method"></param>
		/// <param name="returnValue"></param>
		/// <param name="arguments"></param>
		protected virtual void PostInvoke(object proxy, System.Reflection.MethodInfo method, ref object returnValue, params object[] arguments)
		{
		}

		#region IInvocationHandler Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="proxy"></param>
		/// <param name="method"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public virtual object Invoke(object proxy, System.Reflection.MethodInfo method, params object[] arguments)
		{
			PreInvoke(proxy, method, arguments);

			object returnValue = method.Invoke( Target, arguments );

			PostInvoke(proxy, method, ref returnValue, arguments);
			
			return returnValue;
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public object Target
		{
			get
			{
				return m_target;
			}
		}
	}
}
