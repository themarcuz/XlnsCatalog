using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xlns.Catalog.Core.Model;

namespace Xlns.Catalog.Document.Model
{
    public class Merchant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; } 
        public string Url { get; set; }
        public string LogoUrl { get; set; }
        
    }

}
