namespace NPetshop.Web.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Presentation.Core;
	using NPetshop.Presentation.UserActions;

	/// <summary>
	/// Description résumée de Banner.
	/// </summary>
	public class Banner : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.LinkButton LinkbuttonCart;
		protected System.Web.UI.HtmlControls.HtmlImage Img2;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonSignOut;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonSignIn;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonAccount;
		protected System.Web.UI.HtmlControls.HtmlImage Img1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		private void LinkbuttonSignIn_Click(object sender, System.EventArgs e)
		{
			AccountAction action = new AccountAction(Context);
			action.SignIn();
			this.CurrentController.NextView = action.NextViewToDisplay;
		}

		private void LinkbuttonSignOut_Click(object sender, System.EventArgs e)
		{
			AccountAction action = new AccountAction(Context);
			action.SignOut();
			this.CurrentController.NextView = action.NextViewToDisplay;
		}

		private void LinkbuttonAccount_Click(object sender, System.EventArgs e)
		{
			AccountAction action = new AccountAction(Context);
			action.EditAccount();
			this.CurrentController.NextView = action.NextViewToDisplay;
		}

		private void LinkbuttonCart_Click(object sender, System.EventArgs e)
		{
			ShoppinAction action = new ShoppinAction(Context);
			action.ShowShoppingCart();
			this.CurrentController.NextView = action.NextViewToDisplay;
		}

		protected override void OnPreRender(System.EventArgs e)
		{
			bool userLoggedIn = (this.WebLocalSingleton.CurrentUser!= null);

			LinkbuttonSignIn.Visible = ! userLoggedIn;
			LinkbuttonSignOut.Visible = userLoggedIn;		
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
