namespace NPetshop.Web.UserControls.Billing
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Domain.Billing;

	/// <summary>
	///		Summary description for BillUI.
	/// </summary>
	public class BillUI : NPetshopUC
	{
		protected System.Web.UI.WebControls.Literal LiteralCardNumber;
		protected System.Web.UI.WebControls.Literal LiteralCardExpiration;
		protected System.Web.UI.WebControls.Literal LiteralCardType;
		protected System.Web.UI.WebControls.Literal LiteralOrderDate;
		protected NPetshop.Web.UserControls.Accounts.AddressStatic billingAddress;
		protected System.Web.UI.WebControls.Literal LiteralTotal;
		protected System.Web.UI.WebControls.Repeater RepeaterItems;
		protected NPetshop.Web.UserControls.Accounts.AddressStatic shippingAddress;

		public Order Order
		{
			set
			{
				LiteralCardNumber.Text = value.CreditCard.CardNumber;
				LiteralCardExpiration.Text = value.CreditCard.CardExpiration;
				LiteralCardType.Text = value.CreditCard.CardType;

				LiteralOrderDate.Text = value.OrderDate.ToShortDateString();

				billingAddress.Address = value.BillingAddress;
				shippingAddress.Address = value.ShippingAddress;

				RepeaterItems.DataSource = value.LineItems;
				RepeaterItems.DataBind();
				LiteralTotal.Text = value.TotalPrice.ToString("c");
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
