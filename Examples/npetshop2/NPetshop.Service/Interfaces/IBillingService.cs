using IBatisNet.Common.Pagination;
using NPetshop.Domain.Billing;

namespace NPetshop.Service.Interfaces
{
	public interface IBillingService
	{
		void InsertOrder(Order order);
		IPaginatedList GetOrdersByUsername(string userName);
		int GetNextId(string key);
	}
}