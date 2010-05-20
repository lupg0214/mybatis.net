
using System;
using System.Collections;

namespace IBatisNet.DataMapper.Test.Domain 
{
	public class LineItemCollection : CollectionBase 
	{
		public LineItemCollection() {}

		public LineItem this[int index] 
		{
			get	{ return (LineItem)List[index]; }
			set { List[index] = value; }
		}

		public int Add(LineItem value) 
		{
			return List.Add(value);
		}

		public void AddRange(LineItem[] value) 
		{
			for (int i = 0;	i < value.Length; i++) 
			{
				Add(value[i]);
			}
		}

		public void AddRange(LineItemCollection value) 
		{
			for (int i = 0;	i < value.Count; i++) 
			{
				Add(value[i]);
			}
		}

		public bool Contains(LineItem value) 
		{
			return List.Contains(value);
		}

		public void CopyTo(LineItem[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		public int IndexOf(LineItem value) 
		{
			return List.IndexOf(value);
		}
		
		public void Insert(int index, LineItem value) 
		{
			List.Insert(index, value);
		}
		
		public void Remove(LineItem value) 
		{
			List.Remove(value);
		}
	}
}
