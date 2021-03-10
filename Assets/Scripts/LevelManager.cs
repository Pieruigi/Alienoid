using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        float startDelay = 0;
        bool running = false;

        int enemiesOnScreen = 0;
        

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                // Create pool
                //pool = new List<GameObject>(maxEnemiesOnScreen*(greenCount > 0 ? 1 : 0 + yellowCount > 0 ? 1 : 0 + redCount > 0 ? 1 : 0));
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
            for(int i=0; i<maxEnemiesOnScreen; i++)
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

            float timer = startDelay;
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            // Start level
            running = true;
            PlayerManager.Instance.EnableController(true);

            yield return new WaitForSeconds(0.5f);

            for(int i=0; i<maxEnemiesOnScreen; i++)
            {
                // Get the enemy type we are going to spawn
                EnemyType type = enemies[UnityEngine.Random.Range(0, enemies.Count)];

                // Get the actual enemy
                GameObject enemy = GetRandomEnemy(type);

                // Spawn it
                EnemySpawner.Instance.Spawn(enemy);
            }
            
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

        void HandleOnDead(Enemy enemy)
        {
            // Put the enemy back in the pool
            MoveEnemyToPool(enemy.gameObject);
        }
    }

}
