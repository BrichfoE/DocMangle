using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrMangle;

namespace MangleTest
{
    [TestClass]
    public class ArenaDataTest
    {
        [TestMethod]
        public void TestFightCalculation()
        {
            ArenaBattleCalculator arena = new ArenaBattleCalculator();

            List<PlayerData> fighters = new List<PlayerData>();
            fighters.Add(new PlayerData("TestHuman", false));
            fighters[0].Monster = new MonsterData("test_0", new PartData[] { new PartData(0, 1, 0), new PartData(1, 1, 0), new PartData(2, 1, 0), new PartData(3, 1, 0) });
            for (int i = 1; i < 6; i++)
            {
                fighters.Add(new PlayerData("rand", true));
                fighters[i].Monster = new MonsterData("test_" + i, new PartData[] { new PartData(0, 1, i), new PartData(1, 1, i), new PartData(2, 1, i), new PartData(3, 1, i), });
            }

            PlayerData winner1 = arena.MonsterFight(fighters[0], fighters[5]);
            Console.WriteLine("-------");
            PlayerData winner2 = arena.MonsterFight(fighters[1], fighters[4]);
            Console.WriteLine("-------");
            PlayerData winner3 = arena.MonsterFight(fighters[2], fighters[3]);
            Console.WriteLine("-------");

            Assert.AreEqual(winner1, fighters[0]);
            Assert.AreEqual(winner2, fighters[1]);
            Assert.AreEqual(winner3, fighters[2]);

            PlayerData winner4 = arena.MonsterFight(fighters[1], fighters[2]);
            Console.WriteLine("-------");

            Assert.AreEqual(winner4, fighters[1]);

            PlayerData winner5 = arena.MonsterFight(fighters[0], fighters[1]);
            Console.WriteLine("-------");

            Assert.AreEqual(winner5, fighters[0]);
        }
    }
}
