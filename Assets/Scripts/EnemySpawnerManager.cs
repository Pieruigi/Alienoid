using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        public static EnemySpawnerManager Instance { get; private set; }

        [SerializeField]
        List<EnemySpawner> spawners;

        // This is the next spawner that will be used when randomSpawner is false, otherwise it represents
        // the last used spawner id 
        int nextId = 0;

        DateTime lastSpawnTime;
        float spawnTime = 1f;

        List<GameObject> spawnList = new List<GameObject>();

        bool randomSpawner = true;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                // Randomize first ( both for random and round spawner )
                nextId = UnityEngine.Random.Range(0, spawners.Count);
            }
            else
            {
                Destroy(gameObject);
            }

        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (spawnList.Count == 0)
                return;

            if ((DateTime.UtcNow - lastSpawnTime).TotalSeconds < spawnTime)
                return;

            // Spawn first enemy in the list
            GameObject enemy = spawnList[0];
            // Remove from the list
            spawnList.RemoveAt(0);

            // Activate enemy
            enemy.SetActive(true);

            // Spawn the enemy 
            GetSpawner().Spawn(enemy);


            lastSpawnTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Simply add the next enemy to spawn on the list
        /// </summary>
        /// <param name="enemy"></param>
        public void Spawn(GameObject enemy)
        {
            spawnList.Add(enemy);
        }

        EnemySpawner GetSpawner()
        {
            EnemySpawner ret = null;
            if (!randomSpawner)
            {
                ret = spawners[nextId];
                // Update next spawn id
                nextId++;
                if (nextId >= spawners.Count)
                    nextId = 0;

            }
            else
            {
                // Get all the spawners except for the last used, if any
                List<EnemySpawner> tmp = spawners.FindAll(s => spawners.IndexOf(s) != nextId);

                // Get a random spawner from the temp list
                ret = tmp[UnityEngine.Random.Range(0, tmp.Count)];
                // Update the next id ( which in this case is the last used id )
                nextId = spawners.IndexOf(ret);


            }

            return ret;
        }
    }

}
