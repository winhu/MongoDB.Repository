using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace MongoDB.Repository
{
    public interface IDBClient : IDisposable
    {
        /// <summary>
        /// database name
        /// </summary>
        string DBName { get; }

        /// <summary>
        /// return MongoCollection
        /// </summary>
        MongoCollection Collection { get; }

        /// <summary>
        /// return MongoGridFS
        /// </summary>
        MongoGridFS GridFS { get; }

        /// <summary>
        /// dispose resources
        /// </summary>
        void Close();
    }
}
