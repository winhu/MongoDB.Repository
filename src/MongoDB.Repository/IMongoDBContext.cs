using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Repository
{
    public interface IMongoDBContext
    {
        /// <summary>
        /// register entity type
        /// </summary>
        /// <param name="registration"></param>
        void OnRegisterModel(ITypeRegistration registration);

        /// <summary>
        /// name of ConnectionString in config file
        /// </summary>
        string ConnectionStringName { get; }

        /// <summary>
        /// build Configuration by config file
        /// </summary>
        /// <returns></returns>
        IConfigurationRegistration BuildConfiguration();
    }
}
