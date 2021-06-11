using Firebase;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zom.Pie.Collections;

namespace Zom.Pie.Services
{
    public class LeaderboardManager : MonoBehaviour
    {
        /// <summary>
        /// Params:
        /// int - the level id this leaderboard refers to
        /// </summary>
        public UnityAction<int> OnLeaderboardLoaded;

        public static LeaderboardManager Instance { get; private set; }

        //LevelData[] leaderboards;

        float expireTime = 10;
        FirebaseFirestore db;

        string leaderboardCollection = "leaderboards";
        string leaderboardDocumentFormat = "{0}_{1}";
        string allTimeDocument = "all_time";
        string levelCollection = "levels";
        //string levelDocument = "level_{0}";
        string userCollection = "users";
        string scoreField = "score";
        string timestampField = "timestamp";

#if UNITY_EDITOR
        string fakeLocalUserId = "fake_local_user_id";
#endif

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                // Init firestore
                //db = FirebaseFirestore.DefaultInstance;

                DontDestroyOnLoad(gameObject);

            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

     
      

        public async Task<LeaderboardData> GetLeaderboardDataAsync()
        {
         
            // Init db
            db = FirebaseFirestore.DefaultInstance;

            // Create new data
            LeaderboardData data = new LeaderboardData();

            // Get users
            QuerySnapshot users = await db.Collection("users").OrderByDescending("save").Limit(100).GetSnapshotAsync();

            foreach(DocumentSnapshot user in users)
            {
                string save = user.ToDictionary()["save"].ToString();
                if (save.Split(' ').Length < 2)
                    continue;
                Debug.LogFormat("User:{0}; Save:{1}", user.Id, save);

                string userId = user.Id;
                int speed = int.Parse(save.Split(' ')[0]);
                int level = int.Parse(save.Split(' ')[1]);
                string displayName = user.ToDictionary().ContainsKey("displayName") ? user.ToDictionary()["displayName"].ToString() : "";
                string avatarUrl = user.ToDictionary().ContainsKey("avatarUrl") ? user.ToDictionary()["avatarUrl"].ToString() : "";
               
                LeaderboardData.PlayerData player = new LeaderboardData.PlayerData(userId, speed, level, displayName, avatarUrl);
                data.AddPlayerData(player);
            }

  
            return data;
        }

    }

}
