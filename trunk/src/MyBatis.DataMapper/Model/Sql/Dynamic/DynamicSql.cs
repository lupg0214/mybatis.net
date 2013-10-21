#region Apache Notice

/*****************************************************************************
 * $Revision: 476843 $
 * $LastChangedDate: 2008-10-19 05:25:12 -0600 (Sun, 19 Oct 2008) $
 * $LastChangedBy: gbayon $
 *
 * iBATIS.NET Data Mapper
 * Copyright (C) 2008/2005 - The Apache Software Foundation
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

#endregion Apache Notice

#region Imports

using System.Collections;
using System.Collections.Generic;
using System.Text;
using MyBatis.Common.Contracts;
using MyBatis.Common.Contracts.Constraints;
using MyBatis.DataMapper.Data;
using MyBatis.DataMapper.DataExchange;
using MyBatis.DataMapper.MappedStatements;
using MyBatis.DataMapper.Model.ParameterMapping;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;
using MyBatis.DataMapper.Model.Sql.Dynamic.Handlers;
using MyBatis.DataMapper.Model.Sql.Dynamic.Parsers;
using MyBatis.DataMapper.Model.Sql.SimpleDynamic;
using MyBatis.DataMapper.Model.Statements;
using MyBatis.DataMapper.Scope;
using MyBatis.DataMapper.Session;

#endregion Imports

namespace MyBatis.DataMapper.Model.Sql.Dynamic
{
    /// <summary>
    /// DynamicSql represent the root element of a dynamic sql statement
    /// </summary>
    /// <example>
    ///      <dynamic prepend="where">...</dynamic>
    /// </example>
    public sealed class DynamicSql : ISql, IDynamicParent
    {
        private const string COMMA_TOKEN = ",";
        private const string MARK_TOKEN = "?";

        #region Fields

        private readonly IList<ISqlChild> children = new List<ISqlChild>();
        private readonly DataExchangeFactory dataExchangeFactory = null;
        private readonly DBHelperParameterCache dbHelperParameterCache = null;
        private readonly InlineParameterMapParser paramParser = null;
        private readonly IStatement statement = null;
        private readonly bool usePositionalParameters = false;

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSql"/> class.
        /// </summary>
        /// <param name="usePositionalParameters">if set to <c>true</c> [use positional parameters].</param>
        /// <param name="dbHelperParameterCache">The db helper parameter cache.</param>
        /// <param name="dataExchangeFactory">The data exchange factory.</param>
        /// <param name="statement">The statement.</param>
        public DynamicSql(
            bool usePositionalParameters,
            DBHelperParameterCache dbHelperParameterCache,
            DataExchangeFactory dataExchangeFactory,
            IStatement statement)
        {
            Contract.Require.That(dataExchangeFactory, Is.Not.Null).When("retrieving argument dataExchangeFactory in DynamicSql constructor");
            Contract.Require.That(dbHelperParameterCache, Is.Not.Null).When("retrieving argument dbHelperParameterCache in DynamicSql constructor");
            Contract.Require.That(statement, Is.Not.Null).When("retrieving argument statement in DynamicSql constructor");

            this.statement = statement;
            this.usePositionalParameters = usePositionalParameters;
            this.dataExchangeFactory = dataExchangeFactory;
            this.dbHelperParameterCache = dbHelperParameterCache;
            paramParser = new InlineParameterMapParser();
        }

        #region Methods

        #region ISql IDynamicParent

        /// <summary>
        ///
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(ISqlChild child)
        {
            children.Add(child);
        }

        #endregion ISql IDynamicParent

        #region ISql Members

        /// <summary>
        /// Builds a new <see cref="RequestScope"/> and the <see cref="IDbCommand"/> text to execute.
        /// </summary>
        /// <param name="parameterObject">The parameter object (used in DynamicSql)</param>
        /// <param name="session">The current session</param>
        /// <param name="mappedStatement">The <see cref="IMappedStatement"/>.</param>
        /// <returns>A new <see cref="RequestScope"/>.</returns>
        public RequestScope GetRequestScope(
            IMappedStatement mappedStatement,
            object parameterObject,
            ISession session)
        {
            RequestScope request = new RequestScope(dataExchangeFactory, session, statement);

            string sql = Process(request, parameterObject);
            request.PreparedStatement = BuildPreparedStatement(session, request, sql);
            request.MappedStatement = mappedStatement;

            return request;
        }

        #endregion ISql Members

        /// <summary>
        /// Builds the prepared statement.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="request">The request.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <returns></returns>
        private PreparedStatement BuildPreparedStatement(ISession session, RequestScope request, string sqlStatement)
        {
            PreparedStatementFactory factory = new PreparedStatementFactory(session, dbHelperParameterCache, request, statement, sqlStatement);

            return factory.Prepare(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        private string Process(RequestScope request, object parameterObject)
        {
            SqlTagContext ctx = new SqlTagContext();
            IList<ISqlChild> localChildren = children;

            ProcessBodyChildren(request, ctx, parameterObject, localChildren);

            // Builds a 'dynamic' ParameterMap
            ParameterMap parameterMap = new ParameterMap(
                statement.Id + "-InlineParameterMap",
                statement.ParameterClass.FullName,
                string.Empty,
                statement.ParameterClass,
                dataExchangeFactory.GetDataExchangeForClass(null),
                usePositionalParameters);

            // Adds 'dynamic' ParameterProperty
            var parameters = ctx.GetParameterMappings();

            parameterMap.AddParameterProperties(parameters);

            request.ParameterMap = parameterMap;

            string dynSql = ctx.BodyText;

            if (statement is Procedure)
            {
                dynSql = dynSql.Replace(MARK_TOKEN, string.Empty).Replace(COMMA_TOKEN, string.Empty).Trim();
            }

            // Processes $substitutions$ after DynamicSql
            if (SimpleDynamicSql.IsSimpleDynamicSql(dynSql))
            {
                dynSql = new SimpleDynamicSql(
                    dataExchangeFactory,
                    dbHelperParameterCache,
                    dynSql,
                    statement).GetSql(parameterObject);
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
            object parameterObject, IList<ISqlChild> localChildren)
        {
            StringBuilder buffer = ctx.GetWriter();
            ProcessBodyChildren(request, ctx, parameterObject, localChildren.GetEnumerator(), buffer);
        }

        /// <summary>
        /// Processes the body children.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="ctx">The CTX.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="childEnumerator">The child enumerator.</param>
        /// <param name="buffer">The buffer.</param>
        private void ProcessBodyChildren(
            RequestScope request,
            SqlTagContext ctx,
            object parameterObject,
            IEnumerator<ISqlChild> childEnumerator,
            StringBuilder buffer)
        {
            while (childEnumerator.MoveNext())
            {
                ISqlChild child = childEnumerator.Current;

                if (child is SqlText)
                {
                    // this represents the content within a mybatis xml tag, typically the hand-crafted sql itself
                    var sqlText = (SqlText)child;

                    if (sqlText.IsWhiteSpace)
                    {
                        buffer.Append(sqlText.Text);
                    }
                    else
                    {
                        // BODY OUT

                        var textPropertyProbe = new TextPropertyProbe(sqlText.Text);
                        // process the sql text content, replacing bindings and property usage.
                        var sqlStatment = textPropertyProbe.Process(new DynamicSqlTextTokenHandler(ctx, sqlText));

                        buffer.Append(" ");
                        buffer.Append(sqlStatment);

                        ctx.AddParameterMappings(sqlText);
                    }
                }
                else if (child is SqlTag)
                {
                    SqlTag tag = (SqlTag)child;
                    ISqlTagHandler handler = tag.Handler;
                    int response = BaseTagHandler.INCLUDE_BODY;

                    do
                    {
                        var body = new StringBuilder();

                        // if the tag is a bind element, add the bind element to a list so that we can later on work out how to replace variable usage.
                        if (tag is Bind)
                            ctx.RememberBinding((Bind)tag);

                        response = handler.DoStartFragment(ctx, tag, parameterObject);

                        // replace any bind variables found within the body content.
                        // what's a bit confusing here is that the body element is essentially populated by actions that take place after this line
                        // a recursive call could then be made which results in this being called again.
                        ctx.ReplaceBindingVariables(body);

                        if (response == BaseTagHandler.SKIP_BODY)
                            break;

                        ctx.CheckAssignFirstDynamicTagWithPrepend(tag);

                        ProcessBodyChildren(request, ctx, parameterObject, tag.GetChildrenEnumerator(), body);

                        response = handler.DoEndFragment(ctx, tag, parameterObject, body);

                        handler.DoPrepend(ctx, tag, parameterObject, body);

                        if (response == BaseTagHandler.SKIP_BODY)
                            break;

                        if (body.Length > 0)
                        {
                            // BODY OUT

                            if (handler.IsPostParseRequired)
                            {
                                var sqlText = InlineParameterMapParser.ParseInlineParameterMap(dataExchangeFactory, statement.Id, null, body.ToString());

                                sqlText.Parent = tag;

                                buffer.Append(sqlText.Text);

                                ctx.AddParameterMappings(sqlText);
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
                    while (response == BaseTagHandler.REPEAT_BODY);
                }
            }
        }

        #endregion Methods
    }
}