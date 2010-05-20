namespace NPetshop.Web.UserControls.Catalog
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using IBatisNet.Common.Pagination;
	using NPetshop.Domain.Catalog;
	using NPetshop.Presentation.Core;
	using NPetshop.Presentation.UserActions;

	/// <summary>
	///		Summary description for ProductsByCategory.
	/// </summary>
	public class ProductList : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.LinkButton LinkbuttonPrev;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonNext;
		protected System.Web.UI.WebControls.Label LabelCategory;
		protected System.Web.UI.WebControls.Repeater RepeaterProducts;

		private void Page_Load(object sender, System.EventArgs e)
		{
//			if (this.IsRequestCurrentView)
//			{
//				string categoryName = (string)WebLocalSingleton.GetInstance(Context).CurrentAction.Data[DataViews.CATEGORY_NAME]; 
//
//				Label1.Text = categoryName;
//			}
		}

		public override void DataBind()
		{
			IPaginatedList productList = this.WebLocalSingleton.CurrentList; 
			NPetshop.Domain.Catalog.Category category  = ((NPetshop.Domain.Catalog.Product) productList[0]).Category;

			RepeaterProducts.DataSource = productList;
			RepeaterProducts.DataBind();

			if (productList.IsNextPageAvailable)
			{
				LinkbuttonNext.Visible = true;
				LinkbuttonNext.CommandArgument = category.Id;
			}
			else
			{
				LinkbuttonNext.Visible = false;
			}
			if (productList.IsPreviousPageAvailable)
			{
				LinkbuttonPrev.Visible = true;
				LinkbuttonPrev.CommandArgument = category.Id;
			}
			else
			{
				LinkbuttonPrev.Visible = false;
			}

			LabelCategory.Text = category.Name;
		}

		protected void RepeaterProducts_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			CatalogAction action = new CatalogAction(Context);
			action.ShowItemsByProduct(e.CommandArgument.ToString());
			this.CurrentController.NextView = action.NextViewToDisplay;
		}

		private void LinkbuttonPrev_Command(object source, System.Web.UI.WebControls.CommandEventArgs e)
		{
			IPaginatedList productList = this.WebLocalSingleton.CurrentList; 
			productList.PreviousPage();
			this.CurrentController.NextView = WebViews.PRODUCTS_BY_CATEGORY;
		}

		private void LinkbuttonNext_Command(object source, System.Web.UI.WebControls.CommandEventArgs e)
		{
			IPaginatedList productList = this.WebLocalSingleton.CurrentList; 
			productList.NextPage();
			this.CurrentController.NextView = WebViews.PRODUCTS_BY_CATEGORY;
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
			this.LinkbuttonPrev.Command += new System.Web.UI.WebControls.CommandEventHandler(this.LinkbuttonPrev_Command);
			this.LinkbuttonNext.Command += new System.Web.UI.WebControls.CommandEventHandler(this.LinkbuttonNext_Command);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
