using System.Configuration;
using IBatisNet.DataAccess.Configuration;
using NUnit.Framework;

namespace IBatisNet.DataAccess.Test.NUnit.DaoTests
{
	/// <summary>
	/// Summary description for SqlMapDaoTesto.
	/// </summary>
	public class SqlMapDaoTest : BaseDaoTest
	{
		/// <summary>
		/// Initialisation
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{
			DomDaoManagerBuilder builder = new DomDaoManagerBuilder();
			builder.Configure( "dao"+ "_" + ConfigurationSettings.AppSettings["database"] + "_"
				+ ConfigurationSettings.AppSettings["providerType"] + ".config" );
			daoManager = DaoManager.GetInstance("SqlMapDao");
			
			InitScript( daoManager.LocalDataSource, ScriptDirectory + "account-init.sql" );
		}


	}
}
