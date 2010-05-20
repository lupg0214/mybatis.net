using NUnit.Extensions.Asp;
using NUnit.Extensions.Asp.AspTester;
using NUnit.Framework;

namespace NPetshop.Test.Web
{
	/// <summary>
	/// Description résumée de WebFormTest.
	/// </summary>
	[TestFixture] 
	public class WebFormTest : WebFormTestCase 
	{
		[Test] 
		public void TestGotToCatalog() 
		{ 
			// First, instantiate "Tester" objects: 
			UserControlTester ucStartUp = new UserControlTester("StartUp", CurrentWebForm);
			UserControlTester ucSideBar = new UserControlTester("SideBar", ucStartUp);
			LinkButtonTester linkbuttonFish = new LinkButtonTester("LinkbuttonFish", ucSideBar);
 
			UserControlTester ucCategory= new UserControlTester("Category", CurrentWebForm);
			LabelTester labelCategory = new LabelTester("LabelCategory",ucCategory);

			// Second, visit the page being tested: 
			Browser.GetPage("http://localhost/NPetshop.Web/Views/default.aspx"); 
			string homePage = this.Browser.CurrentUrl.AbsoluteUri.ToString();
			linkbuttonFish.Click(); 

			// First, test
			string catalogPage = this.Browser.CurrentUrl.AbsoluteUri.ToString();
			Assert(catalogPage, homePage != catalogPage);
			AssertEquals("Fish", labelCategory.Text); 
		} 
	}
}
