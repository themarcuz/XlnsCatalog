using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Xlns.Catalog.Core.Repository
{
    public interface IDataContext
    {
        string ConfigurationFilePath { get; set; }
        string ConnectionString { get; set; }
        IList<Assembly> Assemblies { get; set; }
    }

    public class DataContext : IDataContext
    {
        public string ConfigurationFilePath { get; set; }
        public string ConnectionString { get; set; }
        public IList<Assembly> Assemblies { get; set; }
        
    }
}
