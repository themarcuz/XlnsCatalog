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

    public class GoogleTaxonomyItem_GroupCountries : AbstractIndexCreationTask<GoogleTaxonomyItem, GoogleCountryTaxonomyGrouped>
    {        

        public GoogleTaxonomyItem_GroupCountries()
        {
            Map = gti => gti.SelectMany(i => i.Taxonomies, (i, t) => new { CountryCode = t.CountryCode, Count = 1 });

            Reduce = results => results.GroupBy(r => r.CountryCode).Select(x => new { CountryCode = x.Key, Count = x.Sum(t => t.Count) });
            
        }
    }
}
