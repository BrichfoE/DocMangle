using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMangle
{
     class GameData
    {
        string gameName;
        int gameDataId;
        public LevelData currentLevel;
        public PlayerData currentPlayer;
        public PlayerData[] aiPlayers;
        public int currentRegion;
        public string regionText;
        public int aiPlayerCount;

        public GameData(string name, int aiCount)
        {
            //somehow read file to find next available game ID

            gameName = name;
            currentLevel = new LevelData();
            currentPlayer = new PlayerData("New Contestant");
            
            currentRegion = 0; //at the lab
            SetRegionText();

            aiPlayers = new PlayerData[aiPlayerCount];
            GenerateAI(aiPlayers);
        }

        public GameData(int GameId)
        {
            string name = null;
            LevelData level = null;
            PlayerData player = null;
            PlayerData[] ai = null;

            //somehow read file to get the right gameID

            currentRegion = 0; //at the lab
            SetRegionText();

            gameName = name;
            currentLevel = level;
            currentPlayer = player;

            aiPlayers = ai;
        }

        public void SetRegionText()
        {
            regionText = currentLevel.locations[currentRegion].ParkName;
        }

        private void GenerateAI(PlayerData[] ai)
        {
            for (int i = 0; i < ai.Length; i++)
            {
                ai[i] = new PlayerData(i);
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
