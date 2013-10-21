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

using MyBatis.Common.Utilities.Objects.Members;
using MyBatis.DataMapper.Exceptions;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Handlers
{
    /// <summary>
    /// Summary description for IsNullTagHandler.
    /// </summary>
    public class IsNullTagHandler : ConditionalTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsNullTagHandler"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public IsNullTagHandler(AccessorFactory accessorFactory)
            : base(accessorFactory)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="tag"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        public override bool IsCondition(SqlTagContext ctx, SqlTag tag, object parameterObject)
        {
            if (parameterObject == null)
            {
                return true;
            }

            var baseTag = ((BaseTag)tag);
            object value;

            if (!string.IsNullOrEmpty(baseTag.BindName))
            {
                if (!string.IsNullOrEmpty(baseTag.Property))
                    throw new DataMapperException("Error comparing in conditional fragment. Please specify a \"bindName\" or \"property\", not both.");

                value = GetBindValue(ctx, baseTag.BindName, parameterObject);
            }
            else if (!string.IsNullOrEmpty(baseTag.Property))
            {
                // changed to enable references to current iterate items to also be catered for.
                value = GetMemberPropertyValue(ctx, baseTag, parameterObject);
            }
            else
            {
                value = parameterObject;
            }
            return (value == null);
        }
    }
}