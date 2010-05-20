
using System;
using System.Collections;

using NPetshop.Domain.Catalog;

namespace NPetshop.Persistence.Interfaces.Catalog
{
	/// <summary>
	/// Summary description for ICategoryDao.
	/// </summary>
	public interface ICategoryDao
	{
		IList GetCategoryList();

		Category GetCategory(string categoryId);

	}
}
