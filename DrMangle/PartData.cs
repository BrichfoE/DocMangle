using System;
using System.Collections.Generic;

namespace DrMangle
{
    public class PartData 
    {
        public string partName;

        public int PartType { get; set; }
        public int PartStructure { get; set; }
        public int PartRarity { get; set; }

        public float[] Stats { get; set; }

        public float PartDurability { get; set; }

        public PartData() //completely random part
        {
            Stats = new float[4];
            GeneratePart();
            GenerateStats();
            GenerateName();
        }

        public PartData(int type, int structure, int rarity) //random stats and durability
        {
        Stats = new float[4];

        PartType = type;
        PartStructure = structure;
        PartRarity = rarity;
             
        GenerateStats();
        GenerateName();

        }

        public PartData(int type, int structure, int rarity, float alacrity, float strength, float endurance, float special, float durability) //defined part
        { 

        PartType = type;
        PartStructure = structure;
        PartRarity = rarity;

        Stats[0] = alacrity;
        Stats[1] = strength;
        Stats[2] = endurance;
        Stats[3] = special;

        PartDurability = durability;

        GenerateName();
           
        }

        private void GeneratePart()
        {
            Random r = new Random();
            float rarityRoll;

            PartType = r.Next(5);
            PartStructure = r.Next(4) + 1;
            rarityRoll = r.Next(1000) + 1;

            if (rarityRoll < 500)
            {
                PartRarity = 5;
            }
            else if (rarityRoll < 750)
            {
                PartRarity = 4;
            }
            else if (rarityRoll < 900)
            {
                PartRarity = 3;
            }
            else if (rarityRoll < 980)
            {
                PartRarity = 2;
            }
            else if (rarityRoll < 999)
            {
                PartRarity = 1;
            }
            else
            {
                PartRarity = 0;
            }
        }

        private void GenerateStats()
        {
            //rolling for each stat
            Random r = new Random();
            float aRoll = r.Next(20) + 1;
            float sRoll = r.Next(20) + 1;
            float eRoll = r.Next(20) + 1;
            float tRoll = r.Next(20) + 1;

            //multipliers for specials
            float aSpecial = 1;
            float sSpecial = 1;
            float eSpecial = 1;
            float tSpecial = 1;

            int rarityMult;

            switch (PartType)
            {
                case 1:
                    aSpecial = aSpecial + .2f;
                    sSpecial = sSpecial + .2f;
                    eSpecial = eSpecial + .2f;
                    tSpecial = tSpecial + .5f;
                    break;
                case 2:
                    aSpecial = aSpecial + .1f;
                    sSpecial = sSpecial + .1f;
                    eSpecial = eSpecial + .8f;
                    tSpecial = tSpecial + .1f;
                    break;
                case 3:
                case 4:
                    aSpecial = aSpecial + .2f;
                    sSpecial = sSpecial + .5f;
                    eSpecial = eSpecial + .1f;
                    tSpecial = tSpecial + .1f;
                    break;
                case 5:
                case 6:
                    aSpecial = aSpecial + .5f;
                    sSpecial = sSpecial + .1f;
                    eSpecial = eSpecial + .1f;
                    tSpecial = tSpecial + .1f;
                    break;
                default:
                    break;
            }

            switch (PartStructure)
            {
                case 2:
                    tSpecial = tSpecial + 1;
                    break;
                case 4:
                    eSpecial = eSpecial + 1;
                    break;
                case 3:
                    sSpecial = sSpecial + 1f;
                    break;
                case 1:
                    aSpecial = aSpecial + 1f;
                    break;
                default:
                    break;
            }

            int durabilityRoll = r.Next(1, 100);

            switch (PartRarity)
            {
                case 6:
                    rarityMult = 500;
                    PartDurability = 100;
                    break;
                case 5:
                    rarityMult = 200;
                    PartDurability = ((float)(durabilityRoll/3) + 66) / 100;
                    break;
                case 4:
                    rarityMult = 100;
                    PartDurability = ((float)(durabilityRoll * .5f) + 50) / 100;
                    break;
                case 3:
                    rarityMult = 30;
                    PartDurability = ((float)((durabilityRoll *.75f) + 25) / 100);
                    break;
                case 2:
                    rarityMult = 15;
                    PartDurability = ((float)((durabilityRoll *.9f) + 10) / 100);
                    break;
                default:
                    rarityMult = 5;
                    PartDurability = ((float)(durabilityRoll)) / 100;
                    break;
            }


            Stats[0] = rarityMult * aRoll * aSpecial;
            Stats[1] = rarityMult * sRoll * sSpecial;
            Stats[2] = rarityMult * eRoll * eSpecial;
            Stats[3] = rarityMult * tRoll * tSpecial;

            for (int i = 0; i < 4; i++)
            {
                if (Stats[i] > 16666.5f)
                {
                    Stats[i] = 16666.5f;
                }
            }
        }

        private void GenerateName()
        {
            string type = Anatomy.typeList[PartType];
            string structure = Anatomy.structureList[PartStructure];
            string rarity = Anatomy.rarityList[PartRarity];

            partName = rarity + " " + structure + " " + type;
        }
    }

    public class PartComparer : IComparer<PartData>
    {
        public int Compare(PartData x, PartData y)
        {
            if (x == null && y != null)
            {
                return 1;
            }
            else if (x != null && y == null)
            {
                return -1;
            }
            else if (x == null && y == null)
            {
                return 0;
            }
            else if (x.PartStructure.CompareTo(y.PartStructure) != 0)
            {
                return x.PartStructure.CompareTo(y.PartStructure);
            }
            else if (x.PartRarity.CompareTo(y.PartRarity) != 0)
            {
                return x.PartRarity.CompareTo(y.PartRarity);
            }
            else
            {
                return 0;
            }
        }

///////////////////////////equality implementation if I ever need to do non-reference equality
        //public override bool Equals(object obj)
        //{
        //    return base.Equals(obj);
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //public override string ToString()
        //{
        //    return base.ToString();
        //}


    }

    public static class Anatomy
    {
        public static string[] typeList = new string[6]
            {
                  "Head"
                , "Torso"
                , "Left Arm"
                , "Right Arm"
                , "Left Leg"
                , "Right Leg"
            };
        public static string[] structureList = new string[5]
            {
                  "Magical"
                , "Animal"
                , "Human"
                , "Mechanical"
                , "Rock"
            };
        public static string[] rarityList = new string[6]
            {
                 "Unicorn"
               , "Mythic"
               , "Legendary"
               , "Epic"
               , "Rare"
               , "Common"
            };
        public static string[] statList = new string[4]
            {
                  "Alacrity"
                , "Strength"
                , "Endurance"
                , "Technique"
            };
    }
}
