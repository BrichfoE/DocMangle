namespace DrMangle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class PlayerData : IComparer<PlayerData>
    {
        public string Name { get; set; }
        
        public int Wins { get; set; }

        public int Fights { get; set; }

        public MonsterData Monster { get; set; }

        public PartData[] Bag { get; set; }

        public List<PartData> Workshop { get; set; }
        
        public int Luck { get; set; }

        public int[] ComponentList { get; set; }

        public decimal Money { get; set; }

        internal PartComparer Comparer { get; set; }

        #region Methods
        public abstract void CheckBag();

        public abstract void ScrapItem(List<PartData> storage, int reference);

        public abstract void RepairMonster(int reference);

        public void DumpWorkshopNulls()
        {
            this.Workshop = this.Workshop.Where(x => x != null).ToList();
        }

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

        public void CheckWorkshop()
        {
            this.Workshop.Sort(this.Comparer);
            Console.WriteLine("Workshop Items:");
            int count = 1;
            foreach (var part in this.Workshop)
            {
                if (part != null)
                {
                    Console.WriteLine(count + " - " + part.PartName);
                    count += 1;
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

        internal int PartListCount(IEnumerable<PartData> list)
        {
            int count = 0;
            foreach (var item in list)
            {
                if (item != null)
                {
                    count += 1;
                }
            }
            return count;
        }
        #endregion
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
            this.ComponentList = new int[5];
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

        public override void ScrapItem(List<PartData> storage, int reference)
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

            amount = (r.Next(high) * (Int32)(part.PartDurability * 100))/100;

            ComponentList[part.PartStructure] = ComponentList[part.PartStructure] + amount;

            storage[reference] = null;
            Console.WriteLine("You salvaged " + amount + " " + StaticReference.structureList[part.PartStructure] + " parts.");

            this.DumpWorkshopNulls();
           // storage.Sort(this.Comparer);
        }

        public override void RepairMonster(int reference)
        {
            PartData part = Monster.Parts[reference];

            int full = 2;
            switch (part.PartRarity)
            {
                case 0:
                    full = 1000;
                    break;
                case 1:
                    full = 500;
                    break;
                case 2:
                    full = 200;
                    break;
                case 3:
                    full = 100;
                    break;
                case 4:
                    full = 50;
                    break;
                case 5:
                    full = 10;
                    break;
                default:
                    throw new Exception("Cannot Repair Unknown PartRarity");
            }

            int cost = ((Int32)((1 - part.PartDurability)*100) * full) / 100;
            if (cost < 0) cost = 0;

            Console.WriteLine("Full repair will cost " + cost + " " + StaticReference.structureList[part.PartStructure] + " parts. You currently have "+ ComponentList[part.PartStructure] + ".");
            Console.WriteLine("Confirm repair?");
            Console.WriteLine("1 - Yes");
            Console.WriteLine("2 - No");
            int intInput = StaticUtility.CheckInput(1, 2);

            if (intInput == 1)
            {
                if (cost <= ComponentList[part.PartStructure])
                {
                    ComponentList[part.PartStructure] = ComponentList[part.PartStructure] - cost;
                    part.PartDurability = 1;
                }
                else
                {
                    part.PartDurability += (1-part.PartDurability) * (ComponentList[part.PartStructure] / cost);
                    ComponentList[part.PartStructure] = 0;
                }
                Console.WriteLine(part.PartName + " is now at " + part.PartDurability + " durability.");
                Console.WriteLine("You now have " + ComponentList[part.PartStructure] + " " + StaticReference.structureList[part.PartStructure] + " parts.");
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
            this.ComponentList = new int[5];
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

            result = StaticReference.adjectives[adjInt] + " " + StaticReference.names[namInt];

            return result;
        }

        public override void ScrapItem(List<PartData> storage, int reference)
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

            amount = (r.Next(high) * (Int32)(part.PartDurability * 100)) / 100;

            ComponentList[part.PartStructure] = ComponentList[part.PartStructure] + amount;

            storage.RemoveAt(reference);
            //Console.WriteLine("You salvaged " + amount + " " + StaticReference.structureList[part.PartStructure] + " parts.");

            this.DumpWorkshopNulls();
            storage.Sort(this.Comparer);
        }

        public override void RepairMonster(int reference)
        {
            PartData part = Monster.Parts[reference];

            int full = 2;
            switch (part.PartRarity)
            {
                case 0:
                    full = 1000;
                    break;
                case 1:
                    full = 500;
                    break;
                case 2:
                    full = 200;
                    break;
                case 3:
                    full = 100;
                    break;
                case 4:
                    full = 50;
                    break;
                case 5:
                    full = 10;
                    break;
                default:
                    throw new Exception("Cannot Scrap Unknown PartRarity");
            }

            int cost = ((Int32)((1 - part.PartDurability) * 100) * full) / 100;
            if (cost < 0) cost = 0;
            if (cost <= ComponentList[part.PartStructure])
            {
                ComponentList[part.PartStructure] = ComponentList[part.PartStructure] - cost;
                part.PartDurability = 1;
            }
            else
            {
                part.PartDurability += (1 - part.PartDurability) * (ComponentList[part.PartStructure] / cost);
                ComponentList[part.PartStructure] = 0;
            }
            //Console.WriteLine(part.PartName + " is now at " + part.PartDurability + " durability.");
            //Console.WriteLine("You now have " + ComponentList[part.PartStructure] + " " + StaticReference.structureList[part.PartStructure] + " parts.");
        }
    }
}
