using System;

namespace NPetshop.Web.UserControls.Billing
{
	/// <summary>
	///		Summary description for Bill.
	/// </summary>
	public class Bill : NPetshopUC
	{
		protected BillUI ucBill;


		private void Page_Load(object sender, EventArgs e)
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
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion
	}
}
