using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Repository.Tests.Entities;
using NUnit.Framework;

namespace MongoDB.Repository.Tests
{
    public class TestBase
    {
        protected List<Student> students;
        protected List<Teacher> teachers;
        protected List<Grade> grades;
        [SetUp]
        public void Setup()
        {
            MongoDBRepository.RegisterMongoDBContext(new TestDBContext());

            students = new List<Student>() {
                new Student{ Name="hyf", Age=33 },
                new Student{ Name="zhc", Age=30 },
                new Student{ Name="a", Age=18 },
                new Student{ Name="b", Age=19 },
                new Student{ Name="c", Age=20 },
                new Student{ Name="d", Age=21 },
                new Student{ Name="e", Age=22 },
                new Student{ Name="f", Age=23 }
            };
            teachers = new List<Teacher>() {
                new Teacher{ Name="Lee", Age=53 },
                new Teacher{ Name="Chen", Age=50 }
            };
            grades = new List<Grade>() {
                new Grade{  Students=students, Teachers=teachers}
            };

            Entity.Save(students);
            Entity.Save(teachers);
            Entity.Save(grades);
        }

        [TearDown]
        public void Clear()
        {
            Entity.RemoveAll<Student>();
            Entity.RemoveAll<Teacher>();
            Entity.RemoveAll<Grade>();
        }
    }
}
