using System.Xml.Serialization;

namespace MyBatis.DataMapper.Sqlite.Test.Domain.ComplexStructure
{
    public class CustomField
    {
        [XmlAttribute]
        public string ColumnName { get; set; }

        public Compare[] Comparisons { get; set; }

        [XmlAttribute]
        public ColumnDataTypeEnum DataType { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
    }
}