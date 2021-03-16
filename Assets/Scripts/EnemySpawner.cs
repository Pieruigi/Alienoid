using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        float spawnForce = 5f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Spawn(GameObject enemy)
        {
            // Set position
            enemy.transform.position = transform.position;

            // Add force
            Vector3 dir = transform.up;

            enemy.GetComponent<Rigidbody>().AddForce(dir * spawnForce, ForceMode.VelocityChange);
        }
    }

}
