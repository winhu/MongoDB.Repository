using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Repository
{
    public sealed class TypeRegistration : ITypeRegistration
    {
        private string dbName;
        private IDictionary<Type, string[]> registrationTypeAndIndex = new Dictionary<Type, string[]>();
        public ITypeRegistration RegisterDatabase(string dbName)
        {
            this.dbName = dbName;
            return this;
        }
        public ITypeRegistration RegisterType<T>()
        {
            return RegisterType(typeof(T));
        }

        public ITypeRegistration RegisterType(Type entityType)
        {
            var indexs = entityType.GetProperties().Where(p => p.CanRead && p.CanWrite && p.CustomAttributes.Any(a => a.AttributeType == typeof(BsonIndexAttribute))).Select(p => p.Name).ToArray<string>();
            registrationTypeAndIndex.Remove(entityType);
            registrationTypeAndIndex.Add(entityType, indexs);

            return this;
        }

        public ITypeRegistration UnRegisterType<T>()
        {
            return UnRegisterType(typeof(T));
        }

        public ITypeRegistration UnRegisterType(Type entityType)
        {
            registrationTypeAndIndex.Remove(entityType);
            return this;
        }


        public bool IsRegisterType<T>()
        {
            return IsRegisterType(typeof(T));
        }

        public bool IsRegisterType(Type entityType)
        {
            return registrationTypeAndIndex.ContainsKey(entityType);
        }
        
        public void EnsureDBIndex()
        {
            foreach (var key in registrationTypeAndIndex.Keys)
            {
                using (IDBClient client = DBFactory.GetClient(key))
                {
                    client.Collection.EnsureIndex(registrationTypeAndIndex[key]);
                }
            }
        }
        public void EnsureDBIndex(Type type)
        {
            if (!registrationTypeAndIndex.ContainsKey(type)) return;
            using (IDBClient client = DBFactory.GetClient(type))
            {
                client.Collection.EnsureIndex(registrationTypeAndIndex[type]);
            }
        }
    }
}
