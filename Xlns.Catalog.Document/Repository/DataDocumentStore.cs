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
                      "IDocumentStore has not been initialized.");
                return instance;
            }
        }

        public static void Initialize()
        {
            instance = new DocumentStore { ConnectionStringName = "RavenDB" };
            //instance.Conventions.IdentityPartsSeparator = "-";
            instance.Initialize();            
        }
    }
}
