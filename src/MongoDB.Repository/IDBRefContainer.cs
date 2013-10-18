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
        bool Exists(string id);
        bool Exists<T>() where T : IEntity;
        bool Exists<T>(Predicate<T> match) where T : IEntity;

        T Pick<T>(string id) where T : IEntity;
        T Pick<T>(Expression<Func<T, bool>> where) where T : IEntity;

        List<T> GetAll<T>() where T : IEntity;
        List<T> GetMany<T>(Expression<Func<T, bool>> where) where T : IEntity;

        void Add<T>(T entity) where T : IEntity;
        void Add<T>(List<T> entities) where T : IEntity;

        int Remove<T>(Expression<Func<T, bool>> where) where T : IEntity;
        void Remove<T>(T entity) where T : IEntity;
        void Remove<T>() where T : IEntity;

        int Count<T>() where T : IEntity;
        int Count<T>(Expression<Func<T, bool>> where) where T : IEntity;

        List<IEntity> GetAll();
    }
}
