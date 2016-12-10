using PictureTagger.Models;
using System.Data.Entity;
using System.Linq;

namespace PictureTagger.Repositories
{
	public class DatabaseRepository<T> : IRepository<T>
		where T : class
	{
		protected PictureTaggerContext dbContext;

		public DatabaseRepository() : this(false)
		{
        }

		public DatabaseRepository(bool isApiController)
		{
            dbContext = new PictureTaggerContext();
            dbContext.Configuration.ProxyCreationEnabled = !isApiController;
            dbContext.Configuration.LazyLoadingEnabled = !isApiController;
        }

		public void Create(T model)
		{
			dbContext.Set<T>().Add(model);
			dbContext.SaveChanges();
		}

		public void Delete(int? id) => Delete(Find(id));

		public void Delete(T model)
		{
			dbContext.Set<T>().Remove(model);
			dbContext.SaveChanges();
		}

		public void Dispose() => dbContext.Dispose();

		public IQueryable<T> GetAll() => dbContext.Set<T>();

		public T Find(int? id) => dbContext.Set<T>().Find(id);

		public void Update(T model)
		{
			dbContext.Entry(model).State = EntityState.Modified;
			dbContext.SaveChanges();
		}
	}
}