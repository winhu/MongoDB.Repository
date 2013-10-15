using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public interface IConfigurationRegistration
    {
        void Add(Type type, MongoUrl url);
        bool Exist(Type type);
        void Remove(Type type);
        MongoUrl Get(Type type);
        string GetDataBaseName(Type type);
    }
}
