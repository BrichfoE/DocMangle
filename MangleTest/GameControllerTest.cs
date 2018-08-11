using DrMangle;
using DrMangle.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangleTest
{
    [TestClass]
    public class GameControllerTest
    {
        [TestMethod]
        public void ComparerTest()
        {
            //setup
            GameController gc = new GameController(true);
            PlayerManager pm = new PlayerManager();
            GameData gd = new GameData("test", 5, 1, new Random());
            gc.AllPlayers = new PlayerData[6];
            for (int i = 0; i < 6; i++)
            {
                gc.AllPlayers[i] = new PlayerData("rand", true);
            }

            gc.AllPlayers[0].Wins = 2;
            gc.AllPlayers[1].Wins = 5;
            gc.AllPlayers[2].Wins = 3;
            gc.AllPlayers[3].Wins = 1;
            gc.AllPlayers[4].Wins = 7;
            gc.AllPlayers[5].Wins = 4;

            //test
            gc.SortPlayersByWins();

            //validate
            Assert.AreEqual(gc.AllPlayers[0].Wins, 7);
            Assert.AreEqual(gc.AllPlayers[1].Wins, 5);
            Assert.AreEqual(gc.AllPlayers[2].Wins, 4);
            Assert.AreEqual(gc.AllPlayers[3].Wins, 3);
            Assert.AreEqual(gc.AllPlayers[4].Wins, 2);
            Assert.AreEqual(gc.AllPlayers[5].Wins, 1);

        }

        [TestMethod]
        public void TestAIBuild()
        {
            //Setup
            GameController gc = new GameController(true);
            GameData gd = new GameData("test", 3, 1, new Random());

            var testAISuccess = new PlayerData("rand", true);
            testAISuccess.Name = "Success";
            for (int i = 0; i < 6; i++)
            {
                testAISuccess.Workshop.Add(new PartData(5 - i, 1, i));
            }

            var testAIFailure = new PlayerData("rand", true);
            testAIFailure.Name = "Failure";
            for (int i = 0; i < 2; i++)
            {
                testAIFailure.Workshop.Add(new PartData(5 - i, 1, i));
            }

            var testAIUpgrade = new PlayerData("rand", true);
            testAIUpgrade.Name = "Upgrade";
            testAIUpgrade.Workshop.Add(new PartData(4, 1, 0));
            testAIUpgrade.Workshop.Add(new PartData(5, 1, 5));
            PartData[] partsHolder = new PartData[6];
            for (int i = 0; i < 6; i++)
            {
                partsHolder[i] = (new PartData(i, 3, 3));
            }
            testAIUpgrade.Monster = new MonsterData("testMonst", partsHolder);

            gd.AiPlayers = new PlayerData[3] { testAISuccess, testAIFailure, testAIUpgrade };
            var expected = new PartData[6];
            for (int i = 0; i < 6; i++)
            {
                expected[i] = new PartData(i, 1, 5 - i);
            }

            //test method
            gc.AIBuildTurn(gd);

            //verify
            var monsterList = gd.AiPlayers[0].Monster.Parts;
            for (int i = 0; i < monsterList.Length; i++)
            {
                Assert.AreEqual(expected[i].PartStructure, monsterList[i].PartStructure);
                Assert.AreEqual(expected[i].PartRarity, monsterList[i].PartRarity);
                Assert.AreEqual(expected[i].PartType, monsterList[i].PartType);
            }
            Assert.AreEqual(0, gd.AiPlayers[0].Workshop.Count);

            Assert.IsNull(gd.AiPlayers[1].Monster);
            Assert.AreEqual(2, gd.AiPlayers[1].Workshop.Count);

            monsterList = gd.AiPlayers[2].Monster.Parts;
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(3, monsterList[i].PartStructure);
                Assert.AreEqual(3, monsterList[i].PartRarity);
                Assert.AreEqual(expected[i].PartType, monsterList[i].PartType);
            }
            Assert.AreEqual(1, monsterList[4].PartStructure);
            Assert.AreEqual(0, monsterList[4].PartRarity);
            Assert.AreEqual(4, monsterList[4].PartType);
            Assert.AreEqual(3, monsterList[5].PartStructure);
            Assert.AreEqual(3, monsterList[5].PartRarity);
            Assert.AreEqual(5, monsterList[5].PartType);

            Assert.AreEqual(0, gd.AiPlayers[2].Workshop.Count);
        }

        [TestMethod]
        public void TestAISearch()
        {
            GameController gc = new GameController(true);
            GameData gd = new GameData("test", 5, 1, new Random());

            var count = 0;
            foreach (var location in gd.CurrentLevel.Locations)
            {
                count += location.PartsList.Count;
            }

            gc.AISearchTurn(gd, 1);

            var newCount = 0;
            foreach (var location in gd.CurrentLevel.Locations)
            {
                newCount += location.PartsList.Count;
            }

            foreach (var ai in gd.AiPlayers)
            {
                Assert.IsNotNull(ai.Bag[0]);
                Assert.IsNull(ai.Bag[1]);
                Assert.IsNull(ai.Bag[2]);
                Assert.IsNull(ai.Bag[3]);
                Assert.IsNull(ai.Bag[4]);
            }
            Assert.AreEqual(count - 5, newCount);
            

        }

        //[TestMethod]
        //public void TestFights()
        //{
        //    GameController gc = new GameController(true);
        //    GameData gd = new GameData("test", 3, 1, new Random());


        //}
    }
}
