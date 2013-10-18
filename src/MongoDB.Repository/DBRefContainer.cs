using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    internal sealed class DBRefContainer : IDBRefContainer
    {
        IDictionary<Type, IList> container = new Dictionary<Type, IList>();
        List<MongoDBRef> _dbRef;
        public DBRefContainer(List<MongoDBRef> dbRef)
        {
            _dbRef = dbRef;
        }

        private void checkContainer<T>()
        {
            if (!container.ContainsKey(typeof(T)))
            {
                container.Add(typeof(T), new List<T>());
            }
        }

        private List<T> getContainer<T>()
        {
            if (!container.ContainsKey(typeof(T)))
            {
                List<T> lst = new List<T>();
                container.Add(typeof(T), lst);
                return lst;
            }
            return container[typeof(T)] as List<T>;
        }
        public bool Exists(string id)
        {
            return _dbRef.Exists(r => r.Id == id);
        }
        public bool Exists<T>() where T : IEntity
        {
            return _dbRef.Exists(r => r.CollectionName == typeof(T).Name);
        }

        public bool Exists<T>(Predicate<T> match) where T : IEntity
        {
            return getContainer<T>().Exists(match);
        }

        public T Pick<T>(string id) where T : IEntity
        {
            var t = getContainer<T>().SingleOrDefault(e => e.Id == id);
            if (t == null) t = MongoEntity.Get<T>(id);
            Add<T>(t);
            return t;
        }

        public T Pick<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            var t = GetAll<T>().SingleOrDefault(where.Compile());
            if (t == null) t = MongoEntity.Get<T>(where);
            Add<T>(t);
            return t;
        }
        private bool NeedToFreshAll<T>() where T : IEntity
        {
            return _dbRef.Count != getContainer<T>().Count;
        }
        public List<T> GetAll<T>() where T : IEntity
        {
            if (NeedToFreshAll<T>())
            {
                var all = getContainer<T>();
                var refs = _dbRef.Where(r => !all.Exists(e => e.Id == r.Id));
                var ids = refs.Select<MongoDBRef, string>(r => r.Id.ToString()).ToArray();
                all.AddRange(MongoEntity.Select<T>(t => ids.Contains(t.Id)));
                return all;
            }
            return getContainer<T>();
        }

        public List<T> GetMany<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            return getContainer<T>().Where(where.Compile()).ToList();
        }

        public void Add<T>(T entity) where T : IEntity
        {
            if (!Exists(entity.Id))
                _dbRef.Add(entity.ToDBRef());

            var lst = getContainer<T>();
            if (lst.Exists(e => e.Id == entity.Id))
                return;
            lst.Add(entity);
        }

        public void Add<T>(List<T> entities) where T : IEntity
        {
            _dbRef.RemoveAll(r => entities.Exists(e => e.Id == r.Id));
            entities.ForEach(e => _dbRef.Add(e.ToDBRef()));

            var lst = getContainer<T>();
            lst.RemoveAll(e => entities.Exists(_e => _e.Id == e.Id));
            lst.AddRange(entities);
        }

        public int Remove<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            var lst = GetAll<T>().Where(where.Compile()).ToList();
            lst.ForEach(delegate(T t)
            {
                _dbRef.RemoveAll(r => r.Id == t.Id);
                Remove<T>(t);
            });
            return lst.Count;
        }

        public void Remove<T>(T entity) where T : IEntity
        {
            _dbRef.RemoveAll(r => r.Id == entity.Id);
            getContainer<T>().RemoveAll(e => e.Id == entity.Id);
        }

        public void Remove<T>() where T : IEntity
        {
            _dbRef.RemoveAll(r => r.CollectionName == typeof(T).Name);
            getContainer<T>().RemoveAll(e => e != null);
        }

        public int Count<T>() where T : IEntity
        {
            return _dbRef.Count(r => r.CollectionName == typeof(T).Name);
        }

        public int Count<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            return GetAll<T>().Where(where.Compile()).Count();
        }


        public List<IEntity> GetAll()
        {
            List<IEntity> entities = new List<IEntity>();
            foreach (Type type in container.Keys)
            {
                entities.AddRange(container[type].Cast<IEntity>());
            }
            return entities;
        }
    }
}
