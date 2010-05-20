using System;
using NPetshop.Domain.Accounts;
using NPetshop.Domain.Shopping;
using NPetshop.Service.Interfaces;

namespace NPetshop.Presentation
{
	/// <summary>
	/// Summary description for AccountController.
	/// </summary>
	public class AccountController : NPetshopController
	{
		private IAccountService _accountService =null;
		private IShoppingService _shoppingService = null;
		private Account _account =null;

		public Account Account
		{
			get { return _account; }
			set { _account = value; }
		}

		public AccountController(
			IAccountService accountService,
			IShoppingService shoppingService)
		{
			_accountService = accountService;
			_shoppingService = shoppingService;
			_account = new Account();
		}

		private void CreateUserEnvironment()
		{
			this.NState.CurrentUser = _account;
			this.NState.CurrentShoppingCart = new ShoppingCart();
			this.NState.CurrentOrder = null;
			this.NState.Command = "goHome";
			this.Navigate();
		}

		public void CreateNewAccount()
		{
			_accountService.InsertAccount(this.Account);
			CreateUserEnvironment();
		}

		public void UpdateAccount()
		{
			_accountService.UpdateAccount(this.Account);
			this.NState.CurrentUser = this.Account;
			this.State.Command = "goHome";
			this.Navigate();
		}

		public void EditAccount()
		{
			if (this.NState.CurrentUser != null) 
			{
				this.NState.Command = "editAccount";
			}
			else 
			{
				this.NState.Command = "register";			
			}
			this.Navigate();
		}

		public void CancelEdition()
		{
			this.Navigate();
		}

		public void SignIn()
		{
			this.Navigate();
		}

		public void RegisterUser()
		{
			this.Navigate();
		}

		public void SignOut()
		{
			this.NState.CurrentUser = null;
			this.NState.CurrentShoppingCart = null;
			this.Navigate();
		}

		public bool TryToAuthenticate()
		{
			_account = _accountService.GetAccount(this.Account.Login, this.Account.Password);

			if (this.Account!=null)
			{
				//myList = catalogService.getProductListByCategory(account.getFavouriteCategoryId());
				CreateUserEnvironment();
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
