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
        public static IMongoFile CreateFile(string localFileName, string remoteFileName, string contentType = "application/octet-stream")
        {
            return new MongoFile(localFileName, remoteFileName, contentType);
        }
        /// <summary>
        /// 创建一个GridFS文件
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="remoteFileName">存储文件名</param>
        /// <param name="contentType">文件类型</param>
        /// <returns></returns>
        public static IMongoFile CreateFile(Stream fileStream, string remoteFileName, string contentType = "application/octet-stream")
        {
            return new MongoFile(fileStream, remoteFileName, contentType);
        }
        /// <summary>
        /// 加载一个GridFS文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns></returns>
        public static IMongoFile LoadFile(string id)
        {
            if (!id.IsMongoId()) return null;
            return new MongoFile(EntityOperationExtensions.DBLoadGridFS((BsonValue)id));
        }
        /// <summary>
        /// 加载文件集
        /// </summary>
        /// <param name="remoteFileName">存储文件名</param>
        /// <returns></returns>
        public static List<IMongoFile> LoadAllFiles(string remoteFileName)
        {
            List<IMongoFile> files = new List<IMongoFile>();
            if (string.IsNullOrEmpty(remoteFileName)) return files;
            var infos = EntityOperationExtensions.DBLoadGridFS(remoteFileName);
            if (infos.Count == 0) return files;
            infos.ForEach(delegate(MongoGridFSFileInfo info)
            {
                files.Add(new MongoFile(info));
            });
            return files;
        }
        /// <summary>
        /// 移除所有文件（慎用）
        /// </summary>
        public static void RemoveAllFiles()
        {
            EntityOperationExtensions.DBRemoveAllGridFS();
        }
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="id">文件id</param>
        public static void RemoveFile(string id)
        {
            EntityOperationExtensions.DBRemoveGridFS((BsonValue)id);
        }
        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="remoteFileName">存储文件名</param>
        public static void RemoveFiles(string remoteFileName)
        {
            EntityOperationExtensions.DBRemoveGridFS(remoteFileName);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="localFileName">本地路径（绝对路径）</param>
        public static void DownloadFile(string id, string localFileName)
        {
            EntityOperationExtensions.DBDownloadGridFS(id, localFileName);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="stream">文件流</param>
        public static void DownloadFile(string id, Stream stream)
        {
            EntityOperationExtensions.DBDownloadGridFS(id, stream);
        }
    }
}
