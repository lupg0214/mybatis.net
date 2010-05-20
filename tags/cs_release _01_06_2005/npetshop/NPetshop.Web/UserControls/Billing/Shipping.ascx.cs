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
	///		Summary description for Shipping.
	/// </summary>
	public class Shipping : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.Button ButtonBack;
		protected System.Web.UI.WebControls.Button ButtonSubmit;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		private void ButtonBack_Click(object sender, System.EventArgs e)
		{
			this.CurrentController.NextView = WebViews.PAYMENT;
		}

		private void ButtonSubmit_Click(object sender, System.EventArgs e)
		{
		
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
			this.ButtonBack.Click += new System.EventHandler(this.ButtonBack_Click);
			this.ButtonSubmit.Click += new System.EventHandler(this.ButtonSubmit_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
