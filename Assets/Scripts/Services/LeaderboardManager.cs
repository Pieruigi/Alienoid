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

        
        class LevelData
        {
            //int levelId;
            public List<PlayerData> allTimeRanks;
            public List<PlayerData> currentRanks;

            public float localScore;
            public int localPosition = 0;
            

            // Internal use for refresh
            public DateTime timeStamp;

            public LevelData()
            {
                allTimeRanks = new List<PlayerData>();
                currentRanks = new List<PlayerData>();
            }


            public void AddAllTimeRank(PlayerData playerData)
            {
                allTimeRanks.Add(playerData);
            }

            public void AddCurrentRank(PlayerData playerData)
            {
                currentRanks.Add(playerData);
            }

            public void AddLocalScore(float value)
            {
                localScore = value;
            }

            public void AddLocalPosition(int value)
            {
                localPosition = value;
            }
        }

        public class PlayerData
        {
            string userId;
            string playerName;
            float score;

            public PlayerData(string userId, string playerName, float score)
            {
                this.userId = userId;
                this.playerName = playerName;
                this.score = score;
            }
        }

        public static LeaderboardManager Instance { get; private set; }

        LevelData[] leaderboards;

        float expireTime = 10;
        FirebaseFirestore db;

        string leaderboardCollection = "leaderboards";
        string leaderboardDocumentFormat = "{0}_{1}";
        string allTimeDocument = "all_time";
        string levelCollection = "levels";
        //string levelDocument = "level_{0}";
        string userCollection = "users";
        string scoreField = "score";

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
#if UNITY_EDITOR
            if(GameManager.Instance != null)
                leaderboards = new LevelData[GameManager.Instance.GetNumberOfLevels()];
            else
                leaderboards = new LevelData[28];
#else
                leaderboards = new LevelData[GameManager.Instance.GetNumberOfLevels()];
#endif
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.A))
                SaveLocalPlayerScoreByLevel(1, 122442).ConfigureAwait(false);
                //CreateMonthlyStructure().ConfigureAwait(false);

            if (Input.GetKeyDown(KeyCode.S))
            {
                GetLevelMenuScoreDataAsync((LevelMenuScoreData data) => { Debug.Log(data); }).ConfigureAwait(false) ;
            }    
        }

      

        public bool IsLocalPlayerInRankingByLevel(int levelId)
        {
            if (leaderboards[levelId-1] == null)
                return false;

            return leaderboards[levelId-1].localPosition > 0;
        }

        public int GetLocalPlayerPositionByLevel(int levelId)
        {
            return leaderboards[levelId-1].localPosition;
        }

        /// <summary>
        /// To avoid unquerable documents ( italic font document created by code are not readeable ) 
        /// </summary>
        /// <returns></returns>
        public async Task CreateLeaderboardStructure()
        {
            db = FirebaseFirestore.DefaultInstance;

            // Monthly leaderboard
            int month = DateTime.UtcNow.Month;
            int year = DateTime.UtcNow.Year;

            DocumentSnapshot lead = await db.Collection(leaderboardCollection).
                                         Document(string.Format(leaderboardDocumentFormat, month, year)).GetSnapshotAsync();

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

            // All time leadearboard
            lead = await db.Collection(leaderboardCollection).
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

        public async Task GetLevelMenuScoreDataAsync(UnityAction<LevelMenuScoreData> callback)
        {
            // Init db
            db = FirebaseFirestore.DefaultInstance;

            // Create new data
            LevelMenuScoreData data = new LevelMenuScoreData();
                        

            // Get level collection
            int month = DateTime.UtcNow.Month;
            int year = DateTime.UtcNow.Year;


            // Await for task 
            QuerySnapshot lQuery = await db.Collection(leaderboardCollection).
                    Document(string.Format(leaderboardDocumentFormat, month, year)).
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
                        data.AddMonthlyRecord(int.Parse(level.Id), float.Parse(user.ToDictionary()[scoreField].ToString()));
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

            // Now get all time record for each level
            //levels = db.Collection(leaderboardCollection).Document(allTimeDocument).Collection(levelCollection);
            // Await for task 
            lQuery = await db.Collection(leaderboardCollection).Document(allTimeDocument).Collection(levelCollection).GetSnapshotAsync();
            // Loop through each level and get user score data
            foreach(DocumentSnapshot level in lQuery.Documents)
            {
                // Just get the lower score
                QuerySnapshot uQuery = await level.Reference.Collection(userCollection).
                                             OrderByDescending(scoreField).
                                             Limit(1).GetSnapshotAsync();

                if(uQuery.Count > 0)
                {
                    data.AddAllTimeRecord(int.Parse(level.Id), float.Parse(new List<DocumentSnapshot>(uQuery.Documents)[0].ToDictionary()[scoreField].ToString()));
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
            //
            // Get the current player score if any
            int month = DateTime.UtcNow.Month;
            int year = DateTime.UtcNow.Year;
            // Leaderboard collection

            // Check structure
            await CreateLeaderboardStructure();

            // Check for the monthly leaderboard
            DocumentSnapshot doc = await db.Collection(leaderboardCollection).
                                         Document(string.Format(leaderboardDocumentFormat, month, year)).
                                         Collection(levelCollection).Document(levelId.ToString()).
                                         Collection(userCollection).Document(userId.ToString()).GetSnapshotAsync();

            // If document exists check if an update is needed
            Dictionary<string, object> userData = null;
            bool toUpdate = false;
           
            if (doc.Exists)
            {
                Debug.Log("Document found");
                // Document exists
                // Check if the new score is better than the old one
                // Get dictionary
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
                await doc.Reference.SetAsync(userData, SetOptions.MergeAll);
            }

            // Check for all time record
            toUpdate = false;
            doc = await db.Collection(leaderboardCollection).Document(allTimeDocument).
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
                await doc.Reference.SetAsync(userData, SetOptions.MergeAll);
            }

        }

       
    }

}
