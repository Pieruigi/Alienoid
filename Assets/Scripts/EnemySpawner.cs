using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }

        [SerializeField]
        List<Transform> spawnPoints;

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
                nextId = UnityEngine.Random.Range(0, spawnPoints.Count);
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
            
            // Set position
            enemy.transform.position = spawnPoints[nextId].transform.position;
            
            // Activate enemy
            enemy.SetActive(true);

            // Add force
            Vector3 dir = spawnPoints[nextId].up;

            enemy.GetComponent<Rigidbody>().AddForce(spawnPoints[nextId].up * forceMagnitude, ForceMode.VelocityChange);

            // Update next spawn id
            nextId++;
            if (nextId >= spawnPoints.Count)
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
