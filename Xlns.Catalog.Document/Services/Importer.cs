using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xlns.Catalog.Document.Model;

namespace Xlns.Catalog.Document.Services
{
    public interface IImporter 
    {
        Model.ImportResult DraftImport(Stream originalFile, Merchant merchant, string countryId);
        Model.AnalysisResult MakeAnalysis(Catalogue catalogue);
    }

    public abstract class Importer
    {
    }
}
