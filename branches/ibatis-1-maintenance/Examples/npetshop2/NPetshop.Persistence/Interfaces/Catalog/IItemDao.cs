
using System;

using NPetshop.Domain.Catalog;
using NPetshop.Domain.Billing;
using IBatisNet.Common.Pagination;

namespace NPetshop.Persistence.Interfaces.Catalog
{
	/// <summary>
	/// Summary description for IItemDao.
	/// </summary>
	public interface IItemDao
	{
		void UpdateQuantity(Order order);

		bool IsItemInStock(string itemId);

		IPaginatedList GetItemListByProduct(string productId);

		Item GetItem(string itemId);

	}
}
