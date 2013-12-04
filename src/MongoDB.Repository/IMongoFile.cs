using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace MongoDB.Repository
{
    public interface IMongoFile : IEntity
    {
        /// <summary>
        /// real IMongoFile entity type
        /// </summary>
        Type RealEntityType { get; }

        /// <summary>
        /// remote file name
        /// </summary>
        string RemoteFileName { get; set; }

        /// <summary>
        /// local file name
        /// </summary>
        string LocalFileName { get; set; }

        /// <summary>
        /// data
        /// </summary>
        byte[] Data { get; }

        /// <summary>
        /// MD5
        /// </summary>
        string MD5 { get; }

        /// <summary>
        /// content type
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// size
        /// </summary>
        int Size { get; }

        /// <summary>
        /// alias
        /// </summary>
        string[] Aliases { get; }
        /// <summary>
        /// metadata
        /// </summary>
        BsonDocument Metadata { get; set; }

        /// <summary>
        /// upload date
        /// </summary>
        DateTime UploadDate { get; set; }

        /// <summary>
        /// add alias
        /// </summary>
        /// <param name="alias">alias</param>
        void AddAlias(string alias);
        /// <summary>
        /// add aliases
        /// </summary>
        /// <param name="aliases">aliases</param>
        void AddAliases(string[] aliases);

        /// <summary>
        /// download file
        /// </summary>
        /// <param name="localFileName">abs file full path</param>
        void Download(string localFileName);
        /// <summary>
        /// download file
        /// </summary>
        /// <param name="stream">file stream</param>
        void Download(Stream stream);
    }
}
