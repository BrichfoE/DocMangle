using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
     public class GameData
    {
        string GameName { get; set; }
        int GameDataId { get; set; }
        public LevelData CurrentLevel { get; set; }
        public PlayerData CurrentPlayer { get; set; }
        public PlayerData[] AiPlayers { get; set; }
        public int CurrentRegion { get; set; }
        public string RegionText { get; set; }
        public int AiPlayerCount { get; set; }

        public PlayerData[] AllPlayers { get; set; }

        public GameData(string name, int aiCount)
        {
            //somehow read file to find next available game ID

            GameName = name;
            CurrentLevel = new LevelData();
            CurrentPlayer = new HumanPlayerData("New Contestant");
            
            CurrentRegion = 0; //at the lab
            SetRegionText();

            AiPlayers = new PlayerData[AiPlayerCount];
            GenerateAI(AiPlayers);
            var allPlayers = new PlayerData[aiCount + 1];
            allPlayers[0] = CurrentPlayer;
            for (int i = 0; i < AiPlayers.Length; i++)
            {
                allPlayers[i + 1] = AiPlayers[i];
            }
        }

        //public GameData(int GameId)
        //{
        //    string name = null;
        //    LevelData level = null;
        //    PlayerData player = null;
        //    PlayerData[] ai = null;
        //    PlayerData[] all = null;

        //    //somehow read file to get the right gameID

        //    currentRegion = 0; //at the lab
        //    SetRegionText();

        //    gameName = name;
        //    currentLevel = level;
        //    currentPlayer = player;

        //    aiPlayers = ai;
        //    allPlayers = all;
        //}

        public void SetRegionText()
        {
            RegionText = CurrentLevel.locations[CurrentRegion].ParkName;
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

        //    Methods
        //        save state
        //        load state
        //        generate tournament matchup

        //        display tournament matchup
        //        fight monsters
    }
}
