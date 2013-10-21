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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBatis.DataMapper.Model.Binding;
using MyBatis.DataMapper.Model.ParameterMapping;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;
using MyBatis.DataMapper.Model.Sql.Dynamic.Parsers;

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Handlers
{
    /// <summary>
    /// Summary description for SqlTagContext.
    /// </summary>
    public sealed class SqlTagContext
    {
        private readonly Hashtable _attributes = new Hashtable();
        private readonly IList<BindingExpression> _bindings = new List<BindingExpression>();
        private readonly StringBuilder _buffer = new StringBuilder();
        private readonly IList<ParameterProperty> _parameterMappings = new List<ParameterProperty>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlTagContext"/> class.
        /// </summary>
        public SqlTagContext()
        {
            IsOverridePrepend = false;
        }

        /// <summary>
        /// Gets the body text.
        /// </summary>
        /// <value>The body text.</value>
        public string BodyText
        {
            get { return _buffer.ToString().Trim(); }
        }

        /// <summary>
        /// Gets or sets the first non dynamic tag with prepend.
        /// </summary>
        /// <value>The first non dynamic tag with prepend.</value>
        public SqlTag FirstNonDynamicTagWithPrepend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is override prepend.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is override prepend; otherwise, <c>false</c>.
        /// </value>
        public bool IsOverridePrepend { set; get; }

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddAttribute(object key, object value)
        {
            _attributes.Add(key, value);
        }

        public void AddParameterMappings(SqlText sqlText)
        {
            if (sqlText == null)
                throw new ArgumentNullException("sqlText");

            ParameterProperty[] parameters = sqlText.Parameters;

            if (parameters != null)
            {
                int length = parameters.Length;

                for (int i = 0; i < length; i++)
                {
                    var parameter = parameters[i];

                    parameter.ApplyIteratePropertyReferenceHandling(this, sqlText);

                    this.AddParameterMapping(parameter);
                }
            }
        }

        public void CheckAssignFirstDynamicTagWithPrepend(SqlTag tag)
        {
            if (IsOverridePrepend
                && FirstNonDynamicTagWithPrepend == null
                && tag.IsPrependAvailable
                && !(tag.Handler is DynamicTagHandler))
            {
                FirstNonDynamicTagWithPrepend = tag;
            }
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object GetAttribute(object key)
        {
            return _attributes[key];
        }

        /// <summary>
        /// Gets the parameter mappings.
        /// </summary>
        /// <returns></returns>
        public IList<ParameterProperty> GetParameterMappings()
        {
            return _parameterMappings;
        }

        /// <summary>
        /// Gets the writer.
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetWriter()
        {
            return _buffer;
        }

        /// <summary>
        /// </summary>
        /// <param name="tag"></param>
        public void RememberBinding(Bind tag)
        {
            if (tag == null)
                return;

            var bindExpression = GetBindExpression(tag.Name);
            var exists = bindExpression != null;

            if (bindExpression == null)
                bindExpression = new BindingExpression(tag.Name);

            bindExpression.Value = tag.Value;
            bindExpression.PropertyName = tag.Property;
            bindExpression.FullPropertyName = ReflectionMapper.GetReflectedFullName(this, tag, tag.Property);

            if (!exists)
                _bindings.Add(bindExpression);
        }

        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <param name="key"></param>
        public void RemoveAttibute(object key)
        {
            _attributes.Remove(key);
        }

        internal IList<BindingReplacement> BuildComparePropertyBindingReplacements(Conditional tag)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");

            return InlineParameterMapParser.BuildBindingReplacements(_bindings, new StringBuilder(tag.CompareProperty));
        }

        internal IList<BindingReplacement> BuildContentBindingReplacements(StringBuilder bodyContent)
        {
            if (bodyContent == null)
                throw new ArgumentNullException("bodyContent");

            return InlineParameterMapParser.BuildBindingReplacements(_bindings, bodyContent);
        }

        internal IList<BindingReplacement> BuildPropertyBindingReplacements(BaseTag tag)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");

            return InlineParameterMapParser.BuildBindingReplacements(_bindings, new StringBuilder(tag.Property));
        }

        internal IList<BindingReplacement> BuildTextBindingReplacements(SqlText tag)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");

            return InlineParameterMapParser.BuildBindingReplacements(_bindings, new StringBuilder(tag.Text));
        }

        internal BindingExpression GetBindExpression(string bindName)
        {
            if (String.IsNullOrEmpty(bindName))
                throw new ArgumentNullException("bindName");

            var binding = _bindings.SingleOrDefault(w => w.Name == bindName);

            return binding;
        }

        internal void ReplaceBindingVariables(StringBuilder body)
        {
            var replacements = BuildContentBindingReplacements(body);

            ReplaceBindingVariables(replacements, body);
        }

        internal string ReplaceIterateCurrentProperty(BaseTag baseTag)
        {
            return ReflectionMapper.GetReflectedFullName(this, baseTag, baseTag.Property);
        }

        internal string ReplaceIterateCurrentProperty(SqlText sqlText, string tokenValue)
        {
            return ReflectionMapper.GetReflectedFullName(this, sqlText, tokenValue);
        }

        internal string ReplaceIteratePropertiesAndVariables(SqlText sqlText, string sql)
        {
            if (sqlText == null)
                throw new ArgumentNullException("sqlText");
            if (sql == null)
                throw new ArgumentNullException("sql");

            ReplaceParameterIteratePropertiesAndVariables(sqlText);

            var sqlStatementBuilder = new StringBuilder(sql);

            var replacements = BuildTextBindingReplacements(sqlText);

            ReplaceBindingVariables(replacements, sqlStatementBuilder);

            return sqlStatementBuilder.ToString();
        }

        internal void ReplaceParameterIteratePropertiesAndVariables(SqlText sqlText)
        {
            if (sqlText.Parameters != null)
            {
                foreach (var parameter in sqlText.Parameters)
                {
                    var parameterBindingReplacements = InlineParameterMapParser.BuildBindingReplacements(_bindings, new StringBuilder(parameter.PropertyName));

                    foreach (var replacement in parameterBindingReplacements)
                        parameter.ReplaceBindingName(replacement);

                    parameter.ApplyIteratePropertyReferenceHandling(this, sqlText);
                }
            }
        }

        private static void ReplaceBindingVariables(IList<BindingReplacement> replacements, StringBuilder buffer)
        {
            if (replacements == null)
                throw new ArgumentNullException("replacements");
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            foreach (var replacement in replacements)
            {
                var replacementValue = replacement.Value;

                if (!String.IsNullOrEmpty(replacement.FullPropertyName))
                {
                    replacementValue = replacement.FullPropertyName;
                }

                buffer.Replace(replacement.Placeholder, replacementValue);
            }
        }

        /// <summary>
        /// Adds the parameter mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        private void AddParameterMapping(ParameterProperty mapping)
        {
            _parameterMappings.Add(mapping.Clone());
        }
    }
}