
using System;

using NPetshop.Domain.Catalog;

namespace NPetshop.Domain.Shopping
{

	[Serializable]
	public class ShoppingCartLine  
	{
		#region Private Fields
		private Item _item = null;
		private bool _isInStock = false;
		private int _quantity = -1;
		private decimal _total = 0;
		#endregion
		
		#region Constructors
		public ShoppingCartLine(Item item)
		{
			this.Item = item;
			this.Quantity = 1;
		}

		public ShoppingCartLine(Item item, int quantity)
		{
			this.Item = item;
			this.Quantity = quantity;
		}
		#endregion

		#region Properties 
		public Item Item 
		{
			get { return _item; }
			set 
			{ 
				_item = value; 
				CalculateTotal();
			}
		}

		public int Quantity
		{
			get { return _quantity; }
			set 
			{ 
				_quantity = value;
				CalculateTotal();
			}
		}

		public bool IsInStock
		{
			get { return _isInStock; }
			set { _isInStock = value; }
		}

		public decimal Total 
		{
			get { return _total; }
		}

		#endregion

		#region Public methods
		public void IncrementQuantity() 
		{
			_quantity++;
			CalculateTotal();
		}
		#endregion

		#region Private methods
		private void CalculateTotal()
		{
			if (_item != null) 
			{
				_total = _quantity * _item.ListPrice; 
			}
			else 
			{
				_total = 0;
			}
		}
		#endregion

	}
}
