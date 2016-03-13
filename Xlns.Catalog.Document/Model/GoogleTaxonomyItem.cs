using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlns.Catalog.Document.Model
{
    public interface IGoogleTaxonomyItemCountryImplementation
    {
        void CreateFromBase(GoogleTaxonomyItem item);
    }

    public class GoogleTaxonomyItem
    {
        public string Id { get; set; }
        public IList<GoogleCountryTaxonomy> Taxonomies { get; set; }

        public GoogleTaxonomyItem(string id) : this()
        {
            Id = id;            
        }

        public GoogleTaxonomyItem()
        {
            Taxonomies = new List<GoogleCountryTaxonomy>();
        }
    }

    public struct GoogleCountryTaxonomy 
    {
        public string CountryCode;
        public string Taxonomy;
    }
}
