using System;
using Raven.Client;
namespace Xlns.Catalog.Document.Repository
{
    interface ICatalogRepository
    {
        IDocumentSession OpenSession();
        IDocumentStore GetStore();
    }
}
