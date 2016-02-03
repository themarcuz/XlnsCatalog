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

        public void Save<T>(T document) 
        {
            using (IDocumentSession session = DataDocumentStore.Instance.OpenSession())
            {                
                session.Store(document);
                session.SaveChanges();
            }
        }

        public T Load<T>(string Id)
        {
            using (IDocumentSession session = DataDocumentStore.Instance.OpenSession())
            {
                return session.Load<T>(Id);                
            }
        }
    }
}
