namespace NPetshop.Web.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Presentation.Core; // def AbstractWebAction
	using NPetshop.Presentation.UserActions;

	/// <summary>
	///		Summary description for Initial.
	/// </summary>
	public class StartUp : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.HtmlControls.HtmlImage Img1;
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.WebControls.LinkButton LinkButton1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (IsPostBack && (Request.Form["__EVENTTARGET"] == "BODY_CONTROL"))
			{
				CatalogAction action = new CatalogAction(this.Context);
				try
				{
					action.ShowProductsByCategory(Request.Form["__EVENTARGUMENT"]);
					this.CurrentController.NextView = action.NextViewToDisplay;
				}
				catch
				{
					this.CurrentController.NextView = WebViews.ERROR;
				}
			}	
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

//		private void Button1_Click(object sender, System.EventArgs e)
//		{
//			AbstractWebAction action = new TestAction(Context);
//			this.CurrentRouter.NextView = action.NextViewToDisplay;
//		}
	}
}
