using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DrMangle
{
    public class PartData 
    {
        public string PartName
        {
            get
            {
                string type = StaticReference.typeList[PartType];
                string structure = StaticReference.structureList[PartStructure];
                string rarity = StaticReference.rarityList[PartRarity];

                return rarity + " " + structure + " " + type;
            }
        }

        public int PartType { get; set; }
        public int PartStructure { get; set; }
        public int PartRarity { get; set; }

        public float[] Stats { get; set; }

        public decimal PartDurability { get; set; }

        [JsonConstructor]
        public PartData() //empty constructor
        {
            Stats = new float[4];
        }

        public PartData(Random RNG) //completely random part
        {
            Stats = new float[4];
            GeneratePart(RNG);
            GenerateStats();
        }

        public PartData(int type, int structure, int rarity) //random stats and durability
        {
            Stats = new float[4];

            PartType = type;
            PartStructure = structure;
            PartRarity = rarity;

            GenerateStats();
        }

        //[JsonConstructor]
        //public PartData(int type, int structure, int rarity, float alacrity, float[] stats, float durability)  //defined part
        //{ 

        //PartType = type;
        //PartStructure = structure;
        //PartRarity = rarity;

        //Stats[0] = stats[0];
        //Stats[1] = stats[1];
        //Stats[2] = stats[2];
        //Stats[3] = stats[3];

        //PartDurability = durability;           
        //}

        private void GeneratePart(Random RNG)
        {
            //Random r = new Random();
            float rarityRoll;

            PartType = RNG.Next(0, 5);
            PartStructure = RNG.Next(0, 4);
            rarityRoll = RNG.Next(1, 1000);

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
                case 0:
                    rarityMult = 500;
                    PartDurability = 1;
                    break;
                case 1:
                    rarityMult = 200;
                    PartDurability = (decimal)((durabilityRoll/3) + 66) / 100;
                    break;
                case 2:
                    rarityMult = 100;
                    PartDurability = (decimal)((durabilityRoll/2) + 50) / 100;
                    break;
                case 3:
                    rarityMult = 30;
                    PartDurability = (decimal)((durabilityRoll * (decimal).75) + 25) / 100;
                    break;
                case 4:
                    rarityMult = 15;
                    PartDurability = (decimal)((durabilityRoll * (decimal).9) + 10) / 100;
                    break;
                default:
                    rarityMult = 5;
                    PartDurability = (decimal)durabilityRoll / 100;
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
}
