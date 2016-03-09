using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlns.Catalog.Document.Model
{
    public class ProductsList
    {
        public IList<ProductItem> Products { get; set; }
        public ProductsFilter Filter { get; set; }
    }
}
