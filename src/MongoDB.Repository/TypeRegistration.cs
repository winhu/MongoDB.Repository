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
        private List<Type> types = new List<Type>();
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
            if (!types.Contains(entityType))
                types.Add(entityType);
            return this;
        }

        public ITypeRegistration UnRegisterType<T>()
        {
            return UnRegisterType(typeof(T));
        }

        public ITypeRegistration UnRegisterType(Type entityType)
        {
            types.Remove(entityType);
            return this;
        }


        public bool IsRegisterType<T>()
        {
            return IsRegisterType(typeof(T));
        }

        public bool IsRegisterType(Type entityType)
        {
            return types.Contains(entityType);
        }


        public List<Type> GetRegisterTypes()
        {
            return types;
        }
    }
}
