using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xlns.Catalog.Core.Model;

namespace Xlns.Catalog.Admin.Controllers
{
    public class CountryController : Controller
    {
        //
        // GET: /Country/
        public ActionResult Select()
        {
            var countries = new List<Country>
            {
                new Country { Code = "IT", Name = "Italy", Currency = Currency.EUR },
                new Country { Code = "US", Name = "United States", Currency = Currency.USD }
            };
            return PartialView(countries);
        }
	}
}