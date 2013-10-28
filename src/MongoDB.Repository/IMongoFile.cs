using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.GridFS;

namespace MongoDB.Repository
{
    public interface IMongoFile : IEntity
    {
        /// <summary>
        /// 文件名
        /// </summary>
        string FileName { get; set; }

        bool NeedChunk { get; }

        /// <summary>
        /// 文件所在GridFs的Id
        /// </summary>
        string FsId { get; set; }

        /// <summary>
        /// 文件数据
        /// </summary>
        byte[] Data { get; set; }

        /// <summary>
        /// 文件MD5码
        /// </summary>
        string MD5 { get; set; }


        void Load();

        void Load(string id);

        void Attach(MongoGridFSFileInfo gridFSFileInfo);
    }
}
