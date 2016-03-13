using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Document.Repository;
using Xlns.Catalog.Document.Services;

namespace Xlns.Catalog.Admin.Controllers
{
    public class SettingsController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Google()
        {

            return View();
        }

        [HttpPost]
        public ActionResult UploadTaxonomy(string CountryCode)
        {

            var file = Request.Files["gTaxonomyFile"];

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                logger.Info("Uploading Google Taxonomy {0}", fileName);                
                
                var result = GoogleTaxonomy.Import(file.InputStream, CountryCode);

                TempData["importResult"] = result;
                return RedirectToAction("Google");
            }
            throw new FileNotFoundException("No valid file uploaded");
        }
	}
}