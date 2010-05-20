using System;
using NUnit.Framework;
using NPetshop.Presentation;
using NPetshop.Service;

namespace NPetshop.Test.Presentation
{
	/// <summary>
	/// Summary description for ControllerTest.
	/// </summary>
	[TestFixture] 
	public class ControllerTest
	{
		private NPetshopContainer _container;
		private CatalogController _catalogController = null;
		private NPetshopState _state = null;

		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{
			_container = null;
			_catalogController = null;
			_state = null;

			_container = new NPetshopContainer();

			_catalogController = _container[typeof(CatalogController)] as CatalogController;
			_state = _catalogController.State as NPetshopState;
		}
 


		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ 
			_container.Dispose();
		} 

		#endregion

		#region Test Controller

		/// <summary>
		/// Test Container
		/// </summary>
		[Test] 
		public void TestContainer() 
		{
			object controller = _container.Resolve("BillingController");
			Assert.IsTrue(controller.GetType()==typeof(BillingController));
		}

		/// <summary>
		/// Test catalog browsing
		/// </summary>
		[Test] 
		public void TestCatalogController() 
		{
			_state.CurrentView = "no-case";
			_state.Command = "showCategory";
			_catalogController.ShowProductsByCategory("FISH");

			Assert.IsTrue(_state.PreviousView=="no-case");
			Assert.IsTrue(_state.CurrentView=="Category");
			Assert.IsTrue(_state.CurrentList.Count==4);
		}

		#endregion
	}
}
