using System;
using System.Collections;
using System.Collections.Specialized;
using MyBatis.DataMapper;
using MyBatis.DataMapper.Configuration;
using MyBatis.DataMapper.Configuration.Interpreters.Config.Xml;
using MyBatis.DataMapper.Session;
using MyBatis.Common.Data;
using MyBatis.Common.Logging;
using MyBatis.Common.Logging.Impl;
using MyBatis.Common.Utilities;
using MyBatis.DataMapper.Sqlite.Test.Domain;
using MyBatis.DataMapper.Sqlite.Test.Fixtures.Modules;
using NUnit.Framework;

namespace MyBatis.DataMapper.Sqlite.Test.Fixtures
{
    public delegate string KeyConvert(string key);
    [TestFixture]
    public abstract class BaseTest : ScriptBase
    {
        protected static IDataMapper dataMapper;

        protected static KeyConvert ConvertKey = null;
        protected static ISessionFactory sessionFactory = null;
        protected IConfigurationEngine ConfigurationEngine;
        protected ConfigurationSetting configurationSetting;

        [TestFixtureSetUp]
        protected virtual void TestFixtureSetUp()
        {
            LogManager.Adapter = new ConsoleOutLoggerFA(new NameValueCollection());
            configurationSetting = new ConfigurationSetting();
            configurationSetting.Properties.Add("collection2Namespace", "MyBatis.DataMapper.Sqlite.Test.Domain.LineItemCollection2, MyBatis.DataMapper.Sqlite.Test");
            configurationSetting.Properties.Add("nullableInt", "int?");

            ConfigurationEngine = new DefaultConfigurationEngine(configurationSetting);
            ConfigurationEngine.RegisterInterpreter(new XmlConfigurationInterpreter("SqlMap.config"));
            ConfigurationEngine.RegisterModule(new AliasModule());

            IMapperFactory mapperFactory = ConfigurationEngine.BuildMapperFactory();
            sessionFactory = ConfigurationEngine.ModelStore.SessionFactory;
            dataMapper = ((IDataMapperAccessor)mapperFactory).DataMapper;
        }

        /// <summary>
        /// Dispose the SqlMap
        /// </summary>
        [TestFixtureTearDown]
        protected virtual void TestFixtureTearDown()
        {
            dataMapper = null;
        }

        /// <summary>
        /// Run a sql batch for the datasource.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="script">The sql batch</param>
        public static void InitScript(IDataSource datasource, string script)
        {
            InitScript(datasource, script, true);
        }

        /// <summary>
        /// Run a sql batch for the datasource.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="script">The sql batch</param>
        /// <param name="doParse">parse out the statements in the sql script file.</param>
        protected static void InitScript(IDataSource datasource, string script, bool doParse)
        {
            ScriptRunner runner = new ScriptRunner();

            runner.RunScript(datasource, script, doParse);
        }

        /// <summary>
        /// Create a new account with id = 6
        /// </summary>
        /// <returns>An account</returns>
        protected Account NewAccount6()
        {
            Account account = new Account();
            account.Id = 6;
            account.FirstName = "Calamity";
            account.LastName = "Jane";
            account.EmailAddress = "no_email@provided.com";
            return account;
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account object</param>
        protected void AssertGilles(Account account)
        {
            Assert.AreEqual(5, account.Id, "account.Id");
            Assert.AreEqual("Gilles", account.FirstName, "account.FirstName");
            Assert.AreEqual("Bayon", account.LastName, "account.LastName");
            Assert.AreEqual("gilles.bayon@nospam.org", account.EmailAddress, "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account object</param>
        protected void AssertAccount1(Account account)
        {
            Assert.AreEqual(1, account.Id, "account.Id");
            Assert.AreEqual("Joe", account.FirstName, "account.FirstName");
            Assert.AreEqual("Dalton", account.LastName, "account.LastName");
            Assert.AreEqual("Joe.Dalton@somewhere.com", account.EmailAddress, "account.EmailAddress");
        }

        protected void AssertAccount2(Account account)
        {
            Assert.AreEqual(2, account.Id, "account.Id");
            Assert.AreEqual("Averel", account.FirstName, "account.FirstName");
            Assert.AreEqual("Dalton", account.LastName, "account.LastName");
            Assert.AreEqual("Averel.Dalton@somewhere.com", account.EmailAddress, "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account as hashtable</param>
        protected void AssertAccount1AsHashtable(Hashtable account)
        {
            Assert.AreEqual(1, (int)account["Id"], "account.Id");
            Assert.AreEqual("Joe", (string)account["FirstName"], "account.FirstName");
            Assert.AreEqual("Dalton", (string)account["LastName"], "account.LastName");
            Assert.AreEqual("Joe.Dalton@somewhere.com", (string)account["EmailAddress"], "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account as hashtable</param>
        protected void AssertAccount1AsHashtableForResultClass(Hashtable account)
        {
            Assert.AreEqual(1, (int)account[ConvertKey("Id")], "account.Id");
            Assert.AreEqual("Joe", (string)account[ConvertKey("FirstName")], "account.FirstName");
            Assert.AreEqual("Dalton", (string)account[ConvertKey("LastName")], "account.LastName");
            Assert.AreEqual("Joe.Dalton@somewhere.com", (string)account[ConvertKey("EmailAddress")], "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=6).
        /// </summary>
        /// <param name="account">An account object</param>
        protected void AssertAccount6(Account account)
        {
            Assert.AreEqual(6, account.Id, "account.Id");
            Assert.AreEqual("Calamity", account.FirstName, "account.FirstName");
            Assert.AreEqual("Jane", account.LastName, "account.LastName");
            Assert.IsNull(account.EmailAddress, "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input order is equal to the order(id=1).
        /// </summary>
        /// <param name="order">An order object.</param>
        protected void AssertOrder1(Order order)
        {
            DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

            Assert.AreEqual(1, order.Id, "order.Id");
            Assert.AreEqual(date.ToString(), order.Date.ToString(), "order.Date");
            Assert.AreEqual("VISA", order.CardType, "order.CardType");
            Assert.AreEqual("999999999999", order.CardNumber, "order.CardNumber");
            Assert.AreEqual("05/03", order.CardExpiry, "order.CardExpiry");
            Assert.AreEqual("11 This Street", order.Street, "order.Street");
            Assert.AreEqual("Victoria", order.City, "order.City");
            Assert.AreEqual("BC", order.Province, "order.Id");
            Assert.AreEqual("C4B 4F4", order.PostalCode, "order.PostalCode");
        }

        /// <summary>
        /// Verify that the input order is equal to the order(id=1).
        /// </summary>
        /// <param name="order">An order as hashtable.</param>
        protected void AssertOrder1AsHashtable(Hashtable order)
        {
            DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

            Assert.AreEqual(1, (int)order["Id"], "order.Id");
            Assert.AreEqual(date.ToString(), ((DateTime)order["Date"]).ToString(), "order.Date");
            Assert.AreEqual("VISA", (string)order["CardType"], "order.CardType");
            Assert.AreEqual("999999999999", (string)order["CardNumber"], "order.CardNumber");
            Assert.AreEqual("05/03", (string)order["CardExpiry"], "order.CardExpiry");
            Assert.AreEqual("11 This Street", (string)order["Street"], "order.Street");
            Assert.AreEqual("Victoria", (string)order["City"], "order.City");
            Assert.AreEqual("BC", (string)order["Province"], "order.Id");
            Assert.AreEqual("C4B 4F4", (string)order["PostalCode"], "order.PostalCode");
        }
    }
}
