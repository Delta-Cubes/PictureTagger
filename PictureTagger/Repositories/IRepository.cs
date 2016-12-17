using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureTagger.Repositories
{
	/// <summary>
	/// Generic interface to define how a repository should be implemented
	/// </summary>
	/// <typeparam name="T">Type of entity this repository manages</typeparam>
	public interface IRepository<T> : IDisposable
	{
		IQueryable<T> GetAll();
		T Find(params object[] keyValues);
		void Create(T model);
		void Update(T model);
		void Delete(params object[] keyValues);
		void Delete(T model);
	}
}
