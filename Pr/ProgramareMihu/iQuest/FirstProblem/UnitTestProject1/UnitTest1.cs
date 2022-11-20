using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FirstProblem;


namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            WhiteHats test = new WhiteHats();
            int [] testarray={1,1,1,2};
            Assert.AreEqual(test.whiteNumber(testarray), -1);
            
        }
        [TestMethod]
        public void TestMethod2()
        {
            WhiteHats test = new WhiteHats();
            int[] testarray = { 2, 1, 1 };
            Assert.AreEqual(test.whiteNumber(testarray), 2);

        }
        [TestMethod]
        public void TestMethod3()
        {
            WhiteHats test = new WhiteHats();
            int[] testarray = { 0,0 };
            Assert.AreEqual(test.whiteNumber(testarray), 0);

        }
         [TestMethod]
        public void TestMethod4()
        {
            WhiteHats test = new WhiteHats();
            int[] testarray = { 10, 10 };
            Assert.AreEqual(test.whiteNumber(testarray),-1);

        }
         [TestMethod]
        public void TestMethod5()
        {
            WhiteHats test = new WhiteHats();
            int[] testarray = { 2,2,2};
            Assert.AreEqual(test.whiteNumber(testarray), 3);

        }
    }
}
