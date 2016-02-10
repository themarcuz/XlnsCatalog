using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xlns.Catalog.Core.Model;

namespace Xlns.Catalog.Document.Model
{
    public class Catalog
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public CatalogStatus Status { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }

        public string MerchantId { get; set; }
    }

    public enum CatalogStatus 
    {
        DRAFT = 0,
        QUALITY_ASSURANCE = 20,
        APPROVED = 30,
        LIVE = 50
    }
}
