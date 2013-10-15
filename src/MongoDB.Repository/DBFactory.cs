using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public class DBFactory
    {
        /// <summary>
        /// get one MongoDBClient by collection type
        /// </summary>
        /// <param name="type">collection type</param>
        /// <returns></returns>
        public static IDBClient GetClient(Type type)
        {
            if (!MongoDBRepository.IsRegisterType(type)) throw new MongoException("Unregistered type");
            var url = MongoDBRepository.GetConfig(type);
            return new DBClient(url, type);
        }
        /// <summary>
        /// get one MongoDBClient by collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <returns></returns>
        public static IDBClient GetClient<T>() where T : IEntity
        {
            return GetClient(typeof(T));
        }
    }
}
