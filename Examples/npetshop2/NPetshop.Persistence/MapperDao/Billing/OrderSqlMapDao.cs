
using System;

using IBatisNet.Common.Pagination;

using NPetshop.Domain.Billing;
using NPetshop.Persistence.MapperDao;
using NPetshop.Persistence.Interfaces.Billing;

namespace NPetshop.Persistence.MapperDao.Billing
{
	/// <summary>
	/// Summary description for OrderSqlMapDao.
	/// </summary>
	public class OrderSqlMapDao : BaseSqlMapDao, IOrderDao 
	{
		#region IOrderDao Members

		public IPaginatedList GetOrdersByUsername(string userName)
		{
			return ExecuteQueryForPaginatedList("GetOrdersByUsername", userName, 10);
		}

		public Order GetOrder(int orderId)
		{
			Order order = null;
			order = ExecuteQueryForObject("GetOrder", orderId) as Order;
			order.LineItems = ExecuteQueryForList("GetLineItemsByOrderId", order.Id);
			return order;
		}

		public void InsertOrder(Order order)
		{
			ExecuteInsert("InsertOrder", order);
			foreach(LineItem lineItem in order.LineItems) 
			{
				lineItem.Order = order;
				ExecuteInsert("InsertLineItem", lineItem);		
			}
		}

		#endregion
	}
}
