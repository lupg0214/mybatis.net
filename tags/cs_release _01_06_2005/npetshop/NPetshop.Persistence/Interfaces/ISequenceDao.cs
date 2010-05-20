
using System;


namespace NPetshop.Persistence.Interfaces
{
	/// <summary>
	/// Summary description for ISequenceDao.
	/// </summary>
	public interface ISequenceDao
	{
		int GetNextId(string name);
	}
}
