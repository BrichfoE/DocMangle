using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
    public class LevelData
    {
        public Park[] locations;

        public LevelData()
        {
            locations = new Park[6];
            GeneratePark(locations);
            locations[0] = new Park("Lab", 0);
            locations[5] = new Park("Arena", 0);
            AddParts();
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
                locations[i] = new Park(name, i);
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

        public void AddParts()
        {
            Random r = new Random();
            for (int i = 1; i < 5; i++)
            {

                int roll = r.Next(5, 16);

                for (int j = 0; j < roll; j++)
                {
                    PartData newPart = new PartData(true, 0, locations[i].ParkPart, 0, 0f, 0f, 0f, 0f, 0f);
                    locations[i].PartsList.AddLast(newPart);
                }
            }
            
        }

        public void HalveParts()
        {
            for (int i = 1; i < 5; i++)
            {
                decimal count = locations[i].PartsList.Count;
                for (int j = 0; j < Math.Round((count/2), 0); j++)
                {
                    locations[i].PartsList.RemoveFirst();
                }
            }
        }
        




        //Methods

        //    search level for monster parts
        //    assign remaining monster parts

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