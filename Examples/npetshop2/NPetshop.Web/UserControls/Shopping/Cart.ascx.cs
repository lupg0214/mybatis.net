using NPetshop.Presentation;

namespace NPetshop.Web.UserControls.Shopping
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Domain.Shopping;

	/// <summary>
	/// Description résumée de Cart.
	/// </summary>
	public class Cart : NPetshopUC
	{
		protected System.Web.UI.WebControls.Literal LiteralTotal;
		protected NPetshop.Web.Controls.ExtendedRepeater RepeaterCart;
		protected System.Web.UI.WebControls.LinkButton LinkButtonPrev;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonNext;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonProceedCheckout;
		protected System.Web.UI.WebControls.LinkButton LinkButtonRemove;
		protected System.Web.UI.WebControls.LinkButton LinkButtonItem;
		protected System.Web.UI.WebControls.TextBox TextboxQuantity;
		protected System.Web.UI.WebControls.RegularExpressionValidator valInteger;
		protected System.Web.UI.WebControls.LinkButton LinkButtonUpdateCart;
		private ShoppingCart _cart = null; 
		private ShoppingController _shoppingController = null;
		private CatalogController _catalogController = null;

		public ShoppingController ShoppingController
		{
			set { _shoppingController = value; }
		}

		public CatalogController CatalogController
		{
			set { _catalogController = value; }

		}
		//Property to show total
		protected decimal Total
		{
			get { return _cart.Total; }
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			_cart = this.NPetshopState.CurrentShoppingCart;
			if(!IsPostBack)
			{
				DataBind();	
			}
		}

		public override void DataBind()
		{
			bool empty = ((_cart == null) || _cart.IsEmpty);
			if (! empty)
			{
				RepeaterCart.DataSource = _cart;
				RepeaterCart.DataBind();

				if (_cart.Lines.IsNextPageAvailable)
				{
					LinkbuttonNext.Visible = true;
				}
				else
				{
					LinkbuttonNext.Visible = false;
				}
				if (_cart.Lines.IsPreviousPageAvailable)
				{
					LinkButtonPrev.Visible = true;
				}
				else
				{
					LinkButtonPrev.Visible = false;
				}
			}	
			else
			{
				LinkbuttonNext.Visible = false;
				LinkButtonPrev.Visible = false;
				LinkbuttonProceedCheckout.Visible = false;
			}
		}

		protected void QuantityChanged(object o, System.EventArgs e)
		{
			Page.Validate();
			if (Page.IsValid)
			{
				TextBox textboxQuantity = o as System.Web.UI.WebControls.TextBox;
				LinkButton linkButtonRemove = textboxQuantity.Parent.FindControl("LinkButtonRemove") as LinkButton;			

				int quantity = int.Parse(textboxQuantity.Text);
				string itemId = linkButtonRemove.CommandArgument.ToString();

				_shoppingController.UpdateQuantityByItemId(itemId, quantity);
			}
		}

		protected void RepeaterCart_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "removeItem")
			{
				_shoppingController.RemoveItemFromCart(e.CommandArgument.ToString());
			}
			else if (e.CommandName == "update")
			{
				DataBind();	
			}
			else if (e.CommandName == "showItem")
			{
				_catalogController.ShowItem( e.CommandArgument.ToString() );
			}
		}

		private void LinkButtonPrev_Click(object sender, System.EventArgs e)
		{
			_cart.Lines.PreviousPage();
		}

		private void LinkbuttonNext_Click(object sender, System.EventArgs e)
		{
			_cart.Lines.NextPage();
		}

		private void LinkbuttonProceedCheckout_Click(object sender, System.EventArgs e)
		{
			_shoppingController.ProceedCheckout();
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
			this.LinkButtonPrev.Click += new System.EventHandler(this.LinkButtonPrev_Click);
			this.LinkbuttonNext.Click += new System.EventHandler(this.LinkbuttonNext_Click);
			this.LinkbuttonProceedCheckout.Click += new System.EventHandler(this.LinkbuttonProceedCheckout_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
