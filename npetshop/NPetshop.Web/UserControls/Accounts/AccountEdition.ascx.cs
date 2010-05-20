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
	/// Description résumée de AccountEdition.
	/// </summary>
	public class AccountEdition : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.Button ButtonCancel;
		protected System.Web.UI.WebControls.Button ButtonUpdateAccount;
		protected NPetshop.Web.UserControls.Accounts.AccountUI ucAccount;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}
		
		public override void DataBind()
		{
			ucAccount.Account = this.WebLocalSingleton.CurrentUser;
		}

		private void ButtonUpdateAccount_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid)
			{
				AccountAction action = new AccountAction(Context);
				action.Account = ucAccount.Account;

				action.UpdateAccount();
				this.CurrentController.NextView = action.NextViewToDisplay;

				this.WebLocalSingleton.CurrentUser = ucAccount.Account;
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
			this.ButtonUpdateAccount.Click += new System.EventHandler(this.ButtonUpdateAccount_Click);
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
