using System;
using System.Reflection.Metadata;
using lab1;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            UserTable TestTable = new UserTable();
            TestTable.InitData(3, 4);
            Assert.AreEqual(3, TestTable.N, "n is not correct");
            Assert.AreEqual(4, TestTable.M, "m is not correct");
            Assert.AreEqual(3, TestTable.List.Count, "List rows is not correct");
            for (int i = 0; i < 3; ++i)
            {
                Assert.AreEqual(4 + 1, TestTable.List[i].Count, "i-th row is not correct");
            }
        }
        [TestMethod]
        public void TestMethod2()
        {
            UserTable TestTable = new UserTable();
            Tuple<int, int> actual = TestTable.TryParseCell("  B3");
            Tuple<int, int> expexted = new Tuple<int, int>(1, 2);
            Assert.AreEqual(expexted, actual, "not parsed");

            actual = TestTable.TryParseCell("1  C ");
            expexted = new Tuple<int, int>(-1, -1);
            Assert.AreEqual(expexted, actual, "parsing is not correct");
        }
        [TestMethod]
        public void TestMethod3()
        {
            UserTable TestTable = new UserTable();
            string TestS = "3<5 or 3>5";
            Expression res = TestTable.GetStringExpression(TestS);
            Assert.AreEqual(res.Calculated, true, "calculated is wrong");
            Assert.AreEqual(res.Is_correct, true, "is_correct is wrong");
            Assert.AreEqual(res.Value, 1, "value is wrong");

            TestS = "not (4=2*2) and (6=2*3)";
            res = TestTable.GetStringExpression(TestS);
            Assert.AreEqual(res.Calculated, true, "calculated is wrong");
            Assert.AreEqual(res.Is_correct, true, "is_correct is wrong");
            Assert.AreEqual(res.Value, 0, "value is wrong");

            TestS = "6-3=3 or not";
            res = TestTable.GetStringExpression(TestS);
            Assert.AreEqual(res.Calculated, true, "calculated is wrong");
            Assert.AreEqual(res.Is_correct, false, "is_correct is wrong");

            TestS = "1/0 and 1";
            res = TestTable.GetStringExpression(TestS);
            Assert.AreEqual(res.Calculated, true, "calculated is wrong");
            Assert.AreEqual(res.Is_correct, false, "is_correct is wrong");
        }
    }
}