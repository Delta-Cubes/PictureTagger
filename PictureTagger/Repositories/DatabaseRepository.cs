using PictureTagger.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Diagnostics;

namespace PictureTagger.Repositories
{
	public class DatabaseRepository<T> : IRepository<T>
		where T : class
	{
		private PictureTaggerContext _db;

		/// <summary>
		/// Create a new database connection.
		/// </summary>
		/// <param name="context">Existing context to use for this repository.</param>
		/// <param name="proxy">Allow Entity Framework to proxy loaded objects?  Only applied when context is not set.</param>
		public DatabaseRepository(PictureTaggerContext context, bool proxy = true)
		{
			if (context != null)
			{
				_db = context;
			}
			else
			{
				_db = new PictureTaggerContext();
				_db.Configuration.ProxyCreationEnabled = proxy;
				_db.Configuration.LazyLoadingEnabled = proxy;
			}
		}

		private void SafeSaveChanges()
		{
			try
			{
				_db.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				foreach (var validationErrors in e.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						Trace.TraceInformation("Property: {0} Error: {1}",
												validationError.PropertyName,
												validationError.ErrorMessage);
					}
				}
			}
		}

		public void Create(T model)
		{
			_db.Set<T>().Add(model);
			SafeSaveChanges();
		}

		public void Delete(params object[] keyValues) => Delete(Find(keyValues));

		public void Delete(T model)
		{
			_db.Set<T>().Remove(model);
			SafeSaveChanges();
		}

		public void Dispose() => _db.Dispose();

		public IQueryable<T> GetAll() => _db.Set<T>();

		public T Find(params object[] keyValues) => _db.Set<T>().Find(keyValues);

		public void Update(T model)
		{
			_db.Entry(model).State = EntityState.Modified;
			SafeSaveChanges();
		}
	}
}