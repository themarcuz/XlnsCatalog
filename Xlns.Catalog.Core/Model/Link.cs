using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlns.Catalog.Core.Model
{
    public class Link
    {
        public LinkType Type { get; set; }
        public string Url { get; set; }
    }

    public enum LinkType
    {
        DESKTOP,
        MOBILE
    }
}
