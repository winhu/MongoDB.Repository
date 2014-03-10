using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace MongoDB.Repository
{
    public class MongoEntity
    {
        private static bool IsTypeCanBeUsed(Type type)
        {
            if (type.IsAbstract || type.IsInterface || type.IsEnum)
                throw new MongoException(string.Format("Type of {0} can not be used, because of it is not IEntity", type.FullName));
            if (!type.GetConstructors().Any(c => c.GetParameters().Length == 0))
                throw new MongoException(string.Format("Type of {0} can not be used, because of it has not less parameters constructor", type.FullName));
            return true;
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static bool Exists<T>(string id) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBExists<T>(id);
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="where">查询表达式</param>
        /// <returns></returns>
        public static bool Exists<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBExists<T>(where);
        }
        public static List<T> GetAll<T>() where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBFindAll<T>();
        }

        /// <summary>
        /// 获取一个实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static T Get<T>(string id) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBFind<T>(id);
        }
        /// <summary>
        /// 获取一个实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="where">查询表达式</param>
        /// <returns></returns>
        public static T Get<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBFind<T>(where);
        }
        /// <summary>
        /// 移除一个实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static bool Remove<T>(string id) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBRemove<T>(id);
        }
        /// <summary>
        /// 移除所有实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static long RemoveAll<T>() where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBRemoveAll<T>();
        }
        /// <summary>
        /// 移除所有实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="where">查询表达式</param>
        /// <returns></returns>
        public static long RemoveAll<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBRemoveAll<T>(where);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="where">查询表达式</param>
        /// <returns></returns>
        public static IQueryable<T> Select<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBSelect<T>(where);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="where">查询表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页容量</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="allCount">总条目数</param>
        /// <returns></returns>
        public static IQueryable<T> Select<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, int pageIndex, int pageSize, out int pageCount, out int allCount) where T : IEntity
        {
            pageCount = 0;
            allCount = 0;
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBSelect<T>(where, orderby, pageIndex, pageSize, out pageCount, out allCount);
        }
        /// <summary>
        /// 合计
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static long Count<T>() where T : IEntity
        {
            return EntityOperationExtensions.DBCount<T>();
        }
        /// <summary>
        /// 合计
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="where">查询表达式</param>
        /// <returns></returns>
        public static long Count<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            return EntityOperationExtensions.DBCount<T>(where);
        }
        /// <summary>
        /// 批量保存（不存在为插入，已存在为更新）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entities">实例集合</param>
        public static void Save<T>(List<T> entities) where T : IEntity
        {
            entities.DBSave<T>();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        public static void Save<T>(T entity) where T : IEntity
        {
            EntityOperationExtensions.DBSave(typeof(T), entity);
        }
        /// <summary>
        /// 批量插入（已存在则不插入）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entities">实例集合</param>
        public static void InsertBatch<T>(List<T> entities) where T : IEntity
        {
            EntityOperationExtensions.DBInsertBatch<T>(entities);
        }

        /// <summary>
        /// 创建一个GridFS文件
        /// </summary>
        /// <param name="localFileName">本地路径（绝对路径）</param>
        /// <param name="remoteFileName">存储文件名</param>
        /// <param name="contentType">文件类型</param>
        /// <returns></returns>
        public static IMongoFile CreateFile<TMongoFileType>(string localFileName, string remoteFileName, string contentType = "application/octet-stream") where TMongoFileType : IMongoFile
        {
            return new MongoFile<TMongoFileType>(localFileName, remoteFileName, contentType);
        }
        /// <summary>
        /// 创建一个GridFS文件
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="remoteFileName">存储文件名</param>
        /// <param name="contentType">文件类型</param>
        /// <returns></returns>
        public static IMongoFile CreateFile<TMongoFileType>(Stream fileStream, string remoteFileName, string contentType = "application/octet-stream") where TMongoFileType : IMongoFile
        {
            return new MongoFile<TMongoFileType>(fileStream, remoteFileName, contentType);
        }
        /// <summary>
        /// 加载一个GridFS文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns></returns>
        public static IMongoFile LoadFile<TMongoFileType>(string id) where TMongoFileType : IMongoFile
        {
            if (!id.IsMongoId()) return null;
            return new MongoFile<TMongoFileType>(EntityOperationExtensions.DBLoadGridFS<TMongoFileType>((BsonValue)id));
        }
        /// <summary>
        /// 加载文件集
        /// </summary>
        /// <param name="remoteFileName">存储文件名</param>
        /// <returns></returns>
        public static List<IMongoFile> LoadAllFiles<TMongoFileType>(string remoteFileName) where TMongoFileType : IMongoFile
        {
            List<IMongoFile> files = new List<IMongoFile>();
            if (string.IsNullOrEmpty(remoteFileName)) return files;
            var infos = EntityOperationExtensions.DBLoadGridFS<TMongoFileType>(remoteFileName);
            if (infos.Count == 0) return files;
            infos.ForEach(delegate(MongoGridFSFileInfo info)
            {
                files.Add(new MongoFile<TMongoFileType>(info));
            });
            return files;
        }
        /// <summary>
        /// 移除所有文件（慎用）
        /// </summary>
        public static void RemoveAllFiles<TMongoFileType>() where TMongoFileType : IMongoFile
        {
            EntityOperationExtensions.DBRemoveAllGridFS<TMongoFileType>();
        }
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="id">文件id</param>
        public static void RemoveFile<TMongoFileType>(string id) where TMongoFileType : IMongoFile
        {
            EntityOperationExtensions.DBRemoveGridFS<TMongoFileType>((BsonValue)id);
        }
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="remoteFileName">存储文件名</param>
        public static void RemoveFiles<TMongoFileType>(string remoteFileName) where TMongoFileType : IMongoFile
        {
            EntityOperationExtensions.DBRemoveGridFS<TMongoFileType>(remoteFileName);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="localFileName">本地路径（绝对路径）</param>
        public static void DownloadFile<TMongoFileType>(string id, string localFileName) where TMongoFileType : IMongoFile
        {
            EntityOperationExtensions.DBDownloadGridFS<TMongoFileType>(id, localFileName);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="stream">文件流</param>
        public static void DownloadFile<TMongoFileType>(string id, Stream stream) where TMongoFileType : IMongoFile
        {
            EntityOperationExtensions.DBDownloadGridFS<TMongoFileType>(id, stream);
        }
    }
}
