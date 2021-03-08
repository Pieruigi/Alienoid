using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField]
        GameObject bulletPrefab;

        public static BulletPool Instance { get; private set; }

        List<GameObject> pool;
        int poolCapacity = 10;

        int nextBullet = 0;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                pool = new List<GameObject>(poolCapacity);
                // Init the pool
                for(int i=0; i<poolCapacity; i++)
                {
                    // Create bullet
                    GameObject bullet = GameObject.Instantiate(bulletPrefab);
                    bullet.SetActive(false);

                    // Add bullet to pool
                    pool.Add(bullet);
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

        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject GetBullet()
        {
            GameObject ret = pool[nextBullet];

            nextBullet++;
            if (nextBullet >= poolCapacity)
                nextBullet = 0;

            return ret;
        }

        
    }

}
