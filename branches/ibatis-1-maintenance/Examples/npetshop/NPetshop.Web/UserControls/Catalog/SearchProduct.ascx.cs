namespace NPetshop.Web.UserControls.Catalog
{
	using System;
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
	/// Description résumée de SearchProduct.
	/// </summary>
	public class SearchProduct : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.LinkButton LinkbuttonPrev;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonNext;
		protected System.Web.UI.WebControls.Literal LiteralKeywords;
		protected NPetshop.Presentation.Controls.ExtendedRepeater RepeaterProduct;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		public override void DataBind()
		{
			IPaginatedList productList = this.WebLocalSingleton.CurrentList; 
			//string keywords = this.WebLocalSingleton.CurrentAction.Data[DataViews.SEARCH_KEYWORDS] as string;
			
			//LiteralKeywords.Text = keywords;
			RepeaterProduct.DataSource = productList;
			RepeaterProduct.DataBind();

			if (productList.IsNextPageAvailable)
			{
				LinkbuttonNext.Visible = true;
				//LinkbuttonNext.CommandArgument = keywords;
			}
			else
			{
				LinkbuttonNext.Visible = false;
			}
			if (productList.IsPreviousPageAvailable)
			{
				LinkbuttonPrev.Visible = true;
				//LinkbuttonPrev.CommandArgument = keywords;
			}
			else
			{
				LinkbuttonPrev.Visible = false;
			}

		}

		protected void RepeaterProduct_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			CatalogAction action = new CatalogAction(Context);
			action.ShowItemsByProduct(e.CommandArgument.ToString());
			this.CurrentController.NextView = action.NextViewToDisplay;
		}

		protected void LinkbuttonPrev_Command(object source, System.Web.UI.WebControls.CommandEventArgs e)
		{
			IPaginatedList productList = this.WebLocalSingleton.CurrentList; 
			productList.PreviousPage();
			this.CurrentController.NextView = WebViews.SEARCH_PRODUCTS;
		}

		protected void LinkbuttonNext_Command(object source, System.Web.UI.WebControls.CommandEventArgs e)
		{
			IPaginatedList productList = this.WebLocalSingleton.CurrentList; 
			productList.NextPage();
			this.CurrentController.NextView = WebViews.SEARCH_PRODUCTS;
		}

		#region Code généré par le Concepteur Web Form
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		///		le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
