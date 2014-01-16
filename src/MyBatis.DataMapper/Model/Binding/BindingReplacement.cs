using System;
using System.Text;

namespace MyBatis.DataMapper.Model.Binding
{
    internal class BindingReplacement
    {
        public string FullPropertyName;
        public string Name;
        public string Placeholder;
        public string Value;

        public void Replace(StringBuilder buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            var replacementValue = Value;

            if (!String.IsNullOrEmpty(FullPropertyName))
            {
                replacementValue = FullPropertyName;
            }

            buffer.Replace(Placeholder, replacementValue);
        }

        public void Replace(ref string buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            var replacementValue = Value;

            if (!String.IsNullOrEmpty(FullPropertyName))
            {
                replacementValue = FullPropertyName;
            }

            buffer = buffer.Replace(Placeholder, replacementValue);
        }
    }
}