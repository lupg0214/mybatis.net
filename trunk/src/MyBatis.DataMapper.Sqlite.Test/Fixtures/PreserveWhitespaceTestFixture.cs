using System.Collections;
using System.Collections.Specialized;
using MyBatis.Common.Logging;
using MyBatis.Common.Logging.Impl;
using NUnit.Framework;

namespace MyBatis.DataMapper.Sqlite.Test.Fixtures
{
    [TestFixture]
    public class PreserveWhitespaceTestFixture : BaseTest
    {
        [Test]
        public void PreserveWhitespace()
        {
            LogManager.Adapter = new ConsoleOutLoggerFA(new NameValueCollection());

            InitScript(sessionFactory.DataSource, "../../Scripts/account-init.sql");

            ICollection items = dataMapper.QueryForList("Account.GetAllAccounts1", null);
            Assert.IsTrue(items.Count > 1);

            items = dataMapper.QueryForList("Account.GetAllAccounts2", null);
            Assert.IsTrue(items.Count > 1);
        }
    }
}
