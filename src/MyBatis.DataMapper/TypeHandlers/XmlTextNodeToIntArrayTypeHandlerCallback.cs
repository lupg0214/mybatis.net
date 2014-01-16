using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MyBatis.DataMapper.TypeHandlers
{
    /// <summary>
    /// Summary description for XmlNodeToIntArrayTypeHandlerCallback.
    /// </summary>
    public class XmlTextNodeToIntArrayTypeHandlerCallback : ITypeHandlerCallback
    {
        public object NullValue
        {
            get { return null; }
        }

        /// <summary>
        /// SqlServer 2005 upwards supports XML data types. 
        /// You could have xml text node containing data you wish converted to an int array for simpler data mapping
        /// within your mapper file.
        /// </summary>
        /// <param name="getter"></param>
        /// <returns></returns>
        public object GetResult(IResultGetter getter)
        {
            if (getter.Value == null)
                return null;

            var xmlString = getter.Value.ToString();

            if (String.IsNullOrEmpty(xmlString) || xmlString.Trim().Length == 0)
                return null;

            var integerArray = new List<int>();

            using (var reader = new XmlTextReader(new StringReader(xmlString)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Text)
                        continue;

                    if (String.IsNullOrEmpty(reader.Value))
                        continue;

                    integerArray.Add(reader.ReadContentAsInt());
                }
            }

            return integerArray.ToArray();
        }

        public void SetParameter(IParameterSetter setter, object parameter)
        {
            if (parameter == null)
                setter.Value = null;
            setter.Value = parameter.ToString();
        }

        public object ValueOf(string s)
        {
            throw new NotImplementedException("s");
        }
    }
}