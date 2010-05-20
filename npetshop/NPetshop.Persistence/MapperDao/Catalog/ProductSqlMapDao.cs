
using System;
using System.Collections;

using IBatisNet.Common.Pagination;
using IBatisNet.Common.Utilities;

using NPetshop.Domain.Catalog;
using NPetshop.Persistence.Interfaces.Catalog;
using NPetshop.Persistence.MapperDao;

namespace NPetshop.Persistence.MapperDao.Catalog
{
	/// <summary>
	/// Summary description for ProductSqlMapDao.
	/// </summary>
	public class ProductSqlMapDao : BaseSqlMapDao, IProductDao
	{

		#region IProductDao Members

		public IPaginatedList GetProductListByCategory(string categoryId)
		{
			return ExecuteQueryForPaginatedList("GetProductListByCategory", categoryId, PAGE_SIZE);
		}

		public Product GetProduct(string productId)
		{
			return (ExecuteQueryForObject("GetProduct", productId) as Product);
		}

		public IPaginatedList SearchProductList(string keywords)
		{
			object parameterObject = new ProductSearch(keywords);
			return ExecuteQueryForPaginatedList("SearchProductList", parameterObject, PAGE_SIZE);
		}

		#endregion

		#region Inner Classes

		public class ProductSearch 
		{
			private IList keywordList = new ArrayList();

			public ProductSearch(String keywords) 
			{
				StringTokenizer splitter = new StringTokenizer(keywords, " ", false);
				string token = null;

				IEnumerator enumerator = splitter.GetEnumerator();

				while (enumerator.MoveNext()) 
				{
					token = (string)enumerator.Current;
					keywordList.Add("%" + token + "%");
				}
			}

			public IList KeywordList 
			{
				get
				{
					return keywordList;
				}
			}
		}

		#endregion
	}
}
