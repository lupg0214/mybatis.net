
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
using System.Reflection;

using IBatisNet.Common.Exceptions;
#endregion

namespace IBatisNet.Common.Utilities.TypesResolver
{
	/// <summary>
	/// Resolves a <see cref="System.Type"/> by name.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The rationale behind the creation of this class is to centralise the
	/// resolution of type names to <see cref="System.Type"/> instances beyond that
	/// offered by the plain vanilla <see cref="System.Type.GetType"/> method call.
	/// </p>
	/// </remarks>
	/// <version>$Id: TypeResolver.cs,v 1.5 2004/09/28 07:51:47 springboy Exp $</version>
	public class TypeResolver
	{
		#region Constructor (s) / Destructor
		/// <summary>
		/// Creates a new instance of the TypeResolver class.
		/// </summary>
		public TypeResolver () {}
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
		public virtual Type Resolve (string typeName)
		{

			#region Sanity Check
			if (typeName ==  null || typeName.Trim().Length==0)
			{
				throw new ConfigurationException (
					"Could not load type with a null or zero length parameter.");
			}
			#endregion

			Type type = null;
			string canonicalTypeName = TypeAliasResolver.Resolve (typeName);
			TypeAssemblyInfo typeInfo = new TypeAssemblyInfo (canonicalTypeName);
			if (typeInfo.IsAssemblyQualified)
			{
				// assembly qualified... load the assembly, then the Type
				Assembly assembly = Assembly.LoadWithPartialName (typeInfo.AssemblyName);
				if (assembly != null)
				{
					type = assembly.GetType (typeInfo.TypeName, true, true);
				}
			} 
			else
			{
				// bare type name... loop thru all loaded assemblies
				Assembly [] assemblies = AppDomain.CurrentDomain.GetAssemblies ();
				foreach (Assembly assembly in assemblies)
				{
					type = assembly.GetType (typeInfo.TypeName, false, false);
					if (type != null)
					{
						break;
					}
				}
			}
			if (type == null) 
			{
				throw new TypeLoadException (
					"Could not load type : " + typeName);
			}
			return type;
		}
		#endregion

		#region Inner Class : TypeAssemblyInfo
		/// <summary>
		/// Holds data about a <see cref="System.Type"/> and it's
		/// attendant <see cref="System.Reflection.Assembly"/>.
		/// </summary>
		internal class TypeAssemblyInfo
		{
			#region Constants
			/// <summary>
			/// The string that separates <see cref="System.Type"/> names
			/// from their attendant <see cref="System.Reflection.Assembly"/>
			/// names in an assembly qualified type name.
			/// </summary>
			public const string TypeAssemblySeparator = ",";
			#endregion

			#region Fields
			private string unresolvedAssemblyName;
			private string unresolvedTypeName;
			#endregion

			#region Properties
			/// <summary>
			/// The (unresolved) type name portion of the original type name.
			/// </summary>
			public string TypeName
			{
				get
				{
					return unresolvedTypeName;
				}
			}

			/// <summary>
			/// The (unresolved, possibly partial) name of the attandant assembly.
			/// </summary>
			public string AssemblyName
			{
				get
				{
					return unresolvedAssemblyName;
				}
			}

			/// <summary>
			/// Is the type name being resolved assembly qualified?
			/// </summary>
			public bool IsAssemblyQualified
			{
				get
				{
					if (AssemblyName ==  null || AssemblyName.Trim().Length==0)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}

			/// <summary>
			/// The (possibly assembly qualified) <see cref="System.Type"/> name.
			/// </summary>
			public string OriginalTypeName
			{
				get
				{
					System.Text.StringBuilder buffer
						= new System.Text.StringBuilder (TypeName);
					if (IsAssemblyQualified) 
					{
						buffer.Append (TypeAssemblySeparator);
						buffer.Append (AssemblyName);
					}
					return buffer.ToString ();
				}
			}
			#endregion

			#region Constructor (s) / Destructor
			/// <summary>
			/// Creates a new instance of the TypeAssemblyInfo class.
			/// </summary>
			/// <param name="unresolvedTypeName">
			/// The unresolved name of a <see cref="System.Type"/>.
			/// </param>
			public TypeAssemblyInfo (string unresolvedTypeName)
			{
				SplitTypeAndAssemblyNames (unresolvedTypeName);
			}
			#endregion


			#region Methods
			private void SplitTypeAndAssemblyNames (string originalTypeName) 
			{
				int typeAssemblyIndex
					= originalTypeName.IndexOf (
					TypeAssemblyInfo.TypeAssemblySeparator);
				if (typeAssemblyIndex < 0)
				{
					unresolvedTypeName = originalTypeName;
				} 
				else
				{
					unresolvedTypeName = originalTypeName.Substring (
						0, typeAssemblyIndex).Trim ();
					unresolvedAssemblyName = originalTypeName.Substring (
						typeAssemblyIndex + 1).Trim ();
				}
			}
			#endregion

		}
		#endregion
	}
}
