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

           
            // Little randomization on the spawn direction
            Vector3 dir = Quaternion.AngleAxis(Random.Range(-1.5f, 1.5f), transform.forward) * transform.up;
           
            // Little randomization on the force
            float force = spawnForce * Random.Range(0.95f, 1.05f);

            Rigidbody rb = enemy.GetComponent<Rigidbody>();

            rb.AddForce(dir * force, ForceMode.VelocityChange);

            float torqueMag = Random.Range(-3.5f, 3.5f);
            rb.AddTorque(torqueMag * Vector3.forward, ForceMode.VelocityChange);

            
        }
    }

}
