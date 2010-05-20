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
	/// Description résumée de Product.
	/// </summary>
	public class Product : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.Repeater RepeaterItem;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonPrev;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonNext;
		protected System.Web.UI.WebControls.Label LabelProduct;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		public override void DataBind()
		{
			NPetshop.Domain.Catalog.Product product = this.WebLocalSingleton.CurrentAction.Data[DataViews.PRODUCT] as NPetshop.Domain.Catalog.Product; 
			IPaginatedList paginatedList = this.WebLocalSingleton.CurrentList as IPaginatedList; 

			LabelProduct.Text = product.Name;
			RepeaterItem.DataSource = paginatedList; 
			RepeaterItem.DataBind();

			if (paginatedList.IsNextPageAvailable)
			{
				LinkbuttonNext.Visible = true;
				LinkbuttonNext.CommandArgument = product.Id;
			}
			else
			{
				LinkbuttonNext.Visible = false;
			}
			if (paginatedList.IsPreviousPageAvailable)
			{
				LinkbuttonPrev.Visible = true;
				LinkbuttonPrev.CommandArgument = product.Id;
			}
			else
			{
				LinkbuttonPrev.Visible = false;
			}
		}

		protected void RepeaterItem_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "ShowItem")
			{
				CatalogAction action = new CatalogAction(Context);
				action.ShowItem(e.CommandArgument.ToString());
				this.CurrentController.NextView = action.NextViewToDisplay;
			}
			else if (e.CommandName == "AddToCart")
			{
				ShoppinAction action = new ShoppinAction(Context);
				action.AddItemToCart(e.CommandArgument.ToString());
				this.CurrentController.NextView = action.NextViewToDisplay;
			}
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
