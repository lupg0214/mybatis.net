using System.Collections;
using IBatisNet.Common.Pagination;
using NPetshop.Domain.Catalog;

namespace NPetshop.Service.Interfaces
{
	public interface ICatalogService
	{
		IPaginatedList GetItemListByProduct(Product product);
		Item GetItem(string itemId);
		bool IsItemInStock(string itemId);
		Product GetProduct(string productId);
		IPaginatedList GetProductListByCategory(string categoryId);
		IPaginatedList SearchProductList(string keywords);
		IList GetCategoryList();
		Category GetCategory(string categoryId);
	}
}