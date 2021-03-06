﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Xlns.Catalog.Document.Repository;
using Xlns.Catalog.Admin.Helpers;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Core.Repository;
using System.Reflection;

namespace Xlns.Catalog.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Initialize PersistenceManager
            PersistenceManager.Context = new DataContext
            {
                Assemblies = new List<Assembly> 
                { 
                    typeof(Xlns.Catalog.Core.Model.ModelEntity).Assembly 
                },
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnString"].ConnectionString
            };

            //Initialize RavenDB Document Store
            DataDocumentStore.Initialize();
            StagingDocumentStore.Initialize();
            CatalogDocumentStore.Initialize(); 
        }

        protected void Session_Start()
        {
            Session.Add("countryId", "IT");
        }
    }
}
