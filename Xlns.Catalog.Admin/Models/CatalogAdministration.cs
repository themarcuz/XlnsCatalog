using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xlns.Catalog.Document.Model;

namespace Xlns.Catalog.Admin.Models
{
    public class CatalogInfo
    {
        public Catalogue Catalogue { get; set; }
        public Merchant Merchant { get; set; }
        public int ProductsNumber { get; set; }
    }
}