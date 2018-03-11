using System;
using System.Collections.Generic;


namespace DrMangle
{
    public abstract class PlayerData : IComparer<PlayerData>
    {

        public string Name { get; set; }
        public int Wins { get; set; }
        public int Fights { get; set; }

        public MonsterData Monster { get; set; }
        public PartData[] Bag { get; set; }
        public List<PartData> Workshop { get; set; }
        public PartComparer _comparer;

        public int Luck { get; set; }
        public int Meat { get; set; }
        public int Biomatter { get; set; }
        public int Components { get; set; }
        public int Rocks { get; set; }
        public int Ether { get; set; }
        public decimal Money { get; set; }

        //Methods
        public abstract void CheckBag();

        public void DumpBag()
        {
            for (int i = 0; i < Bag.Length; i++)
            {
                if (Bag[i] != null)
                {
                    Workshop.Add(Bag[i]);
                    Bag[i] = null;
                }
            }
            Workshop.Sort(_comparer);
        }

        public void ScrapItem(PartData[] storage, int reference)
        {
            PartData part = storage[reference];
            Random r = new Random();
            int high = 2;
            int amount = 1;

            switch (part.partRarity)
            {
                case 0:
                    high = 1000;
                    break;
                case 1:
                    high = 500;
                    break;
                case 2:
                    high = 200;
                    break;
                case 3:
                    high = 100;
                    break;
                case 4:
                    high = 50;
                    break;
                case 5:
                    high = 10;
                    break;
                default:
                    throw new Exception("Cannot Scrap Unknown PartRarity");
            }

            amount = r.Next(high);

            switch (part.partStructure)
            {
                case 0:
                    Ether = Ether + amount;
                    break;
                case 1:
                    Biomatter = Biomatter + amount;
                    break;
                case 2:
                    Meat = Meat + amount;
                    break;
                case 3:
                    Components = Components + amount;
                    break;
                case 4:
                    Rocks = Rocks + amount;
                    break;
                default:
                    throw new Exception("Cannot Scrap Unknown PartStructure");
            }           

            storage[reference] = null;
            Console.WriteLine("You salvaged " + amount + Anatomy.structureList[part.partStructure] + " parts.");
        }

        public void ScrapItem(List<PartData> storage, int reference)
        {
            PartData part = storage[reference];
            Random r = new Random();
            int high = 2;
            int amount = 1;

            switch (part.partRarity)
            {
                case 0:
                    high = 1000;
                    break;
                case 1:
                    high = 500;
                    break;
                case 2:
                    high = 200;
                    break;
                case 3:
                    high = 100;
                    break;
                case 4:
                    high = 50;
                    break;
                case 5:
                    high = 10;
                    break;
                default:
                    throw new Exception("Cannot Scrap Unknown PartRarity");
            }

            amount = r.Next(high);

            switch (part.partStructure)
            {
                case 0:
                    Ether = Ether + amount;
                    break;
                case 1:
                    Biomatter = Biomatter + amount;
                    break;
                case 2:
                    Meat = Meat + amount;
                    break;
                case 3:
                    Components = Components + amount;
                    break;
                case 4:
                    Rocks = Rocks + amount;
                    break;
                default:
                    throw new Exception("Cannot Scrap Unknown PartStructure");
            }

            storage[reference] = null;
            Console.WriteLine("You salvaged " + amount + Anatomy.structureList[part.partStructure] + " parts.");
            
            storage.Sort(_comparer);
        }

        public void CheckWorkshop()
        {
            Workshop.Sort(_comparer);
            Console.WriteLine("Workshop Items:");
            foreach (var part in Workshop)
            {
                int count = 1;
                if (part != null)
                {
                    Console.WriteLine(count + ": " + part.partName);
                }

            }
        }

        public int Compare(PlayerData x, PlayerData y)
        {
            if (x.Wins.CompareTo(y.Wins) != 0)
            {
                return x.Wins.CompareTo(y.Wins);
            }
            else if (x.Fights.CompareTo(y.Fights) != 0)
            {
                return x.Fights.CompareTo(y.Fights);
            }
            else if (x.Name.CompareTo(y.Name) != 0)
            {
                return x.Name.CompareTo(y.Name);
            }
            else
            {
                return 0;
            }
        }
    }

    public class HumanPlayerData : PlayerData
    {
        public HumanPlayerData(string playerName)
        {
            Name = playerName;
            Wins = 0;
            Fights = 0;
            Monster = null;
            Bag = new PartData[5];
            Workshop = new List<PartData>();
            _comparer = new PartComparer();

        }

        public override void CheckBag()
        {
            int counter = 1;
            foreach (var part in this.Bag)
            {
                if (part != null)
                {
                    Console.WriteLine(counter + " - " + part.partName);
                    counter = counter + 1;
                }
            }
        }

    }

    public class AIPlayerData : PlayerData
    {
        public AIPlayerData(int randInt)
        {
            Name = RandomName(randInt);
            Wins = 0;
            Fights = 0;
            Monster = null;
            Bag = new PartData[5];
            Workshop = new List<PartData>();
            _comparer = new PartComparer();
        }
        bool IsViewable { get; set; }

        public override void CheckBag()
        {
            if (IsViewable)
            {
                int counter = 1;
                foreach (var part in this.Bag)
                {
                    if (part != null)
                    {
                        Console.WriteLine(counter + " - " + part.partName);
                        counter = counter + 1;
                    }
                }
            }
            else
	        {
                Console.WriteLine("Hands Off!");
            }
        }

        private string[] adjectives;
        private string[] names;

        private string RandomName(int input)
        {
            string result = "";
            Random r = new Random();

            int adjInt;
            int namInt;

            if (adjectives == null || names == null)
            {
                adjectives = new string[10] { "Cool", "Nice", "Mad", "Helpful", "Thin", "Dirty", "Slick", "Ugly", "Super", "Octogenarian" };
                names = new string[10] { "Luke", "Matilda", "Martha", "Hannah", "Pete", "Harry", "Rick", "Veronica", "Susan", "Maynard" };
            }

            adjInt = (input * r.Next()) % 10;
            namInt = (input * r.Next()) % 10;

            result = adjectives[adjInt] + " " + names[namInt];

            return result;
        }
    }

    //public class LoadPlayerData : PlayerData
    //{
    //    protected LoadPlayerData(string playerName, int fightsWon, int fightsParticipated, MonsterData currentMonster, PartData[] currentBag, PartData[] currentShop)
    //    {
    //        Name = playerName;
    //        Wins = fightsWon;
    //        Fights = fightsParticipated;
    //        Monster = currentMonster;
    //        Bag = currentBag;
    //        Workshop = currentShop;
    //    }
    //}
}







