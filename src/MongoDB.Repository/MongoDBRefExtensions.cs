using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public static class MongoDBRefExtensions
    {
        public static T Instance<T>(this MongoDBRef instance) where T : IEntity
        {
            if (instance == null || null == instance.Id || string.IsNullOrEmpty(instance.CollectionName)) return default(T);
            return EntityOperationExtensions.DBFind<T>(instance.Id.ToString());
        }

        public static T RefPick<T>(this List<MongoDBRef> lst, string id) where T : IEntity
        {
            if (lst == null) return default(T);
            if (!lst.Exists(l => l.Id == id && l.CollectionName == typeof(T).DBCollectionName()))
                return default(T);
            return EntityOperationExtensions.DBFind<T>(id);
        }
        public static List<T> RefPick<T>(this List<MongoDBRef> lst, Expression<Func<T, bool>> where) where T : IEntity
        {
            if (lst == null) return new List<T>();
            var ids = lst.Where(i => i.CollectionName == typeof(T).DBCollectionName()).Select(i => i.Id).ToArray();
            return EntityOperationExtensions.DBSelect<T>(i => ids.Contains(i.Id)).Where(where).ToList();
        }
        public static List<T> RefPick<T>(this List<MongoDBRef> lst) where T : IEntity
        {
            if (lst == null) return new List<T>();
            var ids = lst.Where(i => i.CollectionName == typeof(T).DBCollectionName()).Select(i => i.Id).ToArray();
            return EntityOperationExtensions.DBSelect<T>(i => ids.Contains(i.Id)).ToList();
        }
        public static bool RefExists<T>(this List<MongoDBRef> lst, string id) where T : IEntity
        {
            if (lst == null) return false;
            return lst.Exists(l => l.Id == id && l.CollectionName == typeof(T).DBCollectionName());
        }
        public static bool RefExists<T>(this List<MongoDBRef> lst, Expression<Func<T, bool>> where) where T : IEntity
        {
            if (lst == null) return false;
            var ids = lst.Where(i => i.CollectionName == typeof(T).DBCollectionName()).Select(i => i.Id).ToArray();
            if (ids.Length == 0) return false;
            return EntityOperationExtensions.DBSelect<T>(i => ids.Contains(i.Id)).All(where);
        }
    }
}
