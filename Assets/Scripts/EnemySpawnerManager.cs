
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

        //DateTime lastSpawnTime;
        float spawnElapsed = 0;
        float spawnTime = 6;//3f; // 1.5f;
        float spawnMinDist = 6;//10f;//3f;//4f;
        
        

        List<GameObject> spawnList = new List<GameObject>();
        float spawnMinDistSqr;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                // Randomize first ( both for random and round spawner )
                nextId = UnityEngine.Random.Range(0, spawners.Count);
                spawnMinDistSqr = spawnMinDist * spawnMinDist;
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

                
                EnemySpawner closer = null;
                float sqrDist = 0;
                // The minimun distance at which the spawner is removed
                

                foreach (EnemySpawner spawner in tmp)
                {
                    float tmpDist = (spawner.transform.position - currentEnemy.transform.position).sqrMagnitude;

                        // We remove only the closer spawner
                    if ((closer == null && tmpDist < spawnMinDistSqr) || sqrDist > tmpDist) 
                    {
                        sqrDist = tmpDist;
                        closer = spawner;
                    }

                }


                if (closer)
                {
                    tmp.Remove(closer);
                }




                }



                // Get a random spawner from the temp list
                ret = tmp[UnityEngine.Random.Range(0, tmp.Count)];
                // Update the next id ( which in this case is the last used id )
                nextId = spawners.IndexOf(ret);

            return ret;
        }
    }

}
