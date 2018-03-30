using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace DrMangle
{
     public class GameData
    {
        public string GameName { get; set; }
        public int GameDataId { get; set; }
        public LevelData CurrentLevel { get; set; }
        public HumanPlayerData CurrentPlayer { get; set; }
        public AIPlayerData[] AiPlayers { get; set; }
        public PlayerData[] AllPlayers { get; set; }
        public List<MonsterGhost> Graveyard { get; set; }
        public int CurrentRegion { get; set; }
        public string RegionText
        {
            get
            {
                return CurrentLevel.locations[CurrentRegion].ParkName;
            }
        }  

        public GameData(string name, int aiCount)
        {
            GameDataId = GameRepo.GetNextGameID();
            
            GameName = name;
            CurrentLevel = new LevelData();
            CurrentPlayer = new HumanPlayerData("New Contestant");
            
            CurrentRegion = 0; //at the lab

            AiPlayers = new AIPlayerData[aiCount];
            GenerateAI(AiPlayers);
            var allPlayers = new PlayerData[aiCount + 1];
            allPlayers[0] = CurrentPlayer;
            for (int i = 0; i < AiPlayers.Length; i++)
            {
                allPlayers[i + 1] = AiPlayers[i];
            }

        }

        private void GenerateAI(PlayerData[] ai)
        {
            for (int i = 0; i < ai.Length; i++)
            {
                ai[i] = new AIPlayerData(i);
            }
        }

        public void SortByWins()
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

    static public class GameRepo
    {
        private static string filePath = "C:\\git\\DocMangle\\DrMangle\\bin\\Debug\\Save\\";
        public static Dictionary<string, int> gameIndex;

        public static void FileSetup()
        {
            string indexFile = Path.Combine(filePath, "Index.txt");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (!File.Exists(indexFile))
            {
                var file = File.Create(indexFile);
                file.Close();
            }
            else
            {
                var text = File.ReadAllText(indexFile);
                gameIndex = JsonConvert.DeserializeObject<Dictionary<string, int>>(text);
            }
        }

        public static void SaveGame(GameData gd)
        {            

            string saveFile = Path.Combine(filePath, "dat_" + gd.GameDataId.ToString() + ".txt");
            if (!File.Exists(saveFile))
            {
                var gameFile = File.Create(saveFile);
                gameFile.Close();
                if (gameIndex == null)
                {
                    gameIndex = new Dictionary<string, int>() { };
                }
                if (!gameIndex.ContainsKey(gd.GameName))
                {
                    gameIndex.Add(gd.GameName, gd.GameDataId);
                    File.WriteAllText(Path.Combine(filePath, "Index.txt"), JsonConvert.SerializeObject(gameIndex, Formatting.Indented));
                }
            }

            File.WriteAllText(saveFile, JsonConvert.SerializeObject(gd, Formatting.Indented));

        }
               
        public static GameData LoadGame()
        {
            GameData data = null;
            int gameId = 0;

            Console.WriteLine("Would you like to load a previous game?");
            foreach (var game in gameIndex)
            {
                Console.WriteLine(game.Value + " - " + game.Key);
            }
            bool halt = true;
            while (halt)
            {
                Console.WriteLine("Please enter the name of the game you would like to load.");
                string gameName = Console.ReadLine();
                
                if (gameIndex.TryGetValue(gameName, out gameId))
                {
                    halt = false;
                }
                else
	            {
                    Console.WriteLine("Invalid game name, please enter the name of a game.");
                }
            }
            string saveFile = Path.Combine(filePath, "dat_" + gameId.ToString() + ".txt");

            if (File.Exists(saveFile))
            {
                string fileText = File.ReadAllText(saveFile);
                data = JsonConvert.DeserializeObject<GameData>(fileText);
                Console.WriteLine("Load successful!");
            }
            
            return data;
        }
               
        public static int GetNextGameID()
        {
            int GameID = 0;

            if (gameIndex == null)
            {
                GameID = 0;
            }
            else
            {
                GameID = gameIndex.Last().Value + 1;
            }

            return GameID;
        }
    }
}