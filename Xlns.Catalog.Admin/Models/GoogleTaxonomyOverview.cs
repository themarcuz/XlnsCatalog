﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xlns.Catalog.Core.DTO;

namespace Xlns.Catalog.Admin.Models
{
    public class GoogleTaxonomyOverview
    {
        public IList<ItemsPerCountry> ImportedCountries { get; set; }
    }
}