using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PictureTagger.Models;

namespace PictureTagger.Repositories
{
    public abstract class DatabaseRepository<T> : IRepository<T>
    {
        protected PictureTaggerContext dbContext;

        protected DatabaseRepository()
        {
            dbContext = new PictureTaggerContext();
        }

        protected DatabaseRepository(bool isApiController) : this()
        {
            dbContext.Configuration.ProxyCreationEnabled = !isApiController;
            dbContext.Configuration.LazyLoadingEnabled = !isApiController;
        }

        public abstract IQueryable<T> Get();
        public abstract T Get(int? id);
        public abstract void Post(T _model);
        public abstract void Put(T _model);
        public abstract void Delete(int? id);

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}