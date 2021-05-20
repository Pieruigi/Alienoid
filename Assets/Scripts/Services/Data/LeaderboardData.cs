using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Services
{
    public class LeaderboardData
    {
        public class LevelData
        {
            int levelId = 0;
            public int LevelId
            {
                get { return levelId; }
            }

            List<PlayerData> players;
            public IList<PlayerData> Players
            {
                get { return players.AsReadOnly(); }
            }

            float localScore;
            public float LocalScore
            {
                get { return localScore; }
            }

            public LevelData(int levelId)
            {
                this.levelId = levelId;
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
            public string UserId
            {
                get { return userId; }
            }
            float score;
            public float Score
            {
                get { return score; }
            }

            string displayName;
            public string DisplayName
            {
                get { return displayName; }
            }

            string avatarUrl;
            public string AvatarUrl
            {
                get { return avatarUrl; }
            }

            public PlayerData(string userId, float score, string displayName, string avatarUrl)
            {
                this.userId = userId;
                this.score = score;
                this.displayName = displayName;
                this.avatarUrl = avatarUrl;
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
