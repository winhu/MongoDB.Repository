using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository.Tests.Entities
{
    public class Student : Entity
    {
        [BsonIndex]
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class Teacher : Entity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class Grade : Entity
    {
        public Grade() { Teachers = new List<MongoDBRef>(); }
        public string Name { get; set; }
        public List<MongoDBRef> Teachers { get; set; }
    }

    public class MyFile : MongoFile
    {
        public MyFile()
            : base(@"c:\testxml.xml", "test.xml", "xml")
        { }
    }

    public class TestDBContext : MongoDBContext
    {
        public TestDBContext() : base("TestDBContext") { }

        public override void OnRegisterModel(ITypeRegistration registration)
        {
            registration.RegisterType<Student>().RegisterType<Teacher>().RegisterType<Grade>().RegisterType<MyFile>();
        }
    }
}
