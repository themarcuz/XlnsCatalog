using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Xlns.Catalog.Document.Services;
using Xlns.Catalog.Document.Repository;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Admin.Helpers;
using Xlns.Catalog.Admin.Models;

namespace Xlns.Catalog.Admin.Controllers
{
    public class CatalogController : Controller
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult List(string merchantId)
        {
            var merchantCatalogs = new CatalogueList { MerchantId = merchantId };
            var documentRepository = new DocumentRepository();
            merchantCatalogs.Catalogs = documentRepository.LoadMany<Catalogue>(0, 0);
            /*
            merchantCatalogs.Add(new Catalogue
            {
                Country = new Core.Model.Country { Name = "Italy", Code = "IT", Currency = Core.Model.Currency.EUR },
                Created = DateTime.Now,
                Name = "Prova 1",
                Status = CatalogStatus.QUALITY_ASSURANCE,
                Updated = DateTime.Now
            });
            merchantCatalogs.Add(new Catalogue
            {
                Country = new Core.Model.Country { Name = "United States", Code = "US", Currency = Core.Model.Currency.USD },
                Created = DateTime.Now,
                Name = "Prova 2",
                Status = CatalogStatus.DRAFT,
                Updated = DateTime.Now
            });
            */
            
            return PartialView(merchantCatalogs);
        }

        [HttpPost]
        public ActionResult Create(Catalogue catalogue)
        {
            var documentRepository = new DocumentRepository();
            catalogue.Created = DateTime.Now;
            catalogue.Updated = DateTime.Now;
            catalogue.Status = CatalogStatus.DRAFT;
            catalogue.Id = string.Empty;
            documentRepository.Save(catalogue);
            return RedirectToAction("Detail", "Merchant", new { Id = catalogue.MerchantId });
        }

        [HttpPost]
        public ActionResult UploadFile(string merchantId)
        {

            var file = Request.Files["catalogFile"];

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                logger.Info("Uploading catalog {0}", fileName);

                var documentRepository = new DocumentRepository();
                var merchant = documentRepository.Load<Merchant>(merchantId);
                
                // TODO: selezionare dinamicamente l'importatore
                var importer = new StandardGoogleImporter();
                var importResults = importer.DraftImport(file.InputStream, merchant, Session.GetCountryId());

                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                //file.SaveAs(path);
                //var XmlDoc = XDocument.Load(new StreamReader(file.InputStream));
                //var itemCount = XmlDoc.Element("rss").Element("channel").Elements("item").Count();
                                
                TempData["importResult"] = importResults;
                return RedirectToAction("ImportResult");
            }
            throw new FileNotFoundException("No valid file uploaded");
        }


        public ActionResult ImportResult()
        {
            var importResult = TempData["importResult"];
            return View(importResult);
        }


        public ActionResult ProductsGrid()
        {
            return View();
        }
    }
}