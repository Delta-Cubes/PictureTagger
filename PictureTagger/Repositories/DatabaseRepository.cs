using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PictureTagger.Models;

namespace PictureTagger.Repositories
{
    public class DatabaseRepository<T> : IRepository<T>
        where T : class
    {
        protected PictureTaggerContext dbContext;

        public DatabaseRepository()
        {
            dbContext = new PictureTaggerContext();
        }

        public DatabaseRepository(bool isApiController) : this()
        {
            dbContext.Configuration.ProxyCreationEnabled = !isApiController;
            dbContext.Configuration.LazyLoadingEnabled = !isApiController;
        }

        public IQueryable<T> Get()
        {
            return dbContext.Set<T>();
        }

        public T Get(int? id)
        {
            return dbContext.Set<T>().Find(id);
        }

        public void Post(T _model)
        {
            dbContext.Set<T>().Add(_model);
            dbContext.SaveChanges();
        }

        public void Put(T _model)
        {
            dbContext.Entry(_model).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void Delete(int? id)
        {
            T _model = dbContext.Set<T>().Find(id);
            dbContext.Set<T>().Remove(_model);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}