using NPetshop.Presentation;
using Castle.MVC.Controllers;// Attribut

namespace NPetshop.Web.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;


	/// <summary>
	/// Description résumée de Banner.
	/// </summary>
	public class Banner : NPetshopUC
	{
		protected System.Web.UI.WebControls.LinkButton LinkbuttonSignOut;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonSignIn;
		protected System.Web.UI.HtmlControls.HtmlImage Img2;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonAccount;
		private AccountController _accountController = null;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonCart;
		private ShoppingController _shoppingController = null;

		public ShoppingController ShoppingController
		{
			set { _shoppingController = value; }
		}

		public AccountController AccountController
		{
			set { _accountController = value; }
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
		}

		private void LinkbuttonSignIn_Click(object sender, System.EventArgs e)
		{
			_accountController.SignIn();
		}

		private void LinkbuttonSignOut_Click(object sender, System.EventArgs e)
		{
			_accountController.SignOut();
		}

		private void LinkbuttonAccount_Click(object sender, System.EventArgs e)
		{
			_accountController.EditAccount();
		}

		protected override void OnPreRender(System.EventArgs e)
		{
			bool userLoggedIn = (this.NPetshopState.CurrentUser!= null);

			LinkbuttonSignIn.Visible = ! userLoggedIn;
			LinkbuttonSignOut.Visible = userLoggedIn;		
		}

		private void LinkbuttonCart_Click(object sender, System.EventArgs e)
		{
			_shoppingController.ShowShoppingCart();
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
			this.LinkbuttonSignOut.Click += new System.EventHandler(this.LinkbuttonSignOut_Click);
			this.LinkbuttonSignIn.Click += new System.EventHandler(this.LinkbuttonSignIn_Click);
			this.LinkbuttonAccount.Click += new System.EventHandler(this.LinkbuttonAccount_Click);
			this.LinkbuttonCart.Click += new System.EventHandler(this.LinkbuttonCart_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
