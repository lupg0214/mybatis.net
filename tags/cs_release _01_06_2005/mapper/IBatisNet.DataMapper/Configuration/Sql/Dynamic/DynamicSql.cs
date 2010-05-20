
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
using System.Collections;
using System.Text;

using IBatisNet.Common;
using IBatisNet.DataMapper.Configuration.Sql;
using IBatisNet.DataMapper.Configuration.Sql.SimpleDynamic;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Handlers;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.TypeHandlers;

#endregion

namespace IBatisNet.DataMapper.Configuration.Sql.Dynamic
{
	/// <summary>
	/// DynamicSql represent the root element of a dynamic sql statement
	/// </summary>
	/// <example>
	///      <dynamic prepend="where">...</dynamic>
	/// </example>
	internal class DynamicSql : ISql, IDynamicParent  
	{

		#region Fields

		private IList _children = new ArrayList();
		private IStatement _statement = null ;
		private InlineParameterMapParser _paramParser = null;
		private TypeHandlerFactory _typeHandlerFactory = null;
		private bool _usePositionalParameters = false;

		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="statement">The mapped statement.</param>
		/// <param name="configScope"></param>
		internal DynamicSql(ConfigurationScope configScope, IStatement statement)
		{
			_statement = statement;
			_typeHandlerFactory = configScope.TypeHandlerFactory;
			_usePositionalParameters = configScope.DataSource.Provider.UsePositionalParameters;
		}
		#endregion

		#region Methods

		#region ISql IDynamicParent

		/// <summary>
		/// 
		/// </summary>
		/// <param name="child"></param>
		public void AddChild(ISqlChild child)
		{
			_children.Add(child);
		}

		#endregion

		#region ISql Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterObject"></param>
		/// <param name="session"></param>
		/// <returns></returns>
		public RequestScope GetRequestScope(object parameterObject, IDalSession session)
		{ 
			RequestScope request = new RequestScope();
			_paramParser = new InlineParameterMapParser(request.ErrorContext);
			request.ResultMap = _statement.ResultMap;

			string sqlStatement = Process(request, parameterObject);
			request.PreparedStatement = BuildPreparedStatement(session, request, sqlStatement);
			
			return request;
		}
	
		
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		private string Process(RequestScope request, object parameterObject) 
		{
			SqlTagContext ctx = new SqlTagContext();
			IList localChildren = _children;

			ProcessBodyChildren(request, ctx, parameterObject, localChildren);

			// Builds a 'dynamic' ParameterMap
			ParameterMap map = new ParameterMap(_usePositionalParameters);
			map.Id = _statement.Id + "-InlineParameterMap";

			// Adds 'dynamic' ParameterProperty
			IList parameters = ctx.GetParameterMappings();
			for(int i=0;i<parameters.Count;i++)
			{
				map.AddParameterProperty( (ParameterProperty)parameters[i] );
			}
			request.ParameterMap = map;

			string dynSql = ctx.BodyText;

			// Processes $substitutions$ after DynamicSql
			if ( SimpleDynamicSql.IsSimpleDynamicSql(dynSql) ) 
			{
				dynSql = new SimpleDynamicSql(_typeHandlerFactory, dynSql, _statement).GetSql(parameterObject);
			}
			return dynSql;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="ctx"></param>
		/// <param name="parameterObject"></param>
		/// <param name="localChildren"></param>
		private void ProcessBodyChildren(RequestScope request, SqlTagContext ctx, 
			object parameterObject, IList localChildren) 
		{
			StringBuilder buffer = ctx.GetWriter();
			ProcessBodyChildren(request, ctx, parameterObject, localChildren.GetEnumerator(), buffer);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="ctx"></param>
		/// <param name="parameterObject"></param>
		/// <param name="localChildren"></param>
		/// <param name="buffer"></param>
		private void ProcessBodyChildren(RequestScope request, SqlTagContext ctx, 
			object parameterObject, IEnumerator localChildren, StringBuilder buffer) 
		{
			while (localChildren.MoveNext()) 
			{
				ISqlChild child = (ISqlChild) localChildren.Current;

				if (child is SqlText) 
				{
					SqlText sqlText = (SqlText) child;
					string sqlStatement = sqlText.Text;
					if (sqlText.IsWhiteSpace) 
					{
						buffer.Append(sqlStatement);
					} 
					else 
					{
//						if (SimpleDynamicSql.IsSimpleDynamicSql(sqlStatement)) 
//						{
//							sqlStatement = new SimpleDynamicSql(sqlStatement, _statement).GetSql(parameterObject);
//							SqlText newSqlText = _paramParser.ParseInlineParameterMap( null, sqlStatement );
//							sqlStatement = newSqlText.Text;
//							ParameterProperty[] mappings = newSqlText.Parameters;
//							if (mappings != null) 
//							{
//								for (int i = 0; i < mappings.Length; i++) 
//								{
//									ctx.AddParameterMapping(mappings[i]);
//								}
//							}
//						}
						// BODY OUT
						buffer.Append(" ");
						buffer.Append(sqlStatement);

						ParameterProperty[] parameters = sqlText.Parameters;
						if (parameters != null) 
						{
							for (int i = 0; i< parameters.Length; i++) 
							{
								ctx.AddParameterMapping(parameters[i]);
							}
						}
					}
				} 
				else if (child is SqlTag) 
				{
					SqlTag tag = (SqlTag) child;
					ISqlTagHandler handler = tag.Handler;
					int response = BaseTagHandler.INCLUDE_BODY;

					do 
					{
						StringBuilder body = new StringBuilder();

						response = handler.DoStartFragment(ctx, tag, parameterObject);
						if (response != BaseTagHandler.SKIP_BODY) 
						{
							if (ctx.IsOverridePrepend
								&& ctx.FirstNonDynamicTagWithPrepend == null
								&& tag.IsPrependAvailable
								&& !(tag.Handler is DynamicTagHandler)) 
							{
								ctx.FirstNonDynamicTagWithPrepend = tag;
							}

							ProcessBodyChildren(request, ctx, parameterObject, tag.GetChildrenEnumerator(), body);
            
							response = handler.DoEndFragment(ctx, tag, parameterObject, body);
							handler.DoPrepend(ctx, tag, parameterObject, body);
							if (response != BaseTagHandler.SKIP_BODY) 
							{
								if (body.Length > 0) 
								{
									// BODY OUT

									if (handler.IsPostParseRequired) 
									{
										SqlText sqlText = _paramParser.ParseInlineParameterMap(_typeHandlerFactory, null, body.ToString() );
										buffer.Append(sqlText.Text);
										ParameterProperty[] mappings = sqlText.Parameters;
										if (mappings != null) 
										{
											for (int i = 0; i< mappings.Length; i++) 
											{
												ctx.AddParameterMapping(mappings[i]);
											}
										}
									} 
									else 
									{
										buffer.Append(" ");
										buffer.Append(body.ToString());
									}
									if (tag.IsPrependAvailable && tag == ctx.FirstNonDynamicTagWithPrepend) 
									{
										ctx.IsOverridePrepend = false;
									}
								}
							}
						}
					} 
					while (response == BaseTagHandler.REPEAT_BODY);
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="request"></param>
		/// <param name="sqlStatement"></param>
		/// <returns></returns>
		private PreparedStatement BuildPreparedStatement(IDalSession session, RequestScope request, string sqlStatement)
		{
			PreparedStatementFactory factory = new PreparedStatementFactory( session, request, _statement, sqlStatement);
			return factory.Prepare();
		}
		#endregion

	}
}
