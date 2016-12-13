using PictureTagger.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System;

namespace PictureTagger.Repositories
{
	public class DatabaseRepository<T> : IRepository<T>
		where T : class
	{
		private PictureTaggerContext _db;

		/// <summary>
		/// Create a new database connection.
		/// </summary>
		/// <param name="proxy">Allow Entity Framework to proxy loaded objects?</param>
		public DatabaseRepository(bool proxy = true)
		{
			_db = new PictureTaggerContext();
			_db.Configuration.ProxyCreationEnabled = proxy;
			_db.Configuration.LazyLoadingEnabled = proxy;
		}

		private void SafeSaveChanges()
		{
			try
			{
				_db.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				throw e;
			}
		}

		public void Create(T model)
		{
			_db.Set<T>().Add(model);
			SafeSaveChanges();
		}

		public void Delete(int? id) => Delete(Find(id));

		public void Delete(T model)
		{
			_db.Set<T>().Remove(model);
			SafeSaveChanges();
		}

		public void Dispose() => _db.Dispose();

		public IQueryable<T> GetAll() => _db.Set<T>();

		public T Find(int? id) => _db.Set<T>().Find(id);

		public void Update(T model)
		{
			_db.Entry(model).State = EntityState.Modified;
			SafeSaveChanges();
		}
	}
}