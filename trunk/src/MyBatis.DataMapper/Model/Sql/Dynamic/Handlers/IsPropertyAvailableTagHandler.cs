#region Apache Notice

/*****************************************************************************
 * $Revision: 405046 $
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

using MyBatis.Common.Utilities.Objects;
using MyBatis.Common.Utilities.Objects.Members;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Handlers
{
    /// <summary>
    /// Summary description for IsPropertyAvailableTagHandler.
    /// </summary>
    public class IsPropertyAvailableTagHandler : ConditionalTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsPropertyAvailableTagHandler"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public IsPropertyAvailableTagHandler(AccessorFactory accessorFactory)
            : base(accessorFactory)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="tag"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        /// <remarks>
        /// Updated By: Richard Beacroft
        /// Updated Date: 11\10\2013
        /// Description: Builds full property name and checks to see if the property is readable.
        /// Not sure why it doesn't also cater for class Fields?
        /// </remarks>
        public override bool IsCondition(SqlTagContext ctx, SqlTag tag, object parameterObject)
        {
            if (parameterObject == null)
            {
                return false;
            }

            var baseTag = (BaseTag)tag;
            // build property full name from property name - specifically handles references to current iterate item, i.e. "[]."
            var propertyFullName = ctx.ReplaceIterateCurrentProperty(baseTag);

            return ObjectProbe.HasReadableProperty(parameterObject, propertyFullName);
        }
    }
}