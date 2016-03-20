using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xlns.Catalog.Document.Model;

namespace Xlns.Catalog.Admin.Models
{
    public class MerchantDetail
    {
        public class Statistics
        {
            public int CataloguesNumber { get; set; }
            public int ProductsNumber { get; set; }
        }

        public Merchant Merchant { get; set; }
        public Statistics Stats { get; set; }

        public MerchantDetail()
        {
            Stats = new Statistics();
        }
    }


}