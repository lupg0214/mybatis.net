using System;
using System.Collections.ObjectModel;

namespace MyBatis.DataMapper.SqlClient.Test.Domain
{
    [Serializable]
    public class LineItemCollection2 : Collection<LineItem>
    {
        public virtual new int Count
        {
            get { return base.Count; }
        }

        public virtual new int IndexOf(LineItem item)
        {
            return base.IndexOf(item);
        }
    }
}
