using System.Text;

namespace UnitTest
{
    [TestClass]
    public class PoolTest
    {

        public class TestObject : IPoolable<StringBuilder>
        {
            int IPoolable.Capacity => 1000;

            StringBuilder IPoolable<StringBuilder>.OnCreate()
            {
                return new StringBuilder();
            }

            void IPoolable<StringBuilder>.OnDestroy(StringBuilder obj)
            {

            }

            void IPoolable<StringBuilder>.OnGet(StringBuilder obj)
            {

            }

            void IPoolable<StringBuilder>.OnSet(StringBuilder obj)
            {
                obj.Clear();
            }
        }


        [TestMethod("hhh")]
        public void TestPoolGetSet()
        {
            Pool.Create(new TestObject());

            for (int i = 0; i < 10000; i++)
            {
                StringBuilder builder = Pool.Get<StringBuilder>();

                builder.Append(i);

                Assert.AreEqual(i.ToString(), builder.ToString());
                Pool.Set(builder);
            }
            Assert.AreEqual(1, Pool.Count<StringBuilder>());
        }



        [TestMethod]
        public void TestPoolNullSet()
        {
            Pool.Create(new TestObject());

            bool catched = false;
            try
            {
                Pool.Set<StringBuilder>(null);
            }
            catch (NullReferenceException e)
            {
                catched = true;
            }

            Assert.IsTrue(catched);
        }





        public class TestObject2
        {
            public TestObject2() { }



            public class TestObject2Poolable : IPoolable<TestObject2>
            {

                int IPoolable.Capacity => throw new NotImplementedException();

                TestObject2 IPoolable<TestObject2>.OnCreate()
                {
                    return new TestObject2();
                }

                void IPoolable<TestObject2>.OnDestroy(TestObject2 obj)
                {

                }

                void IPoolable<TestObject2>.OnGet(TestObject2 obj)
                {

                }

                void IPoolable<TestObject2>.OnSet(TestObject2 obj)
                {

                }
            }
        }

        [TestMethod]
        public void TestStaticCreate()
        {

            var obj = Pool.Get<TestObject2, TestObject2.TestObject2Poolable>();
        }

    }
}