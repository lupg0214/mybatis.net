
using System;
using System.Collections;

using NPetshop.Domain.Accounts;

namespace NPetshop.Persistence.Interfaces.Accounts
{
	/// <summary>
	/// Summary description for IAccountDao.
	/// </summary>
	public interface IAccountDao
	{
		Account GetAccount(string login);

		IList GetUsernameList();

		Account GetAccount(string login, string password);

		void InsertAccount(Account account);

		void UpdateAccount(Account account);
	}
}
