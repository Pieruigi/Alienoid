#if !OLD_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    /// <summary>
    /// We simply increase an integer avery time a new level is beaten to trace progress.
    /// </summary>
    public class GameProgressManager
    {

        //string cacheData;
        string cacheName = "save";

        int progress = 0;

        static GameProgressManager instance;
        public static GameProgressManager Instance
        {
            get { if (instance == null) instance = new GameProgressManager(); return instance; }
        }

        private GameProgressManager()
        {
            Init();
        }

        /// <summary>
        /// Returns the last level that has been unlocked by the player ( or the first level )
        /// </summary>
        /// <returns></returns>
        public int GetLastUnlockedLevel(int speed)
        {
            // No level available 
            if (!IsSpeedUnlocked(speed))
                return 0;

            if(Constants.MaxLevelSpeed > speed && IsSpeedUnlocked(speed + 1))
            {
                // If the next speed is unlocked then all the levels are available
                return GameManager.Instance.GetNumberOfLevels();
            }


            return ((progress % GameManager.Instance.GetNumberOfLevels()) + 1);


        }

        public bool IsSpeedUnlocked(int speed)
        {
            if (speed == 1)
                return true;

            if (progress >= (speed-1) * GameManager.Instance.GetNumberOfLevels())
                return true;
            else
                return false;
        }

        public int GetHigherUnlockedSpeed()
        {
            if (!GameManager.Instance)
                return 0;

            return (progress / GameManager.Instance.GetNumberOfLevels()) + 1;
        }

        /// <summary>
        /// Returns true if the given level is unlocked ( that means this is the first level or the previous
        /// one has bean beaten ) for a given game speed
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public bool LevelIsUnlocked(int levelId, int speed)
        {
            // The speed has not been unlocked yet
            if (!IsSpeedUnlocked(speed))
                return false;

            // If the next speed is unlocked then the level is unlocked
            if (speed < Constants.MaxLevelSpeed && IsSpeedUnlocked(speed + 1))
                return true;

            if (progress % GameManager.Instance.GetNumberOfLevels() < levelId - 1)
                return false;

            return true;
        }

        /// <summary>
        /// Returns true if all levels have been beaten at each available speed
        /// </summary>
        /// <returns></returns>
        public bool IsGameCompleted()
        {
            if (progress >= Constants.MaxLevelSpeed * GameManager.Instance.GetNumberOfLevels())
                return true;
            else
                return false;
        }

        //public bool IsGameSpeedAvailable(int levelId, int gameSpeed)
        //{
        //    // Get the max available speed for the current level
        //    int beatenSpeed = GetMaxBeatenSpeed(levelId);

        //    // We have beaten the level at all the speeds
        //    if (beatenSpeed == Constants.MaxLevelSpeed || gameSpeed <= beatenSpeed + 1)
        //        return true;

        //    return false;

        //}

        /// <summary>
        /// Simply update beaten speed if needed
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="speed"></param>
        public void SetLevelBeaten(int levelId, int speed)
        {
            // Already beaten ( we can play the same level more times )
            if (LevelHasBeenBeaten(levelId, speed))
                return;

            progress++;

            SaveCache();
        }

        public bool LevelHasBeenBeaten(int levelId, int speed)
        {
            if (!IsSpeedUnlocked(speed))
                return false;

            // If the next speed has been unlocked then the level has been beaten
            if (speed < Constants.MaxLevelSpeed && IsSpeedUnlocked(speed + 1))
                return true;

            if (progress % GameManager.Instance.GetNumberOfLevels() < levelId)
                return false;

            return true;
        }

        //public bool AllLevelsBeaten()
        //{
        //    return levels[levels.Count - 1] > 0;
        //}

        public void Reset()
        {
            PlayerPrefs.DeleteKey(cacheName);
            Init();
        }

        void Init()
        {

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

        }

        void SaveCache()
        {
            // Save progress
            PlayerPrefs.SetString(cacheName, progress.ToString());
            PlayerPrefs.Save();
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