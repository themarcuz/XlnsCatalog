using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Document.Repository;
using Xlns.Catalog.Admin.Helpers;
using Xlns.Catalog.Admin.Models;
using Xlns.Catalog.Document.Services;

namespace Xlns.Catalog.Admin.Controllers
{
    public class MerchantController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private DocumentRepository _documentRepository;
        public DocumentRepository DocumentRepository
        {
            get
            {
                if (_documentRepository == null)
                    _documentRepository = new DocumentRepository();
                return _documentRepository;
            }
            set
            {
                _documentRepository = value;
            }
        }

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

        //
        // GET: /Merchant/
        public ActionResult Index()
        {
            var documentRepository = new DocumentRepository();
            var merchants = documentRepository.LoadMany<Merchant>(10, 1);
            return View(merchants);
        }
        
        public ActionResult Detail(string Id)
        {
            var documentRepository = new DocumentRepository();
            var merchantDetail = new MerchantDetail();
            merchantDetail.Merchant = documentRepository.Load<Merchant>(Id);
            var catalogues = MerchantService.GetCatalogues(Id);
            merchantDetail.Stats.CataloguesNumber = catalogues.Count();
            merchantDetail.Stats.ProductsNumber = catalogues.Sum(c => c.GetProductsNumber());
            return View(merchantDetail);
        }

        public ActionResult Create()
        {
            ViewBag.CurrentActionName = "Create";
            return View("Edit");
        }

        public ActionResult Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return RedirectToAction("Create");
            ViewBag.CurrentActionName = "Edit";
            var documentRepository = new DocumentRepository();
            var merchant = documentRepository.Load<Merchant>(Id);
            return View("Edit", merchant);
        }

        [HttpPost]
        public ActionResult Save(Merchant merchant)
        {
            if (ModelState.IsValid)
            {
                var documentRepository = new DocumentRepository();
                if (string.IsNullOrEmpty(merchant.Id)) merchant.Id = string.Empty;
                documentRepository.Save(merchant);
            }
            return RedirectToAction("Index");
        }
	}
}