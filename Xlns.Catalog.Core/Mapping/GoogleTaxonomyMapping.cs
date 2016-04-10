using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Xlns.Catalog.Core.Model
{
    public class GoogleTaxonomyMapping : ClassMap<GoogleTaxonomy>
    {
        public GoogleTaxonomyMapping()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.FatherId);
            Map(x => x.Name_IT).Nullable();
            Map(x => x.Name_US).Nullable();
        }
        
    }
}
