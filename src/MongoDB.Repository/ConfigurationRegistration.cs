using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public sealed class ConfigurationRegistration : IConfigurationRegistration
    {
        private IDictionary<Type, MongoUrl> _configContainer = new Dictionary<Type, MongoUrl>();

        private static object locker = new object();
        /// <summary>
        /// is MongoDB configuration existed
        /// </summary>
        /// <param name="type">MongoDBContext Type</param>
        /// <returns></returns>
        public bool Exist(Type type)
        {
            return _configContainer.ContainsKey(type);
        }
        /// <summary>
        /// add one MongoUrl
        /// </summary>
        /// <param name="type">MongoDBContext Type</param>
        /// <param name="url">MongoUrl</param>
        public void Add(Type type, MongoUrl url)
        {
            lock (locker)
            {
                if (!Exist(type))
                    _configContainer.Add(type, url);
            }
        }
        /// <summary>
        /// remove MongoDB configuration
        /// </summary>
        /// <param name="type">MongoDBContext Type</param>
        public void Remove(Type type)
        {
            lock (locker)
            {
                if (Exist(type))
                    _configContainer.Remove(type);
            }
        }
        /// <summary>
        /// get MongoDB configuration
        /// </summary>
        /// <param name="type">MongoDBContext Type</param>
        /// <returns></returns>
        public MongoUrl Get(Type type)
        {
            return _configContainer[type];
        }
        /// <summary>
        /// get MongoDB database name of configuration of MongoDBContext type
        /// </summary>
        /// <param name="type">MongoDBContext Type</param>
        /// <returns></returns>
        public string GetDataBaseName(Type type)
        {
            if (Exist(type))
                return _configContainer[type].DatabaseName;
            return null;
        }
    }
}
