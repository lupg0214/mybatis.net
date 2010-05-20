namespace NPetshop.Web.UserControls.Shopping
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Domain.Shopping;
	using NPetshop.Presentation.Core;
	using NPetshop.Presentation.UserActions;

	/// <summary>
	///		Summary description for Checkout.
	/// </summary>
	public class Checkout : NPetshop.Presentation.UserControl
	{
		protected NPetshop.Presentation.Controls.ExtendedRepeater RepeaterCart;
		protected System.Web.UI.WebControls.LinkButton LinkButtonPrev;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonNext;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonContinueCheckout;
		private ShoppingCart _cart = null; 

		//Property to show total
		protected decimal Total
		{
			get { return _cart.Total; }
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			_cart = this.WebLocalSingleton.CurrentShoppingCart;
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
		private void LinkbuttonContinueCheckout_Click(object sender, System.EventArgs e)
		{
			ShoppinAction action = new ShoppinAction(this.Context);
			action.ContinueCheckout();
			this.CurrentController.NextView = action.NextViewToDisplay;
		}

		private void LinkButtonPrev_Click(object sender, System.EventArgs e)
		{
			_cart.Lines.PreviousPage();
			this.CurrentController.NextView = WebViews.CART;
		}

		private void LinkbuttonNext_Click(object sender, System.EventArgs e)
		{
			_cart.Lines.NextPage();
			this.CurrentController.NextView = WebViews.CART;
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
			this.LinkButtonPrev.Click += new System.EventHandler(this.LinkButtonPrev_Click);
			this.LinkbuttonNext.Click += new System.EventHandler(this.LinkbuttonNext_Click);
			this.LinkbuttonContinueCheckout.Click += new System.EventHandler(this.LinkbuttonContinueCheckout_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion



	}
}
