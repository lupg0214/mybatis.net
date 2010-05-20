
using System;
using System.Collections;
using System.Threading;
using System.Configuration;

using NUnit.Framework;

using IBatisNet.Common;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper; //<-- To access the definition of the deleagte RowDelegate
using IBatisNet.DataMapper.MappedStatements;

using IBatisNet.DataMapper.Test;
using IBatisNet.DataMapper.Test.Domain;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for ParameterMapTest.
	/// </summary>
	[TestFixture] 
	public class CacheTest : BaseTest
	{
			#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitSqlMap();
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-init.sql" );
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-procedure.sql", false );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Test cache

		/// <summary>
		/// Test for JIRA 29
		/// </summary>
		[Test] 
		public void TestJIRA28()
		{
			Account account = sqlMap.QueryForObject("GetNoAccountWithCache",-99) as Account;

			Assert.IsNull(account);
		}

		/// <summary>
		/// Test Cache query
		/// </summary>
		[Test] 
		public void TestQueryWithCache() 
		{
			IList list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int firstId = IBatisNet.Common.Utilities.HashCodeProvider.GetIdentityHashCode(list);
				//System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(list);

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			//Console.WriteLine(sqlMap.GetDataCacheStats());

			int secondId = IBatisNet.Common.Utilities.HashCodeProvider.GetIdentityHashCode(list);
				//System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(list);

			Assert.AreEqual(firstId, secondId);

			Account account = (Account) list[1];
			account.EmailAddress  = "somebody@cache.com";
			sqlMap.Update("UpdateAccountViaInlineParameters", account);

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int thirdId = IBatisNet.Common.Utilities.HashCodeProvider.GetIdentityHashCode(list);

			Assert.IsTrue(firstId != thirdId);

			//Console.WriteLine(sqlMap.GetDataCacheStats());
		}

		/// <summary>
		/// Test flush Cache
		/// </summary>
		[Test] 
		public void TestFlushDataCache() 
		{
			IList list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int firstId = IBatisNet.Common.Utilities.HashCodeProvider.GetIdentityHashCode(list);

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int secondId = IBatisNet.Common.Utilities.HashCodeProvider.GetIdentityHashCode(list);

			Assert.AreEqual(firstId, secondId);

			sqlMap.FlushCaches();

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int thirdId = IBatisNet.Common.Utilities.HashCodeProvider.GetIdentityHashCode(list);

			Assert.IsTrue(firstId != thirdId);
		}


		/// <summary>
		/// Test MappedStatement Query With Threaded Cache
		/// </summary>
		[Test]
		public void TestMappedStatementQueryWithThreadedCache() 
		{
			Hashtable results1 = new Hashtable();
			Hashtable results2 = new Hashtable();

			TestCacheThread.StartThread(sqlMap, results1, "GetCachedAccountsViaResultMap");
			int firstId = (int) results1["id"];

			TestCacheThread.StartThread(sqlMap, results2, "GetCachedAccountsViaResultMap");
			int secondId = (int) results2["id"];

			Assert.AreEqual(firstId, secondId);

			IList list = (IList) results1["list"];

			Account account = (Account) list[1];
			account.EmailAddress = "new.toto@somewhere.com";
			sqlMap.Update("UpdateAccountViaInlineParameters", account);

			list = sqlMap.QueryForList("GetCachedAccountsViaResultMap", null);

			int thirdId = IBatisNet.Common.Utilities.HashCodeProvider.GetIdentityHashCode(list);

			Assert.IsTrue(firstId != thirdId);

		}


		#endregion


		private class TestCacheThread
		{
			private SqlMapper _sqlMap = null;
			private Hashtable _results = null;
			private string _statementName = string.Empty;

			public TestCacheThread(SqlMapper sqlMap, Hashtable results, string statementName) 
			{
				_sqlMap = sqlMap;
				_results = results;
				_statementName = statementName;
			}

			public void Run() 
			{
				try 
				{
					MappedStatement statement = sqlMap.GetMappedStatement( _statementName );
					IDalSession session = new SqlMapSession(sqlMap.DataSource);
					session.OpenConnection();
					IList list = statement.ExecuteQueryForList(session, null);

					int firstId = IBatisNet.Common.Utilities.HashCodeProvider.GetIdentityHashCode(list);

					list = statement.ExecuteQueryForList(session, null);
					int secondId = IBatisNet.Common.Utilities.HashCodeProvider.GetIdentityHashCode(list);

					_results.Add("id", secondId );
					_results.Add("list", list);
					session.CloseConnection();
				} 
				catch (Exception e) 
				{
					throw e;
				}
			}

			public static void StartThread(SqlMapper sqlMap, Hashtable results, string statementName) 
			{
				TestCacheThread tct = new TestCacheThread(sqlMap, results, statementName);
				Thread thread = new Thread( new ThreadStart(tct.Run) );
				thread.Start();
				try 
				{
					thread.Join();
				} 
				catch (Exception e) 
				{
					throw e;
				}
			}
		}
	}
}
