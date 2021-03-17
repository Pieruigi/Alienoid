using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Collection
{
    public class LevelConfigurationData : ScriptableObject
    {
        public static string ResourceFolder = "Configuration/Levels/";
        public static string FileNamePattern = "{0}";

        [System.Serializable]
        public class BlackHoleData
        {
            [SerializeField]
            EnemyType enemyType;
            public EnemyType EnemyType
            {
                get { return enemyType; }
            }

            [SerializeField]
            bool hasGate = false;
        }

        [SerializeField]
        int numberOfGreenEnemies, numberOfYellowEnemies, numberOfRedEnemies;
        public int NumberOfGreenEnemies
        {
            get { return numberOfGreenEnemies; }
        }
        public int NumberOfYellowEnemies
        {
            get { return numberOfYellowEnemies; }
        }

        public int NumberOfRedEnemies
        {
            get { return numberOfRedEnemies; }
        }

        // They must match the black hole list in the level manager
        [SerializeField]
        List<BlackHoleData> blackHoleDataList;
        public IList<BlackHoleData> BlackHoleDataList
        {
            get { return blackHoleDataList.AsReadOnly(); }
        }

        [SerializeField]
        string availableGroups;
        
        public List<int> GetAvailableGroups()
        {
            List<int> ret = new List<int>();
            if (string.IsNullOrEmpty(availableGroups))
                return ret;

            availableGroups = availableGroups.Trim();

            string[] s = availableGroups.Split(',');
            
            if (s != null)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    ret.Add(int.Parse(s[i]));
                }
            }

            return ret;
        }

    }

}
