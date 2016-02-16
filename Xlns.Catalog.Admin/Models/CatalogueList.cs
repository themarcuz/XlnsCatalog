using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xlns.Catalog.Admin.Models
{
    public class CatalogueList
    {
        public IList<Xlns.Catalog.Document.Model.Catalogue> Catalogs { get; set; }
        public string MerchantId { get; set; }
    }
}