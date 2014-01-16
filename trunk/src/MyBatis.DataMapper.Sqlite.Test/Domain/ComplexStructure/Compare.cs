using System.Xml.Serialization;

namespace MyBatis.DataMapper.Sqlite.Test.Domain.ComplexStructure
{
    public class Compare
    {
        [XmlAttribute]
        public Operator CompareOperator { get; set; }

        public string Contains { get; set; }

        public string Equal { get; set; }

        /// <summary>
        /// Anniversary check specifically for date field types
        /// </summary>
        public DateCompare EqualDayMonthYear { get; set; }

        public string GreaterThan { get; set; }

        public string[] In { get; set; }

        public bool IsAnniversary { get; set; }

        [XmlAttribute]
        public bool IsEmpty { get; set; }

        public string LessThan { get; set; }

        public string NotContains { get; set; }

        public string NotEqual { get; set; }

        public string[] NotIn { get; set; }
    }
}