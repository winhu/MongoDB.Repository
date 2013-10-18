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
        ITypeRegistration typeResolver = new TypeRegistration();
        Type dbContextType;

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
            return typeResolver.IsRegisterType<T>();
        }

        public bool IsRegisterType(Type type)
        {
            return typeResolver.IsRegisterType(type);
        }

        public void RegisterType(Type type)
        {
            typeResolver.RegisterType(type);
        }

        public void UnregisterType(Type type)
        {
            typeResolver.UnRegisterType(type);
        }

        public MongoUrl GetMongoUrl()
        {
            if (dbContextType == null) return null;
            return configuration.Get(dbContextType);
        }

        public void RegisterDBContext(IMongoDBContext context)
        {
            dbContextType = context.GetType();
            this.configuration = context.BuildConfiguration();
            context.OnRegisterModel(typeResolver);
        }

        public void EnsureDBIndex()
        {
            typeResolver.EnsureDBIndex();
        }
        
        public void EnsureDBIndex(Type type)
        {
            if (!typeResolver.IsRegisterType(type)) return;
            typeResolver.EnsureDBIndex(type);
        }
    }
}
