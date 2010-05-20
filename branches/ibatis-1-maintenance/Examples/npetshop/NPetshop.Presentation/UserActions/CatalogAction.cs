
using System;
using System.Collections;
using System.Web;

using IBatisNet.Common.Pagination;
using NPetshop.Service;

using NPetshop.Domain.Catalog;
using NPetshop.Presentation.Core;

namespace NPetshop.Presentation.UserActions
{
	/// <summary>
	/// Summary description for CatalogAction.
	/// </summary>
	public class CatalogAction : AbstractWebAction
	{
		private Category _category =null;
		private static readonly CatalogService _catalogService = CatalogService.GetInstance();

		public CatalogAction(HttpContext context) : base(context) 
		{
			_category = new Category();
		}

		public void ShowProductsByCategory(string categoryId)  
		{
			IPaginatedList productList = null;

			productList = _catalogService.GetProductListByCategory(categoryId);

			this.singleton.CurrentList = productList;
			this.nextViewToDisplay = WebViews.PRODUCTS_BY_CATEGORY;
		}

		public void ShowItemsByProduct(string productId)
		{
			Product product = null;
			IPaginatedList itemList = null;

			product = _catalogService.GetProduct(productId);
			data.Add(DataViews.PRODUCT, product);
			itemList = _catalogService.GetItemListByProduct(productId);
			foreach(Item item in itemList)
			{
				item.Product = product;
			}
			this.singleton.CurrentList = itemList;

			this.nextViewToDisplay = WebViews.ITEMS_BY_PRODUCT;
		}

		public void ShowItem(string itemId)
		{
			Item item = null;

			item = _catalogService.GetItem(itemId);
			data.Add(DataViews.ITEM, item);

			this.nextViewToDisplay = WebViews.ITEM;
		}

		public void SearchProducts(string keywords) 
		{
			IPaginatedList productList = null;

			productList = _catalogService.SearchProductList(keywords.ToLower());
			this.singleton.CurrentList = productList;
			data.Add(DataViews.SEARCH_KEYWORDS, keywords);

			nextViewToDisplay = WebViews.SEARCH_PRODUCTS;	
		}
	}
}
