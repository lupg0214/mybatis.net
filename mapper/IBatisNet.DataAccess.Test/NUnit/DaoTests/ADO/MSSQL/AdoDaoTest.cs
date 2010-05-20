using System.Configuration;
using IBatisNet.DataAccess.Configuration;
using NUnit.Framework;

namespace IBatisNet.DataAccess.Test.NUnit.DaoTests.Ado.MSSQL
{
	/// <summary>
	/// Summary description for AdoDaoTest.
	/// </summary>
	[Category("MSSQL")]
	public class AdoDaoTest : BaseDaoTest
	{
		/// <summary>
		/// Initialisation
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{
			DomDaoManagerBuilder builder = new DomDaoManagerBuilder();
			builder.Configure( "dao_MSSQL_"
				 + ConfigurationSettings.AppSettings["providerType"] + ".config" );
			daoManager = DaoManager.GetInstance();

			InitScript( daoManager.LocalDataSource, ScriptDirectory + "account-init.sql" );
		}

	}
}
