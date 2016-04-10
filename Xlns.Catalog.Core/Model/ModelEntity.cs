using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xlns.Catalog.Core.Model
{
    public abstract class ModelEntity
    {
        public virtual int Id { get; set; }

        public override bool Equals(object obj)
        {
            if(obj.GetType() != this.GetType())
                return false;
            else
                return this.Id == ((ModelEntity)obj).Id;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();               
                return hash;
            }
        }

    }
}
