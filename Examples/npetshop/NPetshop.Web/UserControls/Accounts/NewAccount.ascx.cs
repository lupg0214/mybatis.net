namespace NPetshop.Web.UserControls.Accounts
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
	/// Description résumée de NewAccount.
	/// </summary>
	public class NewAccount : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.Literal LiteralMessage;
		protected System.Web.UI.WebControls.Button ButtonCreateNewAccount;
		protected System.Web.UI.WebControls.Button ButtonCancel;
		protected NPetshop.Web.UserControls.Accounts.AccountUI ucAccount;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		private void ButtonCreateNewAccount_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid)
			{
				AccountAction action = new AccountAction(Context);
				action.Account = ucAccount.Account;

				try
				{
					action.CreateNewAccount();
					this.CurrentController.NextView = action.NextViewToDisplay;
				}
				catch(NPetshop.Presentation.Exceptions.LoginAlreadyExistsException)
				{
					LiteralMessage.Text = "Login already exist.";
				}
			}
		}

		private void ButtonCancel_Click(object sender, System.EventArgs e)
		{
			AbstractWebAction action = new ShowStartPageAction(Context);
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
			this.Load += new System.EventHandler(this.Page_Load);
			this.ButtonCreateNewAccount.Click += new System.EventHandler(this.ButtonCreateNewAccount_Click);
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
		}
		#endregion
	}
}
