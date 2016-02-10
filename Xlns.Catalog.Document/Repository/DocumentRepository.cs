using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;

namespace Xlns.Catalog.Document.Repository
{
    public class DocumentRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        protected string _countryId;

        public DocumentRepository(string countryId)
        {
            _countryId = countryId;
        }

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

        public StagingDocumentRepository(string countryId) : base(countryId) 
        {}

        protected override IDocumentSession OpenSession()
        {
            return StagingDocumentStore.Instance.OpenSession(
                new Raven.Client.Document.OpenSessionOptions
                {
                    Database = ConfigurationManager.AppSettings["StagingRavenDbName"].Replace("{countryId}", _countryId)
                });
        }
    }

    public class CatalogDocumentRepository : DocumentRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public CatalogDocumentRepository(string countryId) : base(countryId)
        { }

        protected override IDocumentSession OpenSession()
        {
            return DataDocumentStore.Instance.OpenSession(
                new Raven.Client.Document.OpenSessionOptions
                {
                    Database = ConfigurationManager.AppSettings["CatalogRavenDbName"].Replace("{countryId}", _countryId)
                });
        }
    }   

}
