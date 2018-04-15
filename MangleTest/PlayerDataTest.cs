using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrMangle;

namespace MangleTest 
{
    [TestClass]
    public class PlayerDataTest 
    {
        [TestMethod]
        public void ScrapTest()
        {
            PlayerData test = new HumanPlayerData("Test");
            test.Bag[0] = new PartData(0, 0, 0);
            test.Bag[1] = new PartData(1, 1, 1);
            test.Bag[2] = new PartData(2, 2, 2);
            test.Bag[3] = new PartData(3, 3, 3);
            test.Bag[4] = new PartData(4, 4, 4);

            Assert.AreEqual(0, test.Ether);
            test.ScrapItem(test.Bag, 0);
            Assert.AreNotEqual(0, test.Ether);
            Assert.IsTrue(test.Ether < 1000);
            Assert.IsNull(test.Bag[0]);

            Assert.AreEqual(0, test.Meat);
            test.ScrapItem(test.Bag, 2);
            Assert.AreNotEqual(0, test.Meat);
            Assert.IsTrue(test.Meat < 200);
            Assert.IsNull(test.Bag[2]);

            Assert.AreEqual(0, test.Components);
            test.ScrapItem(test.Bag, 3);
            Assert.AreNotEqual(0, test.Components);
            Assert.IsTrue(test.Components < 100);
            Assert.IsNull(test.Bag[3]);

            Assert.AreEqual(0, test.Biomatter);
            test.ScrapItem(test.Bag, 1);
            Assert.AreNotEqual(0, test.Biomatter);
            Assert.IsTrue(test.Biomatter < 500);
            Assert.IsNull(test.Bag[1]);

            Assert.AreEqual(0, test.Rocks);
            test.ScrapItem(test.Bag, 4);
            Assert.AreNotEqual(0, test.Rocks);
            Assert.IsTrue(test.Rocks < 50);
            Assert.IsNull(test.Bag[4]);

            PlayerData test2 = new HumanPlayerData("Test2");
            test2.Workshop.Add(new PartData(0, 0, 0));
            test2.Workshop.Add(new PartData(1, 1, 1));
            test2.Workshop.Add(new PartData(2, 2, 2));
            test2.Workshop.Add(new PartData(3, 3, 3));
            test2.Workshop.Add(new PartData(4, 4, 4));
            test2.Workshop.Add(new PartData(5, 4, 5));
                    
            test2.ScrapItem(test2.Workshop, 4); //originally index 5
            TestList(test2, "4");

            Assert.IsNull(test2.Workshop[5]);

            test2.ScrapItem(test2.Workshop, 0); //originally index 0
            TestList(test2, "0");

            test2.ScrapItem(test2.Workshop, 2); //originally index 3
            TestList(test2, "3");

            test2.ScrapItem(test2.Workshop, 0); //originally index 1
            TestList(test2, "1");

            test2.ScrapItem(test2.Workshop, 1); //originally index 5
            TestList(test2, "5");

            test2.ScrapItem(test2.Workshop, 0); //originally index 2
            TestList(test2, "2");

            Assert.IsNull(test2.Workshop[0]);
        }

        private static void TestList(PlayerData t2, string index)
        {
            switch (index)
            {
                case "0":
                    Assert.IsTrue(t2.Ether < 1000);
                    Assert.AreNotEqual(0, t2.Ether);
                    break;
                case "1":
                    Assert.IsTrue(t2.Biomatter < 500);
                    Assert.AreNotEqual(0, t2.Biomatter);
                    break;
                case "2":
                    Assert.IsTrue(t2.Meat < 200);
                    Assert.AreNotEqual(0, t2.Meat);
                    break;
                case "3":
                    Assert.IsTrue(t2.Components < 100);
                    Assert.AreNotEqual(0, t2.Components);
                    break;
                case "4":
                    Assert.IsTrue(t2.Rocks < 50);
                    Assert.AreNotEqual(0, t2.Rocks);
                    t2.Rocks = 0;
                    break;
                case "5":
                    Assert.IsTrue(t2.Rocks < 10);
                    Assert.AreNotEqual(0, t2.Rocks);
                    t2.Rocks = 0;
                    break;
                default:
                    Assert.Fail();
                    break;
            }
            Assert.IsNull(t2.Workshop[t2.Workshop.Count - 1]);
        }

        [TestMethod]
        public void ComparerTest()
        {
            //setup
            GameController gc = new GameController();
            GameData gd = new GameData("test", 5, 1, new Random());
            var players = new PlayerData[6];
            for (int i = 0; i < 6; i++)
            {
                players[i] = new AIPlayerData(1);
            }

            players[0].Wins = 2;  
            players[1].Wins = 5;  
            players[2].Wins = 3;  
            players[3].Wins = 1;  
            players[4].Wins = 7;  
            players[5].Wins = 4;  

            //test
            gc.SortPlayersByWins(players);

            //validate
            Assert.AreEqual(players[0].Wins, 7);
            Assert.AreEqual(players[1].Wins, 5);
            Assert.AreEqual(players[2].Wins, 4);
            Assert.AreEqual(players[3].Wins, 3);
            Assert.AreEqual(players[4].Wins, 2);
            Assert.AreEqual(players[5].Wins, 1);

        }

        [TestMethod]
        public void DumpTest()
        {
            PlayerData test = new HumanPlayerData("Test");

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
            
            test.DumpBag();

            Assert.AreEqual(test.Workshop.Count, 5);
            Assert.IsNull(test.Bag[0]);
            Assert.IsNull(test.Bag[1]);
            Assert.IsNull(test.Bag[2]);
            Assert.IsNull(test.Bag[3]);
            Assert.IsNull(test.Bag[4]);
        }       
    }
}
