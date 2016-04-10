using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xlns.Catalog.Admin.Models;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Document.Repository;
using Xlns.Catalog.Core.Service;

namespace Xlns.Catalog.Admin.Controllers
{
    public class SettingsController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IGoogleTaxonomy _googleTaxonomy;
        public IGoogleTaxonomy GoogleTaxonomy
        {
            get
            {
                if (_googleTaxonomy == null)
                    _googleTaxonomy = new GoogleTaxonomy();
                return _googleTaxonomy;
            }
            set
            {
                _googleTaxonomy = value;
            }
        } 

        public ActionResult Google()
        {
            var gto = new GoogleTaxonomyOverview();
            gto.ImportedCountries = GoogleTaxonomy.GetImportedCountries();
            return View(gto);
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

        [HttpPost]
        public ActionResult GenerateTaxonomyTree()
        {
            int rootNode = int.Parse(System.Configuration.ConfigurationManager.AppSettings["GoogleTaxonomyApparelRootNode"]);
            var taxonomyTree = GoogleTaxonomy.CreateTaxonomyTree(rootNode);
            return PartialView("TaxonomyTree", taxonomyTree);
        }
	}
}