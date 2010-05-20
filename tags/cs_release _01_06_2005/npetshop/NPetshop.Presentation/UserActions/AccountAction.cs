
using System;
using System.Web;

using NPetshop.Service;

using NPetshop.Domain.Accounts;
using NPetshop.Presentation.Core;
using NPetshop.Presentation.Exceptions;

namespace NPetshop.Presentation.UserActions
{
	/// <summary>
	/// Summary description for AccountAction.
	/// </summary>
	public class AccountAction : AbstractWebAction
	{
		private Account _account =null;

		private static readonly AccountService _accountService = AccountService.GetInstance();
		private static readonly CatalogService _catalogService = CatalogService.GetInstance();

		public AccountAction(HttpContext context) : base(context) 
		{
			_account = new Account();
		}

		private void CreateUserEnvironment()
		{
			singleton.CurrentUser = _account;
			singleton.CurrentShoppingCart = ShoppinAction.CreateNewShoppingCart();
			singleton.CurrentOrder = null;
			this.nextViewToDisplay = WebViews.STARTUP;
		}

		public Account Account
		{
			get
			{
				return _account;
			}
			set
			{
				_account = value;
			}
		}

		public void EditAccount()
		{
			if (singleton.CurrentUser != null) 
			{
				this.nextViewToDisplay = WebViews.EDIT_ACCOUNT;
			}
			else 
			{
				this.nextViewToDisplay = WebViews.REGISTER_USER;			
			}
		}

//		public void ExistingUserSignIn()
//		{
//			this.nextViewToDisplay = WebViews.EXISTING_USER_SIGNIN;
//		}

		public void SignIn()
		{
			this.nextViewToDisplay = WebViews.SIGNIN;
		}

		public void RegisterUser()
		{
			this.nextViewToDisplay = WebViews.REGISTER_USER;
		}

		public void SignOut()
		{
			this.singleton.CurrentUser = null;
			this.singleton.CurrentShoppingCart = null;
			this.nextViewToDisplay = WebViews.SIGNOUT;
		}

		public void TryToAuthenticate()
		{
			_account = _accountService.GetAccount(this.Account.Login, this.Account.Password);

			if (this.Account!=null)
			{
				//myList = catalogService.getProductListByCategory(account.getFavouriteCategoryId());
				CreateUserEnvironment();
			}
		}

		public void CreateNewAccount()
		{
			_accountService.InsertAccount(this.Account);
			CreateUserEnvironment();
		}

		public void UpdateAccount()
		{
			_accountService.UpdateAccount(this.Account);
			this.nextViewToDisplay = WebViews.STARTUP;
		}
	}
}
