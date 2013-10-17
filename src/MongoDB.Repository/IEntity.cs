using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public interface IEntity
    {
        /// <summary>
        /// mongo id
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// save document
        /// </summary>
        void Save();
        /// <summary>
        /// remove document
        /// </summary>
        void Remove();
        /// <summary>
        /// convert to mongodbref
        /// </summary>
        /// <returns></returns>
        MongoDBRef ToDBRef();
    }
}
