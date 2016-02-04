using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;

namespace Xlns.Catalog.Document.Repository
{
    public class DocumentRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected virtual IDocumentSession OpenSession() 
        {
            return DataDocumentStore.Instance.OpenSession();
        }

        public void Save<T>(T document) 
        {
            using (IDocumentSession session = OpenSession())
            {                
                session.Store(document);
                session.SaveChanges();
            }
        }

        public T Load<T>(string Id)
        {
            using (IDocumentSession session = OpenSession())
            {
                return session.Load<T>(Id);                
            }
        }

        public IList<T> LoadMany<T>(int itemsNumber, int pageNumber)
        {
            using (IDocumentSession session = OpenSession())
            {
                return session.Query<T>()
                    .Skip(itemsNumber * (pageNumber-1))
                    .Take(itemsNumber)
                    .ToList();
            }
        }
    }

    public class StagingDocumentRepository : DocumentRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected override IDocumentSession OpenSession()
        {
            return StagingDocumentStore.Instance.OpenSession();
        }
    }

}
