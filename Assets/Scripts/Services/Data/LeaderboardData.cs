using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Services
{
    public class LeaderboardData
    {
        public class LevelData
        {
            
            List<PlayerData> players;

            float localScore;
            public float LocalScore
            {
                get { return localScore; }
            }

            public LevelData()
            {
                players = new List<PlayerData>();
            }

            public void AddPlayerData(PlayerData player)
            {
                players.Add(player);
            }

            public void SetLocalScore(float score)
            {
                localScore = score;
            }
        }

        public class PlayerData
        {
            string userId;
            float score;

            public PlayerData(string userId, float score)
            {
                this.userId = userId;
                this.score = score;
            }
        }



        List<LevelData> levels;
        public IList<LevelData> Levels
        {
            get { return levels.AsReadOnly(); }
        }

        public LeaderboardData()
        {
            levels = new List<LevelData>();
        }

        public void AddLevelData(LevelData levelData)
        {
            levels.Add(levelData);
        }

        
       
    }

}
