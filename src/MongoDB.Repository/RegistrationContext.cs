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
        IConfigurationRegistration _configuration;
        ITypeRegistration _typeResolver = new TypeRegistration();
        Type _dbContextType;

        public string Code
        {
            get
            {
                if (_dbContextType == null) return null;
                return _dbContextType.FullName;
            }
        }

        public string DBName
        {
            get
            {
                if (_dbContextType == null) return null;
                return _configuration.GetDataBaseName(_dbContextType);
            }
        }

        public bool IsRegisterType<T>()
        {
            return _typeResolver.IsRegisterType<T>();
        }

        public bool IsRegisterType(Type type)
        {
            return _typeResolver.IsRegisterType(type);
        }

        public void RegisterType(Type type)
        {
            _typeResolver.RegisterType(type);
        }

        public void UnregisterType(Type type)
        {
            _typeResolver.UnRegisterType(type);
        }

        public MongoUrl GetMongoUrl()
        {
            if (_dbContextType == null) return null;
            return _configuration.Get(_dbContextType);
        }

        public void RegisterDBContext(IMongoDBContext context)
        {
            _dbContextType = context.GetType();
            this._configuration = context.BuildConfiguration();
            context.OnRegisterModel(_typeResolver);
            _typeResolver.RegisterType<IMongoFile>();
        }

        public void EnsureDBIndex()
        {
            _typeResolver.EnsureDBIndex();
        }

        public void EnsureDBIndex(Type type)
        {
            if (!_typeResolver.IsRegisterType(type)) return;
            _typeResolver.EnsureDBIndex(type);
        }
    }
}
