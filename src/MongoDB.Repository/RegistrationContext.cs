using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public class RegistrationContext : IRegistrationContext
    {
        IConfigurationRegistration configuration;
        List<Type> types = new List<Type>();

        public string Code
        {
            get
            {
                if (dbContextType == null) return null;
                return dbContextType.FullName;
            }
        }

        public string DBName
        {
            get
            {
                if (dbContextType == null) return null;
                return configuration.GetDataBaseName(dbContextType);
            }
        }

        public bool IsRegisterType<T>()
        {
            return IsRegisterType(typeof(T));
        }

        public bool IsRegisterType(Type type)
        {
            return types.Exists(t => t == type);
        }

        public void RegisterType(Type type)
        {
            if (IsRegisterType(type)) return;
            types.Add(type);
        }

        public void UnregisterType(Type type)
        {
            types.RemoveAll(t => t == type);
        }

        public MongoUrl GetMongoUrl()
        {
            if (dbContextType == null) return null;
            return configuration.Get(dbContextType);
        }

        private Type dbContextType;
        public void RegisterDBContext(IMongoDBContext context)
        {
            dbContextType = context.GetType();
            this.configuration = context.BuildConfiguration();
            ITypeRegistration typeSolver = new TypeRegistration();
            context.OnRegisterModel(typeSolver);
            types.AddRange(typeSolver.GetRegisterTypes());
        }
    }
}
