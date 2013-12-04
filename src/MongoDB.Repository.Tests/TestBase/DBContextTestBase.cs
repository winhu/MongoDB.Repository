using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MongoDB.Repository.Tests
{
    public class Employee : Entity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class Company : Entity
    {
        public Company() { Employees = new List<Employee>(); }

        public string Name { get; set; }
        public string Address { get; set; }

        public List<Employee> Employees { get; set; }
    }

    public class Factory : Entity
    {
        public Factory() { Workers = new List<Worker>(); }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Worker> Workers { get; set; }
    }

    public class Worker : Entity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class CompanyDBContext : MongoDBContext
    {
        public CompanyDBContext() : base("CompanyDBContext") { }
        public override void OnRegisterModel(ITypeRegistration registration)
        {
            registration.RegisterType<Company>().RegisterType<Employee>().RegisterType<Factory>();
        }
    }

    public class FactoryDBContext : MongoDBContext
    {
        public FactoryDBContext() : base("FactoryDBContext") { }
        public override void OnRegisterModel(ITypeRegistration registration)
        {
            registration.RegisterType<Factory>().RegisterType<Worker>();
        }
    }

    public class DBContextTestBase
    {
        [TestFixtureSetUp]
        public void Setup()
        {
        }

        [TestFixtureTearDown]
        public void Clear()
        {
        }
    }
}
