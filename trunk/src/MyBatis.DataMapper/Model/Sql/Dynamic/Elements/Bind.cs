#region Apache Notice

/*****************************************************************************
 * $Header: $
 * $Revision: 383115 $
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

using System;
using System.Xml.Serialization;
using MyBatis.Common.Utilities.Objects.Members;
using MyBatis.DataMapper.Model.Sql.Dynamic.Handlers;

#endregion Using

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Elements
{
    /// <summary>
    /// A bind element can be used as a place holder or variable for later use.
    /// <see cref="MyBatis.DataMapper.Model.Sql.Dynamic.Handlers.BindTagHandler"/>
    /// </summary>
    /// <remarks>
    /// Updated By: Richard Beacroft
    /// Updated Date: 11\10\2013
    /// Description: Below is a complex example and good one to illustrate the power of a bind variable.
    /// The Rule.CustomFields array is iterated and for each item in the array, the Comparisons array field is then iterated.
    /// To reference the current CustomField item being iterated within the Comparisons iteration we can bind within the iterate
    /// block of the Rules.CustomFields to the property we of the current item "[]."
    /// Within the nested iterate block we can use the bind variable "~{custom-field}" against the current Comparisons items Contains property for example
    /// </remarks>
    /// <example>
    /// <code>
    ///     <iterate property="Rules.CustomFields" open="" close="" conjunction="">
    ///         <bind name="custom-field" property="[]." />
    ///         <iterate property="[].Comparisons" open="" close="" conjunction="">
    ///             <isNotEmpty property="[].Contains"><![CDATA[AND CP.[$~{custom-field}.ColumnName$] LIKE '%' || #[].Contains# || '%']]></isNotEmpty>
    ///                 ...
    ///             </isNotEmpty>
    ///         </iterate>
    ///     </iterate>
    /// </code>
    /// </example>
    [Serializable]
    [XmlRoot("bind", Namespace = "http://ibatis.apache.org/mapping")]
    public class Bind : BaseTag
    {
        [NonSerialized]
        private string _bindName = string.Empty;

        [NonSerialized]
        private string _bindValue = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsEqual"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public Bind(AccessorFactory accessorFactory)
        {
            this.Handler = new BindTagHandler(accessorFactory);
        }

        /// <summary>
        /// Name attribute
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return _bindName; }
            set { _bindName = value; }
        }

        /// <summary>
        /// Value attribute
        /// </summary>
        [XmlAttribute("value")]
        public string Value
        {
            get { return _bindValue; }
            set { _bindValue = value; }
        }
    }
}