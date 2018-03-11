using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
     public class GameData
    {
        string gameName;
        int gameDataId;
        public LevelData currentLevel;
        public PlayerData currentPlayer;
        public PlayerData[] aiPlayers;
        public int currentRegion;
        public string regionText;
        public int aiPlayerCount;

        public PlayerData[] allPlayers;

        public GameData(string name, int aiCount)
        {
            //somehow read file to find next available game ID

            gameName = name;
            currentLevel = new LevelData();
            currentPlayer = new HumanPlayerData("New Contestant");
            
            currentRegion = 0; //at the lab
            SetRegionText();

            aiPlayers = new PlayerData[aiPlayerCount];
            GenerateAI(aiPlayers);
            var allPlayers = new PlayerData[aiCount + 1];
            allPlayers[0] = currentPlayer;
            for (int i = 0; i < aiPlayers.Length; i++)
            {
                allPlayers[i + 1] = aiPlayers[i];
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
            regionText = currentLevel.locations[currentRegion].ParkName;
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
            for (int i = 0; i < allPlayers.Length; i++)
            {
                PlayerData left = allPlayers[i];
                PlayerData high = allPlayers[i];
                int highIndex = i;

                for (int j = i + 1; j < allPlayers.Length; j++)
                {
                    if (high.Compare(high, allPlayers[j]) < 0)
                    {
                        high = allPlayers[j];
                        highIndex = j;                        
                    }
                }

                if (left != high)
                {
                    allPlayers[highIndex] = left;
                    allPlayers[i] = high;
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
