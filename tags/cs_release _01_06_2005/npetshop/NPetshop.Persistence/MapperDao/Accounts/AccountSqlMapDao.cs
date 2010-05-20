
using System;
using System.Collections;

using NPetshop.Domain.Accounts;
using NPetshop.Persistence.Interfaces.Accounts;
using NPetshop.Persistence.MapperDao;

namespace NPetshop.Persistence.MapperDao.Accounts
{
	/// <summary>
	/// Summary description for AccountSqlMapDao
	/// </summary>
	public class AccountSqlMapDao : BaseSqlMapDao, IAccountDao
	{
		#region IAccountDao Members

		public Account GetAccount(string login)
		{
			return (ExecuteQueryForObject("GetAccountByUsername", login) as Account);
		}

		public IList GetUsernameList()
		{
			return ExecuteQueryForList("GetUsernameList", null);
		}

		public Account GetAccount(string login, string password)
		{
			Account account = new Account();
			account.Login = login;
			account.Password = password;
			return (ExecuteQueryForObject("GetAccountByLoginAndPassword", account) as Account);
		}

		public void InsertAccount(Account account)
		{
			ExecuteInsert("InsertAccount", account);
			ExecuteInsert("InsertProfile", account);
			ExecuteInsert("InsertSignon", account);
		}

		public void UpdateAccount(Account account)
		{
			ExecuteUpdate("UpdateAccount", account);
			ExecuteUpdate("UpdateProfile", account);

			if (account.Password.Length > 0) 
			{
				ExecuteUpdate("UpdateSignon", account);
			}
		}

		#endregion
	}
}
