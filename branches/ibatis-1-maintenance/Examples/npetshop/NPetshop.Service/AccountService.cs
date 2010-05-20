
using System;
using System.Collections;

using IBatisNet.DataAccess;

using NPetshop.Domain.Accounts;
using NPetshop.Service;
using NPetshop.Persistence.Interfaces.Accounts;

namespace NPetshop.Service
{
	/// <summary>
	/// Summary description for AccountService.
	/// </summary>
	public class AccountService
	{
		#region Private Fields 
		private static AccountService _instance = new AccountService();
		private DaoManager _daoManager = null;
		private IAccountDao _accountDao = null;
		#endregion

		#region Constructor
		private AccountService() 
		{
			_daoManager = ServiceConfig.GetInstance().DaoManager;
			_accountDao = _daoManager.GetDao( typeof(IAccountDao) ) as IAccountDao;
		}
		#endregion

		#region Public methods

		public static AccountService GetInstance() 
		{
			return _instance;
		}


		public Account GetAccount(string username) 
		{
			Account account = null;

			account = _accountDao.GetAccount(username);

			return account;
		}

		public Account GetAccount(string login, string password) 
		{
			Account account = null;

			account = _accountDao.GetAccount(login, password);

			return account;
		}

		public void InsertAccount(Account account) 
		{
			_daoManager.BeginTransaction();
			try
			{
				_accountDao.InsertAccount(account);
				_daoManager.CommitTransaction();
			}
			catch
			{
				_daoManager.RollBackTransaction();
				throw;
			}
		}

		public void UpdateAccount(Account account) 
		{
			_daoManager.BeginTransaction();
			try
			{
				_accountDao.UpdateAccount(account);
				_daoManager.CommitTransaction();
			}
			catch
			{
				_daoManager.RollBackTransaction();
				throw;
			}
		}

		public IList GetUsernameList() 
		{
			return _accountDao.GetUsernameList();
		}
		#endregion

	}
}
