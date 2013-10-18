using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;

namespace MongoDB.Repository
{
    internal static class EntityOperationExtensions
    {
        internal static T DBFind<T>(string id) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.FindOneByIdAs<T>(id);
            }
        }
        internal static T DBFind<T>(Type type, string id) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(type))
            {
                return client.Collection.FindOneByIdAs<T>(id);
            }
        }
        internal static T DBFind<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.FindOneAs<T>(Query<T>.Where(where));
            }
        }
        internal static bool DBRemove<T>(string id) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.Remove(Query<T>.Where(e => e.Id == id)).Ok;
            }
        }
        internal static long DBRemoveAll<T>() where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.RemoveAll().DocumentsAffected;
            }
        }
        internal static long DBRemoveAll<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.Remove(Query<T>.Where(where)).DocumentsAffected;
            }
        }
        internal static IQueryable<T> DBSelect<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.FindAs<T>(Query<T>.Where(where)).AsQueryable();
            }
        }
        internal static IQueryable<T> DBSelect<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, int pageIndex, int pageSize, out int pageCount, out int allCount) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                var queryable = client.Collection.FindAs<T>(Query<T>.Where(where)).AsQueryable();
                allCount = queryable.Count();
                pageCount = (allCount / pageSize) + (allCount % pageSize > 0 ? 1 : 0);
                return queryable.OrderByDescending(orderby).Skip(pageIndex * pageSize).Take(pageSize);
            }
        }
        internal static long DBCount<T>() where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.Count();
            }
        }
        internal static long DBCount<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.Count(Query<T>.Where(where));
            }
        }
        internal static void DBSave<T>(this T entity) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(entity.GetType()))
            {
                client.Collection.Save<T>(entity);
            }
        }
        internal static void DBSave(Type type, object entity)
        {
            using (IDBClient client = DBFactory.GetClient(type))
            {
                client.Collection.Save(type, entity);
            }
        }
        internal static void DBSave<T>(this List<T> entitys) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                entitys.ForEach(e => client.Collection.Save<T>(e));
            }
        }
        internal static bool DBRemove<T>(this T entity) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(entity.GetType()))
            {
                return client.Collection.Remove(Query<T>.EQ(e => e.Id, entity.Id)).Ok;
            }
        }

    }
}
