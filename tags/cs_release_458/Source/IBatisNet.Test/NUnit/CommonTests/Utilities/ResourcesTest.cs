using System;
using System.Xml;

using IBatisNet.Common.Utilities;

using NUnit.Framework;

namespace IBatisNet.Test.NUnit.CommonTests.Utilities
{
	/// <summary>
	/// Description résumée de ResourcesTest.
	/// </summary>
	[TestFixture] 
	public class ResourcesTest
	{
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{
		}


		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ 
		} 

		#endregion

		#region Test ResourcesTest

		/// <summary>
		/// Test loading Embeded Resource
		/// </summary>
		[Test] 
		public void TestEmbededResource() 
		{
			XmlDocument doc = null;

			doc = Resources.GetEmbeddedResourceAsXmlDocument("properties.xml, IBatisNet.Test");

			Assert.IsNotNull(doc);
			Assert.IsTrue(doc.HasChildNodes);
			Assert.AreEqual(doc.ChildNodes.Count,2);
			Assert.AreEqual(doc.SelectNodes("/settings/add").Count, 4);
		}

		#endregion

	}
}
