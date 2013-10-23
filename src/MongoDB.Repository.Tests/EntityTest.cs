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
            //student.Save();

            MongoEntity.Save<Student>(students);

            var stud = MongoEntity.Get<Student>(student.Id);
            MongoEntity.Get<Student>(s => s.Name == "hyf");
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
            var querable = MongoEntity.Select<Student>(s => s.Age >= 19 && s.Age <= 22, s => s.Age, 1, 2, out pageCount, out allCount).ToList();
            Assert.AreEqual(2, querable.Count);
            Assert.AreEqual(2, pageCount);
            Assert.AreEqual(4, allCount);
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

    }
}
