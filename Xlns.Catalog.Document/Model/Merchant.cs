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
        public string Country { get; set; }
        public string Url { get; set; }
        public IList<string> Contacts { get; set; }
        public IList<Link> Logos { get; set; }

        public void GenerateId() 
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
