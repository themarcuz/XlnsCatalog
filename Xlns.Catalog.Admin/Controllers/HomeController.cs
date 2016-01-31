using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xlns.Catalog.Admin.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            ViewData["SubTitle"] = "Welcome in Xlns product management platform";
            ViewData["Message"] = "Create products catalogs, visualize them, create statistics, analyze, merge them... it never been so easy! ";

            return View();
        }
       
    }
}