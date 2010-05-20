using System;
using System.Web.UI.WebControls;
using NPetshop.Presentation;

namespace NPetshop.Web.UserControls.Accounts
{
	/// <summary>
	/// Description résumée de SigneOut.
	/// </summary>
	public class SignOut : NPetshopUC
	{
		protected LinkButton LinkButtonSignIn;
		private AccountController _accountController = null;

		public AccountController AccountController
		{
			set { _accountController = value; }
		}

		private void Page_Load(object sender, EventArgs e)
		{
		}

		private void LinkButtonSignIn_Click(object sender, EventArgs e)
		{
			_accountController.SignIn();
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
			this.LinkButtonSignIn.Click += new EventHandler(this.LinkButtonSignIn_Click);
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion

	}
}
