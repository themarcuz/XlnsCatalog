using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Document.Repository;

namespace Xlns.Catalog.Document.Services
{
    public class CatalogService
    {
        private DocumentRepository _documentRepository;
        public DocumentRepository DocumentRepository
        {
            get
            {
                if (_documentRepository == null)
                    _documentRepository = new DocumentRepository();
                return _documentRepository;
            }
            set
            {
                _documentRepository = value;
            }
        }        

        public IList<Catalogue> GetMerchantsCatalogues(string merchantId)
        {
            using (IDocumentSession session = DocumentRepository.OpenSession())
            {
                var catalogues = session.Query<Catalogue>().Where(c => c.MerchantId == merchantId);
                return catalogues.ToList();
            }
        }


        /// <summary>
        /// Delete all product related to the catalogue, and the catalogue itself
        /// </summary>
        /// <param name="catalogueId"></param>
        /// <returns>Number of products deleted</returns>
        public int DeleteCatalog(string catalogueId)
        {            
            using (IDocumentSession session = DocumentRepository.OpenSession())
            {
                var catalog = DocumentRepository.Load<Catalogue>(catalogueId);
                ICatalogRepository catalogRepository = null;
                if (catalog.Status == CatalogStatus.LIVE) catalogRepository = new CatalogDocumentRepository(catalog.CountryCode);
                else catalogRepository = new StagingDocumentRepository(catalog.CountryCode);
                var products = session.Query<ProductItem>().Where(p => p.CatalogueId == catalogueId).ToList();
                foreach (var product in products)
                {
                    session.Delete(product.Id);
                }
                session.Delete(catalogueId);
                session.SaveChanges();
                return products.Count;
            }
        }

    }
}
