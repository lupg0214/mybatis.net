using System;
using System.Text;
using MyBatis.Common.Utilities;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;
using MyBatis.DataMapper.Model.Sql.Dynamic.Handlers;

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Parsers
{
    /// <summary>
    /// This is used to parse property names that refer to the current iterate item. i.e. "[]."
    /// This place holder is replaced with the current iterate index building up the full path
    /// by looking at the parent tag nodes.
    /// </summary>
    /// <remarks>
    /// Created By: Richard Beacroft
    /// Created Date: 11\10\2013
    /// </remarks>
    internal class ReflectionMapper
    {
        public const string ENUMERATOR_PLACEHOLDER = "[]";
        public const string THIS_ENUMERATOR_PLACEHOLDER = ENUMERATOR_PLACEHOLDER + ".";

        public static string GetReflectedFullName(SqlTagContext ctx, SqlText sqlText, string propertyName)
        {
            if (ctx == null)
                throw new ArgumentNullException("ctx");
            if (sqlText == null)
                throw new ArgumentNullException("sqlText");
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            if (sqlText.Parent == null)
                return propertyName;

            if (sqlText.Parent is Iterate)
                return BuildReflectedFullName(ctx, (Iterate)sqlText.Parent, propertyName);

            if (sqlText.Parent is BaseTag)
            {
                var parentIteratorTag = FindParentIteratorTag(ctx, (BaseTag)sqlText.Parent);

                // is current node a child of another iterate node?
                if (parentIteratorTag != null)
                    return BuildReflectedFullName(ctx, parentIteratorTag, propertyName);
            }

            return propertyName;
        }

        public static string GetReflectedFullName(SqlTagContext ctx, BaseTag tag, string propertyName)
        {
            if (ctx == null)
                throw new ArgumentNullException("ctx");
            if (tag == null)
                throw new ArgumentNullException("tag");
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            var currentIteratorContext = ctx.GetAttribute(tag) as IterateContext;

            // is current tag an iterate?
            if (currentIteratorContext != null)
            {
                propertyName = String.Format("{0}[{1}]", propertyName, currentIteratorContext.Index);
            }

            var parentIteratorTag = FindParentIteratorTag(ctx, tag);

            // is current node a child of another iterate node?
            if (parentIteratorTag != null)
                return BuildReflectedFullName(ctx, parentIteratorTag, propertyName);

            return propertyName;
        }

        public static bool ReplacePropertyIndexerWithFullName(SqlTagContext ctx, BaseTag tag, StringBuilder bodyContent)
        {
            if (ctx == null)
                throw new ArgumentNullException("ctx");
            if (tag == null)
                throw new ArgumentNullException("tag");
            if (bodyContent == null)
                throw new ArgumentNullException("bodyContent");

            string propertyName = tag.Property;

            if (propertyName == null)
                propertyName = String.Empty;

            if (propertyName.StartsWith(THIS_ENUMERATOR_PLACEHOLDER))
            {
                var builtPropertyName = GetReflectedFullName(ctx, tag, propertyName);
                var suffix = String.Empty;

                // if the property name is just the current item reference, then do not suffix the property name with a "."
                if (propertyName != THIS_ENUMERATOR_PLACEHOLDER)
                    suffix = ".";

                StringHandler.Replace(bodyContent, THIS_ENUMERATOR_PLACEHOLDER, builtPropertyName + suffix);

                return true;
            }

            return false;
        }

        private static string BuildReflectedFullName(SqlTagContext ctx, Iterate parentIteratorTag, string propertyName)
        {
            if (parentIteratorTag != null)
            {
                var parentIteratorContext = ctx.GetAttribute(parentIteratorTag) as IterateContext;
                var indexOfIndexer = propertyName.IndexOf(THIS_ENUMERATOR_PLACEHOLDER);

                if (parentIteratorContext == null)
                    return propertyName;

                if (indexOfIndexer == 0)
                {
                    // the property name is a reflection name relative to the iterate.
                    propertyName = propertyName.Substring(indexOfIndexer + THIS_ENUMERATOR_PLACEHOLDER.Length);
                    propertyName = String.Format("{0}[{1}].{2}", parentIteratorTag.Property, parentIteratorContext.Index, propertyName);

                    var parentOrParentIteratorTag = FindParentIteratorTag(ctx, parentIteratorTag);

                    if (parentOrParentIteratorTag != null)
                        return BuildReflectedFullName(ctx, parentOrParentIteratorTag, propertyName);
                }
                else if (propertyName.IndexOf(ENUMERATOR_PLACEHOLDER) > -1)
                {
                    return propertyName + "[" + parentIteratorContext.Index + "]"; //Parameter-Index-Dynamic
                }
            }

            return propertyName;
        }

        private static Iterate FindParentIteratorTag(SqlTagContext ctx, BaseTag tag)
        {
            if (tag.Parent is Iterate)
                return tag.Parent as Iterate;

            var parentBaseTag = tag.Parent as BaseTag;

            if (tag.Parent == null || parentBaseTag == null)
                return null;

            return FindParentIteratorTag(ctx, parentBaseTag);
        }
    }
}