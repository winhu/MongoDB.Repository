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
            IMongoFile file = MongoEntity.CreateFile<MyFile>(@"c:\pic1.jpg", "pic2.jpg", "jpg");
            file.Download(@"c:\beforesave.jpg");
            file.Save();
            IMongoFile fs = MongoEntity.LoadFile<MyFile>(file.Id);
            fs.Download(@"c:\aftersave.jpg");
            var files = MongoEntity.LoadAllFiles<MyFile>("pic2.jpg");

            MongoEntity.DownloadFile<MyFile>(file.Id, @"c:\copy.jpg");

            Assert.AreEqual(file.Id, fs.Id);
            Assert.AreEqual(1, files.Count);
            Assert.AreEqual(file.Id, files[0].Id);
            Assert.IsNull(file.MD5);
            Assert.IsNotNull(fs.MD5);
            Assert.AreEqual(file.Size, fs.Size);
            Assert.AreEqual(file.Data, fs.Data);
        }
    }
}
