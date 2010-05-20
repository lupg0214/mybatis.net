using NPetshop.Presentation;

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

	/// <summary>
	/// Description résumée de SearchProduct.
	/// </summary>
	public class SearchProduct : NPetshopUC
	{
		protected System.Web.UI.WebControls.LinkButton LinkbuttonPrev;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonNext;
		protected System.Web.UI.WebControls.Literal LiteralKeywords;
		protected NPetshop.Web.Controls.ExtendedRepeater RepeaterProduct;
		protected System.Web.UI.WebControls.LinkButton LinkButtonProduct;
		private CatalogController _catalogController = null;

		public CatalogController CatalogController
		{
			set { _catalogController = value; }
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
			IPaginatedList productList = this.NPetshopState.CurrentList; 
			string keywords = this.NPetshopState.CurrentObject as string;
			
			LiteralKeywords.Text = keywords;
			RepeaterProduct.DataSource = productList;
			RepeaterProduct.DataBind();

			if (productList.IsNextPageAvailable)
			{
				LinkbuttonNext.Visible = true;
			}
			else
			{
				LinkbuttonNext.Visible = false;
			}
			if (productList.IsPreviousPageAvailable)
			{
				LinkbuttonPrev.Visible = true;
			}
			else
			{
				LinkbuttonPrev.Visible = false;
			}

		}

		protected void RepeaterProduct_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			_catalogController.ShowItemsByProduct(e.CommandArgument.ToString());
		}

		protected void LinkbuttonPrev_Command(object source, System.Web.UI.WebControls.CommandEventArgs e)
		{
			IPaginatedList productList = this.NPetshopState.CurrentList; 
			productList.PreviousPage();
			DataBind();	
		}

		protected void LinkbuttonNext_Command(object source, System.Web.UI.WebControls.CommandEventArgs e)
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
