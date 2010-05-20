using System;
using System.Collections;
using System.Threading;
using System.Configuration;

using NUnit.Framework;

using IBatisNet.DataMapper.Exceptions;

using IBatisNet.DataMapper.Test;
using IBatisNet.DataMapper.Test.Domain;

using log4net;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for TransactionTest.
	/// </summary>
	[TestFixture] 
	public class ThreadTest: BaseTest
	{
		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static int _numberOfThreads = 10;

		#region SetUp & TearDown

		/// <summary>
		/// SetUp 
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitSqlMap();
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-init.sql" );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Thread test

		/// <summary>
		/// Test BeginTransaction, CommitTransaction
		/// </summary>
		[Test] 
		public void TestThread() 
		{
			Account account = NewAccount6();

			try 
			{
				Thread[] threads = new Thread[_numberOfThreads];

				AccessTest accessTest = new AccessTest();

				for (int i = 0; i < _numberOfThreads; i++) 
				{
					Thread thread = new Thread(new ThreadStart(accessTest.GetAccount));
					threads[i] = thread;
				}
				for (int i = 0; i < _numberOfThreads; i++) 
				{
					threads[i].Start();
				}
			} 
			finally 
			{
			}

		}

		#endregion

		/// <summary>
		/// Summary description for AccessTest.
		/// </summary>
		private class AccessTest
		{
		
			/// <summary>
			/// Get an account
			/// </summary>
			public void GetAccount()
			{
				Assert.IsFalse(sqlMap.IsSessionStarted);
				
				Account account = (Account) sqlMap.QueryForObject("GetAccountViaColumnIndex", 1);
				
				Assert.IsFalse(sqlMap.IsSessionStarted);
				
				Assert.AreEqual(1, account.Id, "account.Id");
				Assert.AreEqual("Joe", account.FirstName, "account.FirstName");
				Assert.AreEqual("Dalton", account.LastName, "account.LastName");

			}
		}	
	}


}
