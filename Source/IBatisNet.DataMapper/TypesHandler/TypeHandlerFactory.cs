
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
using System.Collections.Specialized;

namespace IBatisNet.DataMapper.TypesHandler
{
	/// <summary>
	/// Description résumée de TypeHandlerFactory.
	/// </summary>
	internal class TypeHandlerFactory
	{
		private static readonly HybridDictionary _typeHandlerMap = new HybridDictionary();


		/// <summary>
		/// Constructor
		/// </summary>
		static TypeHandlerFactory() 
		{
			ITypeHandler handler;

			handler = new BooleanTypeHandler();
			_typeHandlerMap.Add(typeof(bool), handler); // key= "System.Boolean"

			handler = new ByteTypeHandler();
			_typeHandlerMap.Add(typeof(Byte), handler);

			handler = new CharTypeHandler();
			_typeHandlerMap.Add(typeof(Char), handler);

			handler = new DateTimeTypeHandler();
			_typeHandlerMap.Add(typeof(DateTime), handler);

			handler = new DecimalTypeHandler();
			_typeHandlerMap.Add(typeof(Decimal), handler);

			handler = new DoubleTypeHandler();
			_typeHandlerMap.Add(typeof(Double), handler);

			handler = new Int16TypeHandler();
			_typeHandlerMap.Add(typeof(Int16), handler);

			handler = new Int32TypeHandler();
			_typeHandlerMap.Add(typeof(Int32), handler);

			handler = new Int64TypeHandler();
			_typeHandlerMap.Add(typeof(Int64), handler);

			handler = new SingleTypeHandler();
			_typeHandlerMap.Add(typeof(Single), handler);

			handler = new StringTypeHandler();
			_typeHandlerMap.Add(typeof(String), handler);

			handler = new GuidTypeHandler();
			_typeHandlerMap.Add(typeof(Guid), handler);

			handler = new TimeSpanTypeHandler();
			_typeHandlerMap.Add(typeof(TimeSpan), handler);

			handler = new ByteArrayTypeHandler();
			_typeHandlerMap.Add(typeof(Byte[]), handler);

			handler = new ObjectTypeHandler();
			_typeHandlerMap.Add(typeof(object), handler);

			handler = new EnumTypeHandler();
			_typeHandlerMap.Add( typeof(System.Enum), handler);

		}


		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static ITypeHandler GetTypeHandler(Type type) 
		{
			if (type.IsEnum)
			{
				return (ITypeHandler) _typeHandlerMap[typeof(System.Enum)];
			}
			else
			{
				return (ITypeHandler) _typeHandlerMap[type];
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public static ITypeHandler GetTypeHandler(Type type, string dbType) 
		{
			return (ITypeHandler) GetTypeHandler(type);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsSimpleType(Type type) 
		{
			bool result = false;
			if (type != null) 
			{
				ITypeHandler handler = TypeHandlerFactory.GetTypeHandler(type);
				if (handler != null) 
				{
					result = handler.IsSimpleType();
				}
			}
			return result;
		}

		#endregion
	}
}
