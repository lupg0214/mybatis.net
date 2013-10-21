#region Apache Notice

/*****************************************************************************
 * $Header: $
 * $Revision: 408164 $
 * $Date: 2008-10-19 05:25:12 -0600 (Sun, 19 Oct 2008) $
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

using MyBatis.DataMapper.Model.ParameterMapping;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;

namespace MyBatis.DataMapper.Model.Sql.Dynamic
{
    /// <summary>
    /// Summary description for SqlText.
    /// </summary>
    public sealed class SqlText : ISqlChild
    {
        #region Fields

        private bool _isWhiteSpace = false;
        private IDynamicParent _parent = null;
        private string _text = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public bool IsWhiteSpace
        {
            get
            {
                return _isWhiteSpace;
            }
        }

        /// <summary>
        /// Core parameters
        /// </summary>
        public ParameterProperty[] Parameters { get; set; }

        /// <summary>
        /// Parent tag element
        /// This is used to determine the parent tag node in order that we can ascertain how to determine
        /// which iterate node we may be within if the "Text" property value contains a property name that
        /// is relative to the current iterate item. i.e. "[]."
        /// </summary>
        /// <remarks>
        /// Created By: Richard Beacroft
        /// Created Date: 11\10\2013
        /// </remarks>
        public IDynamicParent Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                _isWhiteSpace = (_text.Trim().Length == 0);
            }
        }

        #endregion Properties
    }
}