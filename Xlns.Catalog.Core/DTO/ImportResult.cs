using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlns.Catalog.Core.DTO
{
    public class ImportResult
    {
        public int Success { get; set; }
        public int Failure { get; set; }
        public IList<string> FailureDetails { get; set; }

        public ImportResult()
        {
            FailureDetails = new List<string>();
        }
    }
}
