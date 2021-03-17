#define RANDOM
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

#if !RANDOM

            ret = spawners[nextId];
                // Update next spawn id
                nextId++;
                if (nextId >= spawners.Count)
                    nextId = 0;
#else
                // We want the enemy not to spawn too close to another enemy, so we first remove from the
                // list the closer spawner.
                // Since we only have two enemies on screen at the same time, this means that if we are 
                // spawning then only one left on screen, so we simply get the first; we could change 
                // this in case we would like to manage more than two enemies at the same time.

                // Get all the spawners
                List<EnemySpawner> tmp = spawners.GetRange(0, spawners.Count);

                // If there is at least one enemy on screen then remove the closer spawner
                if(LevelManager.Instance.GetOnScreenEnemies().Count > 0)
                {
                    // Get the current enemy
                    GameObject currentEnemy = LevelManager.Instance.GetOnScreenEnemies()[0];

                // Get the closer
                //bool onlyTheCloserOne = false;
#if THE_CLOSER_ONLY
                float sqrDist = 0;
                    EnemySpawner closer = null;
#else
                List<EnemySpawner> closers = new List<EnemySpawner>();
                float minSqrDist = 4f * Enemy.SqrRadius * 16f;
#endif
                // The minimun distance at which the spawner is removed

                foreach (EnemySpawner spawner in tmp)
                    {
                        float tmpDist = (spawner.transform.position - currentEnemy.transform.position).sqrMagnitude;
#if THE_CLOSER_ONLY

                            // We remove only the closer spawner
                        if (closer == null || sqrDist > tmpDist)
                        {
                            sqrDist = tmpDist;
                            closer = spawner;
                        }

#else

                        // We remove all the spawner which are too close
                        if (tmpDist < minSqrDist)
                        {
                            closers.Add(spawner);
                        }
   
#endif


                    }

#if THE_CLOSER_ONLY
                    if (closer)
                    {
                        tmp.Remove(closer);
                    }
#else

                    // Try remove
                    foreach (EnemySpawner spawner in closers)
                    {
                        // Debug.Log("Remove closer spawner:" + closer.gameObject);
                        tmp.Remove(spawner);
                    }
#endif



                }

                // Remove the last used spawner
                tmp = tmp.FindAll(s => spawners.IndexOf(s) != nextId);

                // Get a random spawner from the temp list
                ret = tmp[UnityEngine.Random.Range(0, tmp.Count)];
                // Update the next id ( which in this case is the last used id )
                nextId = spawners.IndexOf(ret);
#endif

                return ret;
        }
    }

}
