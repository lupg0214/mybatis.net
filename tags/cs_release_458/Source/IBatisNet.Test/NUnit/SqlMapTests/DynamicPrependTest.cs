using System;
using System.Collections;
using System.Configuration;

using NUnit.Framework;

using IBatisNet.Test;
using IBatisNet.Test.Domain;

namespace IBatisNet.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for DynamicPrependTest.
	/// </summary>
	[TestFixture] 
	public class DynamicPrependTest : BaseTest
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
				InitScript( sqlMap.DataSource, ScriptDirectory + "order-init.sql" );
				InitScript( sqlMap.DataSource, ScriptDirectory + "line-item-init.sql" );
				InitScript( sqlMap.DataSource, ScriptDirectory + "other-init.sql" );
			}

			/// <summary>
			/// TearDown
			/// </summary>
			[TearDown] 
			public void Dispose()
			{ /* ... */ } 

			#endregion

		#region Dynamic Prepend tests

		/// <summary>
		/// Test Iterate With Prepend (1)
		/// </summary>
		[Test] 
		public void TestIterateWithPrepend1()  
		{
			IList parameters = new ArrayList();
			parameters.Add(1);
			parameters.Add(2);
			parameters.Add(3);
			
			IList list = sqlMap.QueryForList("DynamicIterateWithPrepend1", parameters);

			AssertAccount1((Account) list[0]);
			Assert.AreEqual(3, list.Count);
		}

		/// <summary>
		/// Test Iterate With Prepend (2)
		/// </summary>
		[Test] 
		public void TestIterateWithPrepend2()  
		{
			IList parameters = new ArrayList();
			parameters.Add(1);
			parameters.Add(2);
			parameters.Add(3);
			
			IList list = sqlMap.QueryForList("DynamicIterateWithPrepend2", parameters);

			AssertAccount1((Account) list[0]);
			Assert.AreEqual(3, list.Count);
		}

		/// <summary>
		/// Test Iterate With Prepend (3)
		/// </summary>
		[Test] 
		public void TestIterateWithPrepend3()  
		{
			IList parameters = new ArrayList();
			parameters.Add(1);
			parameters.Add(2);
			parameters.Add(3);
			
			IList list = sqlMap.QueryForList("DynamicIterateWithPrepend3", parameters);
			
			AssertAccount1((Account) list[0]);
			Assert.AreEqual(3, list.Count);
		}

		/// <summary>
		/// Test Dynamic With Prepend (1)
		/// </summary>
		[Test] 
		public void TestDynamicWithPrepend1()  
		{
			Account account = new Account();
			account.Id = 1;

			account = (Account) sqlMap.QueryForObject("DynamicWithPrepend", account);

			AssertAccount1(account);
		}

		/// <summary>
		/// Test Dynamic With Prepend (2)
		/// </summary>
		[Test] 
		public void TestDynamicWithPrepend2()  
		{
			Account account = new Account();
			account.Id = 1;
			account.FirstName = "Joe";

			account = (Account) sqlMap.QueryForObject("DynamicWithPrepend", account);
			AssertAccount1(account);

		}

		/// <summary>
		/// Test Dynamic With Prepend (3)
		/// </summary>
		[Test] 
		public void TestDynamicWithPrepend3()  
		{
			Account account = new Account();
			account.Id = 1;
			account.FirstName = "Joe";
			account.LastName = "Dalton";

			account = (Account) sqlMap.QueryForObject("DynamicWithPrepend", account);
			AssertAccount1(account);
		}

		/// <summary>
		/// Test Dynamic With Prepend (4)
		/// </summary>
		[Test] 
		public void TestDynamicWithPrepend4()  
		{
			IList list = sqlMap.QueryForList("DynamicWithPrepend", null);

			AssertAccount1((Account) list[0]);
			Assert.AreEqual(5, list.Count);
		}

		/// <summary>
		/// Test Iterate With Two Prepends
		/// </summary>
		[Test] 
		public void TestIterateWithTwoPrepends() 
		{
			Account account = new Account();
			account.Id = 1;
			account.FirstName = "Joe";

			account = sqlMap.QueryForObject("DynamicWithPrepend", account) as Account;
			Assert.IsNotNull(account);
			AssertAccount1(account);

			IList list = sqlMap.QueryForList("DynamicWithTwoDynamicElements", account);
			AssertAccount1((Account) list[0]);
		}

		/// <summary>
		/// Test Complex Dynamic
		/// </summary>
		[Test] 
		public void TestComplexDynamic() 
		{
			Account account = new Account();
			account.Id = 1;
			account.FirstName = "Joe";
			account.LastName = "Dalton";
			
			IList list = sqlMap.QueryForList("ComplexDynamicStatement", account);

			AssertAccount1((Account) list[0]);
			Assert.AreEqual(1, list.Count);
		}

		/// <summary>
		/// Test GetAccounts Dynamic
		/// </summary>
		/// <remarks>
		/// Bug Fix http://sourceforge.net/forum/message.php?msg_id=2646964
		/// </remarks>
		[Test] 
		public void TestGetAccountsDynamic() 
		{
			Hashtable map = new Hashtable();
			map.Add("MaximumAllowed",100);

			map.Add("FirstName","Joe");
			map.Add("LastName","Dalton");
			map.Add("EmailAddress","Joe.Dalton@somewhere.com");

			IList list = sqlMap.QueryForList("GetAccountsDynamic", map);

			AssertAccount1((Account) list[0]);
			Assert.AreEqual(1, list.Count);
		}

		/// <summary>
		/// Test IsEqual with HashTable
		/// </summary>
		/// <remarks>
		/// Bug Fix https://sourceforge.net/forum/message.php?msg_id=2840259
		/// </remarks>
		[Test] 
		public void TestDynamicSelectByIntLongc() 
		{
			Hashtable search = new Hashtable(); 
			search.Add("year", 0); 
			search.Add("areaid", 0); 

			IList list = sqlMap.QueryForList("DynamicSelectByIntLong", search);

			Assert.AreEqual(2, list.Count);
			//----------------------
			search.Clear();
			search.Add("year", 1); 
			search.Add("areaid", 0); 

			list= null;
			list = sqlMap.QueryForList("DynamicSelectByIntLong", search);

			Assert.AreEqual(1, list.Count);
			//----------------------
			search.Clear();
			search.Add("year", 0); 
			search.Add("areaid", 9999999999); 

			list= null;
			list = sqlMap.QueryForList("DynamicSelectByIntLong", search);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(2, (list[0] as Other).Int);
			//----------------------
			search.Clear();
			search.Add("year", 2); 
			search.Add("areaid", 9999999999); 

			list= null;
			list = sqlMap.QueryForList("DynamicSelectByIntLong", search);

			Assert.AreEqual(2, (list[0] as Other).Int);
			Assert.AreEqual(1, list.Count);
		}

		/// <summary>
		/// Test Dynamic With GUID
		/// </summary>
		[Test] 
		public void TestDynamicWithGUID() 
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.Empty;

			int key = (int)sqlMap.Insert("InsertCategory", category);

			category = new Category();
			category.Name = "titi";
			category.Guid = Guid.NewGuid();

			Category categoryTest = (Category)sqlMap.QueryForObject("DynamicGuid", category);
			Assert.IsNull(categoryTest);

			category = new Category();
			category.Name = "titi";
			category.Guid = Guid.Empty;

			categoryTest = (Category)sqlMap.QueryForObject("DynamicGuid", category);
			Assert.IsNotNull(categoryTest);
		}

		/// <summary>
		/// Test JIRA 11
		/// </summary>
		[Test] 
		public void TestJIRA11() 
		{
			Search search = new Search();
			search.NumberSearch = 123;
			search.StartDate = new DateTime(2004,12,25);
			search.Operande = "like";
			search.StartDateAnd = true;
			
			IList list = sqlMap.QueryForList("Jira-IBATISNET-11", search);

			Assert.AreEqual(0, list.Count);
		}
		#endregion

		
	}
}
