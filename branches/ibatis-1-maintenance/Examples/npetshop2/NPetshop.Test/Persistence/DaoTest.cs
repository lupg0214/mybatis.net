using System;
using System.Collections;
using NPetshop.Persistence.Interfaces.Accounts;
using NPetshop.Persistence.Interfaces.Catalog;
using NUnit.Framework;


namespace NPetshop.Test.Persistence
{
	/// <summary>
	/// Description résumée de DaoTest.
	/// </summary>
	public class DaoTest : BaseTest
	{

		[Test] 						
		public void TestGetDao()
		{
			Type type = typeof(IAccountDao);

			IAccountDao accountDao = (IAccountDao)daoManager[typeof(IAccountDao)];

			Assert.IsNotNull(accountDao);
			Assert.IsTrue(type.IsInstanceOfType(accountDao));
		}

		[Test] 						
		public void TestCategoryDao()
		{
			ICategoryDao categoryDao = (ICategoryDao)daoManager[typeof(ICategoryDao)];

			IList list = categoryDao.GetCategoryList();
			Assert.IsNotNull(list);
			Assert.IsTrue(list.Count>0);
		}
	}
}
