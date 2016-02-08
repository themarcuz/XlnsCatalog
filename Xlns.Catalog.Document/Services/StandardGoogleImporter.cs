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
    public class StandardGoogleImporter : IImporter
    {

        public ImportResult DraftImport(System.IO.Stream originalFile)
        {
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
                        Color = item.Element(g + "color").Value,
                        Condition = item.Element(g + "condition").Value,
                        Created = DateTime.Now,
                        Description = item.Element("description").Value,
                        // TODO: calcolare DiscountPerc
                        Gender = TranslateGender(item.Element(g + "gender").Value),
                        GoogleProductCategory = item.Element(g + "google_product_category").Value,
                        //TODO: prendere la secona immaginer per HoverImageLink
                        //TODO: calcolare Id = item.Element(g + "id").Value, aggiungendo anche merchant e country
                        //TODO: ImageLinks
                        MainImageLink = item.Element(g + "image_link").Value,
                        Material = item.Element(g + "material").Value,
                        //TODO: impostare il Merchant
                        Price = ExtractPriceValue(item.Element(g + "price").Value),
                        ProductGroup = item.Element(g + "item_group_id").Value,
                        ProductLinks = new List<Link>() 
                        {
                            new Link(){ Type = LinkType.DESKTOP, Url = item.Element("link").Value },
                            new Link(){ Type = LinkType.MOBILE, Url = item.Element("mobile_link").Value }
                        },
                        SalePrice = ExtractPriceValue(item.Element(g + "sale_price").Value),
                        Shipping = new ShippingInfo() 
                        {
                            Price = item.Element(g + "shipping").Element(g + "price").Value,
                            Service = item.Element(g + "shipping").Element(g + "service").Value
                        },
                        Size = item.Element(g + "size").Value,
                        SizeSystem = item.Element(g + "size_system").Value,
                        SizeType = item.Element(g + "size_type").Value,
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
             
        public Model.AnalysisResult MakeAnalysis(Model.Catalog catalog)
        {
            throw new NotImplementedException();
        }

        private decimal ExtractPriceValue(string price)
        {
            //TODO: testare questo metodo!
            Regex regex = new Regex(@"^-?\d+(?:\.\d+)?");
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
            switch (age.ToLower().Trim() )
            {
                case "adult": 
                    return Age.ADULT;
                case "child":
                case "kids":
                    return Age.CHILD;
                default:
                    throw new Exception(string.Format("Can't translate age info: {0}", age));
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
