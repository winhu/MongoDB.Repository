using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Repository.Tests.Entities;
using NUnit.Framework;

namespace MongoDB.Repository.Tests
{
    [TestFixture]
    public class EntityTest : TestBase
    {
        [TestFixtureSetUp]
        public void prepare()
        {
            MongoEntity.Save(students);
        }
        [TestCase]
        public void TestSave()
        {
            Student student = new Student();
            student.Name = "hyf";
            student.Age = 30;
            student.Save();
            MongoEntity.Save<Student>(students);
            var stud = MongoEntity.Get<Student>(student.Id);
            MongoEntity.Get<Student>(s => s.Name == "hyf" && s.Age > 33);
            Assert.AreEqual(student.Name, stud.Name);
            Assert.AreEqual(student.Age, stud.Age);
        }

        [TestCase]
        public void TestSelect()
        {
            var students = MongoEntity.Select<Student>(s => s.Age == 30).ToList();
            Assert.IsNotEmpty(students);
        }

        [TestCase]
        public void TestSelectPaged()
        {
            int pageCount, allCount;
            //MongoEntity.Save<Student>(students);
            var querable = MongoEntity.Select<Student>(s => s.Age >= 19 && s.Age <= 40, s => s.Age, 0, 2, out pageCount, out allCount).ToList();
            Assert.AreEqual(2, querable.Count);
            Assert.AreEqual(2, pageCount);
            Assert.AreEqual(3, allCount);
            MongoEntity.Save(new List<Student>() {
                new Student{ Name="hyf", Age=33 },
                new Student{ Name="zhc", Age=30 }
            });
        }

        [TestCase]
        public void TestRemove()
        {
            long count = MongoEntity.RemoveAll<Student>(e => e.Name == "hyf");
            var ret = MongoEntity.Select<Student>(s => s.Name == "hyf").Count();
            Assert.Greater(count, ret);
        }

        [TestCase]
        public void TestEntityList()
        {
            School school = new School() { Name = "WinStudio" };

            List<Department> depts = new List<Department>(){
                new Department(){ Name="Master"},
                new Department(){ Name="Admin"}
            };
            foreach (Department dept in depts)
            {
                school.Departments.Add(dept);
            }

            foreach (Student student in students)
            {
                school.Students.Add(student);
            }
            school.Save();

            School sch = MongoEntity.Get<School>(school.Id);
            Console.WriteLine(sch.Name);
        }
    }
}
