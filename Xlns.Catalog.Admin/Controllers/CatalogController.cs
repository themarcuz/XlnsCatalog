using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Linq;
using Xlns.Catalog.Document.Services;

namespace Xlns.Catalog.Admin.Controllers
{
    public class CatalogController : Controller
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile()
        {

            var file = Request.Files["catalogFile"];

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                logger.Info("Uploading catalog {0}", fileName);

                // TODO: selezionare dinamicamente l'importatore
                var importer = new StandardGoogleImporter();
                var importResults = importer.DraftImport(file.InputStream);

                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                //file.SaveAs(path);
                //var XmlDoc = XDocument.Load(new StreamReader(file.InputStream));
                //var itemCount = XmlDoc.Element("rss").Element("channel").Elements("item").Count();

                return View("ImportResult", importResults);
            }
            throw new FileNotFoundException("No valid file uploaded");
        }

        public ActionResult ProductsGrid()
        {
            return View();
        }
    }
}