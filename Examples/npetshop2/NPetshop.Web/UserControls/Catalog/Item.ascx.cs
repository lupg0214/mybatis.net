using System;
using System.Web.UI.WebControls;
using NPetshop.Presentation;

namespace NPetshop.Web.UserControls.Catalog
{
	/// <summary>
	/// Description résumée de Item.
	/// </summary>
	public class Item : NPetshopUC
	{
		protected Label LabelDescription;
		protected Label LabelName;
		protected Label LabelPrice;
		protected LinkButton LinkButtonAddToCart;
		protected Label LabelProduct;
		protected Image ImagePhoto;
		protected Label LabelQty;
		private ShoppingController _shoppingController = null;

		public ShoppingController ShoppingController
		{
			set { _shoppingController = value; }
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
			Domain.Catalog.Item item = this.NPetshopState.CurrentObject as Domain.Catalog.Item; 
			if (item != null)
			{
				LabelProduct.Text = item.Product.Name;
				LabelDescription.Text = item.Attribute1;
				LabelPrice.Text = item.ListPrice.ToString();
				LabelQty.Text = item.Quantity.ToString();
				LabelName.Text = item.Id;
				ImagePhoto.ImageUrl = ImagePhoto.ResolveUrl("~/@images/pets/"+item.Photo);
				LinkButtonAddToCart.CommandArgument = item.Id;
			}
		}

		private void LinkButtonAddToCart_Click(object sender, CommandEventArgs e)
		{
			_shoppingController.AddItemToCart(e.CommandArgument.ToString());
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
			this.LinkButtonAddToCart.Command += new CommandEventHandler(this.LinkButtonAddToCart_Click);
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion

	}
}
