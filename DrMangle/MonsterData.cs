using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
    public abstract class MonsterBase
    {
        public int Wins { get; set; }
        public int Fights { get; set; }
        public string Name { get; set; }
    }

    public class MonsterGhost : MonsterBase

    {
        public int DeathDay { get; set; }

        [JsonConstructor]
        public MonsterGhost() { }

        public MonsterGhost(MonsterData deceased, int day)
        {
            Name = deceased.Name;
            Wins = deceased.Wins;
            Fights = deceased.Fights;
            DeathDay = day;
        }
    }

    public class MonsterData : MonsterBase
    {
        public PartData[] Parts {get; set;}
        public float[] MonsterStats { get; set; }

        [JsonConstructor]
        public MonsterData()
        {
            MonsterStats = new float[4];
        }

        public MonsterData(string newName, PartData[] newParts)
        {
            Name = newName;
            Parts = newParts;

            MonsterStats = new float[4];

            for (int i = 0; i < MonsterStats.Length; i++)
            {
                MonsterStats[i] = CalculateStats(i, Parts);
            }

        }

        public float CalculateStats(int stat, PartData[] bodyParts)
        {
            float newStat = 0;

            for (int i = 0; i < bodyParts.Length; i++)
            {
                if (bodyParts[i] != null)
                {
                    newStat = newStat + (bodyParts[i].Stats[stat] * (float)bodyParts[i].PartDurability);
                }
            }

            return newStat;
        }

        public bool CanFight()
        {
            bool canFight = false;
            bool head = false;
            bool torso = false;
            bool limb = false;            

            foreach (var part in Parts)
            {
                if (part != null && part.PartDurability > 0)
                {
                    switch (part.PartType)
                    {
                        case 0:
                            head = true;
                            break;
                        case 1:
                            torso = true;
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            limb = true;
                            break;
                        default:
                            break;
                    }
                }
                if (head && torso && limb)
                {
                    canFight = true;
                    return canFight;
                }
            }

            return canFight;
        }

        //monster
            //display monster
            //add part to monster
            //remove part from monster
            //activate monster
            //scrap monster
            //repair mosnter

        //buffs
    }
}
