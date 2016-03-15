using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xlns.Catalog.Document.Model;

namespace Xlns.Catalog.Admin.Models
{
    public class GoogleTaxonomyOverview
    {
        public IList<GoogleCountryTaxonomyGrouped> ImportedCountries { get; set; }
    }
}