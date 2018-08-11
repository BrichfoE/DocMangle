using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle.Service
{
    public class PlayerManager
    {

        public void CheckBag(PlayerData player)
        {
            if (!player.IsAI)
            {
                int counter = 1;
                foreach (var part in player.Bag)
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

        public void ScrapItem(PlayerData player, List<PartData> storage, int reference)
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

            player.ComponentList[part.PartStructure] = player.ComponentList[part.PartStructure] + amount;

            storage.RemoveAt(reference);
            if (!player.IsAI)
            {
                Console.WriteLine("You salvaged " + amount + " " + StaticReference.structureList[part.PartStructure] + " parts.");
            }

            DumpWorkshopNulls(player);
            storage.Sort(player.Comparer);
        }

        public void RepairMonster(PlayerData player, int reference)
        {
            PartData part = player.Monster.Parts[reference];

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

            int cost = ((Int32)((1 - part.PartDurability) * 100) * full) / 100;
            if (cost < 0) cost = 0;
            int intInput = 1;

            if (!player.IsAI)
            {
                Console.WriteLine("Full repair will cost " + cost + " " + StaticReference.structureList[part.PartStructure] + " parts. You currently have " + player.ComponentList[part.PartStructure] + ".");
                Console.WriteLine("Confirm repair?");
                Console.WriteLine("1 - Yes");
                Console.WriteLine("2 - No");
                intInput = StaticUtility.CheckInput(1, 2);
            }
            if (intInput == 1)
            {
                if (cost <= player.ComponentList[part.PartStructure])
                {
                    player.ComponentList[part.PartStructure] = player.ComponentList[part.PartStructure] - cost;
                    part.PartDurability = 1;
                }
                else
                {  //This could be stated in two lines, but this was easier to debug
                    decimal percentage = ((decimal)player.ComponentList[part.PartStructure] / cost);
                    decimal remaining = (1 - part.PartDurability);
                    part.PartDurability +=  remaining * percentage;
                    player.ComponentList[part.PartStructure] = 0;
                }

                if (!player.IsAI)
                {
                    Console.WriteLine(part.PartName + " is now at " + part.PartDurability + " durability.");
                    Console.WriteLine("You now have " + player.ComponentList[part.PartStructure] + " " + StaticReference.structureList[part.PartStructure] + " parts.");
                }
            }
        }

        public void DumpWorkshopNulls(PlayerData player)
        {
            player.Workshop = player.Workshop.Where(x => x != null).ToList();
        }

        public void DumpBag(PlayerData player)
        {
            for (int i = 0; i < player.Bag.Length; i++)
            {
                if (player.Bag[i] != null)
                {
                    player.Workshop.Add(player.Bag[i]);
                    player.Bag[i] = null;
                }
            } 

            player.Workshop.Sort(player.Comparer);
        }

        public void CheckWorkshop(PlayerData player)
        {
            if (!player.IsAI)
            {
                player.Workshop.Sort(player.Comparer);
                Console.WriteLine("Workshop Items:");
                int count = 1;
                foreach (var part in player.Workshop)
                {
                    if (part != null)
                    {
                        Console.WriteLine(count + " - " + part.PartName);
                        count += 1;
                    }
                }
            }
        }

        

    }
    
}
