using PictureTagger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PictureTagger.Tests.Repositories
{
	public class FakeRepository<T> : IRepository<T>
	{
		private readonly Func<T, int> GetEntityID;
		private readonly IDictionary<int, T> FakeEntities;

		public FakeRepository(Func<T, int> getEntityId)
		{
			if (getEntityId == null) throw new ArgumentNullException(nameof(getEntityId));
			GetEntityID = getEntityId;
			FakeEntities = new Dictionary<int, T>();
		}

		public void Create(T _model) => FakeEntities.Add(GetEntityID(_model), _model);

		public void Delete(int? id) => Delete(Find(id));

		public void Delete(T model) => FakeEntities.Remove(GetEntityID(model));

		public void Dispose() => FakeEntities.Clear();

		public IQueryable<T> GetAll() => FakeEntities.Values.AsQueryable();

		public T Find(int? id)
		{
			T obj = default(T);
			FakeEntities.TryGetValue(id ?? -1, out obj);
			return obj;
		}

		public void Update(T _model) => FakeEntities[GetEntityID(_model)] = _model;
	}
}