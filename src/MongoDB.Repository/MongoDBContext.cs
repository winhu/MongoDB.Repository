using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public abstract class MongoDBContext : IMongoDBContext
    {
        private string _connectionStringName;
        public string ConnectionStringName { get { return _connectionStringName; } }
        public MongoDBContext(string connectionStringName)
        {
            this._connectionStringName = connectionStringName;
        }

        public abstract void OnRegisterModel(ITypeRegistration registration);

        IConfigurationRegistration configuration;
        public IConfigurationRegistration BuildConfiguration()
        {
            if (configuration != null) return configuration;

            var setting = ConfigurationManager.ConnectionStrings[_connectionStringName];
            if (setting == null) throw new MongoConnectionException("Wrong ConnectionString Name");
            var url = new MongoUrl(setting.ConnectionString);
            configuration = new ConfigurationRegistration();
            configuration.Add(this.GetType(), url);
            return configuration;
        }
    }
}
