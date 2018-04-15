using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
    public class ArenaData
    {
        private Random RNG = new Random();

        public PlayerData MonsterFight(PlayerData blue, PlayerData green)
        {
            PlayerData winner;

            MonsterData bm = blue.Monster;
            MonsterData gm = green.Monster;

            Console.WriteLine("In the blue corner, " + blue.Name + " presents " + blue.Monster.Name);
            StaticUtility.TalkPause(blue.Monster.Name + "boasts " + blue.Monster.MonsterStats);
            Console.WriteLine("In the green corner, " + green.Name + " presents " + green.Monster.Name);

            while (bm.Parts[0].PartDurability > 0 && bm.Parts[1].PartDurability > 0 && gm.Parts[0].PartDurability > 0 && gm.Parts[1].PartDurability > 0)
            {
                MonsterData attack;
                MonsterData reply;
                if (gm.MonsterStats[0] > bm.MonsterStats[0])
                {
                    attack = gm;
                    reply = bm;
                }
                else
                {
                    attack = bm;
                    reply = gm;
                }

                float strike = (RNG.Next(1, 101) /100) * attack.MonsterStats[1];
                float parry = (RNG.Next(1, 101) / 100) * reply.MonsterStats[2];
                float repost = (RNG.Next(1, 101) / 100) * reply.MonsterStats[1];
                float block = (RNG.Next(1, 101) / 100) * attack.MonsterStats[2];
                PartData attackTarget;
                PartData replyTarget;

                if ((RNG.Next(1, 101)) > (attack.MonsterStats[4] / 10000))
                {
                    //add technical to strike to hit head or torso
                    attackTarget = GetTarget(reply, 5, 0);
                }
                else
                {
                    attackTarget = GetTarget(reply, 0, 5);
                }
                if ((RNG.Next(1, 101)) > (reply.MonsterStats[4] / 10000))
                {
                    //add technical to repost to hit head or torso
                    replyTarget = GetTarget(attack, 5, 0);
                }
                else
                {
                    replyTarget = GetTarget(attack, 0, 5);
                }

                //strike vs parry, result decreases random part damage
                float strikeDamage = attackTarget.PartDurability - (strike - parry);
                Console.WriteLine(attack.Name + " swings at " + reply.Name + "'s " + attackTarget.PartName + "!");
                StaticUtility.TalkPause(attackTarget + " goes from " + attackTarget.PartDurability + " to " + (attackTarget.PartDurability - strikeDamage));
                attackTarget.PartDurability = attackTarget.PartDurability - strikeDamage;
                if (attackTarget.PartDurability <= 0)
                {
                    StaticUtility.TalkPause(attackTarget.PartName + " has been destroyed!");
                    attackTarget = null;
                }

                //repost vs block, result decreases random part damage
                float repostDamage = replyTarget.PartDurability - (repost - block);
                Console.WriteLine(reply.Name + " counters at " + attack.Name + "'s " + replyTarget.PartName + "!");
                StaticUtility.TalkPause(attackTarget + " goes from " + replyTarget.PartDurability + " to " + (replyTarget.PartDurability - repostDamage));
                replyTarget.PartDurability = replyTarget.PartDurability - repostDamage;
                if (replyTarget.PartDurability <= 0)
                {
                    StaticUtility.TalkPause(replyTarget.PartName + " has been destroyed!");
                    attackTarget = null;
                }

                for (int i = 0; i < 4; i++)
                {
                    bm.MonsterStats[i] = bm.CalculateStats(i, bm.Parts);
                    gm.MonsterStats[i] = bm.CalculateStats(i, gm.Parts);
                }
            }

            if (bm.Parts[0].PartDurability > 0 && bm.Parts[1].PartDurability > 0)
            {
                winner = blue;
                blue.Wins = blue.Wins + 1;
                blue.Monster.Wins = blue.Monster.Wins + 1;
            }
            else
            {
                winner = green;
                green.Wins = green.Wins + 1;
                green.Monster.Wins = green.Monster.Wins + 1;
            }
            blue.Fights = blue.Fights + 1;
            blue.Monster.Fights = blue.Monster.Fights + 1;
            green.Fights = green.Fights + 1;
            green.Monster.Fights = green.Monster.Fights + 1;

            return winner;
        }

        public PartData GetTarget(MonsterData targetMonster, int start, int end)
        {
            PartData target = null;
            while (target != null)
            {
                if (start < end)
                {
                    for (int i = start; i < end + 1; i++)
                    {
                        if ((RNG.Next(i, end)) == i && targetMonster.Parts[i].PartDurability > 0)
                        {
                            target = targetMonster.Parts[i];
                        }
                    }
                }
                else
                {
                    for (int i = start; i > end - 1; i--)
                    {
                        if ((RNG.Next(i, end)) == end && targetMonster.Parts[i].PartDurability > 0)
                        {
                            target = targetMonster.Parts[i];
                        }
                    }
                }
            }
            return target;
        }

        public void GrantCash(PlayerData playerData, int wins)
        {
            Console.WriteLine("I'll add gold here for equipment eventually!");
        }
    }
}
