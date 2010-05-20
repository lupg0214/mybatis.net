using System.Collections;
using NPetshop.Domain.Accounts;

namespace NPetshop.Service.Interfaces
{
	public interface IAccountService
	{
		Account GetAccount(string username);
		Account GetAccount(string login, string password);
		void InsertAccount(Account account);
		void UpdateAccount(Account account);
		IList GetUsernameList();
	}
}