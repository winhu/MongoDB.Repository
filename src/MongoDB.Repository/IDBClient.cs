using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

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
        /// dispose resources
        /// </summary>
        void Close();
    }
}
