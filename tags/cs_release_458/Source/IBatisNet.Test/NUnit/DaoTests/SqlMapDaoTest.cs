using System;
using System.IO;
using System.Reflection;
using System.Configuration;

using IBatisNet.DataAccess;

using IBatisNet.Test.NUnit;

using NUnit.Framework;

namespace IBatisNet.Test.NUnit.DaoTests
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
			DaoManager.Configure( "dao"+ "_" + ConfigurationSettings.AppSettings["database"] + "_"
				+ ConfigurationSettings.AppSettings["providerType"] + ".config" );
			daoManager = DaoManager.GetInstance("SqlMapDao");
			
			InitScript( daoManager.LocalDataSource, ScriptDirectory + "account-init.sql" );
		}


	}
}
