using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrMangle;
using DrMangle.Service;

namespace MangleTest 
{
    [TestClass]
    public class PlayerDataTest 
    {
        //[TestMethod]
        //public void ScrapTestArray()
        //{
        //    PlayerData test = new HumanPlayerData("Test");
        //    test.Bag[0] = new PartData(0, 0, 0);
        //    test.Bag[1] = new PartData(1, 1, 1);
        //    test.Bag[2] = new PartData(2, 2, 2);
        //    test.Bag[3] = new PartData(3, 3, 3);
        //    test.Bag[4] = new PartData(4, 4, 4);

        //    Assert.AreEqual(0, test.Ether);
        //    test.ScrapItem(test.Bag, 0);
        //    Assert.AreNotEqual(0, test.Ether);
        //    Assert.IsTrue(test.Ether < 1000);
        //    Assert.IsNull(test.Bag[0]);

        //    Assert.AreEqual(0, test.Meat);
        //    test.ScrapItem(test.Bag, 2);
        //    Assert.AreNotEqual(0, test.Meat);
        //    Assert.IsTrue(test.Meat < 200);
        //    Assert.IsNull(test.Bag[2]);

        //    Assert.AreEqual(0, test.Components);
        //    test.ScrapItem(test.Bag, 3);
        //    Assert.AreNotEqual(0, test.Components);
        //    Assert.IsTrue(test.Components < 100);
        //    Assert.IsNull(test.Bag[3]);

        //    Assert.AreEqual(0, test.Biomatter);
        //    test.ScrapItem(test.Bag, 1);
        //    Assert.AreNotEqual(0, test.Biomatter);
        //    Assert.IsTrue(test.Biomatter < 500);
        //    Assert.IsNull(test.Bag[1]);

        //    Assert.AreEqual(0, test.Rocks);
        //    test.ScrapItem(test.Bag, 4);
        //    Assert.AreNotEqual(0, test.Rocks);
        //    Assert.IsTrue(test.Rocks < 50);
        //    Assert.IsNull(test.Bag[4]);
        //}

        [TestMethod]
        public void ScrapTest()
        {
            PlayerData test2 = new PlayerData("Test2", false);
            PlayerManager test2man = new PlayerManager();
            test2.Workshop.Add(new PartData(0, 0, 0));
            test2.Workshop.Add(new PartData(1, 1, 1));
            test2.Workshop.Add(new PartData(2, 2, 2));
            test2.Workshop.Add(new PartData(3, 3, 3));
            test2.Workshop.Add(new PartData(4, 4, 4));
            test2.Workshop.Add(new PartData(5, 4, 5));

            foreach (var part in test2.Workshop)
            {
                part.PartDurability = (decimal)0.5;
            }

            int wsLength = 6;

            test2man.ScrapItem(test2, test2.Workshop, 4); //originally index 5
            wsLength--;
            TestList(test2, "4", wsLength);

            wsLength--;
            test2man.ScrapItem(test2, test2.Workshop, 0); //originally index 0
            TestList(test2, "0", wsLength);

            wsLength--;
            test2man.ScrapItem(test2, test2.Workshop, 2); //originally index 3
            TestList(test2, "3", wsLength);

            wsLength--;
            test2man.ScrapItem(test2, test2.Workshop, 0); //originally index 1
            TestList(test2, "1", wsLength);

            wsLength--;
            test2man.ScrapItem(test2, test2.Workshop, 1); //originally index 5
            TestList(test2, "5", wsLength);

            wsLength--;
            test2man.ScrapItem(test2, test2.Workshop, 0); //originally index 2
            TestList(test2, "2", wsLength);

        }

        private static void TestList(PlayerData t2, string index,int workshopLength)
        {
            switch (index)
            {
                case "0":
                    Assert.IsTrue(t2.ComponentList[0] < 1000);
                    Assert.AreNotEqual(0, t2.ComponentList[0]);
                    break;
                case "1":
                    Assert.IsTrue(t2.ComponentList[1] < 500);
                    Assert.AreNotEqual(0, t2.ComponentList[1]);
                    break;
                case "2":
                    Assert.IsTrue(t2.ComponentList[2] < 200);
                    Assert.AreNotEqual(0, t2.ComponentList[2]);
                    break;
                case "3":
                    Assert.IsTrue(t2.ComponentList[3] < 100);
                    Assert.AreNotEqual(0, t2.ComponentList[3]);
                    break;
                case "4":
                    Assert.IsTrue(t2.ComponentList[4] < 50);
                    Assert.AreNotEqual(0, t2.ComponentList[4]);
                    t2.ComponentList[4] = 0;
                    break;
                case "5":
                    Assert.IsTrue(t2.ComponentList[4] < 10);
                    Assert.AreNotEqual(0, t2.ComponentList[4]);
                    t2.ComponentList[4] = 0;
                    break;
                default:
                    Assert.Fail();
                    break;
            }

            Assert.AreEqual(workshopLength, t2.Workshop.Count);
        }

        [TestMethod]
        public void DumpTest()
        {
            PlayerData test = new PlayerData("Test", false);
            PlayerManager testMan = new PlayerManager();

            for (int i = 0; i < 5; i++)
            {
                test.Bag[i] = new PartData(new Random());
            }

            Assert.AreEqual(test.Workshop.Count, 0);
            Assert.IsNotNull(test.Bag[0]);
            Assert.IsNotNull(test.Bag[1]);
            Assert.IsNotNull(test.Bag[2]);
            Assert.IsNotNull(test.Bag[3]);
            Assert.IsNotNull(test.Bag[4]);
            
            testMan.DumpBag(test);

            Assert.AreEqual(test.Workshop.Count, 5);
            Assert.IsNull(test.Bag[0]);
            Assert.IsNull(test.Bag[1]);
            Assert.IsNull(test.Bag[2]);
            Assert.IsNull(test.Bag[3]);
            Assert.IsNull(test.Bag[4]);
        }

        [TestMethod()]
        public void RepairMonsterTest()
        {
            PlayerData test = new PlayerData("rand", true);
            PlayerManager testMan = new PlayerManager();

            test.ComponentList[0] = 450;
            test.ComponentList[1] = 300;
            test.ComponentList[2] = 450;
            test.ComponentList[3] = 10;
            test.ComponentList[4] = 915;


            test.Monster = new MonsterData("test_0", new PartData[] { new PartData(0, 0, 0), new PartData(1, 1, 1), new PartData(2, 2, 2), new PartData(3, 3, 3), new PartData(4, 4, 4), new PartData(5, 4, 5) });
            for (int i = 0; i < 6; i++)
            {
                test.Monster.Parts[i].PartDurability = (decimal)0.5;
            }

            for (int i = 0; i < 6; i++)
            {
                testMan.RepairMonster(test, i);
            }

            Assert.AreEqual((decimal).95, test.Monster.Parts[0].PartDurability);
            Assert.AreEqual(0, test.ComponentList[0]);
            Assert.AreEqual((decimal)1, test.Monster.Parts[1].PartDurability);
            Assert.AreEqual(50, test.ComponentList[1]);
            Assert.AreEqual((decimal)1, test.Monster.Parts[2].PartDurability);
            Assert.AreEqual(350, test.ComponentList[2]);
            Assert.AreEqual((decimal).6, test.Monster.Parts[3].PartDurability);
            Assert.AreEqual(0, test.ComponentList[3]);
            Assert.AreEqual((decimal)1, test.Monster.Parts[4].PartDurability);
            Assert.AreEqual((decimal)1, test.Monster.Parts[5].PartDurability);
            Assert.AreEqual(885, test.ComponentList[4]);
        }
        

    }
}
