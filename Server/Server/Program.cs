using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;

namespace FFramework
{

    class ServerEntryPoint
    {
        protected static IMongoDatabase _database;
        protected static IMongoClient _client;

        class MyClass2
        {
            public int sex = 1;
        }
        class MyClass1
        {
            public string name = "XiaoLi";
            public int age  = 10;
            public MyClass2 SexInfo = new MyClass2();
        }
       
        private static void Main(string[] args)
        {
            const string collectionName = "Student";
            // 读取连接字符串
            string strCon ="mongodb://127.0.0.1:27017/test";
            var mongoUrl = new MongoUrlBuilder(strCon);
            // 获取数据库名称
            string databaseName = mongoUrl.DatabaseName;
            // 创建并实例化客户端
            _client = new MongoClient(mongoUrl.ToMongoUrl());
            //  根据数据库名称实例化数据库
            _database = _client.GetDatabase(databaseName);

          
            // 根据集合名称获取集合
            var collection = _database.GetCollection<BsonDocument>(collectionName);


            //var s = BsonDocument.Parse("{stuId:1,name:\"张三\",age:18,subject:\"语文\",score:90}");
            BsonDocument doc = new MyClass1().ToBsonDocument();
            collection.InsertOne(doc);
            //collection.InsertOne(new MyClass2().ToBsonDocument());
            //var filter = new BsonDocument();
            //// 查询集合中的文档
            //var list = Task.Run(async () => await collection.Find(filter).ToListAsync()).Result;
            //// 循环遍历输出
            //list.ForEach(p =>
            //  {
            //      Console.WriteLine("编号：" + p["stuId"] + ",姓名：" + p["name"].ToString() + ",年龄:" + p["age"].ToString() + ",课程：" + p["subject"].ToString() + ",成绩：" + p["score"].ToString());
            //  });

            Console.ReadKey();
        }
    }
}