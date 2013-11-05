MongoDB.Repository
============================================================================================================
宗旨：简化MongoDB的操作方式，实现类似Entity Framework的编码风格。

============================================================================================================

	//定义实体类型Student
    public class Student : Entity
    {
        [BsonIndex]		//设置索引
        public string Name { get; set; }
        public int Age { get; set; }
    }    
	//定义实体类型Teacher
	public class Teacher : Entity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
	//定义实体类型，继承自RefEntity的类型将有集合操作
    public class Grade : RefEntity
    {
        public string Name { get; set; }
    }

	//定义上下文，并注册定义的实体类型
    public class TestDBContext : MongoDBContext
    {
        public TestDBContext() : base("TestDBContext")		//设置数据库连接字符串
		{ }

        public override void OnRegisterModel(ITypeRegistration registration)
        {
            registration.RegisterType<Student>().RegisterType<Grade>();
        }
	}
	
	//在应用程序运行伊始，加入如下两段代码
	MongoDBRepository.RegisterMongoDBContext(new TestDBContext());		//注册对上下文
    MongoDBRepository.RegisterMongoIndex();								//注册索引，如类型中没有使用BsonIndexAttribute索引，则不需要

	//配置文件中配置相应的连接字符串节点
	<configuration>
		<connectionStrings>
			<add name="TestDBContext" connectionString="mongodb://localhost:27017/TestMongo"/>
		</connectionStrings>
	</configuration>

=================================================================================================================

//单实保存
Student student = new Student()
student.Name = "hyf";
student.Age = 30;
student.Save();

//单实集合保存
MongoEntity.Save(new List<Student>() {
    new Student{ Name="hyf", Age=33 },
    new Student{ Name="zhc", Age=30 }
});

//实体查询
MongoEntity.Get<Student>(student.Id);
MongoEntity.Get<Student>(s => s.Name == "hyf" && s.Age > 33);
MongoEntity.Select<Student>(s => s.Age == 30).ToList();
MongoEntity.Select<Student>(s => s.Age >= 19 && s.Age <= 22, s => s.Age, pageIndex=1, pageSize=2, out pageCount, out allCount).ToList();

//删除操作
MongoEntity.RemoveAll<Student>(e => e.Name == "hyf");

//统计
MongoEntity.Count<Student>(s => s.Age == 30)

//其它操作，请参考MongoEntity

=================================================================================================================

//添加
grade = new Grade();
grade.Name = "No1";
foreach (Student student in students)
    grade.Add<Student>(student);
foreach (Teacher teacher in teachers)
    grade.Add<Teacher>(teacher);
grade.Update();		//将grade及其子集中的Student和Teacher保存至数据，保存规则均为无则添加，有则更新。

//查询子集中的数据
grade.Pick<Student>("BsonId string").Name

//子集统计
grade.Count<Student>()	//统计子集中所有Student类型

//关于子集的其它操作，请参考IRefEntity接口

=================================================================================================================

Auther:WinHu
Blog:http://www.cnblogs.com/winhu/

欢迎大家使用并提出修改意见。