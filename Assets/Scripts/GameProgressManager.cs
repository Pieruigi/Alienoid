#if !OLD_SYSTEM
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zom.Pie.Services;

namespace Zom.Pie
{
    /// <summary>
    /// We simply increase an integer avery time a new level is beaten to trace progress.
    /// </summary>
    public class GameProgressManager: MonoBehaviour
    {
        public UnityAction OnLoadedOrUpdated;
       
        //string cacheData;
        public static readonly string CacheName = "save2";
        
        int speed = 1;
        public int Speed
        {
            get { return speed; }
        }
        int levelId = 1;
        public int LevelId
        {
            get { return levelId; }
        }
       

        public static GameProgressManager Instance { get; private set; }

        bool loaded = false;
        public bool Loaded
        {
            get { return loaded; }
        }

        void Awake()
        {
            if (!Instance)
            {
                Instance = this;


                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Debug.Log("GameProgressManager Start()...");
            AccountManager.Instance.OnLoggedIn += delegate { LoadCache(); };
            AccountManager.Instance.OnLoggedOut += delegate { loaded = false; speed = 1; levelId = 1; };
        }

    

        public void LoadGameProgress()
        {
            LoadCache();
        }
        
        
        
       

        public bool IsSpeedUnlocked(int speed)
        {
            if (speed <= this.speed)
                return true;

            return false;
            
        }

      

        /// <summary>
        /// Returns true if the given level is unlocked ( that means this is the first level or the previous
        /// one has bean beaten ) for a given game speed
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public bool LevelIsUnlocked(int levelId, int speed)
        {

            if (this.speed > speed)
                return true;

            if (this.speed == speed && this.levelId >= levelId)
                return true;

            return false;

        }
     

        public bool LevelHasBeenBeaten(int levelId, int speed)
        {
            if (this.speed > speed)
                return true;

            if (this.speed == speed && this.levelId >= levelId)
                return true;

            return false;
        }


        public void Reset()
        {
            PlayerPrefs.DeleteKey(CacheName);
            LoadCache();
        }

        void LoadCache()
        {
#if UNITY_EDITOR && LOCAL_SAVE
            loaded = false;
            // Load player pref
            string data = PlayerPrefs.GetString(cacheName);

            // Save progress if cache is available
            if (!string.IsNullOrEmpty(data))
            {
                progress = int.Parse(data);
            }
            else
            {
                progress = 0;
            }
            loaded = true;
#else
            // Loading from firebase
            loaded = false;
            LoadCacheAsync().ConfigureAwait(false);
#endif
        }


        public async Task<bool> SetLevelBeatenAsync(int levelId, int speed)
        {
            Debug.LogFormat("Saving progress - levelId:{0}, speed:{1}", levelId, speed);

            if (speed < this.speed)
                return true;

            if (levelId < this.levelId)
                return true;

            if (!AccountManager.Instance.Logged)
            {
                return false;
            }

            Debug.LogFormat("Init firestore");
            Firebase.Firestore.FirebaseFirestore db = Firebase.Firestore.FirebaseFirestore.DefaultInstance;

            DocumentSnapshot user = await db.Collection("users").Document(AccountManager.Instance.GetUserId()).GetSnapshotAsync();
            Debug.LogFormat("User {0} found: {1}", AccountManager.Instance.GetUserId(), user.Exists);

            if (!user.Exists)
            {
                return false;
            }

            int speedOld = this.speed;
            int levelIdOld = this.levelId;

            if(levelId < GameManager.Instance.GetNumberOfLevels())
            {
                this.levelId++;
            }
            else
            {
                this.levelId = 1;
                this.speed++;
            }
            // User found
            //progress++;
            Dictionary<string, object> data = user.ToDictionary();
            string value = string.Format("{0} {1}", this.speed, this.levelId);
            if (!data.ContainsKey(CacheName))
                data.Add(CacheName, value);
            else
                data[CacheName] = value;

            bool ret = false;
            await user.Reference.SetAsync(data, SetOptions.MergeAll).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogWarning("Task not completed; rolling bask progress...");
                    //progress--;
                    this.levelId = levelIdOld;
                    this.speed = speedOld;
                }
                else
                {
                    Debug.Log("Task completed; progress saved...");
                    ret = true;
                }   
            });

            if (ret)
                OnLoadedOrUpdated?.Invoke();

            Debug.Log("Returning " + ret);
            return ret;
        }

        async Task LoadCacheAsync()
        {
           

            Debug.LogFormat("User logged in:" + AccountManager.Instance.Logged);
            if (!AccountManager.Instance.Logged)
                return;
            
            Firebase.Firestore.FirebaseFirestore db = Firebase.Firestore.FirebaseFirestore.DefaultInstance;

            DocumentSnapshot user = await db.Collection("users").Document(AccountManager.Instance.GetUserId()).GetSnapshotAsync();
            Debug.LogFormat("User found:" + user.Exists);

            if (!user.Exists)
                return;

            // User found
            loaded = true;
            
            Dictionary<string, object> data = user.ToDictionary();
            if (!data.ContainsKey(CacheName))
                return;

            Debug.LogFormat("Found save game:"+data[CacheName]);

            string[] splits = data[CacheName].ToString().Split(' ');
            speed = int.Parse(splits[0]);
            levelId = int.Parse(splits[1]);

            OnLoadedOrUpdated?.Invoke();

        }
    }

}
#endif

#if OLD_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    /// <summary>
    /// The cache string contains a charater for each level of the game ( characters are separed
    /// by a blank space ); each character is an integer which tell us whether the level has been already
    /// beaten or not: 0 means we've never beat the level, while every value > 0 means we've beaten the 
    /// level at the corresponding speed. For example '2 3 1 1 0 0 0 0 0 0' means the game has ten levels
    /// and we beat the first four levels; we can also see that different speed at which each level has
    /// been beaten. Basically we can understand if a given level is unlocked by looking at the speed
    /// of the previous level.
    /// 1: normal speed
    /// 2: fast speed
    /// 3: crazy speed 
    /// 4: impossible speed
    /// ......
    /// ......
    /// n: how fast must be this game ?
    /// NB: characters are separated by space
    /// </summary>
    public class GameProgressManager
    {

        //string cacheData;
        string cacheName = "save";

        List<int> levels;

        static GameProgressManager instance;
        public static GameProgressManager Instance
        {
            get { if (instance==null) instance = new GameProgressManager(); return instance; }
        }

        private GameProgressManager()
        {
            Init();
        }

        /// <summary>
        /// Returns the last level that has been unlocked by the player ( or the first level )
        /// </summary>
        /// <returns></returns>
        public int GetLastUnlockedLevel()
        {
            // We just need to loop until we find the first level having speed zero
            for(int i=0; i<levels.Count; i++)
            {
                if(levels[i] == 0)
                {
                    return i + 1;
                }
            }

            // Why ???
            return -1;
        }

        /// <summary>
        /// Returns true if the given level is unlocked ( that means this is the first level or the previous
        /// one has bean beaten )
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public bool LevelIsUnlocked(int levelId)
        {
            // The first level is always unlocked
            if (levelId == 1)
                return true;

            // Has the previous level been beaten?
            if (levels[levelId - 2] > 0)
                return true;

            return false;
        }

        public bool IsGameSpeedAvailable(int levelId, int gameSpeed)
        {
            // Get the max available speed for the current level
            int beatenSpeed = GetMaxBeatenSpeed(levelId);

            // We have beaten the level at all the speeds
            if (beatenSpeed == Constants.MaxLevelSpeed || gameSpeed <= beatenSpeed + 1)
                return true;

            return false;

        }

        /// <summary>
        /// Returns the max speed at which we have beaten the level ( 0 means it's not been beaten yet )
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public int GetMaxBeatenSpeed(int levelId)
        {
            return levels[levelId-1];
        }

        /// <summary>
        /// Simply update beaten speed if needed
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="speed"></param>
        public void SetLevelBeaten(int levelId, int speed)
        {
            // Update the corresponding level if needed
            if (levels[levelId-1] < speed)
                levels[levelId-1] = speed;

            SaveCache();
        }
        
        public bool LevelHasBeenBeaten(int levelId)
        {
            return levels[levelId] > 0;
        }

        public bool AllLevelsBeaten()
        {
            return levels[levels.Count - 1] > 0;
        }

        public void Reset()
        {
            PlayerPrefs.DeleteKey(cacheName);
            Init();
        }

        void Init()
        {
            
            // Create the level list
            int[] tmp = new int[GameManager.Instance.GetNumberOfLevels()];
            levels = new List<int>(tmp);

            // Load player pref
            string data = PlayerPrefs.GetString(cacheName);


            if (!string.IsNullOrEmpty(data))
            {
                // We must check for each level in cache and set the corresponding element in the list
                string[] s = data.Split(' ');
                for (int i = 0; i < s.Length; i++)
                {
                    int speed = int.Parse(s[i]);
                    levels[i] = speed;
                }
            }
        }

        void SaveCache()
        {
            // Create data string
            string data = null;

            foreach(int level in levels)
            {
                if (data != null)
                    data += " ";

                data += level.ToString();
            }

            // Save data
            PlayerPrefs.SetString(cacheName, data);
            PlayerPrefs.Save();
        }
    }

}
#endif