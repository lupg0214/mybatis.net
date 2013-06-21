
using System;
using System.Collections;
using System.IO;
using System.Threading;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Test.Domain;
using NUnit.Framework;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for ParameterMapTest.
	/// </summary>
	[TestFixture] 
	public class ModuleTest : BaseTest
	{
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{
			InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql" );
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-procedure.sql", false);
            InitScript(sqlMap.DataSource, ScriptDirectory + "order-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "line-item-init.sql");
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void TearDown()
		{
		    string path = ScriptDirectory + "teardown.sql";
            // does a teardown exist?
            if ((!File.Exists(path)) || (new FileInfo(path).Length == 0))
            {
                return;
            }
            InitScript(sqlMap.DataSource, path);
        } 

		#endregion

		#region Test cache

        [Test]
        public void Module_Selects_With_ResultMap_Should_work()
        {
            Order order = sqlMap.QueryForObject<Order>("GetOrderWithGenericListLineItemViaModule", 1);

            AssertOrder1(order);

            // Check generic IList collection
            Assert.IsNotNull(order.LineItemsGenericList);
            Assert.AreEqual(3, order.LineItemsGenericList.Count);
        }

		#endregion


	}
}
