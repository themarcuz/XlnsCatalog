using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using FluentNHibernate;
using Environment = NHibernate.Cfg.Environment;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using NHibernate.Mapping.ByCode;

namespace Xlns.Catalog.Core.Repository
{

    public class PersistenceManager : IDisposable
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static PersistenceManager _istance;
        private ISessionFactory _SessionFactory;
        private static Object _lock = new Object();
        public static IDataContext Context
        {
            get;
            set;
        }

        public static PersistenceManager Istance
        {
            get
            {
                lock (_lock)
                {
                    if (_istance == null)
                    {
                        _istance = new PersistenceManager();
                        logger.Info("New Persistence Manager instance created");
                    }
                    return _istance;
                }
            }
        }

        private PersistenceManager()
        {
            if (Context == null) throw new Exception("IDataContext not set: impossible to create the PersistenceManager");
           
            //Configurazione del db
            var db = Fluently.Configure()
                                .Database(MsSqlConfiguration
                                              .MsSql2008
                                              .ConnectionString(Context.ConnectionString)
                                              //.ShowSql()
                                              )                                              
                                 .ExposeConfiguration(cfg => cfg.SetProperty(Environment.CurrentSessionContextClass, "thread_static"))
                                 //.ExposeConfiguration(cfg => cfg.SetProperty(Environment.GenerateStatistics, "true"))
                                 ;

            //importazione dei mapping
            foreach (var item in Context.Assemblies)
            {
                var assembly = item;                
                db.Mappings(m => m.FluentMappings.AddFromAssembly(assembly));
                db.ExposeConfiguration(c => MapAll(c, assembly));
            }
            

            // Create session factory from configuration object
            _SessionFactory = db.BuildSessionFactory();
        }

        public static void MapAll(Configuration cfg, Assembly a)
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(a.GetExportedTypes());
            var domainMapping =
              mapper.CompileMappingForAllExplicitlyAddedEntities();
            cfg.AddMapping(domainMapping);
        }

        public void Dispose()
        {            
            _SessionFactory.Close();
            _SessionFactory.Dispose();
        }


        /// <summary>
        /// Close this Persistence Manager and release all resources (connection pools, etc). It is the responsibility of the application to ensure that there are no open Sessions before calling Close().
        /// </summary>
        public void Close()
        {
            ISession currentSession = CurrentSessionContext.Unbind(_SessionFactory);
            currentSession.Close();
            currentSession.Dispose();            
        }


        public ISession GetSession()
        {
            if (!CurrentSessionContext.HasBind(_SessionFactory))
                CurrentSessionContext.Bind(_SessionFactory.OpenSession());

            return _SessionFactory.GetCurrentSession();            
        }

    }
}

