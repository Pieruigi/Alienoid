
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zom.Pie.Services;

namespace Zom.Pie
{
    /// <summary>
    /// We simply increase an integer avery time a new level is beaten to trace progress.
    /// </summary>
    public class GameProgressManager: MonoBehaviour
    {
        


        //string cacheData;
        string cacheName = "save";

        int progress = 0;

        //static GameProgressManager instance;
        //public static GameProgressManager Instance
        //{
        //    get { if (instance == null) instance = new GameProgressManager(); return instance; }
        //}

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

       

      

        public void LoadGameProgress()
        {
            LoadCache();
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
        /// Simply update beaten speed if needed
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="speed"></param>
        public void SetLevelBeaten(int levelId, int speed)
        {
            Debug.Log("Setting level beaten:" + levelId);
            // Already beaten ( we can play the same level more times )
            if (LevelHasBeenBeaten(levelId, speed))
                return;

            progress++;
            Debug.Log("Setting level beaten - progress increased:" + progress);

            SaveCache();
            Debug.Log("CacheSaved");
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
            LoadCache();
        }

        void LoadCache()
        {

        }


        void SaveCache() { }
      
    }

}
