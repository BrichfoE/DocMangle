using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DrMangle
{
    public class GameRepo
    {
        private string filePath = "C:\\git\\DocMangle\\DrMangle\\bin\\Debug\\Save\\";
        public Dictionary<string, int> gameIndex;

        public void FileSetup()
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

        public void SaveGame(GameData gd)
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
            Console.WriteLine("Game Saved");
        }

        public GameData LoadGame()
        {
            GameData data = null;
            int gameId = 1;
            int intInput;

            Console.WriteLine("Would you like to load a previous game?");
            Console.WriteLine("(please enter the number of the game you would like to load)");
            Console.WriteLine("0 - Start New Game");
            foreach (var game in gameIndex)
            {
                Console.WriteLine(game.Value + " - " + game.Key);
            }
            intInput = StaticUtility.CheckInput(0, gameIndex.Count);
            if (intInput == 0)
            {
                return null;
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

        public int GetNextGameID()
        {
            int GameID = 1;

            if (gameIndex == null)
            {
                GameID = 1;
            }
            else
            {
                GameID = gameIndex.Last().Value + 1;
            }

            return GameID;
        }
    }
}
