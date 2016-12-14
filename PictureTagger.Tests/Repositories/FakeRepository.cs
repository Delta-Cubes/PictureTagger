using PictureTagger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PictureTagger.Tests.Repositories
{
	public class FakeRepository<T> : IRepository<T>
	{
		private readonly Func<T, object> _id;
		private readonly IDictionary<object, T> _entities;

		public FakeRepository(Func<T, object> getEntityId)
		{
			if (getEntityId == null) throw new ArgumentNullException(nameof(getEntityId));
			_id = getEntityId;
			_entities = new Dictionary<object, T>();
		}

		public void Create(T _model) => _entities.Add(_id(_model), _model);

		public void Delete(params object[] keyValues) => Delete(Find(keyValues));

		public void Delete(T model) => _entities.Remove(_id(model));

		public void Dispose() => _entities.Clear();

		public IQueryable<T> GetAll() => _entities.Values.AsQueryable();

		public T Find(params object[] keyValues)
		{
			T obj = default(T);
			_entities.TryGetValue(keyValues[0] ?? -1, out obj);
			return obj;
		}

		public void Update(T _model) => _entities[_id(_model)] = _model;
	}
}