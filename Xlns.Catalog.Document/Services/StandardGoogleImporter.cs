using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xlns.Catalog.Core.Model;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Document.Repository;

namespace Xlns.Catalog.Document.Services
{
    public class StandardGoogleImporter : ICatalogImporter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ImportResult DraftImport(Stream originalFile, Catalogue catalogue)
        {
            var documentRepository = new DocumentRepository();
            var merchant = documentRepository.Load<Merchant>(catalogue.MerchantId);
            var XmlDoc = XDocument.Load(new StreamReader(originalFile));
            var importResult = new ImportResult();
            XNamespace g = "http://base.google.com/ns/1.0";
            foreach (var item in XmlDoc.Element("rss").Element("channel").Elements("item"))
            {
                try
                {
                    var productItem = new ProductItem()
                    {
                        Age = TranslateAge(item.Element(g + "age_group").Value),
                        Availability = item.Element(g + "availability").Value,
                        Brand = item.Element(g + "brand").Value,
                        CatalogueId = catalogue.Id,
                        Color = item.Element(g + "color").Value,
                        Condition = item.Element(g + "condition").Value,
                        Created = DateTime.Now,
                        Description = item.Element("description").Value,                        
                        Gender = TranslateGender(item.Element(g + "gender").Value),
                        GoogleProductCategory = item.Element(g + "google_product_category").Value,
                        AdditionalImageLinks = item.Elements(g + "additional_image_link").Select(el => el.Value).ToList(),
                        Id = string.Format("{0}__{1}", catalogue.Id, item.Element(g + "id").Value),
                        MainImageLink = item.Element(g + "image_link").Value,
                        Material = item.Element(g + "material").Value,
                        Merchant = merchant,
                        Pricing = new PricingInfo
                        {
                            Price = ExtractPriceValue(item.Element(g + "price").Value),
                            SalePrice = ExtractPriceValue(item.Element(g + "sale_price").Value)
                        },
                        ProductGroup = item.Element(g + "item_group_id").Value,
                        ProductLink = item.Element("link").Value,
                        MobileProductLink = item.Element("mobile_link").Value,
                        Shipping = new ShippingInfo()
                        {
                            Price = item.Element(g + "shipping").Element(g + "price").Value,
                            Service = item.Element(g + "shipping").Element(g + "service").Value
                        },
                        SizeInfo = new SizeInfo
                        {
                            Size = item.Element(g + "size").Value,
                            SizeSystem = item.Element(g + "size_system").Value,
                            SizeType = item.Element(g + "size_type").Value
                        },
                        SKU = item.Element(g + "mpn").Value,
                        Title = item.Element("title").Value,
                        Updated = DateTime.Now
                    };

                    var stagingDocumentRepository = new StagingDocumentRepository();
                    stagingDocumentRepository.Save(productItem);
                    importResult.Success++;
                }
                catch (Exception ex)
                {
                    importResult.Failure++;
                    importResult.FailureDetails.Add(ex.Message);
                }
            }
            //TODO: salvare l'importResult anche sul documento del catalogo in staging
            return importResult;
        }
        
        private decimal ExtractPriceValue(string price)
        {
            //TODO: testare questo metodo!
            Regex regex = new Regex(@"^-?\d+(?:,\d+)?");
            Match match = regex.Match(price);
            if (match.Success)
            {
                return decimal.Parse(match.Value, CultureInfo.InvariantCulture);
            }
            else
            {
                throw new ArgumentException("Can't extract decimal value from one of the price fields");
            }
        }

        private Age TranslateAge(string age)
        {
            switch (age.ToLower().Trim())
            {
                case "adult":
                    return Age.ADULT;
                case "child":
                case "kids":
                    return Age.CHILD;
                default:
                    logger.Warn(string.Format("Can't translate age info: '{0}' ; using 'adult' as default value", age));
                    return Age.ADULT;
                    
            }
        }

        private Gender TranslateGender(string gender)
        {
            switch (gender.ToLower().Trim())
            {
                case "man":
                case "men":
                case "male":
                    return Gender.MAN;
                case "woman":
                case "women":
                case "female":
                    return Gender.WOMAN;
                case "child":
                case "kid":
                case "kids":
                    return Gender.CHILD;
                case "unisex":
                    return Gender.UNISEX;
                default:
                    throw new Exception(string.Format("Can't translate gender info: {0}", gender));
            }
        }
    }
}
