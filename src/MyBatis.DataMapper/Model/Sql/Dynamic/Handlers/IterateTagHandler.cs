#region Apache Notice

/*****************************************************************************
 * $Revision: 408164 $
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

#region using

using System;
using System.Text;
using MyBatis.Common.Utilities;
using MyBatis.Common.Utilities.Objects.Members;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;
using MyBatis.DataMapper.Model.Sql.Dynamic.Parsers;

#endregion using

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Handlers
{
    /// <summary>
    /// Summary description for IterateTagHandler.
    /// </summary>
    public sealed class IterateTagHandler : BaseTagHandler
    {
        private readonly TagPropertyProbe _tagPropertyProbe = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="IterateTagHandler"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public IterateTagHandler(AccessorFactory accessorFactory)
            : base(accessorFactory)
        {
            _tagPropertyProbe = new TagPropertyProbe(AccessorFactory);
        }

        /// <summary>
        ///
        /// </summary>
        public override bool IsPostParseRequired
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="tag"></param>
        /// <param name="parameterObject"></param>
        /// <param name="bodyContent"></param>
        /// <returns></returns>
        /// <remarks>
        /// Updated By: Richard Beacroft
        /// Updated Date: 11\10\2013
        /// Description: Enables property iterate item paths to be replaced with full property paths, which will later be reflected.
        /// </remarks>
        public override int DoEndFragment(SqlTagContext ctx, SqlTag tag,
            object parameterObject, StringBuilder bodyContent)
        {
            IterateContext iterate = (IterateContext)ctx.GetAttribute(tag);

            if (iterate.IsCompleted)
                return INCLUDE_BODY;

            var iterateTag = ((Iterate)tag);
            var propertyName = iterateTag.Property;

            if (propertyName == null)
                propertyName = String.Empty;

            // build full reflection path
            if (!ReflectionMapper.ReplacePropertyIndexerWithFullName(ctx, iterateTag, bodyContent))
            {
                string find = propertyName + ReflectionMapper.ENUMERATOR_PLACEHOLDER;
                string replace = propertyName + "[" + iterate.Index + "]"; //Parameter-index-Dynamic

                StringHandler.Replace(bodyContent, find, replace);
            }

            if (iterate.IsFirst)
            {
                if (iterateTag.Open != null)
                {
                    bodyContent.Insert(0, ctx.ReplaceBindingVariables(iterateTag.Open));
                    bodyContent.Insert(0, ' ');
                }
            }

            if (iterate.IsLast)
            {
                if (iterateTag.Close != null)
                {
                    bodyContent.Append(ctx.ReplaceBindingVariables(iterateTag.Close));
                }
            }
            else
            {
                if (iterateTag.Conjunction != null)
                {
                    bodyContent.Append(ctx.ReplaceBindingVariables(iterateTag.Conjunction));
                    bodyContent.Append(' ');
                }
            }

            if (iterate.HasNext)
                return REPEAT_BODY;

            iterate.IsCompleted = true;

            return INCLUDE_BODY;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="tag"></param>
        /// <param name="parameterObject"></param>
        /// <param name="bodyContent"></param>
        /// <remarks>
        /// Updated By: Richard Beacroft
        /// Updated Date: 11\10\2013
        /// Description: Changed the index to check for.
        /// The "iterate" tag handler has been re-worked to ensure we read the next iterate context item
        /// before the child tag elements are processed. We need to do this to ensure that we can parse 
        /// property expressions within tag elements nested within the iterate element.
        /// </remarks>
        public override void DoPrepend(SqlTagContext ctx, SqlTag tag, object parameterObject, StringBuilder bodyContent)
        {
            var iterate = (IterateContext)ctx.GetAttribute(tag);

            // because we move to the first item before we iterate, we need to check to see if this is the first item, by checking the Index = 1 and not 0.
            if (iterate.IsFirst)
            {
                base.DoPrepend(ctx, tag, parameterObject, bodyContent);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="tag"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        /// Updated By: Richard Beacroft
        /// Updated Date: 11\10\2013
        /// Description: Enables one to be able to have sql text within an iterate element that references the current item as "[]."
        /// This is then parsed, along with any reflection path suffix to get the object instance the used is interested in.
        /// and add it to an attributes collection for later use.
        public override int DoStartFragment(SqlTagContext ctx, SqlTag tag, object parameterObject)
        {
            var iterate = (IterateContext)ctx.GetAttribute(tag);

            if (iterate == null)
            {
                var baseTag = (BaseTag)tag;

                object collection;

                if (!string.IsNullOrEmpty(baseTag.Property))
                {
                    // this will either leave the property name as it is, or if it starts with "[].", the 
                    // current iterate context item value will be used, with the rest of the property name
                    // walked to determine the field/property to get the value from.
                    collection = _tagPropertyProbe.GetMemberPropertyValue(ctx, baseTag, parameterObject);
                }
                else
                {
                    collection = parameterObject;
                }

                iterate = new IterateContext(collection);

                ctx.AddAttribute(tag, iterate);

                // if there is another item in the iterate array, then we need to include the body.
                if (iterate.MoveNext())
                    return INCLUDE_BODY;

                iterate.IsCompleted = true;

                return SKIP_BODY;
            }

            if (iterate.IsCompleted)
            {
                // reset the context to cater for nested iterations.
                ctx.RemoveAttibute(tag);
                // now re-process the tag which will re-add the now modified tag element back to the attributes collection.
                return DoStartFragment(ctx, tag, parameterObject);
            }
            else if (iterate.HasNext || iterate.IsLast)
            {
                if (iterate.MoveNext())
                    return INCLUDE_BODY;
            }

            return SKIP_BODY;
        }
    }
}