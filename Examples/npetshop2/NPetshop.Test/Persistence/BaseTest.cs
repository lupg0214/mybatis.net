using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Configuration;
using log4net;
using log4net.Config;
using NUnit.Framework;

[assembly : DOMConfigurator(Watch=true)]

namespace NPetshop.Test.Persistence
{
	/// <summary>
	/// Description résumée de BaseTest.
	/// </summary>
	[TestFixture] 
	public abstract class BaseTest
	{
		protected DaoManager daoManager = null;
		protected readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		[SetUp] 
		public void SetUp() 
		{
			DomDaoManagerBuilder builder = new DomDaoManagerBuilder();
			builder.Configure(@"..\..\..\NPetshop.Persistence\dao.config");

			daoManager = DaoManager.GetInstance("SqlMapDao");
		}

	}
}
