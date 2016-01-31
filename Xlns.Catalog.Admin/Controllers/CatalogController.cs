using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Linq;

namespace Xlns.Catalog.Admin.Controllers
{
    public class CatalogController : Controller
    {
        //
        // GET: /Task/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            foreach (var fileKey in Request.Files.AllKeys)
            {
                var file = Request.Files[fileKey];
                try
                {
                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        file.SaveAs(path);
                        var XmlDoc = XDocument.Load(new StreamReader(file.InputStream));
                        var itemCount = XmlDoc.Element("rss").Element("channel").Elements("item").Count(); ;
                        return Json(new { Message = string.Format("File saved: {0} items imported", itemCount) });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "Error in saving file" });
                }
            }
            return Json(new { Message = "File saved" });
        }

        public ActionResult ProductsGrid() 
        {
            return View();
        }
	}
}