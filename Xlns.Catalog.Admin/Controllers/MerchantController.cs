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
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QuickCreate(Merchant merchant)
        {
            if (ModelState.IsValid)
            {
                var documentRepository = new DocumentRepository();
                merchant.GenerateId();
                documentRepository.Save(merchant);
            }
            return RedirectToAction("Create");
        }
	}
}