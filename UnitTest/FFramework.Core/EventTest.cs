using FFramework;

namespace UnitTest
{
    [TestClass]
    public class EventTest
    {
        /// <summary>
        /// 
        /// </summary>
        public interface MyTestSendEvent : ISendEvent<string, int> { }

        [TestMethod]
        public void TestSend()
        {
            string x = "HelloWorld";
            int y = 201;
            string expected = x + y;
            FEvent.Instance.Operator<MyTestSendEvent>().Subscribe((x, y) =>
            {
                Console.WriteLine(x + y);
            });
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                FEvent.Instance.Operator<MyTestSendEvent>().Send(x, y);
                var result = sw.ToString().Trim();
                Assert.AreEqual(expected, result);
            }
        }

        public interface MyTestSendAllEvent : ISendEvent<string, int> { }

        [TestMethod]
        public void TestSendAll()
        {
            string x = "HelloWorld";
            int y = 201;
            string expected = (x + y) + (y + x);
            FEvent.Instance.Operator<MyTestSendAllEvent>().Subscribe((x, y) =>
            {
                Console.Write(x + y);
            });
            FEvent.Instance.Operator<MyTestSendAllEvent>().Subscribe((x, y) =>
            {
                Console.Write(y + x);
            });

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                FEvent.Instance.Operator<MyTestSendAllEvent>().Send(x, y);
                var result = sw.ToString().Trim();
                Assert.AreEqual(expected, result);
            }
        }
        public interface MyTestCallEvent : ICallEvent<int, string> { }

        [TestMethod]
        public void TestCallFirst()
        {
            string expected = "123hhh";
            int x = 123;
            FEvent.Instance.Operator<MyTestCallEvent>().Subscribe((x) => x + "hhh");
            var result = FEvent.Instance.Operator<MyTestCallEvent>().CallFirst(x);
            Assert.AreEqual(expected, result);
        }

        public interface MyTestCallAllEvent : ICallEvent<int, string> { }
        [TestMethod]
        public void TestCallAll()
        {
            string expected1 = "123hhh1";
            string expected2 = "123hhh2";
            int x = 123;

            FEvent.Instance.Operator<MyTestCallAllEvent>().Subscribe((x) => x + "hhh1");
            FEvent.Instance.Operator<MyTestCallAllEvent>().Subscribe((x) => x + "hhh2");

            var result = FEvent.Instance.Operator<MyTestCallAllEvent>().Call(x);

            Assert.AreEqual(expected1, result[0]);
            Assert.AreEqual(expected2, result[1]);

        }
    }
}