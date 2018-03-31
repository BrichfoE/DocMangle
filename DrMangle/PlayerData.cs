namespace DrMangle
{
    using System;
    using System.Collections.Generic;

    public abstract class PlayerData : IComparer<PlayerData>
    {
        public string Name { get; set; }
        
        public int Wins { get; set; }

        public int Fights { get; set; }

        public MonsterData Monster { get; set; }

        public PartData[] Bag { get; set; }

        public List<PartData> Workshop { get; set; }
        
        public int Luck { get; set; }

        public int Meat { get; set; }

        public int Biomatter { get; set; }

        public int Components { get; set; }

        public int Rocks { get; set; }

        public int Ether { get; set; }

        public decimal Money { get; set; }

        internal PartComparer Comparer { get; set; }

        #region Methods
        public abstract void CheckBag();

        public void DumpBag()
        {
            for (int i = 0; i < this.Bag.Length; i++)
            {
                if (this.Bag[i] != null)
                {
                    this.Workshop.Add(this.Bag[i]);
                    this.Bag[i] = null;
                }
            }

            this.Workshop.Sort(this.Comparer);
        }

        public void ScrapItem(PartData[] storage, int reference)
        {
            PartData part = storage[reference];
            Random r = new Random();
            int high = 2;
            int amount = 1;

            switch (part.PartRarity)
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

            switch (part.PartStructure)
            {
                case 0:
                    this.Ether = this.Ether + amount;
                    break;
                case 1:
                    this.Biomatter = this.Biomatter + amount;
                    break;
                case 2:
                    this.Meat = this.Meat + amount;
                    break;
                case 3:
                    this.Components = this.Components + amount;
                    break;
                case 4:
                    this.Rocks = this.Rocks + amount;
                    break;
                default:
                    throw new Exception("Cannot Scrap Unknown PartStructure");
            }           

            storage[reference] = null;
            Console.WriteLine("You salvaged " + amount + Anatomy.structureList[part.PartStructure] + " parts.");
        }

        public void ScrapItem(List<PartData> storage, int reference)
        {
            PartData part = storage[reference];
            Random r = new Random();
            int high = 2;
            int amount = 1;

            switch (part.PartRarity)
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

            switch (part.PartStructure)
            {
                case 0:
                    this.Ether = this.Ether + amount;
                    break;
                case 1:
                    this.Biomatter = this.Biomatter + amount;
                    break;
                case 2:
                    this.Meat = this.Meat + amount;
                    break;
                case 3:
                    this.Components = this.Components + amount;
                    break;
                case 4:
                    this.Rocks = this.Rocks + amount;
                    break;
                default:
                    throw new Exception("Cannot Scrap Unknown PartStructure");
            }

            storage[reference] = null;
            Console.WriteLine("You salvaged " + amount + Anatomy.structureList[part.PartStructure] + " parts.");
            
            storage.Sort(this.Comparer);
        }

        public void CheckWorkshop()
        {
            this.Workshop.Sort(this.Comparer);
            Console.WriteLine("Workshop Items:");
            foreach (var part in this.Workshop)
            {
                int count = 1;
                if (part != null)
                {
                    Console.WriteLine(count + ": " + part.PartName);
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
            this.Name = playerName;
            this.Wins = 0;
            this.Fights = 0;
            this.Monster = null;
            this.Bag = new PartData[5];
            this.Workshop = new List<PartData>();
            this.Comparer = new PartComparer();
        }

        public override void CheckBag()
        {
            int counter = 1;
            foreach (var part in this.Bag)
            {
                if (part != null)
                {
                    Console.WriteLine(counter + " - " + part.PartName);
                    counter = counter + 1;
                }
            }
        }
    }

    public class AIPlayerData : PlayerData
    {
        public AIPlayerData(int randInt)
        {
            this.Name = this.RandomName(randInt);
            this.Wins = 0;
            this.Fights = 0;
            this.Monster = null;
            this.Bag = new PartData[5];
            this.Workshop = new List<PartData>();
            this.Comparer = new PartComparer();
        }

        public bool IsViewable { get; set; }

        public override void CheckBag()
        {
            if (this.IsViewable)
            {
                int counter = 1;
                foreach (var part in this.Bag)
                {
                    if (part != null)
                    {
                        Console.WriteLine(counter + " - " + part.PartName);
                        counter = counter + 1;
                    }
                }
            }
            else
            {
                Console.WriteLine("Hands Off!");
            }
        }

        private string RandomName(int input)
        {
            string result = string.Empty;
            Random r = new Random();

            int adjInt;
            int namInt;

            adjInt = (input * r.Next(1, 100)) % 10;
            namInt = (input * r.Next(1, 100)) % 10;

            result = Anatomy.adjectives[adjInt] + " " + Anatomy.names[namInt];

            return result;
        }

    #endregion
    }
}
