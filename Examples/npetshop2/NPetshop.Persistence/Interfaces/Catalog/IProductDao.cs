
using System;

using NPetshop.Domain.Catalog;
using IBatisNet.Common.Pagination;

namespace NPetshop.Persistence.Interfaces.Catalog
{
	/// <summary>
	/// Summary description for IProductDao.
	/// </summary>
	public interface IProductDao
	{
		IPaginatedList GetProductListByCategory(string categoryId);

		Product GetProduct(string productId);

		IPaginatedList SearchProductList(string keywords);
	}
}
