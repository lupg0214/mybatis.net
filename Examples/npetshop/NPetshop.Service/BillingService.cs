
using System;
using System.Collections;

using IBatisNet.Common.Pagination;
using IBatisNet.DataAccess;

using NPetshop.Domain;
using NPetshop.Domain.Billing;
using NPetshop.Domain.Catalog;

using NPetshop.Service;

using NPetshop.Persistence.Interfaces;
using NPetshop.Persistence.Interfaces.Billing;
using NPetshop.Persistence.Interfaces.Catalog;

namespace NPetshop.Service
{
	/// <summary>
	/// Summary description for OrderService.
	/// </summary>
	public class BillingService
	{
		#region Private Fields 
		private static BillingService _instance = new BillingService();
		private DaoManager _daoManager = ServiceConfig.GetInstance().DaoManager;
		private IOrderDao _orderDao = null;
		private IItemDao _itemDao = null;
		private ISequenceDao _sequenceDao = null;
		#endregion

		#region Constructor
		public BillingService() 
		{
			_itemDao = _daoManager[typeof(IItemDao)] as IItemDao;
			_orderDao = _daoManager[typeof(IOrderDao)] as IOrderDao;
			_sequenceDao = _daoManager[typeof(ISequenceDao)] as ISequenceDao;

		}
		#endregion

		#region Public methods

		public static BillingService GetInstance() 
		{
			return _instance;
		}


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
