using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Diagnostics;

namespace Xlns.Catalog.Core.Repository
{
    
    public class OperationManager : IDisposable
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        ITransaction tx = null;
        ISession session = null;
        bool isInternalTransaction = false;

        public ISession BeginOperation()
        {
            try
            {
                session = PersistenceManager.Istance.GetSession();
                if (session.Transaction.IsActive)
                {
                    isInternalTransaction = false;
                    tx = session.Transaction;
                    logger.Trace(GetCallerClassDotMethod() + " enrolled in transaction " + tx.GetHashCode());
                }
                else
                {
                    isInternalTransaction = true;
                    tx = session.Transaction;
                    tx.Begin();
                    logger.Debug("Transaction " + tx.GetHashCode() + " created by " + GetCallerClassDotMethod());
                }
                logger.Trace("Session: " + session.GetHashCode());
                return session;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw ex;
            }
        }

        private String GetCallerClassDotMethod() {
            // serve ad intercettare il chiamante per loggare chi sta agendo sulla transazione
            var st = new StackTrace();
            var sf = st.GetFrame(2);
            var methodReference = sf.GetMethod().Name;
            var classReference = sf.GetMethod().DeclaringType.FullName;
            return string.Concat(classReference, ".", methodReference);
        }

        public void CommitOperation()
        {
            if (isInternalTransaction)
            {
                try
                {
                    tx.Commit();
                    logger.Trace(GetCallerClassDotMethod() + " commit transaction " + tx.GetHashCode());
                }
                catch (Exception ex)
                {
                    string msg = "Error committing transaction {0}";
                    logger.Error(ex, msg, tx == null ? "NULL" : tx.GetHashCode().ToString());
                    throw new Exception(msg, ex);
                }
                
            }
        }

        public void RollbackOperation()
        {
            if (!this.isInternalTransaction)
            {
                return;
            }
            this.tx.Rollback();
            logger.Trace(GetCallerClassDotMethod() + " rollback transaction " + this.tx.GetHashCode());
        }

        public void Dispose()
        {
            if (!this.isInternalTransaction)
            {
                return;
            }
            if (this.tx != null)
            {
                if (this.tx.IsActive) this.tx.Rollback();
                this.tx.Dispose();
            }
            PersistenceManager.Istance.Close();
        }
    }
}
