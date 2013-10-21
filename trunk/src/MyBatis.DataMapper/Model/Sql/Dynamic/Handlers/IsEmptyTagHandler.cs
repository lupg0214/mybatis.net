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

#region Imports

using System;
using System.Collections;
using MyBatis.Common.Utilities.Objects.Members;
using MyBatis.DataMapper.Exceptions;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;

#endregion Imports

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Handlers
{
    /// <summary>
    /// IsEmptyTagHandler represent a isEmpty tag element in a dynamic mapped statement.
    /// </summary>
    public class IsEmptyTagHandler : ConditionalTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsEmptyTagHandler"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public IsEmptyTagHandler(AccessorFactory accessorFactory)
            : base(accessorFactory)
        {
        }

        #region Methods

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
            string bindName = baseTag.BindName;
            string propertyName = baseTag.Property;
            object value = null;

            if (!string.IsNullOrEmpty(bindName))
            {
                if (!string.IsNullOrEmpty(propertyName))
                    throw new DataMapperException("Error comparing in conditional fragment. Please specify a \"bindName\" or \"property\", not both.");

                value = GetBindValue(ctx, bindName, parameterObject);
            }
            else if (!string.IsNullOrEmpty(propertyName))
            {
                value = GetMemberPropertyValue(ctx, baseTag, parameterObject);

                //value = ObjectProbe.GetMemberValue(parameterObject, propertyName, AccessorFactory);
            }
            else
            {
                value = parameterObject;
            }
            if (value is ICollection)
            {
                return ((value == null) || (((ICollection)value).Count < 1));
            }
            if (value != null && typeof(Array).IsAssignableFrom(value.GetType())) //value.GetType().IsArray
            {
                return ((Array)value).GetLength(0) == 0;
            }

            return ((value == null) || (Convert.ToString(value).Equals("")));
        }

        #endregion Methods
    }
}