using System;
using System.Web.UI.WebControls;
using IBatisNet.Common.Pagination;
using NPetshop.Presentation;

namespace NPetshop.Web.UserControls.Catalog
{
	/// <summary>
	///	Summary description for Product.
	/// </summary>
	public class Product : NPetshopUC
	{
		protected Label LabelProduct;
		protected Repeater RepeaterItems;
		protected LinkButton LinkbuttonPrev;
		protected LinkButton LinkbuttonNext;
		private CatalogController _catalogController = null;
		private ShoppingController _shoppingController = null;

		public ShoppingController ShoppingController
		{
			set { _shoppingController = value; }
		}

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
			IPaginatedList itemList = this.NPetshopState.CurrentList as IPaginatedList; 
			Domain.Catalog.Product product = itemList[0] as Domain.Catalog.Product; 

			foreach(Domain.Catalog.Item item in itemList)
			{
				product = item.Product;
			}

			LabelProduct.Text = product.Name;
			RepeaterItems.DataSource = itemList; 
			RepeaterItems.DataBind();

			if (itemList.IsNextPageAvailable)
			{
				LinkbuttonNext.Visible = true;
				LinkbuttonNext.CommandArgument = product.Id;
			}
			else
			{
				LinkbuttonNext.Visible = false;
			}
			if (itemList.IsPreviousPageAvailable)
			{
				LinkbuttonPrev.Visible = true;
				LinkbuttonPrev.CommandArgument = product.Id;
			}
			else
			{
				LinkbuttonPrev.Visible = false;
			}
		}

		protected void RepeaterItems_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "showItem")
			{
				_catalogController.ShowItem(e.CommandArgument.ToString());
			}
			else if (e.CommandName == "addToCart")
			{
				_shoppingController.AddItemToCart(e.CommandArgument.ToString());
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
