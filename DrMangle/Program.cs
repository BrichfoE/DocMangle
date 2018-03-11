using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
    class Program
    {
        

        static void Main(string[] args)
        {
            #region initialization
            string textInput;
            int intInput;
            bool activeGame = true;
            //bool halt;


            //Need to work in save data here in a bit
            Console.WriteLine("Welcome to the Isle of Dr. Mangle.");
            Console.WriteLine("Please enter a name for your game data:");
            textInput = Console.ReadLine();
            Console.WriteLine("And how many contestants will you be competing against?");
            intInput = CheckInput(1, 7);
            GameData gd = new GameData(textInput, 3);


            #endregion

            #region introCutscene
            //TalkPause("[You find yourself standing before a stately manor house on tropical island.]");
            //TalkPause("[The evening is overcast and forebodeing, and you wait outside a grand door.]");
            //halt = true;
            //while (halt)
            //{
            //    TalkPause("[You wait a moment.]");
            //    Console.WriteLine("[Do you knock?]Y/N");
            //    textInput = Console.ReadLine();
            //    if (textInput == "Y") { halt = false; }
            //}
            //TalkPause("[The door opens swiftly, and a small strange man appears.]");
            //TalkPause("Strangeman: Welcome to the Isle of Dr. Mangle.");
            //Console.WriteLine("Strangeman:  And your name is?");
            //GetPlayerName(pd);
            //TalkPause("Strangeman: Your research fellowship will begin immidiately.");
            //TalkPause("Strangeman: However, it will not proceed exaclty as advertised.");
            //"Follow me to the rear of the house."
            //"[He takes you there]."
            //"Here is your collection bag.  It has specialized compartments for hauling the materials you will need."
            //"Specifically, you can only carry six items.  At any time if you say 'Bag' you will look and see what you are carrying."
            //"Try that now."
            #endregion

            while (activeGame)
            {

            #region search
            TalkPause("A new day has dawned!");
            TalkPause("The parks will be open for 5 hours...");
            TalkPause("You will then have one more hour in your labs before the evening's entertainment.");

            for (int i = 1; i < 6; i++)
            {
                TalkPause("It is currently "+i+" o'clock. The parks close at 6.");
                ShowRegions(gd);
                ShowTurnOptions(gd, i-1);
            }
            #endregion

            #region build
            TalkPause("It is now 6 o'clock. Return to your lab and prepare for the floorshow at 7.");
            gd.currentRegion = 0;
            gd.SetRegionText();
            ShowTurnOptions(gd, 6);

            #endregion

            #region fight
            TalkPause("Welcome to the evening's entertainment!");
            if (gd.currentPlayer.Monster != null && gd.currentPlayer.Monster.CanFight())
            {
                Console.WriteLine("Would you like to particpate tonight?");
                TalkPause("1 - Yes, 2 - No");
                intInput = CheckInput(1, 2);
                if (intInput != 1)
                {
                    TalkPause("Well, maybe tomorrow then...");
                    Console.WriteLine("Let's find you a comfortable seat.");
                    
                }
                else
                {
                    TalkPause("Let the games begin!");
                }
            }
            else
            {
                TalkPause("Seeing as you do not have a living, able bodied contestant...");
                Console.WriteLine("Let's find you a comfortable seat.");  
            }
            CalculateFights(gd);

                #endregion

            gd.SortByWins();
            gd.currentLevel.AddParts();
            
            }

        }



        #region gameDataFunctions
        private static void GetGameData(string input)
        {


            try
            {
                //check if input matches save game
                //then load that game
                //else


            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        #endregion

        #region bodyFunctions

        private static void ShowRegions(GameData gd)
        {
            int intInput;
            Console.WriteLine("You are currently in the "+gd.regionText + ",");

            Console.WriteLine("what will you do next?");
            for (int i = 0; i < 5; i++)
            {
                if (gd.currentRegion == i)
                {
                    Console.WriteLine(i + " - Stay in the " + gd.currentLevel.locations[i].ParkName);
                }
                else
                {
                    Console.WriteLine(i + " - Go to the " + gd.currentLevel.locations[i].ParkName);
                }
            }

            intInput = CheckInput(0, 4);
            gd.currentRegion = intInput;
            gd.SetRegionText();
        }

        private static void ShowTurnOptions(GameData gd, int bagSlot)
        {
            int intInput;
            Console.WriteLine("Welcome to the " + gd.regionText + "! Here you can: ");
            if (gd.currentRegion == 0)
            {
                Console.WriteLine("1 - Work on the monster");
                Console.WriteLine("2 - Scrap unwanted parts");
                Console.WriteLine("3 - Look in bag");
                Console.WriteLine("4 - Go to another region");
            }
            else
            {
                Console.WriteLine("1 - Search for parts");
                Console.WriteLine("2 - Scan for parts");
                Console.WriteLine("3 - Look in bag");
                Console.WriteLine("4 - Go to another region");
            }

            intInput = CheckInput(1, 4);
            //if in lab
            if (gd.currentRegion == 0)
            {
                gd.currentPlayer.CheckWorkshop();
                if (intInput == 2)
                {
                    TalkPause("Which Item would you like to scrap?");

                }
                else if (intInput == 1)
                {
                    if (gd.currentPlayer.Monster == null)
                    {
                        gd.currentPlayer.Monster = BuildMonster(gd, true);
                    }
                    else
                    {
                        BuildMonster(gd, false);
                    }
                }
            }
            //if in parks
            else
            {
                if (intInput == 2)
                {
                    foreach (var park in gd.currentLevel.locations)
                    {
                        Console.WriteLine("There are " + park.PartsList.Count + " parts left in the " + park.ParkName + ".");
                    }
                }

                else if (intInput == 1)
                {
                    if (gd.currentLevel.locations[gd.currentRegion].PartsList.Count == 0)
                    {
                        Console.WriteLine("There are no more parts in this region");
                    }
                    else
                    {
                        gd.currentPlayer.Bag[bagSlot] = gd.currentLevel.locations[gd.currentRegion].PartsList.Last();
                        gd.currentLevel.locations[gd.currentRegion].PartsList.RemoveLast();
                        Console.WriteLine("You found: " + gd.currentPlayer.Bag[bagSlot].partName);
                    }
                }
            }
            //anywhere
            switch (intInput)
            {
                case 3:
                    gd.currentPlayer.CheckBag();
                    ShowTurnOptions(gd, bagSlot);
                    break;
                case 4:
                    ShowRegions(gd);
                    ShowTurnOptions(gd, bagSlot);
                    break;
                default:
                    break;
            }
        }

        private static MonsterData BuildMonster(GameData gd, bool isNew)
        {
            int intInput;
            PartData[] table = new PartData[6];
            string type = "";
            PartData chosenPart;
            bool halt = false;
            bool leave = false;
            int loopStart = 0;
            MonsterData currentMonster = gd.currentPlayer.Monster;
            //string newName;

            if (isNew)
            {
                loopStart = 0;
                Console.WriteLine("You aproach the empty table...");
            }
            else
            {
                loopStart = 2;
                Console.WriteLine(currentMonster.name + " slides onto the table...");
            }

            for (int i = loopStart; i < 6; i++)
            {
                switch (i)
                {
                    case 0:
                        type = "head";
                        break;
                    case 1:
                        type = "torso";
                        break;
                    case 2:
                        type = "left arm";
                        break;
                    case 3:
                        type = "right arm";
                        break;
                    case 4:
                        type = "left leg";
                        break;
                    case 5:
                        type = "right leg";
                        break;
                    default:
                        break;
                }
                
                halt = true;
                while (halt)
                {
                    if (isNew == false)
                    {
                        table[i] = currentMonster.parts[i];
                        TalkPause("Currently " + currentMonster + " has the below " + type);
                        Console.WriteLine(currentMonster.parts[i].partName);
                        Console.WriteLine("Durability: " + currentMonster.parts[i].partDurability);
                        Console.WriteLine("Alacrity" + currentMonster.parts[i].stats[0]);
                        Console.WriteLine("Strenght" + currentMonster.parts[i].stats[1]);
                        Console.WriteLine("Endurance" + currentMonster.parts[i].stats[2]);
                        TalkPause("Technique" + currentMonster.parts[i].stats[3]);
                    }

                    Console.WriteLine("0 - Exit");
                    gd.currentPlayer.CheckBag();

                    TalkPause("Please choose a " + type + ":");
                    intInput = CheckInput(0, 5);

                    if (intInput == 0)
                    {
                        halt = false;
                        leave = true;
                        break;
                    }
                    chosenPart = gd.currentPlayer.Bag[intInput - 1];

                    Console.WriteLine(chosenPart.partName);
                    if (chosenPart.partType != (i + 1))
                    {
                        Console.WriteLine("That is not a " + type+ "!");
                    }
                    else
                    {
                        Console.WriteLine("Durability: " + chosenPart.partDurability);
                        Console.WriteLine("Alacrity" + currentMonster.parts[i].stats[0]);
                        Console.WriteLine("Strenght" + currentMonster.parts[i].stats[1]);
                        Console.WriteLine("Endurance" + currentMonster.parts[i].stats[2]);
                        TalkPause("Technique" + currentMonster.parts[i].stats[3]);
                        Console.WriteLine("Use this part?");
                        Console.WriteLine("1 - Yes");
                        Console.WriteLine("2 - No");
                        Console.WriteLine("3 - Skip part");
                        TalkPause("4 - Leave Table");
                        intInput = CheckInput(1, 4);

                        switch (intInput)
                        {
                            case 1:
                                table[i] = chosenPart;
                                gd.currentPlayer.Bag[intInput - 1] = null;
                                break;
                            case 2:
                                break;
                            case 3:
                                halt = false;
                                break;
                            case 4:
                                leave = true;
                                halt = false;
                                break;
                            default:
                                break;
                        }
                        
                    }

               }
                //leave table
                if (leave)
                {
                    break;
                }
            }

            MonsterData newMonster = new MonsterData(null, table);
            for (int i = 0; i < 4; i++)
            {
                newMonster.monsterStats[i] = newMonster.CalculateStats(i, newMonster.parts);
            }
            

            TalkPause("This is your monster...");
            foreach (var part in table)
            {
                Console.WriteLine(part.partName);
               
            }
            foreach (var stat in newMonster.monsterStats)
            {
                Console.WriteLine(stat);
            }
            Console.WriteLine("Would you like to keep this monster?");
            TalkPause("1 - Yes, 2 - No");
            intInput = CheckInput(1, 2);
            if (intInput == 1)
            {
                if (isNew)
                {
                    TalkPause("What is its name?");
                    currentMonster = newMonster;
                    currentMonster.name = Console.ReadLine();
                    
                }
                else
                {
                    currentMonster.parts = table;
                }
            }
            else
            {
                Console.WriteLine("Better luck building tomorrow...");
            }        
            
            return currentMonster;

        }

        private static void CalculateFights(GameData gd)
        {
            Queue<PlayerData> fighters = new Queue<PlayerData>();

            //find all available competitors
            foreach (var player in gd.allPlayers)
            {
                if (player.Monster == null)
                { }
                else if (player.Monster.CanFight())
                {
                    fighters.Enqueue(player);
                }
            }
            
            //pair off
            if (fighters.Count == 0)
            {
                TalkPause("There will be no show tonight!  Better luck gathering tomorrow");
            }
            else if (fighters.Count == 1)
            {
                TalkPause("Only one of you managed to scrape together a monster?  No shows tonight, but rewards for the one busy beaver.");
                GrantCash(fighters.Dequeue(), 1);
            }
            else
            {
                decimal countTotal = fighters.Count;
                //fight in rounds
                while (fighters.Count != 0)
                {
                    int round = 0;
                    if (fighters.Count == 1)
                    {
                        TalkPause("And we have a winner!");
                        GrantCash(fighters.Dequeue(), round);
                    }
                    else
                    {
                        TalkPause("Draw your eyes to the arena!");
                        PlayerData left = fighters.Dequeue();
                        PlayerData right = fighters.Dequeue();
                        fighters.Enqueue(MonsterFight(left, right)); 
                        
                    }
                    if (fighters.Count <= Math.Ceiling(countTotal / 2))
                    {
                        round = round + 1;
                        countTotal = fighters.Count;
                    }
                    
                }
                
            }

            //apply luck to losers
        }

        private static PlayerData MonsterFight(PlayerData blue, PlayerData green)
        {
            PlayerData winner;
            
            MonsterData bm = blue.Monster;
            MonsterData gm = green.Monster;

            Console.WriteLine("In the blue corner, " + blue.Name + " presents " + blue.Monster.name);
            TalkPause(blue.Monster.name + "boasts " + blue.Monster.monsterStats);
            Console.WriteLine("In the green corner, " + green.Name + " presents " + green.Monster.name);

            while (bm.parts[0].partDurability > 0 && bm.parts[1].partDurability > 0 && gm.parts[0].partDurability > 0 && gm.parts[1].partDurability > 0)
            {
                MonsterData attack;
                MonsterData reply;
                if (gm.monsterStats[0] > bm.monsterStats[0])
                {
                    attack = gm;
                    reply = bm;
                }
                else
                {
                    attack = bm;
                    reply = gm;
                }

                float strike = RNG(1, 101, true) * attack.monsterStats[1];
                float parry  = RNG(1, 101, true) * reply.monsterStats[2];
                float repost = RNG(1, 101, true) * reply.monsterStats[1];
                float block  = RNG(1, 101, true) * attack.monsterStats[2];
                PartData attackTarget;
                PartData replyTarget;

                if (RNG(1, 101, false)>(attack.monsterStats[4]/10000))
                {
                    //add technical to strike to hit head or torso
                    attackTarget = GetTarget(reply, 5, 0);
                }
                else
                {
                    attackTarget = GetTarget(reply, 0, 5);
                }
                if (RNG(1, 101, false) > (reply.monsterStats[4] / 10000))
                {
                    //add technical to repost to hit head or torso
                    replyTarget = GetTarget(attack, 5, 0);
                }
                else
                {
                    replyTarget = GetTarget(attack, 0, 5);
                }

                //strike vs parry, result decreases random part damage
                float strikeDamage = attackTarget.partDurability - (strike - parry);
                Console.WriteLine(attack.name + " swings at " + reply.name + "'s " + attackTarget.partName + "!");
                TalkPause(attackTarget + " goes from " + attackTarget.partDurability + " to " + (attackTarget.partDurability - strikeDamage));
                attackTarget.partDurability  = attackTarget.partDurability - strikeDamage;
                if (attackTarget.partDurability <= 0)
                {
                    TalkPause(attackTarget.partName + " has been destroyed!");
                    attackTarget = null;
                }

                //repost vs block, result decreases random part damage
                float repostDamage = replyTarget.partDurability - (repost - block);
                Console.WriteLine(reply.name + " counters at " + attack.name + "'s " + replyTarget.partName + "!");
                TalkPause(attackTarget + " goes from " + replyTarget.partDurability + " to " + (replyTarget.partDurability - repostDamage));
                replyTarget.partDurability = replyTarget.partDurability - repostDamage;
                if (replyTarget.partDurability <= 0)
                {
                    TalkPause(replyTarget.partName + " has been destroyed!");
                    attackTarget = null;
                }

                for (int i = 0; i < 4; i++)
                {
                    bm.monsterStats[i] = bm.CalculateStats(i, bm.parts);
                    gm.monsterStats[i] = bm.CalculateStats(i, gm.parts);
                }
            }

            if (bm.parts[0].partDurability > 0 && bm.parts[1].partDurability > 0)
            {
                winner = blue;
                blue.Wins = blue.Wins + 1;
                blue.Monster.wins = blue.Monster.wins + 1;
            }
            else
            {
                winner = green;
                green.Wins = green.Wins + 1;
                green.Monster.wins = green.Monster.wins + 1;
            }
            blue.Fights = blue.Fights + 1;
            blue.Monster.fights = blue.Monster.fights + 1;
            green.Fights = green.Fights + 1;
            green.Monster.fights = green.Monster.fights + 1;

            return winner;
        }

        private static PartData GetTarget(MonsterData targetMonster, int start, int end)
        {
            PartData target = null;
            while (target != null)
            {
                if (start < end)
                {
                    for (int i = start; i < end + 1; i++)
                    {
                        if (RNG(i, end, false) == i  && targetMonster.parts[i].partDurability > 0)
                        {
                            target = targetMonster.parts[i];
                        }
                    }
                }
                else
                {
                    for (int i = start; i > end - 1; i--)
                    {
                        if (RNG(end, i, false) == end && targetMonster.parts[i].partDurability > 0)
                        {
                            target = targetMonster.parts[i];
                        }
                    }
                }
            }
            return target;
        }

        private static void GrantCash(PlayerData playerData, int wins)
        {
            Console.WriteLine("I'll add gold here for equipment eventually!");
        }

        #endregion

        #region uitlityFunctions
        static void TalkPause(string message)
        {
            Console.WriteLine(message);
            Console.ReadKey(true);
        }

        private static int CheckInput(int low, int high)
        {
            string strInput = "";
            int input = 0;
            bool halt = true;
            bool formatError = false;
            while (halt)
            {
                formatError = false;
                strInput = Console.ReadLine();
                if (strInput == null)
                {
                    Console.WriteLine("Please choose a number " + low + "-" + high);
                }
                else
                {
                    try
                    {
                        input = Convert.ToInt32(strInput);
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine("Please choose a number " + low + "-" + high);
                        formatError = true;
                    }
                    if (input < low || input > high && formatError == false)
                    {
                        Console.WriteLine("Please choose a number " + low + "-" + high);
                    }
                    else if (formatError == false)
                    {
                        halt = false;
                    }
                }
                  
                
            }

            return input;
        }

        private static float RNG(int min, int max, bool percent)
        {
            float number; 
            Random r = new Random();
            if (percent)
            {
                number = (r.Next(min, max) / max);
            }
            else
            {
                number = r.Next(min, max);
            }
            
            return number;
        }
        #endregion

    }
}
