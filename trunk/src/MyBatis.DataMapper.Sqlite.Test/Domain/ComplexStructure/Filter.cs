using System;

namespace MyBatis.DataMapper.Sqlite.Test.Domain.ComplexStructure
{
    [Serializable]
    public class Filter
    {
        public Rules Rules { get; set; }
    }
}