using System;
using System.Web.UI.WebControls;
using NPetshop.Domain.Shopping;
using NPetshop.Presentation;
using NPetshop.Web.Controls;

namespace NPetshop.Web.UserControls.Shopping
{
	/// <summary>
	///		Summary description for Checkout.
	/// </summary>
	public class Checkout : NPetshopUC
	{
		protected ExtendedRepeater RepeaterCart;
		protected LinkButton LinkButtonPrev;
		protected LinkButton LinkbuttonNext;
		protected LinkButton LinkbuttonContinueCheckout;
		private ShoppingCart _cart = null; 
		private ShoppingController _shoppingController = null;

		public ShoppingController ShoppingController
		{
			set { _shoppingController = value; }
		}

		//Property to show total
		protected decimal Total
		{
			get { return _cart.Total; }
		}

		private void Page_Load(object sender, EventArgs e)
		{
			_cart = this.NPetshopState.CurrentShoppingCart;
			DataBind();
		}

		public override void DataBind()
		{
			bool empty = ((_cart == null) || _cart.IsEmpty);
			if (! empty)
			{
				RepeaterCart.DataSource = _cart;
				RepeaterCart.DataBind();

				if (_cart.Lines.IsNextPageAvailable)
				{
					LinkbuttonNext.Visible = true;
				}
				else
				{
					LinkbuttonNext.Visible = false;
				}
				if (_cart.Lines.IsPreviousPageAvailable)
				{
					LinkButtonPrev.Visible = true;
				}
				else
				{
					LinkButtonPrev.Visible = false;
				}
			}	
			else
			{
				LinkbuttonNext.Visible = false;
				LinkButtonPrev.Visible = false;
				LinkbuttonContinueCheckout.Visible = false;
			}
		}
		private void LinkbuttonContinueCheckout_Click(object sender, EventArgs e)
		{
			_shoppingController.ContinueCheckout();
		}

		private void LinkButtonPrev_Click(object sender, EventArgs e)
		{
			_cart.Lines.PreviousPage();
//			this.CurrentController.NextView = WebViews.CART;
		}

		private void LinkbuttonNext_Click(object sender, EventArgs e)
		{
			_cart.Lines.NextPage();
//			this.CurrentController.NextView = WebViews.CART;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.LinkButtonPrev.Click += new EventHandler(this.LinkButtonPrev_Click);
			this.LinkbuttonNext.Click += new EventHandler(this.LinkbuttonNext_Click);
			this.LinkbuttonContinueCheckout.Click += new EventHandler(this.LinkbuttonContinueCheckout_Click);
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion



	}
}
