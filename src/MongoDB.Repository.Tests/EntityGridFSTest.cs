using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.GridFS;
using MongoDB.Repository.Tests.Entities;
using NUnit.Framework;

namespace MongoDB.Repository.Tests
{
    [TestFixture]
    public class EntityGridFSTest : TestBase
    {
        [TestCase]
        public void CreateFileTestCase()
        {
            IMongoFile file = MongoEntity.CreateFile(@"c:\pic1.jpg", "pic1.jpg", "jpg");
            file.Download(@"c:\a.jpg");
            file.Save();
            IMongoFile fs = MongoEntity.LoadFile(file.Id);
            var files = MongoEntity.LoadAllFiles("test.xml");

            MongoEntity.DownloadFile(file.Id, @"c:\copy.jpg");

            Assert.AreEqual(file.Id, fs.Id);
            Assert.IsNull(file.MD5);
            Assert.IsNotNull(fs.MD5);
            Assert.AreEqual(file.Size, fs.Size);
            Assert.AreEqual(file.Data, fs.Data);
        }
    }
}
