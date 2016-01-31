using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlns.Catalog.Core.Model
{
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Currency Currency { get; set; }
    }

    public enum Currency
    {
        EUR,
        USD
    }
}
