using System;

namespace NPetshop.Presentation.UserActions
{
	/// <summary>
	/// <summary>
	/// WebViews class simply references the names of
	/// each control that can appear inside the dynamic
	/// part of the site web (the central content).
	/// Those names are const strings, and will avoid
	/// mistakes in copying names across the numerous files
	/// of the project : every control name is verified by
	/// the compiler.
	/// </summary>
	public class WebViews
	{
		public const string ERROR = "Error";
		public const string HOME = "Home";
		public const string STARTUP = "StartUp";

		public const string PRODUCTS_BY_CATEGORY = "Catalog/ProductList";
		public const string ITEMS_BY_PRODUCT = "Catalog/ItemList";
		public const string ITEM = "Catalog/Item";
		public const string SEARCH_PRODUCTS = "Catalog/SearchProduct";

		public const string SIGNIN = "Accounts/SignIn";
		public const string REGISTER_USER = "Accounts/NewAccount";
		public const string EDIT_ACCOUNT = "Accounts/AccountEdition";
		public const string SIGNOUT = "Accounts/SignOut";

		public const string CART = "Shopping/Cart";
		public const string CHECKOUT = "Shopping/Checkout";

		public const string PAYMENT = "Billing/Payment";
		public const string CONFIRMATION = "Billing/Confirmation";
		public const string SHIPPING = "Billing/Shipping";
		public const string BILLING = "Billing/Bill";

	}
}
