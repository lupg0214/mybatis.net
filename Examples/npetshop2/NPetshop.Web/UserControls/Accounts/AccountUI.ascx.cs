namespace NPetshop.Web.UserControls.Accounts
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using NPetshop.Domain.Accounts;
	using NPetshop.Domain.Catalog;

	/// <summary>
	/// Description résumée de Account.
	/// </summary>
	public class AccountUI : NPetshopUC
	{
		protected System.Web.UI.WebControls.TextBox textboxLogin;
		protected System.Web.UI.WebControls.TextBox textboxPassword;
		protected System.Web.UI.WebControls.TextBox textboxEmail;

		protected System.Web.UI.WebControls.RequiredFieldValidator valUserId;
		protected System.Web.UI.WebControls.RequiredFieldValidator valPassword;
		protected System.Web.UI.WebControls.RequiredFieldValidator valEmail;
		protected System.Web.UI.WebControls.DropDownList listLanguage;
		protected System.Web.UI.WebControls.DropDownList listCategory;
		protected System.Web.UI.WebControls.CheckBox chkShowFavorites;
		protected System.Web.UI.WebControls.CheckBox chkShowBanners;
		protected AddressUI ucAddress;
		
		public NPetshop.Domain.Accounts.Account Account
		{
			get
			{
				NPetshop.Domain.Accounts.Account account = new NPetshop.Domain.Accounts.Account();

				account.Login = textboxLogin.Text;
				account.Password = textboxPassword.Text;
				account.Email = textboxEmail.Text;

				account.Address = ucAddress.Address ;

				account.Profile.FavoriteLanguage = listLanguage.SelectedItem.Value ;
				account.Profile.FavouriteCategory  = new Category(); 
				account.Profile.FavouriteCategory.Id = listCategory.SelectedItem.Value;
				account.Profile.IsShowFavorites = chkShowFavorites.Checked;
				account.Profile.IsShowBanners = chkShowBanners.Checked;

				return account;
			} 
			set
			{
				textboxLogin.Text = value.Login;
				textboxPassword.Text = value.Password;
				textboxEmail.Text = value.Email;

				ucAddress.Address = value.Address;

				SetSelectedItem(listLanguage, value.Profile.FavoriteLanguage);
				SetSelectedItem(listCategory, value.Profile.FavouriteCategory.Id);
				chkShowFavorites.Checked = value.Profile.IsShowFavorites;
				chkShowBanners.Checked = value.Profile.IsShowBanners;
			}
		}

		private void SetSelectedItem(DropDownList list, string value)
		{
			ListItem item = list.Items.FindByValue(value);
			if (item != null)
			{
				item.Selected = true;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			this.SetFocus(textboxLogin);
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

		}
		#endregion
	}
}
