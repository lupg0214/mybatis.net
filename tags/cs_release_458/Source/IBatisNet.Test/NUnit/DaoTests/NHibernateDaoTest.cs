using System;
using System.IO;
using System.Configuration;

using IBatisNet.DataAccess;
using IBatisNet.Common; // DataSource definition
using IBatisNet.Common.Utilities; // ScriptRunner definition

using IBatisNet.Test.Dao.Interfaces;

using IBatisNet.Test.NUnit;
using IBatisNet.Test.Domain;

using NUnit.Framework;

namespace IBatisNet.Test.NUnit.DaoTests
{
	/// <summary>
	/// Summary description for NHibernateDaoTest.
	/// </summary>
	[TestFixture] 
	public class NHibernateDaoTest
	{
		/// <summary>
		/// A daoManager
		/// </summary>
		private static  DaoManager _daoManager = null;

		/// <summary>
		/// Initialisation
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{
			string scriptDirectory = Resources.RootDirectory + Path.DirectorySeparatorChar +
				"Scripts" + Path.DirectorySeparatorChar +
				ConfigurationSettings.AppSettings["database"]+ Path.DirectorySeparatorChar;

			DaoManager.Configure( "dao"+ "_" + ConfigurationSettings.AppSettings["database"] + "_"
				+ ConfigurationSettings.AppSettings["providerType"] + ".config" );

			_daoManager = DaoManager.GetInstance("NHibernateDao");
			
			InitScript( _daoManager.LocalDataSource, scriptDirectory + "user-init.sql" );
		}

		/// <summary>
		/// Run a sql batch for the datasource.
		/// </summary>
		/// <param name="datasource">The datasource.</param>
		/// <param name="script">The sql batch</param>
		protected static void InitScript(DataSource datasource, string script)
		{
			ScriptRunner runner = new ScriptRunner();

			runner.RunScript(datasource, script);
		}

		/// <summary>
		/// Test Create user
		/// </summary>
		[Test] 
		public void TestCreateUser () 
		{
			IUserDao userDao = (IUserDao)_daoManager[typeof(IUserDao)];

			User newUser = new User();
			newUser.Id = "joe_cool";
			newUser.UserName = "Joseph Cool";
			newUser.Password = "abc123";
			newUser.EmailAddress = "joe@cool.com";
			newUser.LastLogon = DateTime.Now;

			try
			{
				_daoManager.OpenConnection();
				userDao.Create(newUser);
			}
			catch(Exception e)
			{
				// Ignore
				Console.WriteLine("TestCreateUser, error cause : "+e.Message);
			}
			finally
			{
				_daoManager.CloseConnection();
			}

			DateTime stamp = DateTime.Now;
			User joeCool = null;
			try
			{
				// open another session to retrieve the just inserted user
				_daoManager.OpenConnection();

				//The User object you get back is live! 
				joeCool = userDao.Load("joe_cool");

				Assert.IsNotNull(joeCool);
				Assert.AreEqual("Joseph Cool", joeCool.UserName);
			
				//Change its properties and it will get persisted to the database on Close. 
				// set Joe Cool's Last Login property
				joeCool.LastLogon = stamp;			
			}
			catch(Exception e)
			{
				// Ignore
				Console.WriteLine("TestCreateUser, error cause : "+e.Message);
			}
			finally
			{
				// flush the changes from the Session to the Database
				_daoManager.CloseConnection();
			}

			_daoManager.OpenConnection();
			//The User object you get back is live! 
			joeCool = userDao.Load("joe_cool");
			_daoManager.CloseConnection();

			Assert.IsNotNull(joeCool);
			Assert.AreEqual("Joseph Cool", joeCool.UserName);
			Assert.AreEqual(stamp.ToString(), joeCool.LastLogon.ToString());
		}

	}
}
