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

        float forceMagnitude = 5f;

        int nextId = 0;

        DateTime lastSpawnTime;
        float spawnTime = 1f;

        List<GameObject> spawnList = new List<GameObject>();

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                // Randomize first
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

            spawners[nextId].Spawn(enemy);

            // Update next spawn id
            nextId++;
            if (nextId >= spawners.Count)
                nextId = 0;

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
    }

}
