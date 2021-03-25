using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    /// <summary>
    /// Every time a level is completed a new character is appended to the cache data string ( this means
    /// that the first time the game starts the string is empty ). For example if we have completed
    /// the first 3 levels then the string length will be 3 and the last unlocked level is the level 4;
    /// the character represents the speed at which we have played and completed the level:
    /// 0: normal speed
    /// 1: fast speed
    /// 2: crazy speed 
    /// 4: impossible speed ( and so on )
    /// NB: characters are separated by space
    /// </summary>
    public class GameProgressManager
    {
        //string cacheData;
        string cacheName;

        List<int> levels;

        static GameProgressManager instance;
        public static GameProgressManager Instance
        {
            get { if (instance==null) instance = new GameProgressManager(); return instance; }
        }

        private GameProgressManager()
        {
            // Load player pref
            string data = PlayerPrefs.GetString(cacheName);

            // Create the level list
            levels = new List<int>();

            // Fill the level list if the cache is not empty
            if (!string.IsNullOrEmpty(data))
            {
                string[] s = data.Split(' ');
                for(int i=0; i<s.Length; i++)
                {
                    levels.Add(int.Parse(s[i]));
                }
            }
            

        }

        /// <summary>
        /// Returns the last level that has been unlocked by the player ( or the first level )
        /// </summary>
        /// <returns></returns>
        public int GetLastUnlockedLevel()
        {
            return levels.Count + 1;
        }

        public void ReportLevelCompleted(int levelId, int speed)
        {
            // If cache is empty we just completed the first level at normal speed
            if(levels.Count == 0)
            {
                levels.Add(0);
                SaveCache();
                return;
            }
            
            // If the speed is > 0 then we played a level already completed at normal speed, so we only
            // try to update the speed if needed
            if(speed > 0)
            {
                if (levels[levelId - 1] < speed)
                    levels[levelId - 1] = speed;

                // Save cache and return
                SaveCache();
                return;
            }

            // We just completed the last unlocked level for the first time, so we simply need to 
            // unlock the next level if exists.
            if (GetLastUnlockedLevel() == levelId && levelId < GameManager.Instance.GetNumberOfLevels())
            {
                // Unlock the next level
                levels.Add(0);
                // Store data
                SaveCache();
                return;
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
