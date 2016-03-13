using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xlns.Catalog.Admin.Helpers;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Document.Repository;
using Xlns.Catalog.Document.Services;

namespace Xlns.Catalog.Admin.Controllers
{
    public class ProductController : Controller
    {
       
        public ActionResult List(string catalogueId)
        {
            var documentRepository = new DocumentRepository();
            var catalogue = documentRepository.Load<Catalogue>(catalogueId);
            var products = catalogue.GetProducts(new ProductsFilter(1000, 1));
            var productsList = new ProductsList { Products = products };
            return PartialView(productsList);
        }
	}
}