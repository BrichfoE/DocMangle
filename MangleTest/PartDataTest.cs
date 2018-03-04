using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrMangle;

namespace MangleTest 
{
    [TestClass]
    public class PartDataTest 
    {
        [TestMethod]
        public void TestMethod1()
        {
            PartData head = new PartData(false, 0, 1, 1, 50f, 50f, 50f, 50f, 100f);
        }
    }
}
