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
    public interface IMerchantService 
    {
        DocumentRepository DocumentRepository { get; set; }
        IList<Catalogue> GetCatalogues(string merchantId);
    }

    public class MerchantService : IMerchantService
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

        public IList<Catalogue> GetCatalogues(string merchantId)
        {
            using (IDocumentSession session = DocumentRepository.OpenSession())
            {
                var catalogues = session.Query<Catalogue>()
                    .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                    .Where(c => c.MerchantId == merchantId);
                return catalogues.ToList();
            }
        }
    }
}
