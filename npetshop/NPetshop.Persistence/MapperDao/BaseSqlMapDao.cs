
using System;
using System.Collections;

using IBatisNet.Common;
using IBatisNet.Common.Pagination;
using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Exceptions;
using IBatisNet.DataAccess.DaoSessionHandlers;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataMapper;

namespace NPetshop.Persistence.MapperDao
{
	/// <summary>
	/// Summary description for BaseSqlMapDao.
	/// </summary>
	public class BaseSqlMapDao : IDao
	{
		protected const int PAGE_SIZE = 4;

		/// <summary>
		/// Looks up the parent DaoManager, gets the local transaction
		/// (which should be a SqlMapDaoTransaction) and returns the
		/// SqlMap associated with this DAO.
		/// </summary>
		/// <returns>The SqlMap instance for this DAO.</returns>
		protected SqlMapper GetLocalSqlMap()
		{
			DaoManager daoManager = DaoManager.GetInstance(this);
			SqlMapDaoSession sqlMapDaoSession = (SqlMapDaoSession)daoManager.LocalDaoSession;

			return sqlMapDaoSession.SqlMap;
		}


		/// <summary>
		/// Simple convenience method to wrap the SqlMap method of the same name.
		/// Wraps the exception with a DataAccessException to isolate the SqlMap framework.
		/// </summary>
		/// <param name="statementName"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		protected IList ExecuteQueryForList(string statementName, object parameterObject)
		{
			SqlMapper sqlMap = GetLocalSqlMap();
			try 
			{
				return sqlMap.QueryForList(statementName, parameterObject);
			} 
			catch (Exception e) 
			{
				throw new DataAccessException("Error executing query '"+statementName+"' for list.  Cause: " + e.Message, e);
			}
		}

		/// <summary>
		/// Simple convenience method to wrap the SqlMap method of the same name.
		/// Wraps the exception with a DataAccessException to isolate the SqlMap framework.
		/// </summary>
		/// <param name="statementName"></param>
		/// <param name="parameterObject"></param>
		/// <param name="skipResults"></param>
		/// <param name="maxResults"></param>
		/// <returns></returns>
		protected IList ExecuteQueryForList(string statementName, object parameterObject, int skipResults, int maxResults) 
		{
			SqlMapper sqlMap = GetLocalSqlMap();
			try 
			{
				return sqlMap.QueryForList(statementName, parameterObject, skipResults, maxResults);
			} 
			catch (Exception e) 
			{
				throw new DataAccessException("Error executing query '"+statementName+"' for list.  Cause: " + e.Message, e);
			}
		}

		/// <summary>
		/// Simple convenience method to wrap the SqlMap method of the same name.
		/// Wraps the exception with a DataAccessException to isolate the SqlMap framework.
		/// </summary>
		/// <param name="statementName"></param>
		/// <param name="parameterObject"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		protected IPaginatedList ExecuteQueryForPaginatedList(string statementName, object parameterObject, int pageSize) 
		{
			SqlMapper sqlMap = GetLocalSqlMap();
			try 
			{
				return sqlMap.QueryForPaginatedList(statementName, parameterObject, pageSize);
			} 
			catch (Exception e) 
			{
				throw new DataAccessException("Error executing query '"+statementName+"' for paginated list.  Cause: " + e.Message, e);
			}
		}

		/// <summary>
		/// Simple convenience method to wrap the SqlMap method of the same name.
		/// Wraps the exception with a DataAccessException to isolate the SqlMap framework.
		/// </summary>
		/// <param name="statementName"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		protected object ExecuteQueryForObject(string statementName, object parameterObject) 
		{
			SqlMapper sqlMap = GetLocalSqlMap();

			try 
			{
				return sqlMap.QueryForObject(statementName, parameterObject);
			} 
			catch (Exception e) 
			{
				throw new DataAccessException("Error executing query '"+statementName+"' for object.  Cause: " + e.Message, e);
			}
		}

		/// <summary>
		/// Simple convenience method to wrap the SqlMap method of the same name.
		/// Wraps the exception with a DataAccessException to isolate the SqlMap framework.
		/// </summary>
		/// <param name="statementName"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		protected int ExecuteUpdate(string statementName, object parameterObject) 
		{
			SqlMapper sqlMap = GetLocalSqlMap();

			try 
			{
				return sqlMap.Update(statementName, parameterObject);
			} 
			catch (Exception e) 
			{
				throw new DataAccessException("Error executing query '"+statementName+"' for update.  Cause: " + e.Message, e);
			}
		}

		/// <summary>
		/// Simple convenience method to wrap the SqlMap method of the same name.
		/// Wraps the exception with a DataAccessException to isolate the SqlMap framework.
		/// </summary>
		/// <param name="statementName"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		protected object ExecuteInsert(string statementName, object parameterObject) 
		{
			SqlMapper sqlMap = GetLocalSqlMap();

			try 
			{
				return sqlMap.Insert(statementName, parameterObject);
			} 
			catch (Exception e) 
			{
				throw new DataAccessException("Error executing query '"+statementName+"' for insert.  Cause: " + e.Message, e);
			}
		}
	}
}
