namespace NPetshop.Web.UserControls.Catalog
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Domain.Catalog;
	using NPetshop.Presentation.Core;
	using NPetshop.Presentation.UserActions;
	/// <summary>
	/// Description résumée de Item.
	/// </summary>
	public class Item : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.Label LabelDescription;
		protected System.Web.UI.WebControls.Label LabelName;
		protected System.Web.UI.WebControls.Label LabelPrice;
		protected System.Web.UI.WebControls.LinkButton LinkButtonAddToCart;
		protected System.Web.UI.WebControls.Label LabelProduct;
		protected System.Web.UI.WebControls.Image ImagePhoto;
		protected System.Web.UI.WebControls.Label LabelQty;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		public override void DataBind()
		{				
			NPetshop.Domain.Catalog.Item item = this.WebLocalSingleton.CurrentAction.Data[DataViews.ITEM] as NPetshop.Domain.Catalog.Item; 
			if (item != null)
			{
				LabelProduct.Text = item.Product.Name;
				LabelDescription.Text = item.Attribute1;
				LabelPrice.Text = item.ListPrice.ToString();
				LabelQty.Text = item.Quantity.ToString();
				LabelName.Text = item.Id;
				ImagePhoto.ImageUrl = "~/@images/pets/"+item.Photo;
				LinkButtonAddToCart.CommandArgument = item.Id;
			}
		}

		private void LinkButtonAddToCart_Click(object sender, CommandEventArgs e)
		{
			ShoppinAction action = new ShoppinAction(Context);
			action.AddItemToCart(e.CommandArgument.ToString());
			this.CurrentController.NextView = action.NextViewToDisplay;
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
			this.LinkButtonAddToCart.Command += new System.Web.UI.WebControls.CommandEventHandler(this.LinkButtonAddToCart_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
