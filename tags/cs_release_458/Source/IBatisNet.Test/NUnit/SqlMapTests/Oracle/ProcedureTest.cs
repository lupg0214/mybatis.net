using System;
using System.Collections;

using NUnit.Framework;

using IBatisNet.Test.NUnit;
using IBatisNet.Test.Domain;

namespace IBatisNet.Test.NUnit.SqlMapTests.Oracle
{
	/// <summary>
	/// Summary description for ProcedureTest.
	/// </summary>
	[TestFixture] 
	[Category("Oracle")]
	public class ProcedureTest : BaseTest
	{
		
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitSqlMap();
			InitScript( sqlMap.DataSource, ScriptDirectory + "category-init.sql" );
			InitScript( sqlMap.DataSource, ScriptDirectory + "category-procedure.sql", false );		
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-init.sql" );	
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-procedure.sql", false );
			InitScript( sqlMap.DataSource, ScriptDirectory + "swap-procedure.sql", false );	
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific statement store procedure tests for oracle

		/// <summary>
		/// Test an insert with sequence key via a store procedure.
		/// </summary>
		[Test] 
		public void InsertTestSequenceViaProcedure()
		{
			Category category = new Category();
			category.Name = "Mapping object relational";

			sqlMap.Insert("InsertCategoryViaStoreProcedure", category);
			Assert.AreEqual(1, category.Id );

			category = new Category();
			category.Name = "Nausicaa";

			sqlMap.QueryForObject("InsertCategoryViaStoreProcedure", category);
			Assert.AreEqual(2, category.Id );
		}

		/// <summary>
		/// Test store procedure with output parameters
		/// </summary>
		[Test]
		public void TestProcedureWithOutputParameters() 
		{
			string first = "Joe.Dalton@somewhere.com";
			string second = "Averel.Dalton@somewhere.com";

			Hashtable map = new Hashtable();
			map.Add("email1", first);
			map.Add("email2", second);

			sqlMap.QueryForObject("SwapEmailAddresses", map);

			Assert.AreEqual(first, map["email2"]);
			Assert.AreEqual(second, map["email1"]);
		}

		/// <summary>
		/// Test store procedure with input parameters
		/// passe via Hashtable
		/// </summary>
		[Test]
		public void TestProcedureWithInputParametersViaHashtable() 
		{
			Hashtable map = new Hashtable();
			map.Add("Id", 0);
			map.Add("Name", "Toto");
			map.Add("GuidString", Guid.NewGuid().ToString());

			sqlMap.Insert("InsertCategoryViaStoreProcedure", map);
			Assert.AreEqual(1, map["Id"] );

		}

		/// <summary>
		/// Test Insert Account via store procedure
		/// </summary>
		[Test] 
		public void TestInsertAccountViaStoreProcedure() {
			Account account = new Account();

			account.Id = 99;
			account.FirstName = "Achille";
			account.LastName = "Talon";
			account.EmailAddress = "Achille.Talon@somewhere.com";

			sqlMap.Insert("InsertAccountViaStoreProcedure", account);

			Account testAccount = sqlMap.QueryForObject("GetAccountViaColumnName", 99) as Account;

			Assert.IsNotNull(testAccount);
			Assert.AreEqual(99, testAccount.Id);
		}
		#endregion
	}
}
