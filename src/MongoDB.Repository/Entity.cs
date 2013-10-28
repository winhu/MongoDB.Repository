using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public abstract class Entity : IEntity
    {
        [BsonId]
        [BsonIndex]
        public string Id
        {
            get
            {
                if (_id == ObjectId.Empty)
                    _id = ObjectId.GenerateNewId(DateTime.Now);
                return _id.ToString();
            }
            set
            {
                ObjectId.TryParse(value, out __id);
                if (__id != ObjectId.Empty)
                    _id = __id; ;
            }
        }
        private ObjectId _id;
        private ObjectId __id;


        public virtual void Save()
        {
            //this.DBSave();
            EntityOperationExtensions.DBSave(this.GetType(), this);
        }

        public virtual void Remove()
        {
            this.DBRemove();
        }

        public MongoDBRef ToDBRef()
        {
            return new MongoDBRef(this.GetType().Name, this.Id);
        }
    }
}
