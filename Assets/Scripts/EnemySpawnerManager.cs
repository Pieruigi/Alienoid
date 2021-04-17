
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        public UnityAction<Enemy> OnEnemySpawned;

        public static EnemySpawnerManager Instance { get; private set; }

        [SerializeField]
        List<EnemySpawner> spawners;

        // The next spawner id
        int nextSpawnerId = 0;
        public int NextSpawnerId
        {
            get { return nextSpawnerId; }
        }

        //DateTime lastSpawnTime;
        float spawnElapsed = 0;
        float spawnTime = 3;//3f; // 1.5f;
        //float spawnMinDist = 6;//10f;//3f;//4f;
        
        

        List<GameObject> spawnList = new List<GameObject>();
        //float spawnMinDistSqr;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                // Randomize first ( both for random and round spawner )
                nextSpawnerId = UnityEngine.Random.Range(0, spawners.Count);
                //spawnMinDistSqr = spawnMinDist * spawnMinDist;
            }
            else
            {
                Destroy(gameObject);
            }

        }


        // Start is called before the first frame update
        void Start()
        {

            // Adjust the spawning time
            spawnTime *= Constants.DefaultTimeScale / Time.timeScale;
            Debug.LogFormat("SpawnTime: {0}", spawnTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (spawnList.Count == 0)
                return;

            spawnElapsed += Time.deltaTime;
            if (spawnElapsed < spawnTime)
                return;

            // Spawn first enemy in the list
            GameObject enemy = spawnList[0];
            // Remove from the list
            spawnList.RemoveAt(0);

            // Activate enemy
            enemy.SetActive(true);

            // Spawn the enemy 
            GetSpawner().Spawn(enemy);


            spawnElapsed = 0;

            OnEnemySpawned?.Invoke(enemy.GetComponent<Enemy>());
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
            nextSpawnerId++;
            if (nextSpawnerId >= spawners.Count)
                nextSpawnerId = 0;


            return spawners[nextSpawnerId];
        }
    }

}
