using System;

using Castle.MVC.States;

using IBatisNet.Common.Pagination;
using NPetshop.Domain.Accounts;
using NPetshop.Domain.Shopping;
using NPetshop.Domain.Billing;

namespace NPetshop.Presentation
{
	/// <summary>
	/// Summary description for NPetshopState.
	/// </summary>
	public class NPetshopState: BaseState
	{
		private const string SESSSION_ACCOUNT_KEY = "_SESSSION_ACCOUNT_KEY_";
		private const string CART_SESSION_KEY = "_CART_SESSION_KEY_";
		private const string ORDER_SESSION_KEY = "_ORDER_SESSION_KEY_";
		private const string LIST_SESSION_KEY = "_LIST_SESSION_KEY_";
		private const string OBJECT_SESSION_KEY = "_OBJECT_SESSION_KEY_";

		public Account CurrentUser
		{
			get { return this[SESSSION_ACCOUNT_KEY] as Account; }
			set { this[SESSSION_ACCOUNT_KEY] = value; }
		}

		public ShoppingCart CurrentShoppingCart
		{
			get { return this[CART_SESSION_KEY] as ShoppingCart; }
			set { this[CART_SESSION_KEY] = value; }
		}

		public Order CurrentOrder
		{
			get { return this[ORDER_SESSION_KEY] as Order; }
			set { this[ORDER_SESSION_KEY] = value; }
		}

		public IPaginatedList CurrentList
		{
			get { return this[LIST_SESSION_KEY] as IPaginatedList; }
			set { this[LIST_SESSION_KEY] = value; }
		}

		public object CurrentObject
		{
			get { return this[OBJECT_SESSION_KEY]; }
			set { this[OBJECT_SESSION_KEY] = value; }
		}
	}
}
