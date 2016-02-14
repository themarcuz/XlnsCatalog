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

        /// <summary>
        /// Load a list of elements, with possibilities to page them
        /// </summary>
        /// <typeparam name="T">Document type to load</typeparam>
        /// <param name="itemsNumber">number of item per page; 0 means read them all</param>
        /// <param name="pageNumber">number of page to be skipped; 0 means read from the first</param>
        /// <returns></returns>
        public IList<T> LoadMany<T>(int itemsNumber, int pageNumber)
        {
            using (IDocumentSession session = OpenSession())
            {
                var result = session.Query<T>().AsQueryable();
                if (pageNumber > 0 && itemsNumber > 0) result = result.Skip(itemsNumber * (pageNumber - 1));
                if (itemsNumber > 0) result = result.Take(itemsNumber);
                return result.ToList();
            }
        }
    }

    public class StagingDocumentRepository : DocumentRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        protected string _countryId;

        public StagingDocumentRepository(string countryId) : base() 
        {
            _countryId = countryId;
        }

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
        protected string _countryId;

        public CatalogDocumentRepository(string countryId) : base()
        {
            _countryId = countryId;
        }

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
