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
using System.Text;
using MyBatis.Common.Utilities.Objects.Members;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;
using MyBatis.Common.Utilities.Objects;

#endregion Imports

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Handlers
{
    /// <summary>
    /// Updated By: Richard Beacroft
    /// Updated Date: 11\10\2013
    /// This is the handler associated with the Bind element.
    /// It doesn't do anything and is required due to the implementation approach used.
    /// The logic associated with binding all happens within the <see cref="MyBatis.DataMapper.Model.Sql.Dynamic.DynamicSql"/>.ProcessBodyChildren method.
    /// For a description on how the "bind" element can be used, please see: <see cref="MyBatis.DataMapper.Model.Sql.Dynamic.Elements.Bind"/>.
    /// Deserialization of the "bind" element takes place by class: <see cref="MyBatis.DataMapper.Configuration.Serializers.BindDeSerializer"/>.
    /// </summary>
    /// <remarks>
    public class BindTagHandler : BaseTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalTagHandler"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public BindTagHandler(AccessorFactory accessorFactory)
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
        /// <param name="bodyContent"></param>
        /// <returns></returns>
        public override int DoEndFragment(SqlTagContext ctx, SqlTag tag, Object parameterObject, StringBuilder bodyContent)
        {
            return INCLUDE_BODY;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="tag"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        public override int DoStartFragment(SqlTagContext ctx, SqlTag tag, Object parameterObject)
        {
            return INCLUDE_BODY;
        }

        #endregion Methods
    }
}