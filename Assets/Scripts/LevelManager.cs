using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zom.Pie.Collection;

namespace Zom.Pie
{
    //[ExecuteInEditMode]
    public class LevelManager : MonoBehaviour
    {

        public static LevelManager Instance { get; private set; }

        [SerializeField]
        int greenCount, yellowCount, redCount;

        [SerializeField]
        int maxEnemiesOnScreen = 2;

        [SerializeField]
        GameObject enemyPrefab;

        [SerializeField]
        List<BlackHole> blackHoles;

        [SerializeField]
        List<GameObject> groups;

#if UNITY_EDITOR
        [Header("****************** DEBUG ******************")]
        [SerializeField]
        LevelConfigurationData debug_levelData = null;
#endif

        //List<GameObject> redPool, yellowPool, greenPool;
        List<GameObject> pool = new List<GameObject>();

        List<GameObject> usedList = new List<GameObject>();
        

        // This list represent the actual number of enemies we must destroy
        List<EnemyType> enemies = new List<EnemyType>();

        float startDelay = 0f;
        public float StartDelay
        {
            get { return startDelay; }
        }
        bool running = false;
        public bool Running
        {
            get { return running; }
        }

        int enemiesOnScreen = 0;
        

        private void Awake()
        {
            if (!Instance)
            {

                Instance = this;
/*
#if UNITY_EDITOR
                if(Application.isPlaying)
                    Init(debug_levelData != null ? debug_levelData : GetConfigurationData(GameManager.Instance.CurrentLevelId));
#else
                Init(GetConfigurationData(GameManager.Instance.CurrentLevelId));
#endif
*/
                // Create the enemy pool 
                CreateEnemyPool();


            }
            else
            {
                Destroy(gameObject);
            }

            

           
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(StartLevel());
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            //if(!Application.isPlaying)
            //    Init(debug_levelData);
#endif

            if (!running)
                return;

            if (enemiesOnScreen >= maxEnemiesOnScreen)
                return;

            if (enemies.Count > 0)
            {
                // Spawn a new enemy
                SpawnRandomEnemy();
            }


        }

        public IList<GameObject> GetOnScreenEnemies()
        {
            return usedList.FindAll(e => e.activeSelf).AsReadOnly();
        }

        LevelConfigurationData GetConfigurationData(int levelId)
        {
            // Get the configuration data
            string path = LevelConfigurationData.ResourceFolder;
            path += string.Format(LevelConfigurationData.FileNamePattern, levelId);
            Debug.Log("Path:" + path);
            LevelConfigurationData data = Resources.Load<LevelConfigurationData>(path);
            Debug.Log("ConfData:" + data.name);
            return data;
        }

        void Init(LevelConfigurationData data)
        {
            

            greenCount = data.NumberOfGreenEnemies;
            yellowCount = data.NumberOfYellowEnemies;
            redCount = data.NumberOfRedEnemies;

           
            // Setting black holes
            for(int i=0; i<blackHoles.Count; i++)
            {
                blackHoles[i].SetEnemyType(data.BlackHoleDataList[i].EnemyType);
            }

            // Reset all groups
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].SetActive(false);
            }

            // Setting available groups
            List<int> g = data.GetAvailableGroups();
            for(int i=0; i<g.Count; i++)
            {
                groups[g[i]].SetActive(true);
            }
            
        }

       

        /// <summary>
        /// Create a pool of enemy objects
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        void AddToPool(EnemyType type)
        {
            // Create pool
            List<GameObject> ret = new List<GameObject>();
            for(int i=0; i<maxEnemiesOnScreen+1; i++)
            {
                // Create enemy
                GameObject enemy = GameObject.Instantiate(enemyPrefab);
                // Set enemy type
                enemy.GetComponent<Enemy>().SetType(type);
                // Add handles
                enemy.GetComponent<Enemy>().OnDead += HandleOnDead;

                MoveEnemyToPool(enemy);


            }

        }

        void AddToEnemies(EnemyType type, int count) 
        {
            for (int i = 0; i < count; i++)
                enemies.Add(type);
        }

        /// <summary>
        /// Delayed level start
        /// </summary>
        /// <returns></returns>
        IEnumerator StartLevel()
        {
            if (!PlayerManager.Instance)
                yield break;

            // Disable player controller
            PlayerManager.Instance.EnableController(false);

            // We must take into account the time scale we are playing at
            float timer = startDelay * Time.timeScale;
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            // Start level
            
            PlayerManager.Instance.EnableController(true);

            yield return new WaitForSeconds(0.5f);

            running = true;
            for (int i=0; i<maxEnemiesOnScreen; i++)
            {
                SpawnRandomEnemy();
            }
            
        }

        void SpawnRandomEnemy()
        {
            // Get the enemy type we are going to spawn
            EnemyType type = enemies[UnityEngine.Random.Range(0, enemies.Count)];

            // Remove from the enemy list
            enemies.Remove(type);

            // Get the actual enemy
            GameObject enemy = GetRandomEnemy(type);

            // Spawn it
            EnemySpawnerManager.Instance.Spawn(enemy);

            // Increase number of enemies
            enemiesOnScreen++;

        }

        /// <summary>
        /// Get a random enemy out of the pool: normally the given enemy is passed to the spawner
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        GameObject GetRandomEnemy(EnemyType type)
        {
            // Get a random enemy from the pool
            GameObject enemy = pool.Find(e => e.GetComponent<Enemy>().Type == type);

            // Remove the object from the pool and add it to the used list
            pool.Remove(enemy);
            usedList.Add(enemy);

            return enemy;
        }

        /// <summary>
        /// Move an enemy to the pool
        /// </summary>
        /// <param name="enemy"></param>
        void MoveEnemyToPool(GameObject enemy)
        {
            // Remove from the used list
            usedList.Remove(enemy);

            // Add to pool
            pool.Add(enemy);

            // Disable the enemy
            enemy.SetActive(false);
        }

        void HandleOnDead(Enemy enemy, BlackHole blackHole)
        {
            Debug.Log("EnemyDead:" + enemy.gameObject);

            // Put the enemy back in the pool
            MoveEnemyToPool(enemy.gameObject);

            // If the black hole and the enemy colors don't match we must put the enemy back in the 
            // enemy list
            if (enemy.Type != blackHole.EnemyType)
                enemies.Add(enemy.Type);

            // Decrease number of enemies
            enemiesOnScreen--;

        }

        void CreateEnemyPool()
        {
            if (greenCount > 0)
            {
                AddToPool(EnemyType.Green);
                AddToEnemies(EnemyType.Green, greenCount);
            }

            if (yellowCount > 0)
            {
                AddToPool(EnemyType.Yellow);
                AddToEnemies(EnemyType.Yellow, yellowCount);
            }

            if (redCount > 0)
            {
                AddToPool(EnemyType.Red);
                AddToEnemies(EnemyType.Red, redCount);
            }
        }
    }

}
