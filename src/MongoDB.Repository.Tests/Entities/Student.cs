using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class Grade : RefEntity
    {
        public string Name { get; set; }
    }

    public class TestDBContext : MongoDBContext
    {
        public TestDBContext() : base("TestDBContext") { }

        public override void OnRegisterModel(ITypeRegistration registration)
        {
            registration.RegisterType<Student>().RegisterType<Teacher>().RegisterType<Grade>();
        }
    }
}
