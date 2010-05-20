using System;
using System.Web.UI.WebControls;
using NPetshop.Presentation;

namespace NPetshop.Web.UserControls.Accounts
{
	/// <summary>
	/// Description résumée de SigneOn.
	/// </summary>
	public class SignIn : NPetshopUC
	{
		protected RequiredFieldValidator valUserId;
		protected TextBox TextBoxLogin;
		protected TextBox TextBoxPassword;
		protected Literal LiteralMessage;
		protected Button ButtonLogIn;
		protected Button ButtonRegister;
		protected LinkButton LinkbuttonRegister;
		protected RequiredFieldValidator valPassword;
		private AccountController _accountController = null;

		public AccountController AccountController
		{
			set { _accountController = value; }
		}

		private void Page_Load(object sender, EventArgs e)
		{
			this.SetFocus(this.TextBoxLogin);
		}

		private void LinkbuttonRegister_Click(object sender, EventArgs e)
		{
			_accountController.RegisterUser();
		}

		private void ButtonLogIn_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				_accountController.Account.Login = TextBoxLogin.Text;
				_accountController.Account.Password = TextBoxPassword.Text;

				if (_accountController.TryToAuthenticate()==false)
				{
					LiteralMessage.Text = "Invalid login or password.  SignIn failed.";
				}
			}		
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
			this.ButtonLogIn.Click += new EventHandler(this.ButtonLogIn_Click);
			this.LinkbuttonRegister.Click += new EventHandler(this.LinkbuttonRegister_Click);
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion


	}
}
