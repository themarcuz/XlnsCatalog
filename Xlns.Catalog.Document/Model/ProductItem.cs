using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xlns.Catalog.Core.Model;

namespace Xlns.Catalog.Document.Model
{
    public class ProductItem
    {
        public ProductMerchant Merchant { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public string Id { get; set; }
        public string SKU { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<Link> ProductLinks { get; set; }
        public string MainImageLink { get; set; }
        public string HoverImageLink { get; set; }
        public IList<Link> ImageLinks { get; set; }
        public string Condition { get; set; }
        public string Availability { get; set; }
        public Country Country { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public string DiscountPerc { get; set; }
        public string GoogleProductCategory { get; set; }
        public Gender Gender { get; set; }
        public Age Age { get; set; }
        public string Color { get; set; }
        public string SizeType { get; set; }
        public string SizeSystem { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public string Material { get; set; }
        public string ProductGroup { get; set; }
        public ShippingInfo Shipping { get; set; }
    }

    public class ShippingInfo
    {
        public string Service { get; set; }
        public string Price { get; set; }
    }

    public enum Gender
    {
        MAN,
        WOMAN,
        CHILD,
        UNISEX
    }

    public enum Age
    {
        ADULT,
        CHILD
    }
}
