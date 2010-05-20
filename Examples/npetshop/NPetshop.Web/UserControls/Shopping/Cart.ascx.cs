namespace NPetshop.Web.UserControls.Shopping
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Domain.Shopping;
	using NPetshop.Presentation.Core;
	using NPetshop.Presentation.UserActions;

	/// <summary>
	/// Description résumée de Cart.
	/// </summary>
	public class Cart : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.Literal LiteralTotal;
		protected NPetshop.Presentation.Controls.ExtendedRepeater RepeaterCart;
		protected System.Web.UI.WebControls.LinkButton LinkButtonPrev;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonNext;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonProceedCheckout;
		private ShoppingCart _cart = null; 

		//Property to show total
		protected decimal Total
		{
			get { return _cart.Total; }
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			_cart = this.WebLocalSingleton.CurrentShoppingCart;
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

				ShoppinAction action = new ShoppinAction(this.Context);
				action.UpdateQuantityByItemId(itemId, quantity);
			}
		}

		protected void RepeaterCart_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "RemoveItem")
			{
				ShoppinAction action = new ShoppinAction(this.Context);
				action.RemoveItemFromCart(e.CommandArgument.ToString());
				this.CurrentController.NextView = action.NextViewToDisplay;
			}
			else if (e.CommandName == "Update")
			{
				this.CurrentController.NextView = WebViews.CART;		
			}
			else if (e.CommandName == "ShowItem")
			{
				CatalogAction action = new CatalogAction(this.Context);
				action.ShowItem(e.CommandArgument.ToString());
				this.CurrentController.NextView = action.NextViewToDisplay;
			}
		}

		private void LinkButtonPrev_Click(object sender, System.EventArgs e)
		{
			_cart.Lines.PreviousPage();
			this.CurrentController.NextView = WebViews.CART;
		}

		private void LinkbuttonNext_Click(object sender, System.EventArgs e)
		{
			_cart.Lines.NextPage();
			this.CurrentController.NextView = WebViews.CART;
		}

		private void LinkbuttonProceedCheckout_Click(object sender, System.EventArgs e)
		{
			ShoppinAction action = new ShoppinAction(this.Context);
			action.ProceedCheckout();
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
			this.LinkButtonPrev.Click += new System.EventHandler(this.LinkButtonPrev_Click);
			this.LinkbuttonNext.Click += new System.EventHandler(this.LinkbuttonNext_Click);
			this.LinkbuttonProceedCheckout.Click += new System.EventHandler(this.LinkbuttonProceedCheckout_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
