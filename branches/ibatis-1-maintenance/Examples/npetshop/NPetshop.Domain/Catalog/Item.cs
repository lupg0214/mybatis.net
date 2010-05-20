using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace NPetshop.Domain.Catalog
{
	/// <summary>
	/// Summary description for Item.
	/// </summary>
	public class Item
	{
		#region Private Fields
		private string _id;
		private decimal _listPrice;
		private decimal _unitCost;
		private string _currency;
		private string _photo;
		private int _quantity;
		private string _attribute1;
		private Product _product;
		private Supplier _supplier;
		#endregion

		#region Properties
		public string Attribute1 
		{
			get{return _attribute1;} 
			set{_attribute1 = value;}
		}

		public int Quantity 
		{
			get{return _quantity;} 
			set{_quantity = value;}
		}

		public string Id 
		{
			get{return _id;} 
			set{_id = value;}
		}

		public decimal ListPrice 
		{
			get{return _listPrice;} 
			set{_listPrice = value;}
		}

		public decimal UnitCost 
		{
			get{return _unitCost;} 
			set{_unitCost = value;}
		}

		public string Currency 
		{
			get{return _currency;} 
			set{_currency = value;}
		}

		public Product Product 
		{
			get{return _product;}
			set{_product = value;}
		}

		public Supplier Supplier 
		{
			get{return _supplier;}
			set{_supplier = value;}
		}

		public string Photo 
		{
			set {_photo = value;}
			get {return _photo;}
		}

		
		#endregion

	}
}
