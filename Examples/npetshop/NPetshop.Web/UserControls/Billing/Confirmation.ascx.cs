namespace NPetshop.Web.UserControls.Billing
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Presentation.Core;
	using NPetshop.Presentation.UserActions;

	/// <summary>
	///		Summary description for Confirmation.
	/// </summary>
	public class Confirmation : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.Button ButtonContinue;
		protected NPetshop.Web.UserControls.Billing.BillUI ucBill;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		public override void DataBind()
		{
			ucBill.Order = this.WebLocalSingleton.CurrentOrder;
		}


		private void ButtonContinue_Click(object sender, System.EventArgs e)
		{
			BillinAction action = new BillinAction(this.Context);
			action.ConfirmOrder(this.WebLocalSingleton.CurrentOrder);
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
			this.ButtonContinue.Click += new System.EventHandler(this.ButtonContinue_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
