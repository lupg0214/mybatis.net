namespace NPetshop.Web.UserControls.Billing
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Domain.Billing;
	using NPetshop.Presentation.Core;
	using NPetshop.Presentation.UserActions;

	/// <summary>
	///		Summary description for Order.
	/// </summary>
	public class Payment : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.RequiredFieldValidator valCardNumber;
		protected System.Web.UI.WebControls.DropDownList dropdownlistCardType;
		protected System.Web.UI.WebControls.TextBox textboxCardNumber;
		protected System.Web.UI.WebControls.DropDownList dropdownlistMonth;
		protected System.Web.UI.WebControls.DropDownList dropdownlistYear;
		protected System.Web.UI.WebControls.CheckBox checkboxShipBilling;
		protected System.Web.UI.WebControls.Button ButtonSubmit;
		protected NPetshop.Web.UserControls.Accounts.AddressUI ucBillingAddress;

		public NPetshop.Domain.Billing.Order Order
		{
			get
			{
				NPetshop.Domain.Billing.Order order = new NPetshop.Domain.Billing.Order();

				order.InitOrder(this.WebLocalSingleton.CurrentUser, 
								this.WebLocalSingleton.CurrentShoppingCart,
								ucBillingAddress.Address);
				
				CreditCard creditCard = new CreditCard();
				creditCard.CardExpiration = dropdownlistMonth.SelectedValue +"/"+ dropdownlistYear.SelectedValue;
				creditCard.CardNumber = textboxCardNumber.Text;
				creditCard.CardType = dropdownlistCardType.SelectedValue;
				order.CreditCard = creditCard;

				return order;
			} 
			set
			{

			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		public override void DataBind()
		{
			if (this.WebLocalSingleton.CurrentOrder == null)
			{
				ucBillingAddress.Address = this.WebLocalSingleton.CurrentUser.Address;
			}
			else
			{
				ucBillingAddress.Address = this.WebLocalSingleton.CurrentOrder.BillingAddress;
			}
		}

		private void ButtonSubmit_Click(object sender, System.EventArgs e)
		{
			BillinAction action = new BillinAction(this.Context);
			action.NewOrder(this.Order, checkboxShipBilling.Checked);
			this.CurrentController.NextView = action.NextViewToDisplay;
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
			this.ButtonSubmit.Click += new System.EventHandler(this.ButtonSubmit_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
