using Firebase;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

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
//#if UNITY_EDITOR
//            if(GameManager.Instance != null)
//                leaderboards = new LevelData[GameManager.Instance.GetNumberOfLevels()];
//            else
//                leaderboards = new LevelData[28];
//#else
//                leaderboards = new LevelData[GameManager.Instance.GetNumberOfLevels()];
//#endif
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.A))
                SaveLocalPlayerScoreByLevel(1, 122440).ConfigureAwait(false);
                //CreateMonthlyStructure().ConfigureAwait(false);

            if (Input.GetKeyDown(KeyCode.S))
            {
                GetLevelMenuScoreDataAsync((LevelMenuScoreData data) => { Debug.Log(data); }).ConfigureAwait(false) ;
            }
#endif
        }

     
        /// <summary>
        /// To avoid unquerable documents ( italic font document created by code are not readeable ) 
        /// </summary>
        /// <returns></returns>
        public async Task CheckLeaderboardStructure()
        {
            db = FirebaseFirestore.DefaultInstance;

            // All time leadearboard
            DocumentSnapshot lead = await db.Collection(leaderboardCollection).
                                         Document(allTimeDocument).GetSnapshotAsync();

            if (!lead.Exists)
            {
                await lead.Reference.SetAsync(new Dictionary<string, object>());
            }

            for (int i = 0; i < GameManager.Instance.GetNumberOfLevels(); i++)
            {
                DocumentSnapshot level = await lead.Reference.Collection(levelCollection).Document((i + 1).ToString()).GetSnapshotAsync();
                if (!level.Exists)
                {
                    await level.Reference.SetAsync(new Dictionary<string, object>());
                }

            }
        }

        public async Task GetLeaderboardDataAsync(UnityAction<LeaderboardData> callback)
        {
            await CheckLeaderboardStructure();

            // Init db
            db = FirebaseFirestore.DefaultInstance;

            // Create new data
            LeaderboardData data = new LeaderboardData();

            // Get level collection
            QuerySnapshot lQuery = await db.Collection(leaderboardCollection).
                    Document(allTimeDocument).
                    Collection(levelCollection).GetSnapshotAsync();

            // For each level get the top players
            foreach (DocumentSnapshot level in lQuery.Documents)
            {
                // Create a new level data
                LeaderboardData.LevelData levelData = new LeaderboardData.LevelData();
                data.AddLevelData(levelData);

                // Order players by score               
                QuerySnapshot users = await level.Reference.Collection(userCollection).OrderByDescending(scoreField).Limit(Constants.TopPlayers).GetSnapshotAsync();

                // Loop through the top players
                bool localFound = false;
                string localUserId = null;
                foreach (DocumentSnapshot user in users.Documents)
                {
                    // Create a new player data
                    float score = float.Parse(user.ToDictionary()[scoreField].ToString());
                    LeaderboardData.PlayerData playerData = new LeaderboardData.PlayerData(user.Id, score);

                    // Add player to the corresponding level
                    levelData.AddPlayerData(playerData);                    
              
                   
                    
#if !UNITY_EDITOR
                    userId = AccountManager.Instance.GetUserId();
                        
#else
                    if (AccountManager.Instance == null || !AccountManager.Instance.Logged)
                    {
                        localUserId = fakeLocalUserId;
                    }
                    else
                    {
                        localUserId = AccountManager.Instance.GetUserId();
                    }

#endif
                    // Check if this player is the local player
                    if (localUserId == user.Id)
                    {
                        localFound = true;
                        levelData.SetLocalScore(score);
                    }

                }

                if (!localFound)
                {
                    // We must look for local player in the entire db because he's not a top player
                    DocumentSnapshot localUser = await level.Reference.Collection(userCollection).Document(localUserId).GetSnapshotAsync();
                    if (localUser.Exists)
                    {
                        levelData.SetLocalScore(float.Parse(localUser.ToDictionary()[scoreField].ToString()));
                    }
                }
            }

        }

        public async Task GetLevelMenuScoreDataAsync(UnityAction<LevelMenuScoreData> callback)
        {
            // Init db
            db = FirebaseFirestore.DefaultInstance;

            // Create new data
            LevelMenuScoreData data = new LevelMenuScoreData();
                        


            // Get level collection
            QuerySnapshot lQuery = await db.Collection(leaderboardCollection).
                    Document(allTimeDocument).
                    Collection(levelCollection).GetSnapshotAsync();


            // Loop through each level and get user score data
            foreach (DocumentSnapshot level in lQuery.Documents)
            {
               
                Debug.Log("Doc.id:" + level.Id);
                QuerySnapshot uQuery = await level.Reference.Collection(userCollection).OrderByDescending(scoreField).GetSnapshotAsync();
                int count = 1;
                foreach(DocumentSnapshot user in uQuery.Documents)
                {
                    // Save the monthly best score
                    if (count == 1)
                    {
                        data.AddAllTimeRecord(int.Parse(level.Id), float.Parse(user.ToDictionary()[scoreField].ToString()));
                    }

                    // Get player position and score
                    string userId;
#if !UNITY_EDITOR
                    userId = AccountManager.Instance.GetUserId();
                        
#else
                    if (AccountManager.Instance == null || !AccountManager.Instance.Logged)
                    {
                        userId = fakeLocalUserId;
                    }
                    else
                    {
                        userId = AccountManager.Instance.GetUserId();
                    }

#endif
                    if(userId == user.Id)
                    {
                        data.AddPlayerScore(int.Parse(level.Id), float.Parse(user.ToDictionary()[scoreField].ToString()));
                        data.AddPlayerPosition(int.Parse(level.Id), count);
                    }

                    //Debug.Log("User:" + user.Id);
                }
            }

          
            //Debug.Log(data);

            // Callback
            callback?.Invoke(data);


        }


        public async Task SaveLocalPlayerScoreByLevel(int levelId, float score)
        {

#if !UNITY_EDITOR
            if (!AccountManager.Instance.Logged)
                return;
            string userId = AccountManager.Instance.GetUserId();
            
#endif
            // Init db instance
            db = FirebaseFirestore.DefaultInstance;//; FirebaseFirestore.GetInstance(FirebaseApp.Create());

            


#if UNITY_EDITOR
            // Fake user id for editor ( we may not be logged )
            string userId;
            if (!AccountManager.Instance.Logged)
                userId = fakeLocalUserId;
            else
                userId = AccountManager.Instance.GetUserId();
#endif
       

            // Check structure
            await CheckLeaderboardStructure();

            // If document exists check if an update is needed
            Dictionary<string, object> userData = null;
     

            // Check for all time record
            bool toUpdate = false;
            DocumentSnapshot doc = await db.Collection(leaderboardCollection).Document(allTimeDocument).
                        Collection(levelCollection).Document(levelId.ToString()).
                        Collection(userCollection).Document(userId.ToString()).GetSnapshotAsync();

            if (doc.Exists)
            {
                // Check if an update is needed
                userData = doc.ToDictionary();
                Debug.Log("Saved Score:" + userData[scoreField]);
                Debug.Log("Current Score:" + score);
                bool higher = (float.Parse(userData[scoreField].ToString())) > score;
                Debug.Log("higher:" + higher);
                if (!userData.ContainsKey(scoreField) || (float.Parse(userData[scoreField].ToString())) > score)
                {
                    Debug.Log("Update field to " + score);
                    // Set update flag
                    toUpdate = true;

                    // Update score
                    if (userData.ContainsKey(scoreField))
                        userData[scoreField] = score;
                    else
                        userData.Add(scoreField, score); // For safe
                }
            }
            else // Document doesn't exist
            {
                Debug.Log("Document not found");
                // Document doesn't exist, means user score has not been saved in this collection yet
                toUpdate = true;
                userData = new Dictionary<string, object>();
                userData.Add(scoreField, score);
            }

            // Check if we need to update db
            if (toUpdate)
            {
                await doc.Reference.SetAsync(userData, SetOptions.MergeAll).ContinueWith(task=>
                {
                    doc.Reference.UpdateAsync(timestampField, FieldValue.ServerTimestamp);
                });
                
            }

        }

       
    }

}
