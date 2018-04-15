using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DrMangle
{
    public class LevelData
    {
        public Park[] Locations { get; set; }

        [JsonConstructor]
        public LevelData()
        {
            Locations = new Park[6];
        }

        public LevelData(Random RNG, int players)
        {
            Locations = new Park[6];
            GeneratePark(Locations);
            Locations[0] = new Park("Lab", 0);
            Locations[5] = new Park("Arena", 0);
            AddParts(RNG, players);
        }

        private void GeneratePark(Park[] parks)
        {
            for (int i = 1; i < 5; i++)
            {
                string name = "";
                switch (i)
                {
                    case 1:
                        name = "Backyard";
                        break;
                    case 2:
                        name = "Boneyard";
                        break;
                    case 3:
                        name = "Junkyard";
                        break;
                    case 4:
                        name = "Rockyard";
                        break;
                    default:
                        throw new System.ArgumentException("No name for Park[" + i + "]", "original"); ;
                }
                Locations[i] = new Park(name, i);
            }
            
        }

        public class Park
        {
            public string ParkName { get; set; }
            public int ParkPart { get; set; }
            public LinkedList<PartData> PartsList { get; set; }

            public Park(string name, int part)
            {
                ParkName = name;
                ParkPart = part;
                PartsList = new LinkedList<PartData>();
            }
        }

        public void AddParts(Random RNG, int players)
        {
            for (int i = 1; i < 5; i++)
            {
                int roll = RNG.Next(1, players * 5);

                for (int j = 0; j < roll; j++)
                {
                    PartData newPart = new PartData(RNG);
                    newPart.PartStructure = Locations[i].ParkPart;
                    Locations[i].PartsList.AddLast(newPart);
                }
            }
            
        }

        public void HalveParts()
        {
            for (int i = 1; i < 5; i++)
            {
                decimal count = Locations[i].PartsList.Count;
                for (int j = 0; j < Math.Round((count/2), 0); j++)
                {
                    Locations[i].PartsList.RemoveFirst();
                }
            }
        }
        
    }
}


//public class LevelData
//{
//    public PartData[] backyard;
//    public PartData[] boneyard;
//    public PartData[] junkyard;
//    public PartData[] rockyard;

//    public LevelData()
//    {
//        backyard = new PartData[10];
//        boneyard = new PartData[10];
//        junkyard = new PartData[10];
//        rockyard = new PartData[10];

//        GenerateSection(backyard, 1);
//        GenerateSection(boneyard, 2);
//        GenerateSection(junkyard, 3);
//        GenerateSection(rockyard, 4);
//    }


//    private void GenerateSection(PartData[] section, int structure)
//    {
//        for (int i = 0; i < section.Length; i++)
//        {
//            section[i] = new PartData(true, 0, structure, 0, 0f, 0f, 0f, 0f, 0f);
//        }
//    }
//}