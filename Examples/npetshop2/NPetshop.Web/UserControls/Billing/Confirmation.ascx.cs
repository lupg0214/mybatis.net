using NPetshop.Presentation;

namespace NPetshop.Web.UserControls.Billing
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for Confirmation.
	/// </summary>
	public class Confirmation : NPetshopUC
	{
		protected System.Web.UI.WebControls.Button ButtonContinue;
		protected NPetshop.Web.UserControls.Billing.BillUI ucBill;
		private BillingController _billingController = null;

		public BillingController BillingController
		{
			set { _billingController = value; }
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DataBind();	
			}
		}

		public override void DataBind()
		{
			ucBill.Order = this.NPetshopState.CurrentOrder;
		}


		private void ButtonContinue_Click(object sender, System.EventArgs e)
		{
			_billingController.ConfirmOrder(this.NPetshopState.CurrentOrder);
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
			this.ButtonContinue.Click += new System.EventHandler(this.ButtonContinue_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
