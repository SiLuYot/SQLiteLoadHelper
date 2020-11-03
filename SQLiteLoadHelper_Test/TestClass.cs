using SQLiteLoadHelper;
using SQLiteLoadHelper.Data;
using SQLiteLoadHelper.Database;
using SQLiteLoadHelper.Sqlite;
using System;
using System.Threading.Tasks;

namespace SQLiteLoadHelper_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass testClass = new TestClass();
            testClass.Test();
        }
    }

    class TestClass
    {
        public void Test()
        {
            var helper = new SQLiteLoadHelperClass("my_data_repository", new SystemSQLite()); //new MonoSQLite()

            helper.AddTaskEvent(
                (v) => Console.WriteLine("진행 퍼센트 : " + v),
                (s) => Console.WriteLine("데이터 이름 : " + s),
                (task) => Console.WriteLine(task.GetType().Name + " 작업 종료"));

            IDataBase[] database = new IDataBase[]
            {
                new TestDataBase()

            };

            Task.Run(() =>
            {
                if (!helper.Process(database))
                {
                    Console.WriteLine(helper.ErrorMsg);
                }
            }).Wait();

            foreach (var data in helper.GetDataList("test"))
            {
                var testData = data as TestData;

                Console.WriteLine(
                    string.Format("{0} {1} {2} {3}",
                    testData.id, testData.name, testData.count, testData.value));
            }
        }
    }

    class TestData : ReadData
    {
        public int id = 0;
        public string name = string.Empty;
        public int count = 0;
        public float value = 0;

        public override int ID => id;
        public override string NAME => name;
    }

    class TestDataBase : DataBase<TestData>
    {
        public override string DBKey => "test";
    }
}
