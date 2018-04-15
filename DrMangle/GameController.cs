using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
    public class GameController
    {
        public GameData Data { get; set; }
        public GameRepo Repo { get; set; }
        public ArenaData Arena { get; set; }
        public PlayerData[] AllPlayers { get; set; }
        private Random RNG = new Random();

        public GameController()
        {
            string textInput;
            int intInput;

            Repo = new GameRepo();
            Arena = new ArenaData();

            Repo.FileSetup();
            StaticUtility.TalkPause("Welcome to the Isle of Dr. Mangle.");
            if (Repo.gameIndex != null)
            {
                Data = Repo.LoadGame();
            }
            if (Data == null)
            {
                Console.WriteLine("Please enter a name for your game data:");
                textInput = Console.ReadLine();
                Console.WriteLine("And how many contestants will you be competing against?");
                intInput = StaticUtility.CheckInput(1, 7);
                Data = new GameData(textInput, intInput, Repo.GetNextGameID(), RNG);
                Repo.SaveGame(Data);
            }
            AllPlayers = new PlayerData[Data.AiPlayers.Length + 1];
            AllPlayers[0] = Data.CurrentPlayer;
            for (int i = 0; i < Data.AiPlayers.Length; i++)
            {
                AllPlayers[i + 1] = Data.AiPlayers[i];
            }
        }

        public bool RunGame()
        {
            bool gameStatus = true;
            int intInput;

            #region search
            StaticUtility.TalkPause("A new day has dawned!");
            StaticUtility.TalkPause("The parks will be open for 5 hours...");
            StaticUtility.TalkPause("You will then have one more hour in your labs before the evening's entertainment.");

            for (int i = 1; i < 6; i++)
            {
                StaticUtility.TalkPause("It is currently " + i + " o'clock. The parks close at 6.");
                Data.MoveRegions();
                gameStatus = ShowSearchOptions(i - 1);
                AISearchTurn(Data, i);
                if (!gameStatus)
                {
                    return gameStatus;
                }
            }
            #endregion

            #region build
            StaticUtility.TalkPause("It is now 6 o'clock. Return to your lab and prepare for the floorshow at 7.");
            Data.CurrentRegion = 0;
            foreach (var player in AllPlayers)
            {
                player.DumpBag();
            }
            Console.WriteLine("Bag contents added to workshop inventory.");
            gameStatus = ShowLabOptions();
            if (!gameStatus)
            {
                return gameStatus;
            }
            AIBuildTurn(Data);

            #endregion

            #region fight
            StaticUtility.TalkPause("Welcome to the evening's entertainment!");
            if (Data.CurrentPlayer.Monster != null && Data.CurrentPlayer.Monster.CanFight())
            {
                Console.WriteLine("Would you like to particpate tonight?");
                StaticUtility.TalkPause("1 - Yes, 2 - No");
                intInput = StaticUtility.CheckInput(1, 2);
                if (intInput != 1)
                {
                    StaticUtility.TalkPause("Well, maybe tomorrow then...");
                    Console.WriteLine("Let's find you a comfortable seat.");

                }
                else
                {
                    StaticUtility.TalkPause("Let the games begin!");
                }
            }
            else
            {
                StaticUtility.TalkPause("Seeing as you do not have a living, able bodied contestant...");
                Console.WriteLine("Let's find you a comfortable seat.");
            }
            CalculateFights();

            #endregion

            SortPlayersByWins(AllPlayers);
            Data.CurrentLevel.AddParts(RNG, AllPlayers.Length);
            Data.CurrentLevel.HalveParts();
            Repo.SaveGame(Data);

            return gameStatus;
        }

        private void AIBuildTurn(GameData data)
        {
            foreach (var ai in Data.AiPlayers)
            {
                int start;
                var monst = new PartData[6];
                if (ai.Monster == null)
                {
                    start = 0;
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        monst[i] = ai.Monster.Parts[i];
                    }
                    start = 2;
                }
                for (int i = start; i < 6; i++)
                {
                    for (int j = ai.Workshop.Count - 1; j >= 0; j--)
                    {
                        PartData oldP = monst[i];
                        PartData newP = ai.Workshop[j];
                        float score = 0;

                        if (newP != null)
                        {
                            if (oldP != null && newP.PartType == i)
                            {
                                score += newP.Stats[0] - monst[i].Stats[0];
                                score += newP.Stats[1] - monst[i].Stats[1];
                                score += newP.Stats[2] - monst[i].Stats[2];
                                score += newP.Stats[3] - monst[i].Stats[3];
                            }
                            if (oldP == null || score > 0f)
                            {
                                monst[i] = newP;
                            }
                            else
                            {
                                ai.ScrapItem(ai.Workshop, j);
                            }
                        }
                    }
                }
                if (monst[0] != null && monst[1] != null)
                {
                    if (monst[2] != null || monst[3] != null || monst[4] != null || monst[5] != null)
                    {
                        ai.Monster = new MonsterData(ai.Name + "'s Monster", monst);
                    }
                }
            }
        }

        private void AISearchTurn(GameData gd, int round)
        {
            foreach (var ai in gd.AiPlayers)
            {
                int region = RNG.Next(1, 4);
                if (gd.CurrentLevel.Locations[region].PartsList.Count != 0)
                {
                    ai.Bag[round - 1] = gd.CurrentLevel.Locations[region].PartsList.Last.Value;
                    gd.CurrentLevel.Locations[region].PartsList.RemoveLast();
                }
            }
        }
       
        private bool ShowSearchOptions(int bagSlot)
        {
            bool status = true;
            bool searching = true;
            while (searching)
            {
                int intInput;
                Console.WriteLine("Welcome to the " + Data.RegionText + "! Here you can: ");

                Console.WriteLine("0 - Menu");
                Console.WriteLine("1 - Search for parts");
                Console.WriteLine("2 - Scan for parts");
                Console.WriteLine("3 - Look in bag");
                Console.WriteLine("4 - Go to another region");

                intInput = StaticUtility.CheckInput(0, 4);

                switch (intInput)
                {
                    case 0:
                        status = RunMenu();
                        searching = status;
                        break;
                    case 1:
                        if (Data.CurrentLevel.Locations[Data.CurrentRegion].PartsList.Count == 0)
                        {
                            Console.WriteLine("There are no more parts in this region");
                        }
                        else
                        {
                            Data.CurrentPlayer.Bag[bagSlot] = Data.CurrentLevel.Locations[Data.CurrentRegion].PartsList.Last();
                            Data.CurrentLevel.Locations[Data.CurrentRegion].PartsList.RemoveLast();
                            Console.WriteLine("You found: " + Data.CurrentPlayer.Bag[bagSlot].PartName);
                        }
                        searching = false;
                        break;
                    case 2:
                        foreach (var park in Data.CurrentLevel.Locations)
                            Console.WriteLine("There are " + park.PartsList.Count + " parts left in the " + park.ParkName + ".");
                        searching = false;
                        break;
                    case 3:
                        Data.CurrentPlayer.CheckBag();
                        break;
                    case 4:
                        Data.MoveRegions();
                        break;
                    default:
                        throw new Exception("Bad Input in GameController.ShowSearchOptions");
                }
            }
            return status;
        }

        private bool RunMenu()
        {
            bool gameStatus = true;

            Console.WriteLine("Would you like to quit?  Today's progress will not be saved.");
            Console.WriteLine("1 - Yes");
            Console.WriteLine("2 - No");
            int intInput = StaticUtility.CheckInput(1, 2);

            if (intInput == 1)
            {
                gameStatus = false;
            }

            return gameStatus;
        }

        private bool ShowLabOptions()
        {
            bool status = true;
            bool halt = true;
            while (halt)
            {
                Console.WriteLine("0 - Menu");
                Console.WriteLine("1 - Work on the monster");
                Console.WriteLine("2 - Scrap unwanted parts");
                Console.WriteLine("3 - Head out to the floor show");

                int intInput = StaticUtility.CheckInput(0, 3);

                switch (intInput)
                {
                    case 0:
                        status = RunMenu();
                        halt = status;
                        break;
                    case 1:
                        if (Data.CurrentPlayer.Monster == null)
                        {
                            Data.CurrentPlayer.Monster = BuildMonster(true);
                        }
                        else
                        {
                            BuildMonster(false);
                        }
                        break;
                    case 2:
                        Console.WriteLine("Which Item would you like to scrap?");
                        Console.WriteLine("0 - Exit");
                        Data.CurrentPlayer.CheckWorkshop();
                        int answer = StaticUtility.CheckInput(0, Data.CurrentPlayer.Workshop.Count);
                        if (answer != 0)
                        {
                            Data.CurrentPlayer.ScrapItem(Data.CurrentPlayer.Workshop, answer - 1);
                        }
                        break;
                    case 3:
                        halt = false;
                        break;
                    default:
                        throw new Exception("Bad Input in GameController.ShowLabOptions");
                }
            }
            return status;
        }

        private MonsterData BuildMonster(bool isNew)
        {
            int intInput;
            PartData[] table = new PartData[6];
            string type = "";
            PartData chosenPart;
            bool halt = false;
            bool leave = false;
            int loopStart = 0;
            MonsterData currentMonster = Data.CurrentPlayer.Monster;
            //string newName;

            if (isNew)
            {
                loopStart = 0;
                Console.WriteLine("You aproach the empty table...");
            }
            else
            {
                loopStart = 2;
                Console.WriteLine(currentMonster.Name + " slides onto the table...");
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
                        table[i] = currentMonster.Parts[i];
                        StaticUtility.TalkPause("Currently " + currentMonster + " has the below " + type);
                        Console.WriteLine(currentMonster.Parts[i].PartName);
                        Console.WriteLine("Durability: " + currentMonster.Parts[i].PartDurability);
                        Console.WriteLine("Alacrity" + currentMonster.Parts[i].Stats[0]);
                        Console.WriteLine("Strenght" + currentMonster.Parts[i].Stats[1]);
                        Console.WriteLine("Endurance" + currentMonster.Parts[i].Stats[2]);
                        StaticUtility.TalkPause("Technique" + currentMonster.Parts[i].Stats[3]);
                    }

                    Console.WriteLine("0 - Exit");
                    Data.CurrentPlayer.CheckWorkshop();

                    StaticUtility.TalkPause("Please choose a " + type + ":");
                    intInput = StaticUtility.CheckInput(0, Data.CurrentPlayer.Workshop.Count);

                    if (intInput == 0)
                    {
                        halt = false;
                        leave = true;
                        break;
                    }
                    chosenPart = Data.CurrentPlayer.Workshop[intInput - 1];

                    Console.WriteLine(chosenPart.PartName);
                    if (chosenPart.PartType != (i))
                    {
                        Console.WriteLine("That is not a " + type + "!");
                    }
                    else
                    {
                        Console.WriteLine("Durability: " + chosenPart.PartDurability);
                        Console.WriteLine("Alacrity" + chosenPart.Stats[0]);
                        Console.WriteLine("Strenght" + chosenPart.Stats[1]);
                        Console.WriteLine("Endurance" + chosenPart.Stats[2]);
                        StaticUtility.TalkPause("Technique" + chosenPart.Stats[3]);
                        Console.WriteLine("Use this part?");
                        Console.WriteLine("1 - Yes");
                        Console.WriteLine("2 - No");
                        Console.WriteLine("3 - Skip part");
                        Console.WriteLine("4 - Leave Table");
                        intInput = StaticUtility.CheckInput(1, 4);

                        switch (intInput)
                        {
                            case 1:
                                table[i] = chosenPart;
                                Data.CurrentPlayer.Bag[intInput - 1] = null;
                                halt = false;
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

            if (table[0] != null)
            {
                MonsterData newMonster = new MonsterData(null, table);
                for (int i = 0; i < 4; i++)
                {
                    newMonster.MonsterStats[i] = newMonster.CalculateStats(i, newMonster.Parts);
                }


                StaticUtility.TalkPause("This is your monster...");
                foreach (var part in table)
                {
                    if (part != null)
                    {
                        Console.WriteLine(part.PartName);
                    } 
                }
                foreach (var stat in newMonster.MonsterStats)
                {
                    Console.WriteLine(stat);
                }
                Console.WriteLine("Would you like to keep this monster?");
                StaticUtility.TalkPause("1 - Yes, 2 - No");
                intInput = StaticUtility.CheckInput(1, 2);
                if (intInput == 1)
                {
                    if (isNew)
                    {
                        StaticUtility.TalkPause("What is its name?");
                        currentMonster = newMonster;
                        currentMonster.Name = Console.ReadLine();

                    }
                    else
                    {
                        currentMonster.Parts = table;
                    }
                }
                else
                {
                    Console.WriteLine("Better luck building tomorrow...");
                }
            }           

            return currentMonster;

        }

        private void CalculateFights()
        {
            Queue<PlayerData> fighters = new Queue<PlayerData>();

            //find all available competitors
            foreach (var player in AllPlayers)
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
                StaticUtility.TalkPause("There will be no show tonight!  Better luck gathering tomorrow");
            }
            else if (fighters.Count == 1)
            {
                StaticUtility.TalkPause("Only one of you managed to scrape together a monster?  No shows tonight, but rewards for the one busy beaver.");
                Arena.GrantCash(fighters.Dequeue(), 1);
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
                        StaticUtility.TalkPause("And we have a winner!");
                        Arena.GrantCash(fighters.Dequeue(), round);
                    }
                    else
                    {
                        StaticUtility.TalkPause("Draw your eyes to the arena!");
                        PlayerData left = fighters.Dequeue();
                        PlayerData right = fighters.Dequeue();
                        fighters.Enqueue(Arena.MonsterFight(left, right));

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

        public void SortPlayersByWins(PlayerData[] players)
        {
            for (int i = 0; i < AllPlayers.Length; i++)
            {
                PlayerData left = AllPlayers[i];
                PlayerData high = AllPlayers[i];
                int highIndex = i;

                for (int j = i + 1; j < AllPlayers.Length; j++)
                {
                    if (high.Compare(high, AllPlayers[j]) < 0)
                    {
                        high = AllPlayers[j];
                        highIndex = j;
                    }
                }

                if (left != high)
                {
                    AllPlayers[highIndex] = left;
                    AllPlayers[i] = high;
                }
            }
        }
    }
}
