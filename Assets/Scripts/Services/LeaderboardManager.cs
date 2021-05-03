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
        string leaderboardDocument = "leaderboard_{0}_{1}";
        string allTimeDocument = "all_time";
        string levelCollection = "levels";
        string levelDocument = "level_{0}";
        string userCollection = "users";
        string scoreField = "score";

        

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
            leaderboards = new LevelData[GameManager.Instance.GetNumberOfLevels()];
        }

        // Update is called once per frame
        void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.A))
                SaveLocalPlayerScoreByLevel(1, 122442);
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

        public float GetLocalPlayerScoreByLevelAsync(int levelId, UnityAction<bool, float> callback)
        {
            //return leaderboards[levelId-1].localScore;
            db = FirebaseFirestore.DefaultInstance;
            int month = DateTime.UtcNow.Month;
            int year = DateTime.UtcNow.Year;

            string userId = AccountManager.Instance.GetUserId();

#if UNITY_EDITOR
            if (!AccountManager.Instance.Logged)
                userId = "fake_local_user_id";
#endif
            DocumentReference doc = db.Collection(leaderboardCollection).Document(string.Format(leaderboardDocument, month, year)).
                Collection(levelCollection).Document(string.Format(levelDocument, levelId)).
                Collection(userCollection).Document(userId);

            doc.GetSnapshotAsync().ContinueWith(task =>
            {
                if(!task.Result.Exists)
                {
                    callback?.Invoke(false, 0);
                }
                else
                {
                    callback?.Invoke(true, float.Parse( task.Result.ToDictionary()[scoreField].ToString()));
                }
            });

            return 13;
        }

        public void LoadLeaderboard(int levelId)
        {
            Debug.Log("Loading leaderboard...");
            db = FirebaseFirestore.DefaultInstance;

            // Get level 'i' leaderboard
            // Create an element in the dictionary if needed
            if (leaderboards[levelId-1] == null)
            {
                leaderboards[levelId-1] = new LevelData();
            }

            
            if ((DateTime.UtcNow - leaderboards[levelId-1].timeStamp).TotalSeconds > expireTime)
            {
#if !FAKE_LEAD
                // Get the current player score if any
                int month = DateTime.UtcNow.Month;
                int year = DateTime.UtcNow.Year;
                // Leaderboard collection
                CollectionReference leads = db.Collection(leaderboardCollection);

                DocumentReference lead = leads.Document(string.Format(leaderboardDocument, month, year));
                DocumentReference level = lead.Collection(levelCollection).Document(string.Format(levelDocument, levelId));
                //DocumentReference user = level.Collection(userCollection).Document(userId);

                // Query all the users in the current leaderboard
                Query users = level.Collection(userCollection).OrderBy(scoreField);
                
                users.GetSnapshotAsync().ContinueWith(task => 
                {
                    LevelData ld = leaderboards[levelId - 1];
        
                    //List<DocumentSnapshot> docs = (List<DocumentSnapshot>)task.Result.Documents;
                    QuerySnapshot query = task.Result;
                    int count = 1;
                    foreach (DocumentSnapshot doc in query)
                    {
                       
                        Dictionary<string, object> data = doc.ToDictionary();
                        float score = float.Parse(data[scoreField].ToString());
                        ld.AddCurrentRank(new PlayerData(doc.Id, "", score));

                        if(doc.Id == AccountManager.Instance.GetUserId())
                        {
                            ld.AddLocalScore(score);
                            ld.AddLocalPosition(count);
                        }
                        count++;
                    }
                    ld.timeStamp = DateTime.UtcNow;
                });

#else
                // Get the corresponding element from dictionary
                LevelData ld = leaderboards[levelId-1];

                // Fill element from firebase db
                ld.AddAllTimeRank(new PlayerData("0001", "qui", 110000));
                ld.AddAllTimeRank(new PlayerData("0002", "paperone", 120000));
                ld.AddAllTimeRank(new PlayerData("0003", "paperino", 130000));

                ld.AddCurrentRank(new PlayerData("0004", "minnie", 180000));
                ld.AddCurrentRank(new PlayerData("0005", "quo", 190000));
                ld.AddCurrentRank(new PlayerData("0002", "paperone", 195000));
                ld.AddCurrentRank(new PlayerData("0006", "qua", 198000));

                ld.AddLocalScore(21000);
                ld.AddLocalPosition(3);

                ld.timeStamp = DateTime.UtcNow;
#endif
            }

            // Tell the others that this leaderboard is ready 
            OnLeaderboardLoaded?.Invoke(levelId);
        }

        public void SaveLocalPlayerScoreByLevel(int levelId, float score)
        {
#if !UNITY_EDITOR
            if (!AccountManager.Instance.Logged)
                return;
#endif
            // Init db instance
            db = FirebaseFirestore.DefaultInstance;//; FirebaseFirestore.GetInstance(FirebaseApp.Create());

            // Get local user id
            string userId = AccountManager.Instance.GetUserId();

#if UNITY_EDITOR
            // Fake user id for editor ( we may not be logged )
            if (!AccountManager.Instance.Logged)
                userId = "fake_local_user_id";
#endif
            //
            // Get the current player score if any
            int month = DateTime.UtcNow.Month;
            int year = DateTime.UtcNow.Year;
            // Leaderboard collection
            CollectionReference leads = db.Collection(leaderboardCollection);
            
            DocumentReference lead = leads.Document(string.Format(leaderboardDocument, month, year));
            DocumentReference level = lead.Collection(levelCollection).Document(string.Format(levelDocument, levelId));
            DocumentReference user = level.Collection(userCollection).Document(userId);

            user.GetSnapshotAsync().ContinueWith(task =>
                TryUpdateScore(task, user, score)
            );
            
            
            
            //Dictionary<string, object> data = new Dictionary<string, object>();
            //data.Add(scoreField, score);
            //user.SetAsync(data, SetOptions.MergeAll);

            

            //// Each leaderboard is a specific collection of levels
            //int month = DateTime.UtcNow.Month;
            //int year = DateTime.UtcNow.Year;
            //CollectionReference lead = db.Collection(string.Format(leadCollection, month, year));
            //CollectionReference users = lead.Document(string.Format(levelDocument, levelId)).Collection(userCollection);
            //DocumentReference user = users.Document(userId);
            //Dictionary<string, object> data = new Dictionary<string, object>();
            //data.Add(scoreField, 100211);
            //user.SetAsync(data, SetOptions.MergeAll);



        }

        void TryUpdateScore(Task<DocumentSnapshot> task, DocumentReference user, float score)
        {
            {
                bool toUpdate = false;
                Dictionary<string, object> userData = null;
                DocumentSnapshot doc = task.Result;
                if (doc.Exists)
                {
                    Debug.Log("Document found");
                    // Document exists
                    // Check if the new score better than the old one
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
                else
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
                    user.SetAsync(userData, SetOptions.MergeAll);
                }
            }
        }
    }

}
