using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;

namespace MongoDB.Repository
{
    internal static class EntityOperationExtensions
    {

        internal static bool DBExists<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.Count(Query<T>.Where(where)) > 0;
            }
        }
        internal static bool DBExists<T>(string id) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.Count(Query<T>.Where(t => t.Id == id)) > 0;
            }
        }
        internal static List<T> DBFindAll<T>() where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                return client.Collection.FindAllAs<T>().ToList();
            }
        }
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
                return !client.Collection.Remove(Query<T>.Where(e => e.Id == id)).HasLastErrorMessage;
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
        internal static void DBSave<T>(this List<T> entities) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                entities.ForEach(e => client.Collection.Save<T>(e));
            }
        }
        internal static void DBInsertBatch<T>(this List<T> entities) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(typeof(T)))
            {
                try
                {
                    var ret = client.Collection.InsertBatch<T>(entities, new Driver.MongoInsertOptions() { Flags = Driver.InsertFlags.ContinueOnError });
                }
                catch (MongoWriteConcernException e)
                {
                    if (e.WriteConcernResult.DocumentsAffected!=1)
                        throw e;
                }
            }
        }
        internal static bool DBRemove<T>(this T entity) where T : IEntity
        {
            using (IDBClient client = DBFactory.GetClient(entity.GetType()))
            {
                return client.Collection.Remove(Query<T>.EQ(e => e.Id, entity.Id)).DocumentsAffected==1;
            }
        }


        internal static MongoGridFSFileInfo DBSaveGridFS<TFileType>(Type type, IMongoFile file) where TFileType : IMongoFile
        { return DBSaveGridFS(typeof(TFileType), file); }
        internal static MongoGridFSFileInfo DBSaveGridFS(Type type, IMongoFile file)
        {
            using (IDBClient client = DBFactory.GetClient(type))
            {
                using (Stream stream = File.OpenRead(file.LocalFileName))
                {
                    return client.GridFS.Upload(stream, file.RemoteFileName, BuildMongoGridFSCreateOptions(file));//.Upload(file.FileName);
                }
            }
        }
        //internal static IMongoFile DBLoadGridFS(IMongoFile file)
        //{
        //    using (IDBClient client = DBFactory.GetClient(typeof(IMongoFile)))
        //    {
        //        file.Attach(client.GridFS.FindOneById(file.Id));
        //        return file;
        //    }
        //}
        internal static MongoGridFSCreateOptions BuildMongoGridFSCreateOptions(IMongoFile file)
        {
            MongoGridFSCreateOptions options = new MongoGridFSCreateOptions();
            options.Id = file.Id;
            options.ChunkSize = file.Size;
            options.ContentType = file.ContentType;
            options.Aliases = file.Aliases;
            options.Metadata = file.Metadata;
            options.UploadDate = file.UploadDate;
            return options;
        }
        internal static MongoGridFSFileInfo DBLoadGridFS<TFileType>(BsonValue id) where TFileType : IMongoFile
        {
            return DBLoadGridFS(typeof(TFileType), id);
        }
        internal static MongoGridFSFileInfo DBLoadGridFS(Type type, BsonValue id)
        {
            using (IDBClient client = DBFactory.GetClient(type))
            {
                return client.GridFS.FindOneById(id);
            }
        }
        internal static List<MongoGridFSFileInfo> DBLoadGridFS<TFileType>(string remoteFileName) where TFileType : IMongoFile
        { return DBLoadGridFS(typeof(TFileType), remoteFileName); }
        internal static List<MongoGridFSFileInfo> DBLoadGridFS(Type type, string remoteFileName)
        {
            using (IDBClient client = DBFactory.GetClient(type))
            {
                return client.GridFS.Find(remoteFileName).ToList<MongoGridFSFileInfo>();
            }
        }
        internal static void DBRemoveGridFS<TFileType>(BsonValue id) where TFileType : IMongoFile
        {
            DBRemoveGridFS(typeof(TFileType), id);
        }
        internal static void DBRemoveGridFS(Type type, BsonValue id)
        {
            using (IDBClient client = DBFactory.GetClient(type))
            {
                client.GridFS.DeleteById(id);
            }
        }
        internal static void DBRemoveGridFS<TFileType>(Type type, string[] ids) where TFileType : IMongoFile
        {
            DBRemoveGridFS(typeof(TFileType), ids);
        }
        internal static void DBRemoveGridFS(Type type, string[] ids)
        {
            using (IDBClient client = DBFactory.GetClient(type))
            {
                client.GridFS.Delete(Query<IMongoFile>.Where(f => ids.Contains(f.Id)));
            }
        }
        internal static void DBRemoveGridFS<TFileType>(string remoteFileName) where TFileType : IMongoFile
        { DBRemoveGridFS(typeof(TFileType), remoteFileName); }
        internal static void DBRemoveGridFS(Type type, string remoteFileName)
        {
            using (IDBClient client = DBFactory.GetClient(type))
            {
                client.GridFS.Delete(remoteFileName);
            }
        }

        internal static void DBRemoveAllGridFS<TFileType>() where TFileType : IMongoFile
        {
            DBRemoveAllGridFS(typeof(TFileType));
        }
        internal static void DBRemoveAllGridFS(Type type)
        {
            using (IDBClient client = DBFactory.GetClient(type))
            {
                client.GridFS.Files.RemoveAll();
                client.GridFS.Chunks.RemoveAll();
            }
        }
        internal static void DBDownloadGridFS<TFileType>(string id, string localFileName) where TFileType : IMongoFile
        {
            DBDownloadGridFS(typeof(TFileType), id, localFileName);
        }
        internal static void DBDownloadGridFS(Type type, string id, string localFileName)
        {
            using (Stream stream = File.OpenWrite(localFileName))
            {
                DBDownloadGridFS(type, id, stream);
            }
        }
        internal static void DBDownloadGridFS<TFileType>(string id, Stream stream) where TFileType : IMongoFile
        {
            DBDownloadGridFS(typeof(TFileType), id, stream);
        }
        internal static void DBDownloadGridFS(Type type, string id, Stream stream)
        {
            using (IDBClient client = DBFactory.GetClient(type))
            {
                client.GridFS.Download(stream, Query.EQ("_id", id));
            }
        }

    }

    public static class Utilities
    {
        public static bool IsMongoId(this string id)
        {
            ObjectId oid;
            return ObjectId.TryParse(id, out oid);
        }
        internal static string DBCollectionName(this Type type)
        {
            return type.Name;
        }
    }
}
