using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        float timer = 10;

        [SerializeField]
        GameObject enemyPrefab;

        [SerializeField]
        List<SpawnPoint> spawnPoints;

        int enemyCount = 0;

        DateTime lastSpawnTime;

        SpawnPoint lastSpawnPoint;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // If time to spawn or there is no enemy left then spawn
            if ((DateTime.UtcNow - lastSpawnTime).TotalSeconds > timer || enemyCount == 0)
            {
                // Some enemy might be to close 
                TrySpawn();
            }
                
        }

        void TrySpawn()
        {
            // Get all safe spawn points
            List<SpawnPoint> points = spawnPoints.FindAll(s => s.IsSafe() && s != lastSpawnPoint);

            // If there is no safe point return...
            if (points.Count == 0)
                return;

            // ... otherwise spawn
            // Get a random point
            SpawnPoint point = points[UnityEngine.Random.Range(0, points.Count)];


            GameObject enemy = point.Spawn(enemyPrefab);
            lastSpawnPoint = point;
            
            enemy.GetComponent<Enemy>().OnDestroy += HandleEnemyOnDestroy;
            enemyCount++;
            lastSpawnTime = DateTime.UtcNow;
        }

        void HandleEnemyOnDestroy(Enemy enemy)
        {
            enemyCount--;
        }

       
    }

}
