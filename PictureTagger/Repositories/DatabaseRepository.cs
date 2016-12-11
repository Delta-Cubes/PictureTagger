using PictureTagger.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

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

		private void SafeSaveChanges()
		{
			try
			{
				dbContext.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				throw e;
			}
		}

		public DatabaseRepository(bool isApiController) : this()
		{
			dbContext.Configuration.ProxyCreationEnabled = !isApiController;
			dbContext.Configuration.LazyLoadingEnabled = !isApiController;
		}

		public void Create(T model)
		{
			dbContext.Set<T>().Add(model);
			SafeSaveChanges();
		}

		public void Delete(int? id) => Delete(Get(id));

		public void Delete(T model)
		{
			dbContext.Set<T>().Remove(model);
			SafeSaveChanges();
		}

		public void Dispose() => dbContext.Dispose();

		public IQueryable<T> Get() => dbContext.Set<T>();

		public T Get(int? id) => dbContext.Set<T>().Find(id);

		public void Update(T model)
		{
			dbContext.Entry(model).State = EntityState.Modified;
			SafeSaveChanges();
		}
	}
}