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
            //加载本地文件，并实例一个IMongoFile
            IMongoFile file = MongoEntity.CreateFile<MyFile>(@"c:\pic1.jpg", "pic2.jpg", "jpg");

            //下载文件，等同于文件另存为 
            file.Download(@"c:\beforesave.jpg");

            //文件保存至数据库 
            file.Save();

            //从数据中加载刚才保存的文件
            IMongoFile fs = MongoEntity.LoadFile<MyFile>(file.Id);

            //将从数据中加载的文件下载
            fs.Download(@"c:\aftersave.jpg");

            //根据数据库中的文件名检索文件
            var files = MongoEntity.LoadAllFiles<MyFile>("pic2.jpg");

            //根据文件id，将数据库中的文件下载到本地
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
