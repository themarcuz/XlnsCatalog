using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Indexes;
using Xlns.Catalog.Document.Index;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Document.Repository;

namespace Xlns.Catalog.Document.Services
{
    public interface ICatalogService
    {
        DocumentRepository DocumentRepository { get; set; }
        int DeleteCatalog(Catalogue catalog);
        int DeleteProducts(Catalogue catalog);
        IList<ProductItem> GetAllProducts(Catalogue catalog);
        int GetProductsNumber(Catalogue catalog);
    }

    public static class CatalogServiceExtensions
    {
        private static ICatalogService _catalogService;
        public static ICatalogService CatalogService
        {
            get
            {
                if (_catalogService == null)
                    _catalogService = new CatalogService();
                return _catalogService;
            }
            set
            {
                _catalogService = value;
            }
        }

        public static int GetProductsNumber(this Catalogue catalog) { return CatalogService.GetProductsNumber(catalog); }
        public static int DeleteCatalog(this Catalogue catalog) { return CatalogService.DeleteCatalog(catalog); }
        public static int DeleteProducts(this Catalogue catalog) { return CatalogService.DeleteProducts(catalog); }
        public static IList<ProductItem> GetAllProducts(this Catalogue catalog) { return CatalogService.GetAllProducts(catalog); }

    }

    public class CatalogService : Xlns.Catalog.Document.Services.ICatalogService
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

        public int GetProductsNumber(Catalogue catalog)
        {
            var catalogRepository = GetCatalogRepository(catalog);
            using (IDocumentSession session = catalogRepository.OpenSession())
            {
                var productNumber = session.Query<ProductItem>().Where(p => p.CatalogueId == catalog.Id).Count();
                return productNumber;
            }
        }

        /// <summary>
        /// Delete all product related to the catalogue, and the catalogue itself
        /// </summary>
        /// <param name="catalogueId"></param>
        /// <returns>Number of products deleted</returns>       
        public int DeleteCatalog(Catalogue catalog)
        {
            var deletedProducts = DeleteProducts(catalog);
            using (IDocumentSession session = DocumentRepository.OpenSession())
            {
                session.Delete(catalog.Id);
                session.SaveChanges();
            }
            return deletedProducts;
        }

        public int DeleteProducts(Catalogue catalog)
        {
            var catalogRepository = GetCatalogRepository(catalog);
            var productsNumber = GetProductsNumber(catalog);
            var indexName = new ProductItems_ByCatalogueId().IndexName;
            var operation = catalogRepository.GetStore().DatabaseCommands.DeleteByIndex(
                                                        indexName,
                                                        new IndexQuery
                                                        {
                                                            Query = string.Format("CatalogueId: {0}", catalog.Id)
                                                        },
                                                        new BulkOperationOptions
                                                        {
                                                            AllowStale = true
                                                        });
            var result = operation.WaitForCompletion();

            return productsNumber;
        }

        public IList<ProductItem> GetAllProducts(Catalogue catalog)
        {
            var catalogRepository = GetCatalogRepository(catalog);
            using (IDocumentSession session = catalogRepository.OpenSession())
            {
                var products = session.Query<ProductItem>().Where(p => p.CatalogueId == catalog.Id).ToList();
                return products;
            }
        }

        private ICatalogRepository GetCatalogRepository(Catalogue catalog)
        {
            ICatalogRepository catalogRepository = null;
            if (catalog.Status == CatalogStatus.LIVE) catalogRepository = new CatalogDocumentRepository();
            else catalogRepository = new StagingDocumentRepository();
            return catalogRepository;
        }

    }
}
