using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper; // SqlMap API
using IBatisNet.DataMapper.Configuration;
using NUnit.Framework;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Description résumée de ConfigureTest.
	/// </summary>
	[TestFixture] 
	public class ConfigureTest : BaseTest
	{
		private string _fileName = string.Empty;

		#region SetUp

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			_fileName = "sqlmap" + "_" + ConfigurationSettings.AppSettings["database"] + "_" + ConfigurationSettings.AppSettings["providerType"] + ".config";
		}
		#endregion 

		#region Relatives Path tests

		/// <summary>
		/// Test Configure via relative path
		/// </summary>
		[Test] 
		public void TestConfigureRelativePath()
		{
			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.Configure( _fileName );
				//SqlMapper.Configure( _fileName );

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test ConfigureAndWatch via relative path
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchRelativePath()
		{
			ConfigureHandler handler = new ConfigureHandler(Configure);
			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.ConfigureAndWatch( _fileName , handler);

			//SqlMapper sqlMap = SqlMapper.ConfigureAndWatch( _fileName , handler);

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via relative path
		/// </summary>
		[Test] 
		public void TestConfigureRelativePathViaBuilder()
		{
			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.Configure( _fileName );

			Assert.IsNotNull(sqlMap);

		}

		/// <summary>
		/// Test ConfigureAndWatch via relative path
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchRelativePathViaBuilder()
		{
			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.ConfigureAndWatch( _fileName , handler);

			Assert.IsNotNull(sqlMap);
		}
		#endregion 

		#region Absolute Paths

		/// <summary>
		/// Test Configure via absolute path
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePath()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.Configure( _fileName );

			//SqlMapper sqlMap = SqlMapper.Configure( _fileName );

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via absolute path
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePathViaBuilder()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.Configure( _fileName );

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via absolute path with file suffix
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePathWithFileSuffix()
		{
			_fileName = "file://"+Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.Configure( _fileName );

			//SqlMapper sqlMap = SqlMapper.Configure( _fileName );

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via absolute path with file suffix
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePathWithFileSuffixViaBuilder()
		{
			_fileName = "file://"+Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.Configure( _fileName );

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via absolute path via FileIfno
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePathViaFileInfo()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			FileInfo fileInfo = new FileInfo(_fileName);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.Configure( fileInfo );

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via absolute path via Uri
		/// </summary>
		[Test] 
		public void TestConfigureAbsolutePathViaUri()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			Uri uri = new Uri(_fileName);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.Configure( uri );

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via absolute path
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchAbsolutePath()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.ConfigureAndWatch( _fileName , handler);

			//SqlMapper sqlMap = SqlMapper.ConfigureAndWatch( _fileName , handler);

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via absolute path
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchAbsolutePathViaBuilder()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.ConfigureAndWatch( _fileName , handler);

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via absolute path 
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchAbsolutePathWithFileSuffix()
		{
			_fileName = "file://"+Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.ConfigureAndWatch( _fileName , handler);

			//SqlMapper sqlMap = SqlMapper.ConfigureAndWatch( _fileName , handler);

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via absolute path 
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchAbsolutePathWithFileSuffixViaBuilder()
		{
			_fileName = "file://"+Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.ConfigureAndWatch( _fileName , handler);

			Assert.IsNotNull(sqlMap);
		}

		/// <summary>
		/// Test Configure via absolute path via FileInfo
		/// </summary>
		[Test] 
		public void TestConfigureAndWatchAbsolutePathViaFileInfo()
		{
			_fileName = Resources.BaseDirectory+Path.DirectorySeparatorChar+_fileName;
			FileInfo fileInfo = new FileInfo(_fileName);

			ConfigureHandler handler = new ConfigureHandler(Configure);

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.ConfigureAndWatch( fileInfo , handler);

			Assert.IsNotNull(sqlMap);
		}
		#endregion 

		#region Stream / Embedded 

		/// <summary>
		/// Test Configure via Stream/embedded
		/// </summary>
		[Test] 
		public void TestConfigureViaStream()
		{
			// embeddedResource = "bin.Debug.SqlMap_MSSQL_SqlClient.config, IBatisNet.DataMapper.Test";
			
			Assembly assembly = Assembly.LoadWithPartialName ("IBatisNet.DataMapper.Test");
			Stream stream = assembly.GetManifestResourceStream("IBatisNet.DataMapper.Test.bin.Debug.SqlMap_MSSQL_SqlClient.config");

			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			SqlMapper sqlMap = builder.Configure( stream );

			Assert.IsNotNull(sqlMap);
		}
		#endregion 


	}
}
