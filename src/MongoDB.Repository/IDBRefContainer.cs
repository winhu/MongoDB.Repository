using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Repository
{
    public interface IDBRefContainer
    {
        /// <summary>
        /// exist by id
        /// </summary>
        /// <param name="id">BsonId</param>
        /// <returns></returns>
        bool Exists(string id);
        /// <summary>
        /// exist Type
        /// </summary>
        /// <typeparam name="T">collection Type</typeparam>
        /// <returns></returns>
        bool Exists<T>() where T : IEntity;
        /// <summary>
        /// exist
        /// </summary>
        /// <typeparam name="T">collection Type</typeparam>
        /// <param name="match">Predicate</param>
        /// <returns></returns>
        bool Exists<T>(Predicate<T> match) where T : IEntity;
        /// <summary>
        /// pick on document, load from mongodb if not exist in list
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <param name="id">BsonId</param>
        /// <returns></returns>
        T Pick<T>(string id) where T : IEntity;
        /// <summary>
        /// pick on document, load from mongodb if not exist in DBRefs
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <param name="where">where</param>
        /// <returns></returns>
        T Pick<T>(Expression<Func<T, bool>> where) where T : IEntity;
        /// <summary>
        /// get all documents by collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <returns></returns>
        List<T> GetAll<T>() where T : IEntity;
        /// <summary>
        /// get all documents by collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <param name="where">where</param>
        /// <returns></returns>
        List<T> GetMany<T>(Expression<Func<T, bool>> where) where T : IEntity;
        /// <summary>
        /// add one document, both DBRefs and IDBRefContainer
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <param name="entity">document</param>
        void Add<T>(T entity) where T : IEntity;
        /// <summary>
        /// add documents, both DBRefs and IDBRefContainer
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <param name="entities">documents</param>
        void Add<T>(List<T> entities) where T : IEntity;
        /// <summary>
        /// remove documents from DBRefs, do IEntity.Save() to update to mongodb
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <param name="where">where</param>
        /// <returns></returns>
        int Remove<T>(Expression<Func<T, bool>> where) where T : IEntity;
        /// <summary>
        /// remove document from DBRefs, do IEntity.Save() to update to mongodb
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <param name="entity">document</param>
        void Remove<T>(T entity) where T : IEntity;
        /// <summary>
        /// remove all documents of collection type, do IEntity.Save() to update to mongodb
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        void Remove<T>() where T : IEntity;
        /// <summary>
        /// count collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <returns></returns>
        int Count<T>() where T : IEntity;
        /// <summary>
        /// count collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <param name="where">where</param>
        /// <returns></returns>
        int Count<T>(Expression<Func<T, bool>> where) where T : IEntity;
        /// <summary>
        /// get all documents as IEntity
        /// </summary>
        /// <returns></returns>
        List<IEntity> GetAll();
    }
}
