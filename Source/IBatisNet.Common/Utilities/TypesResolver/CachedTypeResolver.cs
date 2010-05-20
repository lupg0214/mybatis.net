
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: $
 * $Date: $
 * 
 * Copyright 2004 the original author or authors.
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

#region Remarks
// Code from Spring.NET
#endregion

#region Imports

using System;
using System.Collections;
using System.Collections.Specialized;

#endregion

namespace IBatisNet.Common.Utilities.TypesResolver
{
	/// <summary>
	/// Summary description for CachedTypeResolver.
	/// </summary>
	public class CachedTypeResolver: TypeResolver
	{
		#region Constructor (s) / Destructor
		/// <summary>
		/// Creates a new instance of the CachedTypeResolver class.
		/// </summary>
		public CachedTypeResolver () {}
		#endregion

		#region Properties
		/// <summary>
		/// The cache, mapping type names against a <see cref="System.Type"/>
		/// instance.
		/// </summary>
		private IDictionary TypeCache 
		{
			get 
			{
				return typeCache;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Resolves the supplied type name into a <see cref="System.Type"/>
		/// instance.
		/// </summary>
		/// <param name="typeName">
		/// The (possibly partially assembly qualified) name of a <see cref="System.Type"/>.
		/// </param>
		/// <returns>
		/// A resolved <see cref="System.Type"/> instance.
		/// </returns>
		/// <exception cref="System.TypeLoadException">
		/// If the type could not be resolved.
		/// </exception>
		public override Type Resolve (string typeName)
		{
			Type type = TypeCache [typeName] as Type;
			if (type == null)
			{
				type = base.Resolve (typeName);
				TypeCache [typeName] = type;
			}
			return type;
		}
		#endregion

		#region Fields
		private IDictionary typeCache = new HybridDictionary ();
		#endregion
	}
}
