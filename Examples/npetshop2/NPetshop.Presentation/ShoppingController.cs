using System;
using NPetshop.Domain.Shopping;
using NPetshop.Service.Interfaces;

namespace NPetshop.Presentation
{
	/// <summary>
	/// Summary description for ShoppingController.
	/// </summary>
	public class ShoppingController : NPetshopController
	{
		private ICatalogService _catalogService = null;

		public ShoppingController(ICatalogService catalogService)
		{
			_catalogService = catalogService;
		}

		public void AddItemToCart(string itemId)
		{
			if (this.NState.CurrentShoppingCart != null)
			{
				this.NState.CurrentShoppingCart.Add(_catalogService.GetItem(itemId));
			}
			else 
			{
				this.NState.Command = "signIn";
			}
			this.Navigate();
		}

		public void RemoveItemFromCart(string itemId)
		{
			if (this.NState.CurrentShoppingCart != null)
			{				
				this.NState.CurrentShoppingCart.RemoveLine(_catalogService.GetItem(itemId));
				this.NState.Command = "showCart";
			}
			else 
			{
				this.NState.Command = "signIn";
			}		
			this.Navigate();
		}

		public void ShowShoppingCart()
		{
			if (this.NState.CurrentShoppingCart != null) 
			{
				this.NState.Command = "showCart";
			}
			else 
			{
				this.NState.Command = "signIn";
			}
			this.Navigate();
		}

		public void UpdateQuantityByItemId(string itemId, int quantity)
		{
			ShoppingCartLine cartLine = this.NState.CurrentShoppingCart.FindLine(_catalogService.GetItem(itemId));
			cartLine.Quantity = quantity;
			this.NState.Save();
		}

		public void ProceedCheckout()
		{
			this.Navigate(); 
		}

		public void ContinueCheckout()
		{
			this.Navigate();
		}
	}
}
