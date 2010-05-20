
using System;

using NPetshop.Domain.Shopping;

namespace NPetshop.Service
{
	/// <summary>
	/// Summary description for ShoppingService.
	/// </summary>
	public class ShoppingService
	{
		#region Private Fields 
		private static ShoppingService _instance = new ShoppingService();
		#endregion

		#region Public methods

		public static ShoppingService GetInstance() 
		{
			return _instance;
		}

		public ShoppingCart CreateNewShoppingCart()
		{
			return new ShoppingCart();
		}
		#endregion
	}
}
