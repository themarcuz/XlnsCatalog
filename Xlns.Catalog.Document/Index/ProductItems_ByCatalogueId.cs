using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Indexes;
using Xlns.Catalog.Document.Model;

namespace Xlns.Catalog.Document.Index
{
    public class ProductItems_ByCatalogueId : AbstractIndexCreationTask<ProductItem>
    {
        public ProductItems_ByCatalogueId()
        {                         
            Map = items => from product in items
                           select new
                           {
                               CatalogueId = product.CatalogueId
                           };
        }
    }
}
