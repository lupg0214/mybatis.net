
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
	using System.Reflection;
	using System.Reflection.Emit;

	/// <summary>
	/// Generates a Java style proxy. This overrides the .Net proxy requirements 
	/// that forces one to extend MarshalByRefObject or (for a different purpose)
	/// ContextBoundObject to have a Proxiable class.
	/// </summary>
	/// <remarks>
	/// The <see cref="ProxyGenerator"/> should be used to generate a class 
	/// implementing the specified interfaces. The class implementation will 
	/// only call the internal <see cref="IInvocationHandler"/> instance.
	/// </remarks>
	/// <remarks>
	/// This proxy implementation currently doesn't not supports ref and out arguments 
	/// in methods.
	/// </remarks>
	/// <example>
	/// <code>
	/// MyInvocationHandler handler = ...
	/// IInterfaceExposed proxy = 
	///		ProxyGenerator.CreateProxy( new Type[] { typeof(IInterfaceExposed) }, handler );
	/// </code>
	/// </example>
	public class ProxyGenerator
	{
		/// <summary>
		/// Private construtor
		/// </summary>
		private ProxyGenerator()
		{
		}

		/// <summary>
		/// Generates a proxy implementing all the specified interfaces and
		/// redirecting method invocations to the specifed handler.
		/// </summary>
		/// <param name="theInterface">Interface to be implemented</param>
		/// <param name="handler">instance of <see cref="IInvocationHandler"/></param>
		/// <returns>Proxy instance</returns>
		public static object CreateProxy(Type theInterface, IInvocationHandler handler)
		{
			return CreateProxy(new Type[] { theInterface }, handler );
		}

		/// <summary>
		/// Generates a proxy implementing all the specified interfaces and
		/// redirecting method invocations to the specifed handler.
		/// </summary>
		/// <param name="interfaces">Array of interfaces to be implemented</param>
		/// <param name="handler">instance of <see cref="IInvocationHandler"/></param>
		/// <returns>Proxy instance</returns>
		public static object CreateProxy(Type[] interfaces, IInvocationHandler handler)
		{
			if (interfaces == null)
			{
				throw new ArgumentNullException("interfaces");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (interfaces.Length == 0)
			{
				throw new ArgumentException("Can't handle an empty interface array");
			}

			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = "DynamicAssemblyProxyGen";

			AssemblyBuilder assemblyBuilder = 
				AppDomain.CurrentDomain.DefineDynamicAssembly(
				assemblyName, 
				AssemblyBuilderAccess.Run);
			
			ModuleBuilder moduleBuilder = 
				assemblyBuilder.DefineDynamicModule( assemblyName.Name, true );

			TypeBuilder typeBuilder = moduleBuilder.DefineType( 
				"ProxyType", TypeAttributes.Public|TypeAttributes.Class, null, interfaces);

			FieldBuilder handlerField = GenerateField( typeBuilder );
			ConstructorBuilder constr = GenerateConstructor( typeBuilder, handlerField );

			GenerateInterfaceImplementation( typeBuilder, interfaces, handlerField );

			Type generatedType = typeBuilder.CreateType();

			return Activator.CreateInstance( generatedType, new object[] { handler } );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeBuilder"></param>
		/// <param name="interfaces"></param>
		/// <param name="handlerField"></param>
		private static void GenerateInterfaceImplementation( TypeBuilder typeBuilder, 
			Type[] interfaces, FieldBuilder handlerField )
		{
			foreach(Type inter in interfaces)
			{
				GenerateInterfaceImplementation( typeBuilder, inter, handlerField );
			}
		}

		/// <summary>
		/// Iterates over the interfaces and generate implementation 
		/// for each method in it.
		/// </summary>
		/// <param name="typeBuilder"><see cref="TypeBuilder"/> being constructed.</param>
		/// <param name="inter">Interface type</param>
		/// <param name="handlerField"></param>
		private static void GenerateInterfaceImplementation( TypeBuilder typeBuilder, 
			Type inter, FieldBuilder handlerField )
		{
			if (!inter.IsInterface)
			{
				throw new ArgumentException("Type array expects interfaces only.");
			}

			Type[] baseInterfaces = inter.FindInterfaces( new TypeFilter( NoFilterImpl ), inter );

			GenerateInterfaceImplementation( typeBuilder, baseInterfaces, handlerField );

			PropertyInfo[] properties = inter.GetProperties();
			PropertyBuilder[] propertiesBuilder = new PropertyBuilder[properties.Length];

			for(int i=0; i < properties.Length; i++)
			{
				GeneratePropertyImplementation( typeBuilder, properties[i], ref propertiesBuilder[i] );
			}

			MethodInfo[] methods = inter.GetMethods();

			foreach(MethodInfo method in methods)
			{
				GenerateMethodImplementation( typeBuilder, method, 
					propertiesBuilder, handlerField, inter );
			}
		}

		/// <summary>
		/// Generates a public field holding the <see cref="IInvocationHandler"/>
		/// </summary>
		/// <param name="typeBuilder"><see cref="TypeBuilder"/> being constructed.</param>
		/// <returns><see cref="FieldBuilder"/> instance</returns>
		private static FieldBuilder GenerateField( TypeBuilder typeBuilder )
		{
			return typeBuilder.DefineField( "handler", 
				typeof(IInvocationHandler), FieldAttributes.Public );
		}

		/// <summary>
		/// Generates one public constructor receiving 
		/// the <see cref="IInvocationHandler"/> instance.
		/// </summary>
		/// <param name="typeBuilder"><see cref="TypeBuilder"/> being constructed.</param>
		/// <param name="handlerField"><see cref="FieldBuilder"/> instance representing the handler field</param>
		/// <returns><see cref="ConstructorBuilder"/> instance</returns>
		private static ConstructorBuilder GenerateConstructor( 
			TypeBuilder typeBuilder, FieldBuilder handlerField )
		{
			ConstructorBuilder consBuilder = typeBuilder.DefineConstructor(
				MethodAttributes.Public, 
				CallingConventions.Standard, 
				new Type[] { typeof(IInvocationHandler) } );
			
			ILGenerator ilGenerator = consBuilder.GetILGenerator();
			ilGenerator.Emit(OpCodes.Ldarg_0);
			ilGenerator.Emit(OpCodes.Call, typeof(Object).GetConstructor(new Type[0]));
			ilGenerator.Emit(OpCodes.Ldarg_0);
			ilGenerator.Emit(OpCodes.Ldarg_1);
			ilGenerator.Emit(OpCodes.Stfld, handlerField);
			ilGenerator.Emit(OpCodes.Ret);

			return consBuilder;
		}

		/// <summary>
		/// Generate property implementation
		/// </summary>
		/// <param name="typeBuilder"><see cref="TypeBuilder"/> being constructed.</param>
		/// <param name="property"></param>
		/// <param name="propertyBuilder"></param>
		private static void GeneratePropertyImplementation( 
			TypeBuilder typeBuilder, PropertyInfo property, ref PropertyBuilder propertyBuilder )
		{
			propertyBuilder = typeBuilder.DefineProperty( 
				property.Name, property.Attributes, property.PropertyType, null);
		}

		/// <summary>
		/// Generates implementation for each method.
		/// </summary>
		/// <param name="typeBuilder"><see cref="TypeBuilder"/> being constructed.</param>
		/// <param name="method"></param>
		/// <param name="properties"></param>
		/// <param name="handlerField"></param>
		/// <param name="inter"></param>
		private static void GenerateMethodImplementation( 
			TypeBuilder typeBuilder, MethodInfo method, 
			PropertyBuilder[] properties, FieldBuilder handlerField, Type inter )
		{
			ParameterInfo[] parameterInfo = method.GetParameters();

			System.Type[] parameters = new System.Type[parameterInfo.Length];
			
			for (int i=0; i<parameterInfo.Length; i++)
			{
				parameters[i] = parameterInfo[i].ParameterType;
			}

			MethodAttributes atts = MethodAttributes.Public|MethodAttributes.Virtual;

			if ( method.Name.StartsWith("set_") || method.Name.StartsWith("get_") )
			{
				atts = MethodAttributes.Public|MethodAttributes.SpecialName|MethodAttributes.Virtual;
			}

			MethodBuilder methodBuilder = 
				typeBuilder.DefineMethod( method.Name, atts, CallingConventions.Standard, 
				method.ReturnType, parameters );

			if ( method.Name.StartsWith("set_") || method.Name.StartsWith("get_") )
			{
				foreach( PropertyBuilder property in properties )
				{
					if (property == null)
					{
						break;
					}

					if (!property.Name.Equals( method.Name.Substring(4) ))
					{
						continue;
					}

					if ( methodBuilder.Name.StartsWith("set_") )
					{
						property.SetSetMethod( methodBuilder );
						break;
					}
					else 
					{
						property.SetGetMethod( methodBuilder );
						break;
					}
				}
			}

			WriteILForMethod( method, methodBuilder, parameters, handlerField );
		}

		/// <summary>
		/// Writes the stack for the method implementation. This 
		/// method generates the IL stack for property get/set method and
		/// ordinary methods.
		/// </summary>
		/// <remarks>
		/// The method implementation would be as simple as:
		/// <code>
		/// public void SomeMethod( int parameter )
		/// {
		///     MethodBase method = MethodBase.GetCurrentMethod();
		///     handler.Invoke( this, method, new object[] { parameter } );
		/// }
		/// </code>
		/// </remarks>
		/// <param name="parameters"></param>
		/// <param name="handlerField"></param>
		/// <param name="builder"></param>
		/// <param name="method"></param>
		private static void WriteILForMethod( MethodInfo method, MethodBuilder builder, 
			System.Type[] parameters, FieldBuilder handlerField )
		{
			int arrayPositionInStack = 1;

			ILGenerator ilGenerator = builder.GetILGenerator();

			ilGenerator.DeclareLocal( typeof( MethodBase ) );

			if (builder.ReturnType != typeof(void))
			{
				ilGenerator.DeclareLocal(builder.ReturnType);
				arrayPositionInStack = 2;
			}

			ilGenerator.DeclareLocal( typeof(object[]) );

			ilGenerator.Emit(OpCodes.Ldtoken, method);
			ilGenerator.Emit(OpCodes.Call, typeof(MethodBase).GetMethod("GetMethodFromHandle"));

			ilGenerator.Emit(OpCodes.Stloc_0);
			ilGenerator.Emit(OpCodes.Ldarg_0);
			ilGenerator.Emit(OpCodes.Ldfld, handlerField);
			ilGenerator.Emit(OpCodes.Ldarg_0);
			ilGenerator.Emit(OpCodes.Ldloc_0);
			ilGenerator.Emit(OpCodes.Ldc_I4, parameters.Length);
			ilGenerator.Emit(OpCodes.Newarr, typeof(object) );

			if (parameters.Length != 0)
			{
				ilGenerator.Emit(OpCodes.Stloc, arrayPositionInStack);
				ilGenerator.Emit(OpCodes.Ldloc, arrayPositionInStack);
			}

			for (int c=0; c<parameters.Length; c++)
			{
				ilGenerator.Emit(OpCodes.Ldc_I4, c);
				ilGenerator.Emit(OpCodes.Ldarg, c+1);

				if (parameters[c].IsValueType)
				{
					ilGenerator.Emit(OpCodes.Box, parameters[c].UnderlyingSystemType);
				}

				ilGenerator.Emit(OpCodes.Stelem_Ref);
				ilGenerator.Emit(OpCodes.Ldloc, arrayPositionInStack);
			}

			ilGenerator.Emit(OpCodes.Callvirt, typeof(IInvocationHandler).GetMethod("Invoke") );

			if (builder.ReturnType != typeof(void))
			{
				if (!builder.ReturnType.IsValueType)
				{
					ilGenerator.Emit(OpCodes.Castclass, builder.ReturnType);
				}
				else
				{
					ilGenerator.Emit(OpCodes.Unbox, builder.ReturnType);
					ilGenerator.Emit(ConvertTypeToOpCode(builder.ReturnType));
				}

				ilGenerator.Emit(OpCodes.Stloc, 1);

				Label label = ilGenerator.DefineLabel();
				ilGenerator.Emit(OpCodes.Br_S, label);
				ilGenerator.MarkLabel(label);
				ilGenerator.Emit(OpCodes.Ldloc, 1);
			}
			else
			{
				ilGenerator.Emit(OpCodes.Pop);
			}

			ilGenerator.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// Converts a Value type to a correspondent OpCode of 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private static OpCode ConvertTypeToOpCode( Type type )
		{
			if (type.IsEnum)
			{
				System.Enum baseType = (System.Enum) Activator.CreateInstance( type );
				TypeCode code = baseType.GetTypeCode();
				
				switch(code)
				{
					case TypeCode.Byte:
						type = typeof(Byte);
						break;
					case TypeCode.Int16:
						type = typeof(Int16);
						break;
					case TypeCode.Int32:
						type = typeof(Int32);
						break;
					case TypeCode.Int64:
						type = typeof(Int64);
						break;
				}

				return ConvertTypeToOpCode( type );
			}

			if ( type.Equals( typeof(Int32) ) )
			{
				return OpCodes.Ldind_I4;
			}
			else if ( type.Equals( typeof(Int16) ) )
			{
				return OpCodes.Ldind_I2;
			}
			else if ( type.Equals( typeof(Int64) ) )
			{
				return OpCodes.Ldind_I8;
			}
			else if ( type.Equals( typeof(Single) ) )
			{
				return OpCodes.Ldind_R4;
			}
			else if ( type.Equals( typeof(Double) ) )
			{
				return OpCodes.Ldind_R8;
			}
			else if ( type.Equals( typeof(UInt16) ) )
			{
				return OpCodes.Ldind_U2;
			}
			else if ( type.Equals( typeof(UInt32) ) )
			{
				return OpCodes.Ldind_U4;
			}
			else if ( type.Equals( typeof(Boolean) ) )
			{
				return OpCodes.Ldind_I4;
			}
			else
			{
				throw new ArgumentException("Type " + type + " could not be converted to a OpCode");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="criteria"></param>
		/// <returns></returns>
		public static bool NoFilterImpl( Type type, object criteria )
		{
			return true;
		}
	}
}
