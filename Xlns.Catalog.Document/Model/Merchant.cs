using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xlns.Catalog.Core.Model;

namespace Xlns.Catalog.Document.Model
{
    public abstract class MerchantBase 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Url { get; set; }
        public string LogoUrl { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class Merchant : MerchantBase
    {
        public string Contact { get; set; }
        public string Address { get; set; }
        //public IList<string> Contacts { get; set; }
        public IList<Link> Logos { get; set; }
        public IList<Catalog> Catalogs { get; set; }

    }

    public class ProductMerchant : MerchantBase 
    {
        public Catalog Catalog { get; set; }
    }
}
