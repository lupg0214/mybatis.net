
using System;
using System.Web;

using NPetshop.Domain.Accounts;
using NPetshop.Domain.Billing;
using NPetshop.Presentation.Core;

using NPetshop.Service;


namespace NPetshop.Presentation.UserActions
{
	/// <summary>
	/// Summary description for BillinAction.
	/// </summary>
	public class BillinAction : AbstractWebAction
	{

		private static readonly BillingService _billingService = BillingService.GetInstance();

		public BillinAction(HttpContext context) : base(context) 
		{
		}

		
		public void NewOrder(Order order, bool shipToBillinAddress)
		{
			if (shipToBillinAddress)
			{
				order.ShippingAddress = order.BillingAddress;
				singleton.CurrentOrder = order;
				this.nextViewToDisplay = WebViews.CONFIRMATION;
			}
			else
			{
				this.nextViewToDisplay = WebViews.SHIPPING;
			}
		}

		public void SubmitShippingAddress(Address address)
		{
			singleton.CurrentOrder.ShippingAddress = address;
			this.nextViewToDisplay = WebViews.CONFIRMATION;
		}

		public void ConfirmOrder(Order order)
		{
			// save the order
			_billingService.InsertOrder(order);
			singleton.CurrentShoppingCart = ShoppinAction.CreateNewShoppingCart();
			this.nextViewToDisplay = WebViews.BILLING;
		}
	}
}
