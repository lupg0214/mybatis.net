
using System;
using System.Collections;
using System.Configuration;

using NUnit.Framework;

using IBatisNet.Test.Domain;

namespace IBatisNet.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for ComplexTypeTest.
	/// </summary>
	[TestFixture] 
	public class ComplexTypeTest : BaseTest
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
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Complex type tests

		/// <summary>
		/// Complex type test
		/// </summary>
		[Test] 
		public void TestMapObjMap() 
		{
			Hashtable map = new Hashtable();
			Complex obj = new Complex();
			obj.Map = new Hashtable();
			obj.Map.Add("Id", 1);
			map.Add("obj", obj);

			int id = (int)sqlMap.QueryForObject("ComplexMap", map);

			Assert.AreEqual(id, obj.Map["Id"]);
		}

		#endregion


	}
}
