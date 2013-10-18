using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public class RefEntity : Entity, IRefEntity
    {
        public RefEntity()
        {
            container = new DBRefContainer(this.DBRefs);
        }
        private IDBRefContainer container;

        [BsonIgnoreIfNull]
        public List<MongoDBRef> DBRefs
        {
            get
            {
                if (_dbrefs == null)
                    _dbrefs = new List<MongoDBRef>();
                return _dbrefs;
            }
            set
            {
                if (value != null)
                {
                    _dbrefs = value;
                    container = new DBRefContainer(_dbrefs);
                }
                container = new DBRefContainer(_dbrefs);
            }
        }
        private List<MongoDBRef> _dbrefs;

        public bool Exists(string id)
        {
            return container.Exists(id);
        }

        public bool Exists<T>() where T : IEntity
        {
            return container.Exists<T>();
        }

        public bool Exists<T>(Predicate<T> match) where T : IEntity
        {
            return container.Exists<T>(match);
        }

        public T Pick<T>(string id) where T : IEntity
        {
            return container.Pick<T>(id);
        }

        public T Pick<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            return container.Pick<T>(where);
        }

        public List<T> GetAll<T>() where T : IEntity
        {
            return container.GetAll<T>();
        }

        public List<T> GetMany<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            return container.GetMany<T>(where);
        }

        public void Add<T>(T entity) where T : IEntity
        {
            container.Add<T>(entity);
        }

        public void Add<T>(List<T> entities) where T : IEntity
        {
            container.Add<T>(entities);
        }

        public int Remove<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            return container.Remove<T>(where);
        }

        public void Remove<T>(T entity) where T : IEntity
        {
            container.Remove<T>(entity);
        }

        public void Remove<T>() where T : IEntity
        {
            container.Remove<T>();
        }

        public int Count<T>() where T : IEntity
        {
            return container.Count<T>();
        }

        public int Count<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            return container.Count<T>(where);
        }

        public void Update()
        {
            Save();
            container.GetAll().ForEach(e => e.Save());
        }
    }
}
