using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Repository
{
    public sealed class TypeRegistration : ITypeRegistration
    {
        private string _dbName;
        private IDictionary<Type, string[]> _registrationTypeAndIndex = new Dictionary<Type, string[]>();
        public ITypeRegistration RegisterDatabase(string dbName)
        {
            this._dbName = dbName;
            return this;
        }
        public ITypeRegistration RegisterType<T>()
        {
            return RegisterType(typeof(T));
        }

        public ITypeRegistration RegisterType(Type entityType)
        {
            var indexs = entityType.GetProperties().Where(p => p.CanRead && p.CanWrite && p.CustomAttributes.Any(a => a.AttributeType == typeof(BsonIndexAttribute))).Select(p => p.Name).ToArray<string>();
            _registrationTypeAndIndex.Remove(entityType);
            _registrationTypeAndIndex.Add(entityType, indexs);

            return this;
        }

        public ITypeRegistration UnRegisterType<T>()
        {
            return UnRegisterType(typeof(T));
        }

        public ITypeRegistration UnRegisterType(Type entityType)
        {
            _registrationTypeAndIndex.Remove(entityType);
            return this;
        }


        public bool IsRegisterType<T>()
        {
            return IsRegisterType(typeof(T));
        }

        public bool IsRegisterType(Type entityType)
        {
            return _registrationTypeAndIndex.ContainsKey(entityType);
        }

        public void EnsureDBIndex()
        {
            foreach (var key in _registrationTypeAndIndex.Keys)
            {
                using (IDBClient client = DBFactory.GetClient(key))
                {
                    if (_registrationTypeAndIndex[key].Length > 0)
                        client.Collection.EnsureIndex(_registrationTypeAndIndex[key]);
                }
            }
        }
        public void EnsureDBIndex(Type type)
        {
            if (!_registrationTypeAndIndex.ContainsKey(type)) return;
            using (IDBClient client = DBFactory.GetClient(type))
            {
                client.Collection.EnsureIndex(_registrationTypeAndIndex[type]);
            }
        }
    }
}
