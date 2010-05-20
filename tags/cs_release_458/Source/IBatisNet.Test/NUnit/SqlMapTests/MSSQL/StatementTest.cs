using System;
using System.Collections;

using NUnit.Framework;

using IBatisNet.Test.NUnit;
using IBatisNet.Test.Domain;

namespace IBatisNet.Test.NUnit.SqlMapTests.MSSQL
{
	/// <summary>
	/// Summary description for StatementTest.
	/// </summary>
	[TestFixture] 
	[Category("MSSQL")]
	public class StatementTest : BaseTest
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
			InitScript( sqlMap.DataSource, ScriptDirectory + "category-init.sql" );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific statement test for sql server

		/// <summary>
		/// Test Insert Account via store procedure
		/// </summary>
		[Test] 
		public void TestInsertAccountViaStoreProcedure() 
		{
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

		/// <summary>
		/// Test an insert with identity key.
		/// </summary>
		[Test] 
		public void TestInsertIdentityViaInsertQuery()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategory", category);
			Assert.AreEqual(1, key);
		}

		/// <summary>
		/// Test Insert Via Insert Statement.
		/// </summary>
		[Test] 
		public void TestInsertViaInsertStatement()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryViaInsertSatement", category);
			Assert.AreEqual(1, key);
		}

		/// <summary>
		/// Test guid column/field.
		/// </summary>
		[Test] 
		public void TestGuidColumn()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategory", category);

			Category categoryTest = (Category)sqlMap.QueryForObject("GetCategory", key);
			Assert.AreEqual(key, categoryTest.Id);
			Assert.AreEqual(category.Name, categoryTest.Name);
			Assert.AreEqual(category.Guid, categoryTest.Guid);
		}

		/// <summary>
		/// Test Insert Category Via ParameterMap.
		/// </summary>
		[Test] 
		public void TestInsertCategoryViaParameterMap()
		{
			Category category = new Category();
			category.Name = "Cat";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryViaParameterMap", category);
			Assert.AreEqual(1, key);
		}

		/// <summary>
		/// Test Update Category with Extended ParameterMap
		/// </summary>
		[Test] 
		public void TestUpdateCategoryWithExtendParameterMap()
		{
			Category category = new Category();
			category.Name = "Cat";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryViaParameterMap", category);
			category.Id = key;

			category.Name = "Dog";
			category.Guid = Guid.NewGuid();

			sqlMap.Update("UpdateCategoryViaParameterMap", category);

			Category categoryRead = null;
			categoryRead = (Category) sqlMap.QueryForObject("GetCategory", key);

			Assert.AreEqual(category.Id, categoryRead.Id);
			Assert.AreEqual(category.Name, categoryRead.Name);
			Assert.AreEqual(category.Guid.ToString(), categoryRead.Guid.ToString());
		}
		#endregion


	}
}
