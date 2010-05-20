
using System;

using NPetshop.Domain;
using NPetshop.Persistence.Interfaces;
using IBatisNet.DataMapper.Exceptions;

namespace NPetshop.Persistence.MapperDao.Billing.MSSQL
{
	/// <summary>
	/// Summary description for SequenceSqlMapDao.
	/// </summary>
	public class SequenceSqlMapDao : BaseSqlMapDao, ISequenceDao
	{

		#region ISequenceDao Members

		/// <summary>
		/// This is a generic sequence ID generator that is based on a database
		/// table called 'SEQUENCE', which contains two columns (NAME, NEXTID).
		/// </summary>
		/// <param name="name">name The name of the sequence.</param>
		/// <returns>The Next ID</returns>
		/// <remarks>
		/// Dummy version for SQL Server. Actual orderId
		/// won't be known until we acutlaly insert!
		/// </remarks>
		public int GetNextId(string name)
		{
			return -1;
		}

		#endregion
	}
}
