using System;
using System.IO;
using System.Threading;
using System.Configuration;

using IBatisNet.DataAccess;
using IBatisNet.Common; // DataSource definition
using IBatisNet.Common.Utilities; // ScriptRunner definition

using IBatisNet.Test.Dao.Interfaces;

using IBatisNet.Test.NUnit;
using IBatisNet.Test.Domain;

using NUnit.Framework;

namespace IBatisNet.Test.NUnit.DaoTests
{
	/// <summary>
	/// Summary description for BaseDaoTest.
	/// </summary>
	[TestFixture] 
	public abstract class BaseDaoTest
	{
		/// <summary>
		/// A daoManager
		/// </summary>
		protected static  DaoManager daoManager = null;
		protected static string ScriptDirectory = null;

		/// <summary>
		/// Constructor
		/// </summary>
		static BaseDaoTest()
		{
	
			ScriptDirectory = Resources.RootDirectory + Path.DirectorySeparatorChar +
				"Scripts" + Path.DirectorySeparatorChar +
				ConfigurationSettings.AppSettings["database"]+ Path.DirectorySeparatorChar;
		}

		/// <summary>
		/// Run a sql batch for the datasource.
		/// </summary>
		/// <param name="datasource">The datasource.</param>
		/// <param name="script">The sql batch</param>
		protected static void InitScript(DataSource datasource, string script)
		{
			ScriptRunner runner = new ScriptRunner();

			runner.RunScript(datasource, script);
		}

		#region Dao statement tests

		/// <summary>
		/// Verify that DaoManager.GetDao("Account")
		/// return an object that implemetent the interface IAccountDao.
		/// </summary>
		[Test] 						
		public void TestGetDao()
		{
			Type type = typeof(IAccountDao);

			IAccountDao accountDao = (IAccountDao)daoManager[typeof(IAccountDao)];

			Assert.IsNotNull(accountDao);
			Assert.IsTrue(type.IsInstanceOfType(accountDao));
		}


		/// <summary>
		/// Test CreateAccount
		/// </summary>
		[Test] 
		public void TestCreateAccount () 
		{
			IAccountDao accountDao = (IAccountDao)daoManager[typeof(IAccountDao)];

			Account account = NewAccount();

			try
			{
				daoManager.OpenConnection();
				accountDao.Create(account);

				account = accountDao.GetAccountById(1001);
			}
			catch(Exception e)
			{
				// Ignore
				Console.WriteLine("TestCreateAccount, error cause : "+e.Message);
			}
			finally
			{
				daoManager.CloseConnection();
			}

			Assert.IsNotNull(account);
			Assert.AreEqual("Calamity.Jane@somewhere.com", account.EmailAddress);
		}

		/// <summary>
		/// Test CreateAccount
		/// </summary>
		[Test] 
		public void TestCreateAccountExplicitOpenSession () 
		{
			IAccountDao accountDao = daoManager[typeof(IAccountDao)] as IAccountDao;

			Account account = NewAccount();

			try
			{
				accountDao.Create(account);

				account = accountDao.GetAccountById(1001);
			}
			catch(Exception e)
			{
				// Ignore
				Console.WriteLine("TestCreateAccount, error cause : "+e.Message);
			}
			finally
			{
			}

			Assert.IsNotNull(account);
			Assert.AreEqual("Calamity.Jane@somewhere.com", account.EmailAddress);
		}

		/// <summary>
		/// Test Transaction Rollback
		/// </summary>
		[Test] 
		public void TestTransactionRollback () 
		{
			IAccountDao accountDao = (IAccountDao)daoManager[typeof(IAccountDao)];

			Account account = NewAccount();
			daoManager.OpenConnection();
			Account account2 = accountDao.GetAccountById(1);
			daoManager.CloseConnection();

			account2.EmailAddress = "someotherAddress@somewhere.com";

			try 
			{
				daoManager.BeginTransaction();

				accountDao.Create(account);
				accountDao.Update(account2);
				throw new Exception("BOOM!");

				//daoManager.CommitTransaction();
			} 
			catch
			{
				daoManager.RollBackTransaction();
			} 
			finally 
			{
			}

			daoManager.OpenConnection();
			account = accountDao.GetAccountById(account.Id);
			account2 = accountDao.GetAccountById(1);
			daoManager.CloseConnection();

			Assert.IsNull(account);
			Assert.AreEqual("Joe.Dalton@somewhere.com", account2.EmailAddress);
		}


		/// <summary>
		/// Test Transaction Commit
		/// </summary>
		[Test] 
		public void TestTransactionCommit () 
		{
			IAccountDao accountDao = (IAccountDao)daoManager[typeof(IAccountDao)];

			Account account = NewAccount();

			daoManager.OpenConnection();
			Account account2 = accountDao.GetAccountById(1);
			daoManager.CloseConnection();

			account2.EmailAddress = "someotherAddress@somewhere.com";

			try 
			{
				daoManager.BeginTransaction();
				accountDao.Create(account);
				accountDao.Update(account2);
				daoManager.CommitTransaction();
			} 
			finally 
			{
				// Nothing
			}

			daoManager.OpenConnection();
			account = accountDao.GetAccountById(account.Id);
			account2 = accountDao.GetAccountById(1);
			daoManager.CloseConnection();

			Assert.IsNotNull(account);
			Assert.AreEqual("someotherAddress@somewhere.com", account2.EmailAddress);
		}

		/// <summary>
		/// Test Delete
		/// </summary>
		[Test] 
		public void TestDeleteAccount () 
		{
			IAccountDao accountDao = (IAccountDao)daoManager[typeof(IAccountDao)];

			Account account = NewAccount();

			daoManager.OpenConnection();

			accountDao.Create(account);
			account = accountDao.GetAccountById(1001);
			
			Assert.IsNotNull(account);
			Assert.AreEqual("Calamity.Jane@somewhere.com", account.EmailAddress);

			accountDao.Delete(account);

			account = accountDao.GetAccountById(1001);
			Assert.IsNull(account);

			daoManager.CloseConnection();
		}

		/// <summary>
		/// Test Using syntax on daoManager.OpenConnection
		/// </summary>
		[Test] 
		public void TestUsingConnection() 
		{
			IAccountDao accountDao = (IAccountDao)daoManager[typeof(IAccountDao)];

			using ( IDalSession session = daoManager.OpenConnection() )
			{
				Account account = NewAccount();
				accountDao.Create(account);
			}   // compiler will call Dispose on DaoSession
		}

		/// <summary>
		/// Test Test Using syntax on daoManager.BeginTransaction
		/// </summary>
		[Test] 
		public void TestUsingTransaction() 
		{
			IAccountDao accountDao = (IAccountDao)daoManager[typeof(IAccountDao)];

			using ( IDalSession session = daoManager.BeginTransaction() )
			{
				Account account = NewAccount();
				Account account2 = accountDao.GetAccountById(1);
				account2.EmailAddress = "someotherAddress@somewhere.com";

				accountDao.Create(account);
				accountDao.Update(account2);

				session.Complete(); // Commit
			} // compiler will call Dispose on IDalSession
		}

		#endregion

		/// <summary>
		/// Create a new account with id = 1001
		/// </summary>
		/// <returns>An account</returns>
		protected Account NewAccount() 
		{
			Account account = new Account();
			account.Id = 1001;
			account.FirstName = "Calamity";
			account.LastName = "Jane";
			account.EmailAddress = "Calamity.Jane@somewhere.com";
			return account;
		}

	}
}
