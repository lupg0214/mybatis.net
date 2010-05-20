
using System;

using NPetshop.Domain.Shopping;
using NPetshop.Domain.Catalog;

namespace NPetshop.Domain.Billing
{
	/// <summary>
	/// Business entity used to model an order line item
	/// </summary>
	public class LineItem
	{

		#region Private Fields
		private Order _order = null;
		private Item _item = null;
		private int _lineNumber = 0;
		private int _quantity = 0;
		private decimal _total = 0.0m;
		#endregion

		#region Constructors
		public LineItem() { }

		public LineItem(int lineNumber, ShoppingCartLine cartItem) 
		{
			_lineNumber = lineNumber;
			this.Quantity = cartItem.Quantity;
			this.Item = cartItem.Item;
		}
		#endregion

		#region Properties

		public Order Order
		{
			get{return _order;} 
			set{_order = value;}
		}

		public int LineNumber 
		{
			get{return _lineNumber;} 
			set{_lineNumber = value;}
		}

		public decimal Total 
		{
			get{return _total;} 
		}

		public Item Item
		{
			get{return _item;} 
			set
			{
				_item = value;
				CalculateTotal();
			}
		}


		public int Quantity 
		{
			get{return _quantity;} 
			set
			{
				_quantity = value;
				CalculateTotal();
			}		
		}
		#endregion

		#region Private methods

		private void CalculateTotal() 
		{
			if (_item != null) 
			{
				_total = this.Item.ListPrice * this.Quantity;
			} 
			else 
			{
				_total = 0.0m;
			}
		}
		#endregion

	}
}
