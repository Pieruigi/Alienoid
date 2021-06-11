using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Services
{
    public class LeaderboardData
    {
       
        public class PlayerData
        {
            string userId;
            public string UserId
            {
                get { return userId; }
            }

            int speed;
            public int Speed
            {
                get { return speed; }
            }

            int level;
            public int Level
            {
                get { return level; }
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

            public PlayerData(string userId, int speed, int level, string displayName, string avatarUrl)
            {
                this.userId = userId;
                this.speed = speed;
                this.level = level;
                this.displayName = displayName;
                this.avatarUrl = avatarUrl;
            }
        }

        List<PlayerData> players;
        public IList<PlayerData> Players
        {
            get { return players.AsReadOnly(); }
        }

        public LeaderboardData()
        {
            players = new List<PlayerData>();
        }
        
        public void AddPlayerData(PlayerData playerData)
        {
            players.Add(playerData);
        }
       
    }

}
