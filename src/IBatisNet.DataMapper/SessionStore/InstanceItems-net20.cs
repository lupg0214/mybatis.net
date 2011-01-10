using System;
using System.Collections.Generic;

namespace IBatisNet.DataMapper.SessionStore
{
    public class InstanceItems
    {


        public object Find(object key)
        {
            if (Items.ContainsKey(key))
            {
                return Items[key];
            }
            return null;

        }





        public void Set(object key, object value)
        {

            Items[key] = value;

        }





        public void Remove(object key)
        {

            Items.Remove(key);

        }





        private Dictionary<object, object> Items = new Dictionary<object, object>();


        public void CleanUp(object sender, EventArgs e)
        {
            foreach (object item in Items.Values)
            {
                if (item is IDisposable)
                {
                    ((IDisposable)item).Dispose();
                }
            }
        }
    }



}

