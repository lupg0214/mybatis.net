
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
	/// Description résumée de SigneOn.
	/// </summary>
	public class SignIn : NPetshop.Presentation.UserControl
	{
		protected System.Web.UI.WebControls.RequiredFieldValidator valUserId;
		protected System.Web.UI.WebControls.TextBox TextBoxLogin;
		protected System.Web.UI.WebControls.TextBox TextBoxPassword;
		protected System.Web.UI.WebControls.Literal LiteralMessage;
		protected System.Web.UI.WebControls.Button ButtonLogIn;
		protected System.Web.UI.WebControls.Button ButtonRegister;
		protected System.Web.UI.WebControls.LinkButton LinkbuttonRegister;
		protected System.Web.UI.WebControls.RequiredFieldValidator valPassword;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Placer ici le code utilisateur pour initialiser la page
		}

		private void LinkbuttonRegister_Click(object sender, System.EventArgs e)
		{
			AccountAction action = new AccountAction(Context);
			action.RegisterUser();
			this.CurrentController.NextView = action.NextViewToDisplay;
		}

		private void ButtonLogIn_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid)
			{
				AccountAction action = new AccountAction(Context);
				action.Account.Login = TextBoxLogin.Text;
				action.Account.Password = TextBoxPassword.Text;

				action.TryToAuthenticate();

				if (action.Account != null)
				{
					this.CurrentController.NextView = action.NextViewToDisplay;
				}
				else
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
			this.ButtonLogIn.Click += new System.EventHandler(this.ButtonLogIn_Click);
			this.LinkbuttonRegister.Click += new System.EventHandler(this.LinkbuttonRegister_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
