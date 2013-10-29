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
    public class RefEntityTest : TestBase
    {
        [TestFixtureSetUp]
        public void prepare()
        {
            grade = new Grade();
            grade.Name = "No1";
            foreach (Student student in students)
                grade.Add<Student>(student);
            foreach (Teacher teacher in teachers)
                grade.Add<Teacher>(teacher);
        }

        Grade grade;
        [TestCase]
        public void TestAdd()
        {
            grade.Update();

            students[0].Name = "NameChanged";
            students[0].Save();

            var g = MongoEntity.Get<Grade>(grade.Id);

            Assert.AreSame(students[0].Name, grade.Pick<Student>(students[0].Id).Name);
            Assert.AreNotSame(grade.Pick<Student>(students[0].Id).Name, g.Pick<Student>(students[0].Id).Name);
            Assert.AreEqual(grade.Count<Student>(), g.Count<Student>());
            Assert.AreEqual(grade.Count<Teacher>(), g.Count<Teacher>());

            Assert.AreEqual(g.Count<Student>(), MongoEntity.Select<Student>(s => s.Age > 0).Count());
        }

        [TestCase]
        public void TestUpdate()
        {
            Student student2 = grade.Pick<Student>(students[2].Id);
            student2.Age = 100;
            grade.Update();

            var g = MongoEntity.Get<Grade>(grade.Id);

            Assert.AreEqual(student2.Age, g.Pick<Student>(student2.Id).Age);
        }
    }
}
