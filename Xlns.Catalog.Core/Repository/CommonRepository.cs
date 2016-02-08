using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;

namespace Xlns.Catalog.Core.Repository
{
    public class CommonRepository : IRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IQueryable<T> GetAll<T>() where T : Model.ModelEntity
        {
            using (var om = new OperationManager())
            {
                try
                {
                    var session = om.BeginOperation();
                    var items = session.Query<T>();
                    om.CommitOperation();
                    return items;
                }
                catch (Exception ex)
                {
                    om.RollbackOperation();
                    logger.Error(ex, "Error accessing entity " + typeof(T).ToString());
                    return null;
                }
            }
        }

        public int SaveUpdate<T>(T domainModelObject) where T : Model.ModelEntity
        {
            using (var om = new OperationManager())
            {
                try
                {
                    var session = om.BeginOperation();
                    session.SaveOrUpdate(domainModelObject);
                    om.CommitOperation();
                    logger.Info("Saved " + domainModelObject.GetType().ToString()
                        + " with Id = " + domainModelObject.Id);
                }
                catch (Exception ex)
                {
                    om.RollbackOperation();
                    logger.Error(ex, "Error saving " + domainModelObject.GetType().ToString() + " with Id = " + domainModelObject.Id);
                    throw;
                }
                return domainModelObject.Id;
            }
        }

        public void Delete<T>(T domainModelObject) where T : Model.ModelEntity
        {
            using (var om = new OperationManager())
            {
                try
                {
                    var session = om.BeginOperation();
                    session.Delete(domainModelObject);                    
                    logger.Info("Deleted " + domainModelObject.GetType().ToString()
                        + " with Id = " + domainModelObject.Id);
                    om.CommitOperation();
                }
                catch (Exception ex)
                {
                    om.RollbackOperation();
                    string msg = string.Format("Error deleting {0} with Id={1}",
                        domainModelObject.GetType().ToString(), domainModelObject.Id);
                    logger.Error(ex, msg);
                    throw new Exception(msg, ex);
                }
            }
        }
    }
}
