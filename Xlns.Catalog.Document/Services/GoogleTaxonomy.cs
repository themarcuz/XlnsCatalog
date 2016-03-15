using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Xlns.Catalog.Document.Index;
using Xlns.Catalog.Document.Model;
using Xlns.Catalog.Document.Repository;

namespace Xlns.Catalog.Document.Services
{
    public class GoogleTaxonomy
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static IList<GoogleCountryTaxonomyGrouped> GetImportedCountries()
        {
            var documentRepository = new DocumentRepository();

            using (IDocumentSession session = documentRepository.OpenSession())
            {
                var countries = session.Query<GoogleCountryTaxonomyGrouped, GoogleTaxonomyItem_GroupCountries>()
                                       .Select(t => t)
                                       .ToList();
                logger.Info("Countries known by Google Taxonomy imported data is {0}", string.Join(", ", countries.Select(c => c.CountryCode).ToArray()));
                return countries;
            }
        }

        public static ImportResult Import(Stream originalFile, string countryCode)
        {
            var result = new ImportResult();
            var documentRepository = new DocumentRepository();
            var lineNumber = 0;
            using (var reader = new StreamReader(originalFile))
            {
                do
                {
                    lineNumber++;
                    try
                    {
                        string textLine = reader.ReadLine();
                        logger.Trace("Processing line: {0}", lineNumber);
                        string id = string.Empty;
                        var taxonomy = ParseLine(textLine, out id);
                        int temp;
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(taxonomy) && int.TryParse(id, out temp))
                        {
                            var gtItem = documentRepository.Load<GoogleTaxonomyItem>(id);
                            if (gtItem == null) gtItem = new GoogleTaxonomyItem(id);
                            var tax = gtItem.Taxonomies.FirstOrDefault(t => t.CountryCode == countryCode);
                            if (tax == null)
                            {
                                tax = new GoogleCountryTaxonomy { CountryCode = countryCode, Taxonomy = taxonomy };
                                gtItem.Taxonomies.Add(tax);
                            }
                            else
                            {
                                tax.Taxonomy = taxonomy;
                            }
                            documentRepository.Save(gtItem);
                            result.Success++;
                            logger.Trace("Line {0} successfully processed", lineNumber);
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
                        result.FailureDetails.Add(string.Format("Error occurred on line {0} : {1}",lineNumber, ex.Message));
                    }
                } while (reader.Peek() != -1);
                reader.Close();
            }
            return result;
        }

        private static string ParseLine(string line, out string Id)
        {
            int sepIndex = line.IndexOf("-");
            Id = line.Substring(0, sepIndex - 1);
            var taxonomy = line.Substring(sepIndex + 2, line.Length - sepIndex - 2);
            return taxonomy;
        }
    }
}
