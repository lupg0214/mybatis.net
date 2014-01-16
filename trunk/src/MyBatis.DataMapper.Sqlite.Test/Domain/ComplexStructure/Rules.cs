using System;
using System.Collections.Generic;

namespace MyBatis.DataMapper.Sqlite.Test.Domain.ComplexStructure
{
    [Serializable]
    public class Rules
    {
        [NonSerialized]
        private string[] _uniqueCustomColumnNames;

        public CustomField[] CustomFields { get; set; }

        public string[] UniqueCustomColumnNames
        {
            get
            {
                if (_uniqueCustomColumnNames == null)
                {
                    if (CustomFields == null)
                        return new string[] { };

                    var unique = new List<string>();

                    foreach (var customField in CustomFields)
                    {
                        if (!unique.Contains(customField.ColumnName))
                            unique.Add(customField.ColumnName);
                    }

                    _uniqueCustomColumnNames = unique.ToArray();
                }

                return _uniqueCustomColumnNames;
            }
            set
            {
                _uniqueCustomColumnNames = value;
            }
        }

        public bool? Unsubscribed { get; set; }
    }
}