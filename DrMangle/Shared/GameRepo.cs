﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DrMangle
{
    public class GameRepo
    {
        private readonly string filePath = "C:\\git\\DocMangle\\DrMangle\\bin\\Debug\\Data\\";
        private int exceptionCount;
        public Dictionary<string, int> gameIndex;

        public void FileSetup()
        {      
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (!Directory.Exists(filePath + "Save\\"))
            {
                Directory.CreateDirectory(filePath + "Save\\");
            }
            if (!Directory.Exists(filePath + "Errors\\"))
            {
                Directory.CreateDirectory(filePath + "Errors\\");
            }

            string indexFile = Path.Combine(filePath, "Save\\Index.txt");
            if (!File.Exists(indexFile))
            {
                var file = File.Create(indexFile);
                file.Close();
                gameIndex = new Dictionary<string, int>();
                gameIndex.Add("_placeholder", 0);
            }
            else
            {
                var text = File.ReadAllText(indexFile);
                gameIndex = JsonConvert.DeserializeObject<Dictionary<string, int>>(text);
            }
        }

        public void SaveGame(GameData gd)
        {
            string saveFile = Path.Combine(filePath, "Save\\dat_" + gd.GameDataId.ToString() + ".txt");
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
                    File.WriteAllText(Path.Combine(filePath, "Save\\Index.txt"), JsonConvert.SerializeObject(gameIndex, Formatting.Indented));
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
            Console.WriteLine("0 - Start New Game");
            Console.WriteLine("1 - Load a Previous Game");
            intInput = StaticUtility.CheckInput(0, 1);
            if (intInput == 0)
            {
                return null;
            }
            bool halt = true;
            while (halt)
            {
                Console.WriteLine("Please enter the name of the game you would like to load.");
                foreach (var game in gameIndex)
                {
                    Console.WriteLine(game.Value + " - " + game.Key);
                }
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
            string saveFile = Path.Combine(filePath, "Save\\dat_" + gameId.ToString() + ".txt");

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

        public void LogException(GameData gd, string exceptionText, Exception ex, bool willClose)
        {
            exceptionCount += 1;
            string errorFileName = Path.Combine(filePath, "Errors\\dat_" + gd.GameDataId.ToString() + "_Exception" + exceptionCount.ToString() +"_"+ DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt");
            if (!File.Exists(errorFileName))
            {
                var errorFile = File.Create(errorFileName);
                errorFile.Close();
            }

            var sw = File.AppendText(errorFileName);
            sw.WriteLine(exceptionText);
            sw.WriteLine("Message: " + ex.Message);
            sw.WriteLine("HelpLink: " + ex.HelpLink);
            sw.WriteLine("Source: " + ex.Source);
            sw.WriteLine("TargetSite: " + ex.TargetSite);
            sw.WriteLine("StackTrace: " + ex.StackTrace);
            sw.WriteLine("InnerException: " + ex.InnerException);
            sw.WriteLine(JsonConvert.SerializeObject(gd, Formatting.Indented));

            if (willClose)
                StaticUtility.TalkPause("Something has gone wrong, the game will now close and unsaved progress will be lost.");
            else
                StaticUtility.TalkPause("Error Logged, section skipped.");
        }
    }
}
