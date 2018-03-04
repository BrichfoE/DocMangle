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

        //   0   public float partAlacrity;
        //   1   public float partStrength;
        //   2   public float partEndurance;
        //   3   public float partSpecial;
        
        public float partDurability;

        public PartData(bool newPart, int type, int structure, int rarity, float alacrity, float strength, float endurance, float special, float durability)
        {
            stats = new float[4];

            if (newPart)
            {
                GeneratePart();
                if (type != 0)
                {
                    partType = type;
                }
                if (structure != 0)
                {
                    partStructure = structure;
                }
                if (rarity != 0)
                {
                    partRarity = rarity;
                }

                GenerateStats();
                GenerateName();
            }
            else
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

            //if (partAlacrity > 9999)
            //{
            //    partAlacrity = 9999;
            //}
            //if (partStrength > 9999)
            //{
            //    partStrength = 9999;
            //}
            //if (partEndurance > 9999)
            //{
            //    partEndurance = 9999;
            //}
            //if (partSpecial > 9999)
            //{
            //    partSpecial = 9999;
            //}

            

        }

        private void GenerateName()
        {
            string type = Anatomy.typeList[partType];
            string structure = Anatomy.structureList[partStructure];
            string rarity = Anatomy.rarityList[partRarity];

            partName = rarity + " " + structure + " " + type;
        }

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
                , "Human"
                , "Mechanical"
                , "Rock"
                , "Animal"
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

        //static Anatomy()
        //{

        //    var rarityList = new string[6]
        //    {
        //         "Unicorn"
        //       , "Mythic"
        //       , "Legendary"
        //       , "Epic"
        //       , "Rare"
        //       , "Common"
        //    };

        //    var structureList = new string[5]
        //    { 
        //          "Magical"
        //        , "Human"
        //        , "Mechanical"
        //        , "Rock"
        //        , "Animal"
        //    };
        //}
    }           
}
