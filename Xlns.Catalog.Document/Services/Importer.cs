using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xlns.Catalog.Document.Model;
using DTO = Xlns.Catalog.Core.DTO;

namespace Xlns.Catalog.Document.Services
{
    public interface ICatalogImporter 
    {
        DTO.ImportResult DraftImport(Stream originalFile, Catalogue catalogue);        
    }
 
}
