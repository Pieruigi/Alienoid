using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie
{
    public class LevelManager : MonoBehaviour
    {
        
        public static LevelManager Instance { get; private set; }

        [SerializeField]
        int greenCount, yellowCount, redCount;

        [SerializeField]
        int maxEnemiesOnScreen = 3;

        [SerializeField]
        GameObject enemyPrefab;



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
                // Create pool 
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
                    
                if(redCount>0)
                {
                    AddToPool(EnemyType.Red);
                    AddToEnemies(EnemyType.Red, redCount);
                }

                // Adjust the starting delay
                //startDelay = startDelay * Time.timeScale;
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
    }

}
