using System;
using NPetshop.Domain.Shopping;
using NPetshop.Service.Interfaces;

using NPetshop.Domain.Accounts;
using NPetshop.Domain.Billing;

namespace NPetshop.Presentation
{
	/// <summary>
	/// Summary description for BillingController.
	/// </summary>
	public class BillingController : NPetshopController
	{
		private IBillingService _billingService = null;

		public BillingController(IBillingService billingService)
		{
			_billingService = billingService;
		}

		public void NewOrder(Order order, bool shipToBillinAddress)
		{
			this.NState.CurrentOrder = order;
			if (shipToBillinAddress)
			{
				order.ShippingAddress = order.BillingAddress;
				this.NState.Command = "confirm";
			}
			else
			{
				this.NState.Command = "ship";
			}
			this.Navigate();
		}

		public void SubmitShippingAddress(Address address)
		{
			this.NState.CurrentOrder.ShippingAddress = address;
			this.Navigate();
		}

		public void CancelShippingAddress()
		{
			this.Navigate();
		}

		public void ConfirmOrder(Order order)
		{
			// save the order
			_billingService.InsertOrder(order);
			this.NState.CurrentShoppingCart = new ShoppingCart();
			this.Navigate();
		}
	}
}
