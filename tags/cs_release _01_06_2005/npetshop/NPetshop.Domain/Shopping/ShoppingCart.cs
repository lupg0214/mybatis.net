
using System;
using System.Collections;

using IBatisNet.Common.Pagination;
using NPetshop.Domain.Catalog;

namespace NPetshop.Domain.Shopping
{

	[Serializable]
	public class ShoppingCart : IEnumerable	
	{
		#region Private Fields
		private IPaginatedList _cartLines = new PaginatedArrayList(4);
		#endregion

		#region Properties

		public IPaginatedList Lines 
		{
			get{ return _cartLines; }
		}

		public bool IsEmpty
		{
			get{ return _cartLines.Count == 0; }
		}

		public decimal Total
		{
			get
			{
				decimal total = 0;
				IEnumerator lines = GetAllLines();
				while (lines.MoveNext())
				{
					ShoppingCartLine line = (ShoppingCartLine) lines.Current;
					total += line.Total;
				}
				return total;
			}
		}
		#endregion

		#region Pulbic methods
		public IEnumerator GetEnumerator()
		{
			return _cartLines.GetEnumerator();
		}

		public IEnumerator GetAllLines() 
		{
			ArrayList allItems = new ArrayList();
			int index = _cartLines.PageIndex;
			_cartLines.GotoPage(0);

			foreach(ShoppingCartLine line in _cartLines)
			{
				allItems.Add(line);
			}
			while (_cartLines.NextPage()) 
			{
				foreach(ShoppingCartLine line in _cartLines)
				{
					allItems.Add(line);
				}
			}
			
			_cartLines.GotoPage(index);
			return allItems.GetEnumerator();
		}

		public void Add(ShoppingCartLine newLine){
			ShoppingCartLine existingLine = FindLine(newLine);
			if (existingLine != null) 
			{
				existingLine.Quantity += newLine.Quantity;
			}
			else 
			{
				_cartLines.Add(newLine);
			}
		}

		public void Add(Item item){
			ShoppingCartLine existingLine = FindLine(item);
			if (existingLine != null) 
			{
				existingLine.Quantity += 1;
			}
			else 
			{
				_cartLines.Add(new ShoppingCartLine(item));
			}
		}
		
		public void RemoveLine(ShoppingCartLine otherLine){
			RemoveLine(otherLine.Item);
		}

		public void RemoveLine(Item item){
			foreach (ShoppingCartLine line in _cartLines)
			{
				if (line.Item.Product.Name  == item.Product.Name)
				{
					_cartLines.Remove(line);
					break;
				}
			}
		}

		public ShoppingCartLine FindLine(ShoppingCartLine otherLine){
			return FindLine(otherLine.Item);
		}

		public ShoppingCartLine FindLine(Item item){
			foreach (ShoppingCartLine line in _cartLines){
				if (line.Item.Product.Name == item.Product.Name){
					return line;
				}
			}
			return null;
		}
		#endregion
	}
}