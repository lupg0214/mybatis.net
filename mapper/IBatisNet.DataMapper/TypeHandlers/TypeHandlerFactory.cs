
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

#region Using

using System;
using System.Collections.Specialized;

#endregion 

namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Not much of a suprise, this is a factory class for TypeHandler objects.
	/// </summary>
	internal class TypeHandlerFactory
	{

		#region Fields

		private HybridDictionary _typeHandlerMap = new HybridDictionary();
		private ITypeHandler _unknownTypeHandler = null;
		private const string NULL = "_NULL_TYPE_";

		#endregion 

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public TypeHandlerFactory() 
		{
			ITypeHandler handler = null;

			handler = new BooleanTypeHandler();
			this.Register(typeof(bool), handler); // key= "System.Boolean"

			handler = new ByteTypeHandler();
			this.Register(typeof(Byte), handler);

			handler = new CharTypeHandler();
			this.Register(typeof(Char), handler);

			handler = new DateTimeTypeHandler();
			this.Register(typeof(DateTime), handler);

			handler = new DecimalTypeHandler();
			this.Register(typeof(Decimal), handler);

			handler = new DoubleTypeHandler();
			this.Register(typeof(Double), handler);

			handler = new Int16TypeHandler();
			this.Register(typeof(Int16), handler);

			handler = new Int32TypeHandler();
			this.Register(typeof(Int32), handler);

			handler = new Int64TypeHandler();
			this.Register(typeof(Int64), handler);

			handler = new SingleTypeHandler();
			this.Register(typeof(Single), handler);

			handler = new StringTypeHandler();
			this.Register(typeof(String), handler);

			handler = new GuidTypeHandler();
			this.Register(typeof(Guid), handler);

			handler = new TimeSpanTypeHandler();
			this.Register(typeof(TimeSpan), handler);

			handler = new ByteArrayTypeHandler();
			this.Register(typeof(Byte[]), handler);

			handler = new ObjectTypeHandler();
			this.Register(typeof(object), handler);

			handler = new EnumTypeHandler();
			this.Register( typeof(System.Enum), handler);

			_unknownTypeHandler = new UnknownTypeHandler(this);

		}

		#endregion 

		#region Methods

		/// <summary>
		/// Get a TypeHandler for a Type
		/// </summary>
		/// <param name="type">the Type you want a TypeHandler for</param>
		/// <returns>the handler</returns>
		public ITypeHandler GetTypeHandler(Type type)
		{
			return GetTypeHandler(type, null);
		}

		/// <summary>
		/// Get a TypeHandler for a type
		/// </summary>
		/// <param name="type">the type you want a TypeHandler for</param>
		/// <param name="dbType">the database type</param>
		/// <returns>the handler</returns>
		public ITypeHandler GetTypeHandler(Type type, string dbType) 
		{
			if (type.IsEnum)
			{
				return this.GetPrivateTypeHandler(typeof(System.Enum), dbType);
			}
			else
			{
				return this.GetPrivateTypeHandler(type, dbType);
			}
		}

		/// <summary>
		///  Get a TypeHandler for a type and a dbType type
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="dbType">the dbType type</param>
		/// <returns>the handler</returns>
		private ITypeHandler GetPrivateTypeHandler(Type type, string dbType) 
		{
			HybridDictionary dbTypeHandlerMap = (HybridDictionary) _typeHandlerMap[ type ];
			ITypeHandler handler = null;

			if (dbTypeHandlerMap != null) 
			{
				if (dbType==null)
				{
					handler = (ITypeHandler) dbTypeHandlerMap[ NULL ];
				}
				else
				{
					handler = (ITypeHandler) dbTypeHandlerMap[ dbType ];
					if (handler == null) 
					{
						handler = (ITypeHandler) dbTypeHandlerMap[ NULL ];
					}					
				}

			}
			return handler;
		}


		/// <summary>
		/// Register (add) a type handler for a type
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="handler">the handler instance</param>
		public void Register(Type type, ITypeHandler handler) 
		{
			this.Register(type, null, handler);
		}

		/// <summary>
		/// Register (add) a type handler for a type and dbType
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="dbType">the dbType</param>
		/// <param name="handler">the handler instance</param>
		public void Register(Type type, string dbType, ITypeHandler handler) 
		{
			HybridDictionary map = (HybridDictionary) _typeHandlerMap[ type ];
			if (map == null) 
			{
				map = new HybridDictionary();
				_typeHandlerMap.Add(type, map)  ;
			}
			if (dbType==null)
			{
				map.Add(NULL, handler);
			}
			else
			{
				map.Add(dbType, handler);
			}
		}

		/// <summary>
		/// When in doubt, get the "unknown" type handler
		/// </summary>
		/// <returns>if I told you, it would not be unknown, would it?</returns>
		public ITypeHandler GetUnkownTypeHandler() 
		{
			return _unknownTypeHandler;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool IsSimpleType(Type type) 
		{
			bool result = false;
			if (type != null) 
			{
				ITypeHandler handler = this.GetTypeHandler(type, null);
				if (handler != null) 
				{
					result = handler.IsSimpleType;
				}
			}
			return result;
		}

		#endregion
	}
}
