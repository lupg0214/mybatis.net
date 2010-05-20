
using System;
using System.Collections;

using NPetshop.Domain.Accounts;
using NPetshop.Domain.Shopping;

namespace NPetshop.Domain.Billing
{

	/// <summary>
	/// Business entity used to model an order
	/// </summary>
	[Serializable]
	public class Order 
	{

		#region Private Fields
		private int _id;
		private Account _account;
		private DateTime _orderDate;
		private CreditCard _creditCard = new CreditCard();
		private Address _billingAddress = new Address();
		private Address _shippingAddress = new Address();
		private decimal _totalPrice;
		private IList _lineItems = new ArrayList();
		#endregion

		#region Properties 


		public int Id 
		{
			get{return _id;} 
			set{_id = value;}
		}

		public Account Account 
		{
			get{return _account;} 
			set{_account = value;}
		}


		public DateTime OrderDate 
		{
			get{return _orderDate;} 
			set{_orderDate = value;}
		}

		public CreditCard CreditCard 
		{
			get{return _creditCard;} 
			set{_creditCard = value;}
		}


		public Address BillingAddress 
		{
			get{return _billingAddress;} 
			set{_billingAddress = value;}
		}

		public Address ShippingAddress 
		{
			get{return _shippingAddress;} 
			set{_shippingAddress = value;}
		}

		public decimal TotalPrice 
		{
			get{return _totalPrice;} 
			set{_totalPrice = value;}
		}

		public IList LineItems 
		{
			get{return _lineItems;} 
			set{_lineItems = value;}
		}
		#endregion

		public Order(Account account, ShoppingCart cart, Address address) 
		{
			_account = account;
			_orderDate = DateTime.Now;

			_shippingAddress = address;
			_billingAddress = address;

			_totalPrice = cart.Total;

			IEnumerator enumerator = cart.GetAllLines();
			while( enumerator.MoveNext() )
			{
				ShoppingCartLine cartLine = (ShoppingCartLine) enumerator.Current;
				AddLineItem( cartLine );
			}
		}		
		
		#region Public Methods

		public void AddLineItem(ShoppingCartLine cartItem) 
		{
			LineItem lineItem = new LineItem(_lineItems.Count + 1, cartItem);
			AddLineItem(lineItem);
		}

		public void AddLineItem(LineItem lineItem) 
		{
			_lineItems.Add(lineItem);
		}
		#endregion
	}
}