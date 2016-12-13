using PictureTagger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PictureTagger.Tests.Repositories
{
	public class FakeRepository<T> : IRepository<T>
	{
		private readonly Func<T, int> GetEntityID;
		private readonly IDictionary<int, T> _FakeEntities;

		public FakeRepository(Func<T, int> getEntityId)
		{
			if (getEntityId == null) throw new ArgumentNullException(nameof(getEntityId));
			GetEntityID = getEntityId;
			_FakeEntities = new Dictionary<int, T>();
		}

		public void Create(T _model) => _FakeEntities.Add(GetEntityID(_model), _model);

		public void Delete(int? id) => Delete(Find(id));

		public void Delete(T model) => _FakeEntities.Remove(GetEntityID(model));

		public void Dispose() => _FakeEntities.Clear();

		public IQueryable<T> GetAll() => _FakeEntities.Values.AsQueryable();

		public T Find(int? id)
		{
			T obj = default(T);
			_FakeEntities.TryGetValue(id ?? -1, out obj);
			return obj;
		}

		public void Update(T _model) => _FakeEntities[GetEntityID(_model)] = _model;
	}
}