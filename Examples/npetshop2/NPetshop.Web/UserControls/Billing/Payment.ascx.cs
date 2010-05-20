using System;
using System.Web.UI.WebControls;
using NPetshop.Domain.Billing;
using NPetshop.Presentation;
using NPetshop.Web.UserControls.Accounts;

namespace NPetshop.Web.UserControls.Billing
{
	/// <summary>
	///		Summary description for Order.
	/// </summary>
	public class Payment : NPetshopUC
	{
		protected RequiredFieldValidator valCardNumber;
		protected DropDownList dropdownlistCardType;
		protected TextBox textboxCardNumber;
		protected DropDownList dropdownlistMonth;
		protected DropDownList dropdownlistYear;
		protected CheckBox checkboxShipBilling;
		protected Button ButtonSubmit;
		protected AddressUI ucBillingAddress;
		private BillingController _billingController = null;

		public BillingController BillingController
		{
			set { _billingController = value; }
		}

		public Order Order
		{
			get
			{
				Order order = new Order(this.NPetshopState.CurrentUser, 
					this.NPetshopState.CurrentShoppingCart,
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

		private void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				DataBind();	
			}
		}

		public override void DataBind()
		{
			if (this.NPetshopState.CurrentOrder == null)
			{
				ucBillingAddress.Address = this.NPetshopState.CurrentUser.Address;
			}
			else
			{
				ucBillingAddress.Address = this.NPetshopState.CurrentOrder.BillingAddress;
			}
		}

		private void ButtonSubmit_Click(object sender, EventArgs e)
		{
			_billingController.NewOrder(this.Order, checkboxShipBilling.Checked);
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
			this.ButtonSubmit.Click += new EventHandler(this.ButtonSubmit_Click);
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion

	}
}
