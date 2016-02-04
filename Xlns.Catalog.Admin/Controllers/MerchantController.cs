using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Document.Repository;

namespace Xlns.Catalog.Admin.Controllers
{
    public class MerchantController : Controller
    {
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
            var merchant = documentRepository.Load<Merchant>(Id);
            return View(merchant);
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
                documentRepository.Save(merchant);
            }
            return RedirectToAction("Create");
        }
	}
}