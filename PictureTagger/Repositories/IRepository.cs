using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureTagger.Repositories
{
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
