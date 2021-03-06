﻿using System;
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

        private IMerchantService _merchantService;        
        public IMerchantService MerchantService
        {
            get
            {
                if (_merchantService == null)
                    _merchantService = new MerchantService();
                return _merchantService;
            }
            set
            {
                _merchantService = value;
            }
        }

        /* private ICatalogService _catalogService;
        public ICatalogService CatalogService
        {
            get
            {
                if (_catalogService == null)
                    _catalogService = new CatalogService();
                return _catalogService;
            }
            set
            {
                _catalogService = value;
            }
        }  */

        public ActionResult Index() 
        {
            var fullCatalog = new FullCatalog();
            return View(fullCatalog);
        }

        public ActionResult List(string merchantId)
        {
            var merchantCatalogs = new CatalogueList { MerchantId = merchantId };
            merchantCatalogs.Catalogs = MerchantService.GetCatalogues(merchantId);
            return PartialView(merchantCatalogs);
        }

        public ActionResult Admin(string Id)
        {
            var documentRepository = new DocumentRepository();
            var catalogue = documentRepository.Load<Catalogue>(Id);
            var merchant = documentRepository.Load<Merchant>(catalogue.MerchantId);

            var catalogAdmin = new CatalogInfo 
            { 
                Catalogue = catalogue,
                Merchant = merchant,
                ProductsNumber = catalogue.GetProductsNumber()
            };
            return View(catalogAdmin);
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
        public ActionResult Delete(string catalogueId)
        {
            var documentRepository = new DocumentRepository();
            var catalogue = documentRepository.Load<Catalogue>(catalogueId);
            var merchantId = catalogue.MerchantId;
            var productDeleted = catalogue.DeleteCatalog();
            TempData.Add("notification",
                new Notification
                {
                    Title = "Catalogue succesfully deleted",
                    Message = string.Format("{0} products deleted from catalog {1}", productDeleted, catalogue.Name),
                    Type = "success"
                });
            return RedirectToAction("Detail", "Merchant", new { Id = merchantId });
        }

        [HttpPost]
        public ActionResult UploadFile(string catalogueId)
        {

            var file = Request.Files["catalogFile"];

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                logger.Info("Uploading catalog {0}", fileName);

                var documentRepository = new DocumentRepository();
                var catalogue = documentRepository.Load<Catalogue>(catalogueId);

                // TODO: selezionare dinamicamente l'importatore
                var importer = new StandardGoogleImporter();
                var importResults = importer.DraftImport(file.InputStream, catalogue);

                catalogue.Updated = DateTime.Now;
                catalogue.Status = CatalogStatus.QUALITY_ASSURANCE;
                documentRepository.Save(catalogue);

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