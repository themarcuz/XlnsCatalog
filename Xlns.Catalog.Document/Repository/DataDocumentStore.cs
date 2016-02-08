using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Document;

namespace Xlns.Catalog.Document.Repository
{
    public class DataDocumentStore
    {
        private static IDocumentStore instance;

        public static IDocumentStore Instance
        {
            get
            {
                if (instance == null)
                    throw new InvalidOperationException(
                      "DataDocumentStore has not been initialized.");
                return instance;
            }
        }

        public static void Initialize()
        {
            instance = new DocumentStore { ConnectionStringName = "RavenDB" };
            instance.Conventions.IdentityPartsSeparator = "-";
            instance.Initialize();            
        }
    }

    public class StagingDocumentStore
    {
        private static IDocumentStore instance;

        public static IDocumentStore Instance
        {
            get
            {
                if (instance == null)
                    throw new InvalidOperationException(
                      "StagingDocumentStore has not been initialized.");
                return instance;
            }
        }

        public static void Initialize()
        {
            instance = new DocumentStore { ConnectionStringName = "StagingRavenDB"};
            instance.Conventions.IdentityPartsSeparator = "-";            
            instance.Initialize();
        }
    }
}
