using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
    public class PartData 
    {
        public string partName;

        public int partType;
        public int partStructure;
        public int partRarity;

        public float[] stats;
       
        public float partDurability;

        public PartData()
        {
            stats = new float[4];
            GeneratePart();
            GenerateStats();
            GenerateName();
        }

        public PartData(int type, int structure, int rarity)
        {
        stats = new float[4];

        partType = type;
        partStructure = structure;
        partRarity = rarity;
             
        GenerateStats();
        GenerateName();

        }

        public PartData(int type, int structure, int rarity, float alacrity, float strength, float endurance, float special, float durability)
        { 

        partType = type;
        partStructure = structure;
        partRarity = rarity;

        stats[0] = alacrity;
        stats[1] = strength;
        stats[2] = endurance;
        stats[3] = special;

        partDurability = durability;

        GenerateName();
           
        }


        private void GeneratePart()
        {
            Random r = new Random();
            float rarityRoll;

            partType = r.Next(5);
            partStructure = r.Next(4) + 1;
            rarityRoll = r.Next(1000) + 1;

            if (rarityRoll < 500)
            {
                partRarity = 5;
            }
            else if (rarityRoll < 750)
            {
                partRarity = 4;
            }
            else if (rarityRoll < 900)
            {
                partRarity = 3;
            }
            else if (rarityRoll < 980)
            {
                partRarity = 2;
            }
            else if (rarityRoll < 999)
            {
                partRarity = 1;
            }
            else
            {
                partRarity = 0;
            }

        }

        private void GenerateStats()
        {
            //rolling for each stat
            Random r = new Random();
            float aRoll = r.Next(20) + 1;
            float sRoll = r.Next(20) + 1;
            float eRoll = r.Next(20) + 1;
            float mRoll = r.Next(20) + 1;

            //multipliers for specials
            float aSpecial = 1;
            float sSpecial = 1;
            float eSpecial = 1;
            float mSpecial = 1;

            int rarityMult;

            switch (partType)
            {
                case 1:
                    aSpecial = aSpecial + .2f;
                    sSpecial = sSpecial + .2f;
                    eSpecial = eSpecial + .2f;
                    mSpecial = mSpecial + .5f;
                    break;
                case 2:
                    aSpecial = aSpecial + .1f;
                    sSpecial = sSpecial + .1f;
                    eSpecial = eSpecial + .8f;
                    mSpecial = mSpecial + .1f;
                    break;
                case 3:
                case 4:
                    aSpecial = aSpecial + .2f;
                    sSpecial = sSpecial + .5f;
                    eSpecial = eSpecial + .1f;
                    mSpecial = mSpecial + .1f;
                    break;
                case 5:
                case 6:
                    aSpecial = aSpecial + .5f;
                    sSpecial = sSpecial + .1f;
                    eSpecial = eSpecial + .1f;
                    mSpecial = mSpecial + .1f;
                    break;
                default:
                    break;
            }

            switch (partStructure)
            {
                case 2:
                    mSpecial = mSpecial + 1;
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

            switch (partRarity)
            {
                case 6:
                    rarityMult = 500;
                    partDurability = 100;
                    break;
                case 5:
                    rarityMult = 200;
                    partDurability = ((float)(durabilityRoll/3) + 66) / 100;
                    break;
                case 4:
                    rarityMult = 100;
                    partDurability = ((float)(durabilityRoll * .5f) + 50) / 100;
                    break;
                case 3:
                    rarityMult = 30;
                    partDurability = ((float)((durabilityRoll *.75f) + 25) / 100);
                    break;
                case 2:
                    rarityMult = 15;
                    partDurability = ((float)((durabilityRoll *.9f) + 10) / 100);
                    break;
                default:
                    rarityMult = 5;
                    partDurability = ((float)(durabilityRoll)) / 100;
                    break;
            }


            stats[0] = rarityMult * aRoll * aSpecial;
            stats[1] = rarityMult * sRoll * sSpecial;
            stats[2] = rarityMult * eRoll * eSpecial;
            stats[3] = rarityMult * mRoll * mSpecial;

            for (int i = 0; i < 4; i++)
            {
                if (stats[i] > 16666.5f)
                {
                    stats[i] = 16666.5f;
                }
            }
        }

        private void GenerateName()
        {
            string type = Anatomy.typeList[partType];
            string structure = Anatomy.structureList[partStructure];
            string rarity = Anatomy.rarityList[partRarity];

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
            else if (x.partStructure.CompareTo(y.partStructure) != 0)
            {
                return x.partStructure.CompareTo(y.partStructure);
            }
            else if (x.partRarity.CompareTo(y.partRarity) != 0)
            {
                return x.partRarity.CompareTo(y.partRarity);
            }
            else
            {
                return 0;
            }
        }

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
