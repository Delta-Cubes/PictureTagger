using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureTagger.Repositories
{
    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> Get();
        T Get(int? id);
        void Post(T _model);
        void Put(T _model);
        void Delete(int? id);
    }
}
