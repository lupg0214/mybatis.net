#region Apache Notice

/*****************************************************************************
 * $Header: $
 * $Revision: 408164 $
 * $Date: 2008-06-28 09:26:16 -0600 (Sat, 28 Jun 2008) $
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

#region Using

using MyBatis.Common.Configuration;
using MyBatis.Common.Utilities.Objects.Members;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;

#endregion Using

namespace MyBatis.DataMapper.Configuration.Serializers
{
    /// <summary>
    /// This is the class responsible for de-serializing the "bind" element for usage within class <see cref="MyBatis.DataMapper.Model.Sql.Dynamic.DynamicSql"/>.
    /// The logic associated with binding all happens within the <see cref="MyBatis.DataMapper.Model.Sql.Dynamic.DynamicSql"/>.ProcessBodyChildren method.
    /// For a description on how the "bind" element can be used, please see: <see cref="MyBatis.DataMapper.Model.Sql.Dynamic.Elements.Bind"/>.
    /// </summary>
    /// <remarks>
    /// Created By: Richard Beacroft
    /// Created Date: 11\10\2013
    /// </remarks>
    public sealed class BindDeSerializer : BaseDynamicDeSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindDeSerializer"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public BindDeSerializer(AccessorFactory accessorFactory)
            : base(accessorFactory)
        { }

        #region IDeSerializer Members

        /// <summary>
        /// Deserializes the specified configuration in an <see cref="Bind"/> object
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public override SqlTag Deserialize(IConfiguration configuration)
        {
            Bind bind = new Bind(accessorFactory);

            bind.Prepend = ConfigurationUtils.GetStringAttribute(configuration.Attributes, "prepend");
            bind.Property = ConfigurationUtils.GetStringAttribute(configuration.Attributes, "property");
            bind.Value = ConfigurationUtils.GetStringAttribute(configuration.Attributes, "value");
            bind.Name = ConfigurationUtils.GetStringAttribute(configuration.Attributes, "name");

            return bind;
        }

        #endregion IDeSerializer Members
    }
}