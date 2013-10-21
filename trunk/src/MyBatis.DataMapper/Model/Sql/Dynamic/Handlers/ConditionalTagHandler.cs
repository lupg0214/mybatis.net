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
using System.Linq;
using System.Text;
using MyBatis.Common.Utilities.Objects;
using MyBatis.Common.Utilities.Objects.Members;
using MyBatis.DataMapper.Exceptions;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;
using MyBatis.DataMapper.Model.Sql.Dynamic.Parsers;

#endregion Imports

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Handlers
{
    /// <summary>
    /// Description résumée de ConditionalTagHandler.
    /// </summary>
    public abstract class ConditionalTagHandler : BaseTagHandler
    {
        private readonly TagPropertyProbe _tagPropertyProbe = null;

        #region Const

        /// <summary>
        ///
        /// </summary>
        public const long NOT_COMPARABLE = long.MinValue;

        #endregion Const

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalTagHandler"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        protected ConditionalTagHandler(AccessorFactory accessorFactory)
            : base(accessorFactory)
        {
            _tagPropertyProbe = new TagPropertyProbe(AccessorFactory);
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
            if (IsCondition(ctx, tag, parameterObject))
            {
                return INCLUDE_BODY;
            }
            return SKIP_BODY;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="tag"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        public abstract bool IsCondition(SqlTagContext ctx, SqlTag tag, object parameterObject);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        protected static long CompareValues(Type type, object value1, object value2)
        {
            long result = NOT_COMPARABLE;

            if (value1 == null || value2 == null)
            {
                result = value1 == value2 ? 0 : NOT_COMPARABLE;
            }
            else
            {
                if (value2.GetType() != type)
                {
                    value2 = ConvertValue(type, value2.ToString());
                }
                if (value2 is string && type != typeof(string))
                {
                    value1 = value1.ToString();
                }
                if (!(value1 is IComparable && value2 is IComparable))
                {
                    value1 = value1.ToString();
                    value2 = value2.ToString();
                }
                result = ((IComparable)value1).CompareTo(value2);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static object ConvertValue(Type type, string value)
        {
            if (type == typeof(String))
            {
                return value;
            }
            if (type == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }
            if (type == typeof(Byte))
            {
                return Convert.ToByte(value);
            }
            if (type == typeof(Char))
            {
                return Convert.ToChar(value.Substring(0, 1));//new Character(value.charAt(0));
            }
            if (type == typeof(DateTime))
            {
                try
                {
                    return Convert.ToDateTime(value);
                }
                catch (Exception e)
                {
                    throw new DataMapperException("Error parsing date. Cause: " + e.Message, e);
                }
            }
            if (type == typeof(Decimal))
            {
                return Convert.ToDecimal(value);
            }
            if (type == typeof(Double))
            {
                return Convert.ToDouble(value);
            }
            if (type == typeof(Int16))
            {
                return Convert.ToInt16(value);
            }
            if (type == typeof(Int32))
            {
                return Convert.ToInt32(value);
            }
            if (type == typeof(Int64))
            {
                return Convert.ToInt64(value);
            }
            if (type == typeof(Single))
            {
                return Convert.ToSingle(value);
            }
            return value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="sqlTag"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        protected long Compare(SqlTagContext ctx, SqlTag sqlTag, object parameterObject)
        {
            Conditional tag = (Conditional)sqlTag;
            string bindName = tag.BindName;
            string propertyName = tag.Property;
            string comparePropertyName = tag.CompareProperty;
            string compareValue = tag.CompareValue;
            object value1 = null;
            Type type = null;

            if (!string.IsNullOrEmpty(bindName))
            {
                if (!string.IsNullOrEmpty(propertyName))
                    throw new DataMapperException("Error comparing in conditional fragment. Please specify a \"bindName\" or \"property\", not both.");
                value1 = GetBindValue(ctx, bindName, parameterObject);
                type = value1.GetType();
            }
            else if (!string.IsNullOrEmpty(propertyName))
            {
                value1 = GetMemberPropertyValue(ctx, tag, parameterObject);
                type = value1.GetType();
            }
            else
            {
                value1 = parameterObject;
                if (value1 != null)
                {
                    type = parameterObject.GetType();
                }
                else
                {
                    type = typeof(object);
                }
            }
            if (!string.IsNullOrEmpty(comparePropertyName))
            {
                object value2 = GetMemberComparePropertyValue(ctx, tag, parameterObject);
                return CompareValues(type, value1, value2);
            }
            if (!string.IsNullOrEmpty(compareValue))
            {
                return CompareValues(type, value1, compareValue);
            }
            throw new DataMapperException("Error comparing in conditional fragment.  Unknown 'compare to' values.");
        }

        protected object GetBindValue(SqlTagContext ctx, string bindName, object parameterObject)
        {
            var bindingExpression = ctx.GetBindExpression(bindName);

            if (bindingExpression == null)
                return null;

            if (String.IsNullOrEmpty(bindingExpression.FullPropertyName))
                return bindingExpression.Value;

            return ObjectProbe.GetMemberValue(parameterObject, bindingExpression.FullPropertyName, AccessorFactory);
        }

        /// <summary>
        /// This class is responsible for getting the current iterate item object within an iteration. i.e. The compare property name starts with "[]."
        /// We do this by navigating up through the parent nodes to determine which of them are iterate elements.
        /// Once found we get the current iteration context item.
        /// If "[]." is not specified, the original approach is used of reflecting the parameterObject to the reflection path specified in the compareProperty
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="baseTag"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created By: Richard Beacroft
        /// Created Date: 11\10\2013
        /// </remarks>
        protected object GetMemberComparePropertyValue(SqlTagContext ctx, BaseTag baseTag, object parameterObject)
        {
            Conditional tag = (Conditional)baseTag;

            if (String.IsNullOrEmpty(tag.CompareProperty))
                return null;

            var bindingReplacement = ctx.BuildComparePropertyBindingReplacements(tag).FirstOrDefault();

            if (bindingReplacement != null)
            {
                if (String.IsNullOrEmpty(bindingReplacement.FullPropertyName))
                    return bindingReplacement.Value;
                return _tagPropertyProbe.GetMemberValue(ctx, baseTag, bindingReplacement.FullPropertyName, parameterObject);
            }

            return _tagPropertyProbe.GetMemberComparePropertyValue(ctx, tag, parameterObject);
        }

        /// <summary>
        /// This class is responsible for getting the current iterate item object within an iteration. i.e. The property name starts with "[]."
        /// We do this by navigating up through the parent nodes to determine which of them are iterate elements.
        /// Once found we get the current iteration context item.
        /// If "[]." is not specified, the original approach is used of reflecting the parameterObject to the reflection path specified in the property
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="baseTag"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created By: Richard Beacroft
        /// Created Date: 11\10\2013
        /// </remarks>
        protected object GetMemberPropertyValue(SqlTagContext ctx, BaseTag baseTag, object parameterObject)
        {
            if (String.IsNullOrEmpty(baseTag.Property))
                return parameterObject;

            var bindingReplacement = ctx.BuildPropertyBindingReplacements(baseTag).FirstOrDefault();

            if (bindingReplacement != null)
            {
                if (String.IsNullOrEmpty(bindingReplacement.FullPropertyName))
                    return bindingReplacement.Value;
                return _tagPropertyProbe.GetMemberValue(ctx, baseTag, bindingReplacement.FullPropertyName, parameterObject);
            }

            return _tagPropertyProbe.GetMemberPropertyValue(ctx, baseTag, parameterObject);
        }

        #endregion Methods
    }
}