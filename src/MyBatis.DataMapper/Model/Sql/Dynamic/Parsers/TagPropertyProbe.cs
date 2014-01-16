using System;
using MyBatis.Common.Utilities.Objects;
using MyBatis.Common.Utilities.Objects.Members;
using MyBatis.DataMapper.Model.Sql.Dynamic.Elements;
using MyBatis.DataMapper.Model.Sql.Dynamic.Handlers;

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Parsers
{
    /// <summary>
    /// This class is responsible for getting the current iterate item object within an iteration. i.e. The property name starts with "[]."
    /// We do this by navigating up through the parent nodes to determine which of them are iterate elements.
    /// Once found we get the current iteration context item.
    /// </summary>
    /// <remarks>
    /// Created By: Richard Beacroft
    /// Created Date: 11\10\2013
    /// </remarks>
    internal class TagPropertyProbe
    {
        private readonly AccessorFactory _accessorFactory;

        public TagPropertyProbe(AccessorFactory accessorFactory)
        {
            if (accessorFactory == null)
                throw new ArgumentNullException("accessorFactory");
            _accessorFactory = accessorFactory;
        }

        public object GetMemberComparePropertyValue(SqlTagContext ctx, Conditional tag, object parameterObject)
        {
            if (ctx == null)
                throw new ArgumentNullException("ctx");
            if (tag == null)
                throw new ArgumentNullException("tag");

            return GetMemberValue(ctx, tag, tag.CompareProperty, parameterObject);
        }

        public object GetMemberPropertyValue(SqlTagContext ctx, BaseTag tag, object parameterObject)
        {
            if (ctx == null)
                throw new ArgumentNullException("ctx");
            if (tag == null)
                throw new ArgumentNullException("tag");

            return GetMemberValue(ctx, tag, tag.Property, parameterObject);
        }

        public object GetMemberValue(SqlTagContext ctx, BaseTag tag, string propertyName, object parameterObject)
        {
            var iteratorContext = FindParentIteratorContext(ctx, tag);

            if (iteratorContext != null)
            {
                var indexOfIndexer = propertyName.IndexOf(ReflectionMapper.THIS_ENUMERATOR_PLACEHOLDER);

                if (indexOfIndexer == 0)
                {
                    parameterObject = iteratorContext.Current;
                    propertyName = propertyName.Substring(indexOfIndexer + ReflectionMapper.THIS_ENUMERATOR_PLACEHOLDER.Length);
                }
            }

            return ObjectProbe.GetMemberValue(parameterObject, propertyName, _accessorFactory);
        }

        private IterateContext FindParentIteratorContext(SqlTagContext ctx, BaseTag tag)
        {
            if (tag.Parent is Iterate)
                return ctx.GetAttribute(tag.Parent) as IterateContext;

            var parentBaseTag = tag.Parent as BaseTag;

            if (tag.Parent == null || parentBaseTag == null)
                return null;

            return FindParentIteratorContext(ctx, parentBaseTag);
        }
    }
}