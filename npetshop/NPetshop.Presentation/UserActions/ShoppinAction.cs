
using System;
using System.Web;

using NPetshop.Domain.Shopping;
using NPetshop.Service;
using NPetshop.Presentation.Core;

namespace NPetshop.Presentation.UserActions
{
	/// <summary>
	/// Summary description for ShoppinAction.
	/// </summary>
	public class ShoppinAction : AbstractWebAction
	{
		private ShoppingCart _cart = null;
		private static readonly CatalogService _catalogService = CatalogService.GetInstance();

		public ShoppinAction(HttpContext context) : base(context) 
		{
			_cart = singleton.CurrentShoppingCart;
		}

		public static ShoppingCart CreateNewShoppingCart()
		{
			return new ShoppingCart();
		}

		public void AddItemToCart(string itemId)
		{
			if (_cart != null)
			{
				_cart.Add(_catalogService.GetItem(itemId));
				this.nextViewToDisplay = WebViews.CART;
			}
			else 
			{
				this.nextViewToDisplay = WebViews.SIGNIN;
			}
		}

		public void RemoveItemFromCart(string itemId)
		{
			if (_cart != null)
			{				
				_cart.RemoveLine(_catalogService.GetItem(itemId));
				this.nextViewToDisplay = WebViews.CART;
			}
			else 
			{
				this.nextViewToDisplay = WebViews.SIGNIN;
			}		
		}

		public void ShowShoppingCart()
		{
			if (_cart != null) 
			{
				this.nextViewToDisplay = WebViews.CART;
			}
			else 
			{
				this.nextViewToDisplay = WebViews.SIGNIN;
			}
		}

		public void UpdateQuantityByItemId(string itemId, int quantity)
		{
			ShoppingCartLine cartLine = _cart.FindLine(_catalogService.GetItem(itemId));
			cartLine.Quantity = quantity;
		}

		public void ProceedCheckout()
		{
			this.nextViewToDisplay = WebViews.CHECKOUT;
		}

		public void ContinueCheckout()
		{
			this.nextViewToDisplay = WebViews.PAYMENT;
		}
	}
}
