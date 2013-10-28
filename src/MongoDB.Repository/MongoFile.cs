using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.GridFS;

namespace MongoDB.Repository
{
    public abstract class MongoFile : Entity, IMongoFile
    {
        public MongoFile(string remoteFileName, bool needChunk = false)
        {
            _fileName = remoteFileName;
            _needChunk = needChunk;
        }
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string FsId { get; set; }

        public byte[] Data { get; set; }

        public int Length { get { return _length; } }
        private int _length;

        public string MD5 { get; set; }

        public bool NeedChunk { get { return _needChunk; } }
        private bool _needChunk;

        public virtual void Attach(MongoGridFSFileInfo file)
        {
            if (file == null) throw new MongoGridFSException(this.ToString());
            MD5 = file.MD5;
            _length = file.OpenRead().Read(this.Data, 0, (int)file.Length);

        }

        public virtual void Load()
        {
            Load(this.Id);
        }

        public virtual void Load(string id)
        {
            Attach(EntityOperationExtensions.DBLoadGridFS(this.GetType(), id));
        }

        public override void Save()
        {
            Attach(EntityOperationExtensions.DBSaveGridFS(this));
        }

        public override void Remove()
        {
            EntityOperationExtensions.DBRemoveGridFS(this.GetType(), this.Id);
        }


    }
}
