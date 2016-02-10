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
        public Merchant Merchant { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public string Id { get; set; }
        public string SKU { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProductLink { get; set; }
        public string MobileProductLink { get; set; }
        public string MainImageLink { get; set; }
        public IList<string> AdditionalImageLinks { get; set; }
        public string Condition { get; set; }
        public string Availability { get; set; }
        public Country Country { get; set; }
        public PricingInfo Pricing { get; set; }
        public string GoogleProductCategory { get; set; }
        public Gender Gender { get; set; }
        public Age Age { get; set; }
        public string Color { get; set; }
        public SizeInfo SizeInfo { get; set; }
        public string Brand { get; set; }
        public string Material { get; set; }
        public string ProductGroup { get; set; }
        public ShippingInfo Shipping { get; set; }
    }

    public class SizeInfo
    {
        public string SizeType { get; set; }
        public string SizeSystem { get; set; }
        public string Size { get; set; }
    }
    public class PricingInfo
    {
        public decimal Price { get; set; }
        private decimal _salePrice;
        public decimal SalePrice
        {
            get
            {
                if (_salePrice == 0) _salePrice = Price;
                return _salePrice;
            }
            set { _salePrice = value; }
        }
        private int _discountPerc;
        public int DiscountPerc
        {
            get
            {
                if (_discountPerc == 0 && Price != SalePrice)
                    _discountPerc = (int)(SalePrice / Price) * 100;
                return _discountPerc;
            }
            set { _discountPerc = value; }
        }
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
