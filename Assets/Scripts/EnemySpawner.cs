using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        int greenCount, redCount, yellowCount;

        [SerializeField]
        GameObject enemyPrefab;

        List<GameObject> redPool, yellowPool, greenPool;

        private void Awake()
        {
            // Create pools
            greenPool = CreateEnemyPool(EnemyType.Green);
            redPool = CreateEnemyPool(EnemyType.Red);
            yellowPool = CreateEnemyPool(EnemyType.Yellow);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        List<GameObject> CreateEnemyPool(EnemyType type)
        {
            // Get number of enemies of the given type
            int count = 0;
            switch (type)
            {
                case EnemyType.Green:
                    count = greenCount;
                    break;
                case EnemyType.Red:
                    count = redCount;
                    break;
                case EnemyType.Yellow:
                    count = yellowCount;
                    break;
            }
            // No enemies
            if (count == 0)
                return null;

            // Create pool
            List<GameObject> ret = new List<GameObject>(count);
            for(int i=0; i<count; i++)
            {
                // Create enemy
                GameObject enemy = GameObject.Instantiate(enemyPrefab);
                // Set enemy type
                enemy.GetComponent<Enemy>().SetType(type);
                // Deactivate enemy
                enemy.SetActive(false);
                // Add to pool
                ret.Add(enemy);
            }


            return ret;
        }
    }

}
