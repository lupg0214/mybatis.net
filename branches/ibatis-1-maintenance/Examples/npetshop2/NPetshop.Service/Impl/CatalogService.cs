

using System.Collections;
using IBatisNet.Common.Pagination;
using IBatisNet.DataAccess;
using NPetshop.Domain.Catalog;
using NPetshop.Persistence.Interfaces.Catalog;
using NPetshop.Service.Interfaces;

namespace NPetshop.Service.Impl
{
	/// <summary>
	/// Summary description for CatalogService.
	/// </summary>
	public class CatalogService : ICatalogService
	{
		#region Private Fields 
		private DaoManager _daoManager = null;
		private IItemDao _itemDao = null;
		private IProductDao _productDao = null;
		private ICategoryDao _categoryDao = null;
		#endregion

		#region Constructor
		public CatalogService(DaoManager daoManager) 
		{
			_daoManager = daoManager;
			_categoryDao = _daoManager[typeof(ICategoryDao)] as ICategoryDao;
			_productDao = _daoManager[typeof(IProductDao)] as IProductDao;
			_itemDao = _daoManager[typeof(IItemDao)] as IItemDao;
		}
		#endregion

		#region Public methods

		#region Item
		public IPaginatedList GetItemListByProduct(Product product) 
		{
			IPaginatedList itemList = null;

			itemList = _itemDao.GetItemListByProduct(product.Id);
			foreach(Item item in itemList)
			{
				item.Product = product;
			}

			return itemList;
		}

		public Item GetItem(string itemId) 
		{
			Item item = null; 

			item = _itemDao.GetItem(itemId);

			return item;
		}

		public bool IsItemInStock(string itemId) 
		{
			return _itemDao.IsItemInStock(itemId);
		}
		#endregion

		#region Product
		public Product GetProduct(string productId) 
		{
			Product product = null; 

			product = _productDao.GetProduct(productId);

			return product;
		}

		public IPaginatedList GetProductListByCategory(string categoryId) 
		{
			IPaginatedList productList = null;

			productList = _productDao.GetProductListByCategory(categoryId);

			return productList;
		}

		public IPaginatedList SearchProductList(string keywords) 
		{
			IPaginatedList productList = null;

			productList = _productDao.SearchProductList(keywords);

			return productList;
		}
		#endregion

		#region Category
		public IList GetCategoryList() 
		{
			return _categoryDao.GetCategoryList();
		}

		public Category GetCategory(string categoryId) 
		{
			Category category = null; 

			category =  _categoryDao.GetCategory(categoryId);

			return category;
		}
		#endregion
		
		#endregion

	}
}
