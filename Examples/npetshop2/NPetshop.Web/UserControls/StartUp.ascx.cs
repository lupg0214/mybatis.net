using System;
using System.Web.UI.HtmlControls;

using Castle.MVC.Controllers;
using NPetshop.Presentation;

namespace NPetshop.Web.UserControls
{
	/// <summary>
	///	Summary description for Initial.
	/// </summary>
	public class StartUp : NPetshopUC
	{
		protected HtmlImage Img1;
		private CatalogController _catalogController = null;

		public CatalogController CatalogController
		{
			set { _catalogController = value; }
		}

		private void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack && this.Request.Form["__EVENTTARGET"]=="checkMap")
			{
				this.State.Command = "showCategory";
				_catalogController.ShowProductsByCategory(Request.Form["__EVENTARGUMENT"]);
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
			this.Load += new EventHandler(this.Page_Load);

		}

		#endregion
	}
}
