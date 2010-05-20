using IBatisNet.Common.Pagination;
using NPetshop.Domain.Catalog;
using NPetshop.Service.Interfaces;

namespace NPetshop.Presentation
{
	/// <summary>
	/// Summary description for CatalogController.
	/// </summary>
	public class CatalogController : NPetshopController
	{
		private ICatalogService _catalogService = null;

		public CatalogController(ICatalogService catalogService)
		{
			_catalogService = catalogService;
		}

		public void ShowProductsByCategory(string categoryId)  
		{
			IPaginatedList productList = _catalogService.GetProductListByCategory(categoryId);

			this.NState.CurrentList = productList;
			this.Navigate(); 
		}

		public void ShowItemsByProduct(string productId)
		{
			Product product = _catalogService.GetProduct(productId);
			IPaginatedList itemList = _catalogService.GetItemListByProduct(product);

			this.NState.CurrentList = itemList;
			this.Navigate();
		}

		public void ShowItem(string itemId)
		{
			Item item = _catalogService.GetItem(itemId);
			this.NState.CurrentObject = item;
			this.Navigate();
		}

		public void SearchProducts(string keywords) 
		{
			IPaginatedList productList = null;

			productList = _catalogService.SearchProductList(keywords.ToLower());
			this.NState.CurrentList = productList;
			this.NState.CurrentObject = keywords;
			this.Navigate();
		}
	}
}
