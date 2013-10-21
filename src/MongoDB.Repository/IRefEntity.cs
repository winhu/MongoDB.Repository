using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public interface IRefEntity : IEntity, IDBRefContainer
    {
        /// <summary>
        /// list of MongoDBRef
        /// </summary>
        List<MongoDBRef> DBRefs { get; set; }

        /// <summary>
        /// save IEntity first, then save list of MongoDBRef
        /// </summary>
        void Update();
    }
}
