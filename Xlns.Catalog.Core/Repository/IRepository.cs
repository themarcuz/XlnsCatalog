using System;
namespace Xlns.Catalog.Core.Repository
{
    public interface IRepository
    {
        void Delete<T>(T domainModelObject) where T : Model.ModelEntity;
        System.Linq.IQueryable<T> GetAll<T>() where T : Model.ModelEntity;
        int SaveUpdate<T>(T domainModelObject) where T : Model.ModelEntity;
    }
}
