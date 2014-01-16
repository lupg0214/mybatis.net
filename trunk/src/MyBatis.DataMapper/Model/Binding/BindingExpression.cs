using System;
namespace MyBatis.DataMapper.Model.Binding
{
    internal class BindingExpression
    {
        public string FullPropertyName;

        public string Name;

        public string PropertyName;

        public string Value;

        public BindingExpression()
        {
        }

        public BindingExpression(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            Name = name;
        }
    }
}