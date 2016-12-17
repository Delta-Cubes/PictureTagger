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
			// Use the existing context, or create a new one?
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

		/// <summary>
		/// Calles SaveChanges() in a way that entity validation errors are caught properly
		/// </summary>
		private void SafeSaveChanges()
		{
			try
			{
				_db.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				// Log each error
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

		/// <summary>
		/// Add a new entity to the database
		/// </summary>
		/// <param name="model">New entity to add</param>
		public void Create(T model)
		{
			_db.Set<T>().Add(model);
			SafeSaveChanges();
		}

		/// <summary>
		/// Delete an entity from the database
		/// </summary>
		/// <param name="keyValues">Key values</param>
		public void Delete(params object[] keyValues) => Delete(Find(keyValues));

		/// <summary>
		/// Delete an entity from the database
		/// </summary>
		/// <param name="model">Instantiated model to remove</param>
		public void Delete(T model)
		{
			_db.Set<T>().Remove(model);
			SafeSaveChanges();
		}

		/// <summary>
		/// Release unmanaged memory associated with the context
		/// </summary>
		public void Dispose() => _db.Dispose();

		/// <summary>
		/// Get all entities in the set of type T
		/// </summary>
		public IQueryable<T> GetAll() => _db.Set<T>();

		/// <summary>
		/// Find an entity by its key values
		/// </summary>
		/// <param name="keyValues">Key values</param>
		public T Find(params object[] keyValues) => _db.Set<T>().Find(keyValues);

		/// <summary>
		/// Update all properties in a proxied entity
		/// </summary>
		/// <param name="model">Entity with properties to update</param>
		public void Update(T model)
		{
			_db.Entry(model).State = EntityState.Modified;
			SafeSaveChanges();
		}
	}
}