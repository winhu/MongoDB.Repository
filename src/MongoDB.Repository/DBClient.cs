using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace MongoDB.Repository
{
    public class DBClient : IDBClient
    {
        private MongoClient _client;
        private string _dbName;
        private string _collectionName;
        //Type _type;
        public DBClient(MongoUrl url, Type type)
        {
            if (url == null)
                throw new MongoAuthenticationException(null, "Wrong MongoUrl");
            if (type == null)
                throw new MongoAuthenticationException(null, "Wrong Type");
            _dbName = url.DatabaseName;
            _collectionName = type.DBCollectionName();
            //this._type = type;
            MongoClientSettings setting = MongoClientSettings.FromUrl(url);
            _client = new MongoClient(url);
        }
        public DBClient(MongoUrl url, MongoDBRef dbRef)
        {
            if (url == null)
                throw new MongoAuthenticationException(null, "Wrong MongoUrl");
            if (dbRef == null)
                throw new MongoAuthenticationException(null, "Wrong Collection Value");
            _dbName = dbRef.DatabaseName;
            _collectionName = dbRef.CollectionName;
            //this._type = type;
            MongoClientSettings setting = MongoClientSettings.FromUrl(url);
            _client = new MongoClient(url);
        }

        //public DBClient(MongoClientSettings setting, Type type)
        //{
        //    var credentials = setting.Credentials.ToArray();
        //    if (credentials.Length == 0) throw new MongoAuthenticationException("UnAuthentication MongoClient");
        //    dbName = credentials[0].Source;
        //    this.type = type;
        //    client = new MongoClient(setting);
        //}
        /// <summary>
        /// database name
        /// </summary>
        public string DBName
        {
            get
            {
                return _dbName;
            }
        }
        /// <summary>
        /// MongoCollection
        /// </summary>
        public MongoCollection Collection
        {
            get
            {
                return _client.GetServer().GetDatabase(DBName).GetCollection(_collectionName);
            }
        }

        public Driver.GridFS.MongoGridFS GridFS
        {
            get { return _client.GetServer().GetDatabase(DBName).GridFS; }
        }

        MongoGridFS IDBClient.GridFS => throw new NotImplementedException();

        #region 资源回收
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Close()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_mDisposed)
            {
                if (disposing)
                {
                    _client = null;
                    _collectionName = null;
                    _dbName = null;
                }
                // Release unmanaged resources
                _mDisposed = true;
            }
        }
        ~DBClient()
        {
            Dispose(false);
        }

        private bool _mDisposed;
        #endregion


    }
}
