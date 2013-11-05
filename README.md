MongoDB.Repository
============================================================================================================
宗旨：简化MongoDB的操作，实现类似Entity Framework风格的代码

============================================================================================================
使用方式：

	//定义Student类型
	public class Student : Entity
	{
		[BsonIndex]		//设置索引
		public string Name { get; set; }
		public int Age { get; set; }
	}    
	//定义Teacher类型
	public class Teacher : Entity
	{
        	public string Name { get; set; }
        	public int Age { get; set; }
	}
	//定义RefEntity类型，该类型中含有子集及其操作
	public class Grade : RefEntity
	{
        	public string Name { get; set; }
	}

	//定义上下文
	public class TestDBContext : MongoDBContext
	{
		public TestDBContext() : base("TestDBContext")		//TestDBContext为配置文件中的节点
		{ }

		public override void OnRegisterModel(ITypeRegistration registration)
		{
			registration.RegisterType<Student>().RegisterType<Student>(Teacher).RegisterType<Grade>();
		}
	}
	
	//在程序运行伊始，写入如下两段代码进行注册
	MongoDBRepository.RegisterMongoDBContext(new TestDBContext());		//注册上下文
	MongoDBRepository.RegisterMongoIndex();					//注册索引

	//配置文件中的MongoDB连接字符串节点
	<configuration>
		<connectionStrings>
			<add name="TestDBContext" connectionString="mongodb://localhost:27017/TestMongo"/>
		</connectionStrings>
	</configuration>

	//Entity使用
	//单实体保存
	Student student = new Student()
	student.Name = "hyf";
	student.Age = 30;
	student.Save();

	//集合保存
	MongoEntity.Save(new List<Student>() {
	    new Student{ Name="hyf", Age=33 },
	    new Student{ Name="zhc", Age=30 }
	});

	//查询
	MongoEntity.Get<Student>(student.Id);
	MongoEntity.Get<Student>(s => s.Name == "hyf" && s.Age > 33);
	MongoEntity.Select<Student>(s => s.Age == 30).ToList();
	MongoEntity.Select<Student>(s => s.Age >= 19 && s.Age <= 22, s => s.Age, pageIndex=1, pageSize=2, out pageCount, out allCount).ToList();
	
	//删除
	MongoEntity.RemoveAll<Student>(e => e.Name == "hyf");
	
	//统计
	MongoEntity.Count<Student>(s => s.Age == 30)
	
	//更多操作请参考MongoEntity
	
	//=================================================================================================================
	//RefEntity使用
	//保存
	grade = new Grade();
	grade.Name = "No1";
	foreach (Student student in students)
	    grade.Add<Student>(student);
	foreach (Teacher teacher in teachers)
	    grade.Add<Teacher>(teacher);
	grade.Update();		//保存grade实例及其子集，如使用grade.save()，则只保存grade实例，不保存子集。
	
	//查询子集
	grade.Pick<Student>("BsonId string").Name
	
	//统计子集
	grade.Count<Student>()	//统计当前子集中的Student类型
	
	//更多关于自己的操作如查询，判断，删除等请参考IRefEntity接口


=================================================================================================================

Auther: WinHu

Blog: http://www.cnblogs.com/winhu/

欢迎大家参与并指正，提出更好的意见。
