
using System;
using System.Collections;

using NPetshop.Domain.Catalog;
using NPetshop.Domain.Billing;
using IBatisNet.Common.Pagination;

namespace NPetshop.Persistence.Interfaces.Billing
{
	/// <summary>
	/// Summary description for IOrderDao.
	/// </summary>
	public interface IOrderDao
	{
		IPaginatedList GetOrdersByUsername(string userName);

		Order GetOrder(int orderId);

		void InsertOrder(Order order);
	}
}
