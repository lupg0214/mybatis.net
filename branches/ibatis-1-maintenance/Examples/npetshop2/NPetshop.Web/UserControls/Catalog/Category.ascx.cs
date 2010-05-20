using System;
using System.Web.UI.WebControls;
using IBatisNet.Common.Pagination;
using NPetshop.Domain.Catalog;
using NPetshop.Presentation;


namespace NPetshop.Web.UserControls.Catalog
{
	/// <summary>
	/// Description résumée de Category.
	/// </summary>
	public class Category : NPetshopUC
	{
		protected LinkButton LinkbuttonPrev;
		protected Label LabelCategory;
		protected Repeater RepeaterProducts;
		protected LinkButton LinkbuttonNext;
		private CatalogController _catalogController = null;

		public CatalogController CatalogController
		{
			set { _catalogController = value; }
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
			IPaginatedList productList = this.NPetshopState.CurrentList; 
			Domain.Catalog.Category category  = ((Domain.Catalog.Product) productList[0]).Category;

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

		protected void RepeaterProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			_catalogController.ShowItemsByProduct(e.CommandArgument.ToString());
		}

		private void LinkbuttonPrev_Command(object source, CommandEventArgs e)
		{
			IPaginatedList productList = this.NPetshopState.CurrentList; 
			productList.PreviousPage();
			DataBind();	
		}

		private void LinkbuttonNext_Command(object source, CommandEventArgs e)
		{
			IPaginatedList productList = this.NPetshopState.CurrentList; 
			productList.NextPage();
			DataBind();	
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
			this.Load += new EventHandler(this.Page_Load);
			this.LinkbuttonPrev.Command += new System.Web.UI.WebControls.CommandEventHandler(this.LinkbuttonPrev_Command);
			this.LinkbuttonNext.Command += new System.Web.UI.WebControls.CommandEventHandler(this.LinkbuttonNext_Command);

		}
		#endregion
	}
}
