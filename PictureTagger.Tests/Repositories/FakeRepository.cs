using PictureTagger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PictureTagger.Tests.Repositories
{
	public class FakeRepository<T> : IRepository<T>
	{
		private readonly Func<T, int> _entityId;
		private readonly IDictionary<int, T> _entities;

		public FakeRepository(Func<T, int> entityId)
		{
			if (entityId == null) throw new ArgumentNullException(nameof(entityId));
			_entityId = entityId;
			_entities = new Dictionary<int, T>();
		}

		public void Create(T _model) => _entities.Add(_entityId(_model), _model);

		public void Delete(int? id) => Delete(Get(id));

		public void Delete(T model) => _entities.Remove(_entityId(model));

		public void Dispose() => _entities.Clear();

		public IQueryable<T> Get() => _entities.Values.AsQueryable();

		public T Get(int? id)
		{
			T obj = default(T);
			_entities.TryGetValue(id ?? -1, out obj);
			return obj;
		}

		public void Update(T _model) => _entities[_entityId(_model)] = _model;
	}
}