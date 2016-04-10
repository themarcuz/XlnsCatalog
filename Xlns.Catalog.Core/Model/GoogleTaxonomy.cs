using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xlns.Catalog.Core.Model
{
    public class GoogleTaxonomy : ModelEntity
    {
        public virtual int FatherId { get; set; }
        public virtual string Name_IT { get; set; }
        public virtual string Name_US { get; set; }

        public override bool Equals(object obj)
        {            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var o = obj as GoogleTaxonomy;
            return base.Equals(obj) && FatherId ==  o.FatherId && Name_IT == o.Name_IT && Name_US == o.Name_US;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;                
                hash = hash * 23 + base.GetHashCode();
                hash = hash * 23 + FatherId.GetHashCode();
                if (Name_IT != null) hash = hash * 23 + Name_IT.GetHashCode();
                if (Name_US != null) hash = hash * 23 + Name_US.GetHashCode();
                return hash;
            }
        }
    }
}
