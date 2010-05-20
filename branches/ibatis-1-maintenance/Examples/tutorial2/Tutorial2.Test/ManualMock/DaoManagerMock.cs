using System;
using System.Collections.Generic;
using System.Text;
using IBatisNet.Common;
using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Interfaces;

namespace Tutorial2.Test
{
    public class DaoManagerMock : IDaoManager
    {
        #region IDaoManager Members

        public IDalSession BeginTransaction()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IDalSession BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CloseConnection()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CommitTransaction()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IBatisNet.DataAccess.Interfaces.IDao GetDao(Type daoInterface)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DaoSession GetDaoSession()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsDaoSessionStarted()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IDalSession LocalDaoSession
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public DataSource LocalDataSource
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IDalSession OpenConnection()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IDalSession OpenConnection(string connectionString)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RollBackTransaction()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IDao this[Type daoInterface]
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
