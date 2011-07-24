using System.Collections.Specialized;
using MyBatis.DataMapper;
using MyBatis.DataMapper.Configuration;
using MyBatis.DataMapper.Configuration.Interpreters.Config.Xml;
using MyBatis.DataMapper.Session;
using MyBatis.Common.Data;
using MyBatis.Common.Logging;
using MyBatis.Common.Logging.Impl;
using MyBatis.Common.Utilities;
using NUnit.Framework;

namespace MyBatis.DataMapper.Sqlite.Test.Fixtures
{
    [TestFixture]
    public abstract class BaseTest : ScriptBase
    {
        protected IDataMapper DataMapper;
        protected ISessionFactory SessionFactory;
        protected IConfigurationEngine ConfigurationEngine;

        [TestFixtureSetUp]
        protected virtual void TestFixtureSetUp()
        {
            LogManager.Adapter = new ConsoleOutLoggerFA(new NameValueCollection());

            ConfigurationEngine = new DefaultConfigurationEngine();
            ConfigurationEngine.RegisterInterpreter(new XmlConfigurationInterpreter("SqlMap.config"));

            IMapperFactory mapperFactory = ConfigurationEngine.BuildMapperFactory();
            SessionFactory = ConfigurationEngine.ModelStore.SessionFactory;
            DataMapper = ((IDataMapperAccessor)mapperFactory).DataMapper;
        }

        /// <summary>
        /// Dispose the SqlMap
        /// </summary>
        [TestFixtureTearDown]
        protected virtual void TestFixtureTearDown()
        {
            DataMapper = null;
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
    }
}
