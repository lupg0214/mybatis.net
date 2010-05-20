
using System;
using System.Collections;

using IBatisNet.Common.Pagination;

using NPetshop.Domain.Catalog;
using NPetshop.Domain.Billing;
using NPetshop.Persistence.Interfaces.Catalog;
using NPetshop.Persistence.MapperDao;

namespace NPetshop.Persistence.MapperDao.Catalog
{
	/// <summary>
	/// Summary description for ItemSqlMapDao.
	/// </summary>
	public class ItemSqlMapDao : BaseSqlMapDao, IItemDao
	{
		#region IItemDao Members

		public void UpdateQuantity(Order order)
		{
			foreach(LineItem lineItem in order.LineItems) 
			{
				string itemId = lineItem.Item.Id;
				int increment = lineItem.Quantity;

				Hashtable param = new Hashtable();
				param.Add("ItemId", itemId);
				param.Add("Increment", increment);

				ExecuteUpdate("UpdateInventoryQuantity", param);
			}
		}

		public bool IsItemInStock(string itemId)
		{
			int i = (int)ExecuteQueryForObject("GetInventoryQuantity", itemId);
			return (i > 0);
		}

		public IPaginatedList GetItemListByProduct(string productId)
		{
			return ExecuteQueryForPaginatedList("GetItemListByProduct", productId, PAGE_SIZE);
		}

		public Item GetItem(string itemId)
		{
			int inventoryQuantity = (int) ExecuteQueryForObject("GetInventoryQuantity", itemId);
			Item item = (Item) ExecuteQueryForObject("GetItem", itemId);
			item.Quantity = inventoryQuantity;
			return item;
		}

		#endregion

	}
}
