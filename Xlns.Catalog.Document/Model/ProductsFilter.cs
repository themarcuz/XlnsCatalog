using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlns.Catalog.Document.Model
{
    public class ProductsFilter
    {
        public int MaxPageSize { get; set; }
        public int PageNumber { get; set; }

        public ProductsFilter(int maxPageSize, int pageNumber)
        {
            MaxPageSize = maxPageSize;
            PageNumber = pageNumber;
        }
    }
}
