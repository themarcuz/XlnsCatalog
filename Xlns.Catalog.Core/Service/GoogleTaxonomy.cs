using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xlns.Catalog.Core.Repository;

namespace Xlns.Catalog.Core.Service
{

    public interface IGoogleTaxonomy
    {
        IList<DTO.ItemsPerCountry> GetImportedCountries();
        string GetTaxonomy(int googleTaxonomyId, string countryCode);
        DTO.ImportResult Import(Stream originalFile, string countryCode);
        string CreateTaxonomyTree(int rootNode);
    }

    public class GoogleTaxonomy : IGoogleTaxonomy
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IRepository _respository;
        public IRepository Repository { 
            get 
            {
                if (_respository == null) _respository = new CommonRepository();
                return _respository;
            }
            set 
            {
                _respository = value;
            }
        }

        public IList<DTO.ItemsPerCountry> GetImportedCountries()
        {
            IList<Model.GoogleTaxonomy> allTaxonomy = null;
            using (var om = new OperationManager())
            {
                try
                {
                    om.BeginOperation();
                    allTaxonomy = Repository.GetAll<Model.GoogleTaxonomy>().ToList();
                    om.CommitOperation();
                }
                catch (Exception ex)
                {
                    om.RollbackOperation();
                    logger.Error(ex);
                    throw ex;
                }
            }
            IList<DTO.ItemsPerCountry> taxonomyPerCountry = new List<DTO.ItemsPerCountry>();
            taxonomyPerCountry.Add(new DTO.ItemsPerCountry { CountryCode = "IT", Value = allTaxonomy.Count(t => !string.IsNullOrEmpty(t.Name_IT)) });
            taxonomyPerCountry.Add(new DTO.ItemsPerCountry { CountryCode = "US", Value = allTaxonomy.Count(t => !string.IsNullOrEmpty(t.Name_US)) });
            return taxonomyPerCountry;            
        }

        public DTO.ImportResult Import(Stream originalFile, string countryCode)
        {
            var result = new DTO.ImportResult();
            var lineNumber = 0;
            logger.Trace("Start importing google taxonomy file for {0}", countryCode);
            using (var reader = new StreamReader(originalFile))
            {
                do
                {
                    lineNumber++;
                    try
                    {
                        string textLine = reader.ReadLine();
                        logger.Trace("Processing line: {0}", lineNumber);
                        string idstr = string.Empty;
                        var taxonomy = ParseLine(textLine, out idstr);
                        int id;
                        if (!string.IsNullOrEmpty(idstr) && !string.IsNullOrEmpty(taxonomy) && int.TryParse(idstr, out id))
                        {
                            using (var om = new OperationManager())
                            {
                                try
                                {
                                    var session = om.BeginOperation();
                                    var gtItem = session.Get<Model.GoogleTaxonomy>(id);
                                    if (gtItem == null)
                                    {
                                        logger.Trace("Item {0} not found... creating", id);
                                        gtItem = new Model.GoogleTaxonomy() { Id = id };
                                    }
                                    else 
                                    {
                                        logger.Trace("Item {0} found... updating", id);
                                    }
                                    switch (countryCode)
                                    {
                                        case "IT":
                                            gtItem.Name_IT = taxonomy;
                                            break;
                                        case "US":
                                            gtItem.Name_US = taxonomy;
                                            break;
                                        default:
                                            break;
                                    }
                                    Repository.SaveUpdate(gtItem);
                                    om.CommitOperation();
                                    result.Success++;
                                    logger.Trace("Line {0} successfully processed", lineNumber);
                                }
                                catch (Exception ex)
                                {
                                    om.RollbackOperation();
                                    throw ex;
                                }
                            }
                            
                        }
                        else
                        {
                            result.Failure++;
                            logger.Trace("Error occurred on line {0}", lineNumber);
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Failure++;
                        var msg = string.Format("Error occurred on line {0}", lineNumber);
                        result.FailureDetails.Add(string.Format("{0} : {1}", msg, ex.Message));
                        logger.Error(ex, msg);
                    }
                } while (reader.Peek() != -1);
                reader.Close();
            }
            return result;
        }

        public string GetTaxonomy(int googleTaxonomyId, string countryCode)
        {
            logger.Trace("Requested taxonomy translation for CategoryId = {0} and country {1}", googleTaxonomyId, countryCode);

            using (var om = new OperationManager())
            {
                try
                {
                    var session = om.BeginOperation();
                    var taxonomy = Repository.GetAll<Model.GoogleTaxonomy>().FirstOrDefault(gt => gt.Id == googleTaxonomyId);
                    om.CommitOperation();
                    if (taxonomy == null)
                    {
                        logger.Warn("Taxonomy label not loaded for id={0} , country={1}", googleTaxonomyId, countryCode);
                        return string.Empty;
                    }
                    var taxonomyLabel = string.Empty;
                    switch (countryCode)
                    {
                        case "IT":
                            taxonomyLabel = taxonomy.Name_IT;
                            break;
                        case "US":
                            taxonomyLabel = taxonomy.Name_US;
                            break;
                        default:
                            break;
                    }
                    logger.Trace("Taxonomy translation found: {0}", taxonomyLabel);
                    return taxonomyLabel;
                }
                catch (Exception ex)
                {
                    om.RollbackOperation();
                    string msg = String.Format("Error retrieving Google taxonomy value for id = {0} and country =  {1}", googleTaxonomyId, countryCode);
                    logger.Error(ex, msg);
                    throw new Exception(msg, ex);
                }
            }
        }

        public string CreateTaxonomyTree(int rootNode)
        {
            logger.Info("Start Google Category tree rebuild");
            //string defaultCountryCode = System.Configuration.ConfigurationManager.AppSettings["DefaultCountryCode"];
            //var documentRepository = new DocumentRepository();
            //var tree = new TaxonomyNode();
            //using (IDocumentSession session = documentRepository.OpenSession())
            //{
            //    var query = session.Query<GoogleTaxonomyItem>();
            //    using (var enumerator = session.Advanced.Stream(query))
            //    {
            //        while (enumerator.MoveNext())
            //        {
            //            GoogleTaxonomyItem activeGTI = enumerator.Current.Document;
            //            var tax = activeGTI.Taxonomies.FirstOrDefault(t => t.CountryCode == defaultCountryCode);
            //            logger.Debug("Read '{0}' ...", tax);
            //            if (tax != null && tax.Taxonomy.Contains(">")) // is someone's child
            //            {
            //                logger.Debug("... looking for a father");
            //                var taxParts = tax.Taxonomy.Split(new string[] { " > " }, StringSplitOptions.None);
            //                var fatherToLookFor = string.Join(" > ", taxParts, 0, taxParts.Length - 1);

            //                var possiblFathers = session.Query<GoogleTaxonomyItem>()
            //                    .SelectMany(gti => gti.Taxonomies,
            //                                (gti, t) => new { Id = gti.Id, countryCode = t.CountryCode, Taxonomy = t.Taxonomy })
            //                    .Where(flatTax => flatTax.countryCode == defaultCountryCode)
            //                    .Where(flatTax => flatTax.Taxonomy == fatherToLookFor)
            //                    .ToList();

            //            }
            //            else
            //            {
            //                logger.Debug("... found a root element");
            //                gtItem.FatherId = string.Empty;
            //            }
            //        }
            //    }                
            //}
            return "prova";
        }

        private string ParseLine(string line, out string Id)
        {
            int sepIndex = line.IndexOf("-");
            Id = line.Substring(0, sepIndex - 1);
            var taxonomy = line.Substring(sepIndex + 2, line.Length - sepIndex - 2);
            return taxonomy;
        }
    }
}
