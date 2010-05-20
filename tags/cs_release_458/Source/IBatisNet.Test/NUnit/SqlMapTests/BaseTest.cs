using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Configuration;

using log4net;

using NUnit.Framework;

using IBatisNet.DataAccess;

using IBatisNet.Common; // DataSource definition
using IBatisNet.Common.Utilities; // ScriptRunner definition
using IBatisNet.DataMapper; // SqlMap API

using IBatisNet.Test.Domain;

[assembly:log4net.Config.DOMConfigurator(Watch=true)]

namespace IBatisNet.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for BaseTest.
	/// </summary>
	public abstract class BaseTest
	{
		/// <summary>
		/// The sqlMap
		/// </summary>
		protected static SqlMapper sqlMap;
		/// <summary>
		/// A daoManager with a SimpledaoSession
		/// </summary>
		protected static DaoManager daoManager = null;
		/// <summary>
		/// A daoManager with a sqlMapSession
		/// </summary>
		protected static DaoManager daoManagerSqlMap = null;

		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
		
		protected static string ScriptDirectory = null;

		/// <summary>
		/// Constructor
		/// </summary>
		static BaseTest()
		{

			ScriptDirectory = Resources.RootDirectory + Path.DirectorySeparatorChar +
				"Scripts" + Path.DirectorySeparatorChar +
				ConfigurationSettings.AppSettings["database"]+ Path.DirectorySeparatorChar;
		}

		/// <summary>
		/// Initialize an sqlMap
		/// </summary>
		protected static void InitSqlMap()
		{
			//DateTime start = DateTime.Now;

			ConfigureHandler handler = new ConfigureHandler( Configure );
			sqlMap = SqlMapper.ConfigureAndWatch( "sqlmap"+ "_" +  ConfigurationSettings.AppSettings["database"] + "_"
				+ ConfigurationSettings.AppSettings["providerType"] +".config", handler );

//			string loadTime = DateTime.Now.Subtract(start).ToString();
//			Console.WriteLine("Loading configuration time :"+loadTime);
		}

		/// <summary>
		/// Configure the SqlMap
		/// </summary>
		/// <remarks>
		/// Must verify ConfigureHandler signature.
		/// </remarks>
		/// <param name="obj">
		/// The reconfigured sqlMap.
		/// </param>
		protected static void Configure(object obj)
		{
			sqlMap = (SqlMapper)obj;
		}

		/// <summary>
		/// Run a sql batch for the datasource.
		/// </summary>
		/// <param name="datasource">The datasource.</param>
		/// <param name="script">The sql batch</param>
		protected static void InitScript(DataSource datasource, string script)
		{
			InitScript(datasource, script, true);
		}

		/// <summary>
		/// Run a sql batch for the datasource.
		/// </summary>
		/// <param name="datasource">The datasource.</param>
		/// <param name="script">The sql batch</param>
		/// <param name="doParse">parse out the statements in the sql script file.</param>
		protected static void InitScript(DataSource datasource, string script, bool doParse) {
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
		protected void AssertAccount1(Account account) 
		{
			Assert.AreEqual(1, account.Id, "account.Id");
			Assert.AreEqual("Joe", account.FirstName, "account.FirstName");
			Assert.AreEqual("Dalton", account.LastName, "account.LastName");
			Assert.AreEqual("Joe.Dalton@somewhere.com", account.EmailAddress, "account.EmailAddress");
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
			System.DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

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
			System.DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

			Assert.AreEqual(1,					(int)order["Id"], "order.Id");
			Assert.AreEqual(date.ToString(),	((DateTime)order["Date"]).ToString(), "order.Date");
			Assert.AreEqual("VISA",				(string)order["CardType"], "order.CardType");
			Assert.AreEqual("999999999999",		(string)order["CardNumber"], "order.CardNumber");
			Assert.AreEqual("05/03",			(string)order["CardExpiry"], "order.CardExpiry");
			Assert.AreEqual("11 This Street",	(string)order["Street"], "order.Street");
			Assert.AreEqual("Victoria",			(string)order["City"], "order.City");
			Assert.AreEqual("BC",				(string)order["Province"], "order.Id");
			Assert.AreEqual("C4B 4F4",			(string)order["PostalCode"], "order.PostalCode");
		}
	}
}
