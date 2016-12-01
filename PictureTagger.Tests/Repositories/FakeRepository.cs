using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PictureTagger.Repositories;

namespace PictureTagger.Tests.Repositories
{
    public class FakeRepository<T> : IRepository<T>
    {

        private IDictionary<int, T> _Entities;

        public Func<T, int> EntityId;

        public FakeRepository()
        {
            _Entities = new Dictionary<int, T>();
        }

        public IQueryable<T> Get()
        {
            return _Entities.Select(e => e.Value).AsQueryable();
        }

        public T Get(int? id)
        {
            T obj = default(T);
            _Entities.TryGetValue(id ?? -1, out obj);
            return obj;
        }

        public void Post(T _model)
        {
            _Entities.Add(EntityId(_model), _model);
        }

        public void Put(T _model)
        {
            _Entities[EntityId(_model)] = _model;
        }

        public void Delete(int? id)
        {
            T obj;
            if (_Entities.TryGetValue(id ?? -1, out obj))
            {
                _Entities.Remove(EntityId(obj));
            }
        }

        public void Dispose()
        {
            _Entities.Clear();
        }
    }
}
