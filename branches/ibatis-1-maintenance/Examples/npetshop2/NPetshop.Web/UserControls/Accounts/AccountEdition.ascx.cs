using System;
using System.Web.UI.WebControls;
using NPetshop.Presentation;

namespace NPetshop.Web.UserControls.Accounts
{
	/// <summary>
	/// Description résumée de AccountEdition.
	/// </summary>
	public class AccountEdition : NPetshopUC
	{
		protected Button ButtonCancel;
		protected Button ButtonUpdateAccount;
		protected AccountUI ucAccount;
		private AccountController _accountController = null;

		public AccountController AccountController
		{
			set { _accountController = value; }
		}

		private void Page_Load(object sender, EventArgs e)
		{
			if(!IsPostBack )
			{
				DataBind();
			}
		}
		
		public override void DataBind()
		{
			ucAccount.Account = this.NPetshopState.CurrentUser;
		}

		private void ButtonUpdateAccount_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				_accountController.Account = ucAccount.Account;
				_accountController.UpdateAccount();
			}
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			_accountController.CancelEdition();
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
			this.ButtonUpdateAccount.Click += new EventHandler(this.ButtonUpdateAccount_Click);
			this.ButtonCancel.Click += new EventHandler(this.ButtonCancel_Click);
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion

	}
}
