

using IBatisNet.Common.Pagination;
using IBatisNet.DataAccess;
using NPetshop.Domain.Billing;
using NPetshop.Persistence.Interfaces;
using NPetshop.Persistence.Interfaces.Billing;
using NPetshop.Persistence.Interfaces.Catalog;
using NPetshop.Service.Interfaces;

namespace NPetshop.Service.Impl
{
	/// <summary>
	/// Summary description for OrderService.
	/// </summary>
	public class BillingService : IBillingService
	{
		#region Private Fields 
		private DaoManager _daoManager = null;
		private IOrderDao _orderDao = null;
		private IItemDao _itemDao = null;
		private ISequenceDao _sequenceDao = null;
		#endregion

		#region Constructor
		public BillingService(DaoManager daoManager) 
		{
			_daoManager = daoManager;
			_itemDao = _daoManager[typeof(IItemDao)] as IItemDao;
			_orderDao = _daoManager[typeof(IOrderDao)] as IOrderDao;
			_sequenceDao = _daoManager[typeof(ISequenceDao)] as ISequenceDao;

		}
		#endregion

		#region Public methods


		#region Order

		public void InsertOrder(Order order) 
		{
			// Get the next id within a separate transaction
			order.Id = GetNextId("OrderNum");

			_daoManager.BeginTransaction();
			try 
			{
				_itemDao.UpdateQuantity(order);
				_orderDao.InsertOrder(order);

				_daoManager.CommitTransaction();
			} 
			catch
			{
				_daoManager.RollBackTransaction();
				throw;
			}
		}

		public IPaginatedList GetOrdersByUsername(string userName) 
		{
			return _orderDao.GetOrdersByUsername(userName);
		}

		#endregion

		#region Sequence
		public int GetNextId(string key) 
		{
			int id = -1;

			_daoManager.BeginTransaction();
			try 
			{
				id = _sequenceDao.GetNextId(key);
				_daoManager.CommitTransaction();
			} 
			catch
			{
				_daoManager.RollBackTransaction();
				throw;
			}

			return id;
		}
		#endregion

		#endregion
	}
}
