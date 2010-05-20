
using System;
using System.Collections;

using NPetshop.Domain.Catalog;
using NPetshop.Persistence.Interfaces.Catalog;
using NPetshop.Persistence.MapperDao;

namespace NPetshop.Persistence.MapperDao.Catalog
{
	/// <summary>
	/// Summary description for CategorySqlMapDao.
	/// </summary>
	public class CategorySqlMapDao : BaseSqlMapDao, ICategoryDao
	{

		#region ICategoryDao Members

		public IList GetCategoryList()
		{
			return ExecuteQueryForList("GetCategoryList", null);
		}

		public Category GetCategory(string categoryId)
		{
			return (Category) ExecuteQueryForObject("GetCategory", categoryId);
		}

		#endregion
	}
}
