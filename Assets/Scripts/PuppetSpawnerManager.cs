using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class PuppetSpawnerManager : MonoBehaviour
    {
        [SerializeField]
        List<Transform> spawners;

        [SerializeField]
        GameObject puppetPrefab;

        Transform lastSpawner;

        int puppetCount = 0;
        DateTime lastSpawnTime;
        float spawnInterval = 30f;

        List<GameObject> puppets = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Check if it's time to spawn.
            if((DateTime.UtcNow - lastSpawnTime).TotalSeconds > spawnInterval)
            {
                // Spawn a new puppet.
                // Get the next spawner avoiding the last used one.
                if (spawners.Count > 1)
                {
                    // If we have more than one spawner we chose a random one
                    List<Transform> tmp = spawners.FindAll(s => s != lastSpawner);
                    lastSpawner = tmp[UnityEngine.Random.Range(0, tmp.Count)];
                }
                else
                {
                    // We get the first one
                    lastSpawner = spawners[0];
                }
                
                
                // Spawn the puppet
                GameObject puppet = GameObject.Instantiate(puppetPrefab, lastSpawner.position, lastSpawner.rotation);

                // Add to the list
                puppets.Add(puppet);

                // Set handles
                puppet.GetComponent<Puppet>().OnDead += HandleOnDead;

                // Set the direction of the puppet.
                if (lastSpawner.right == Vector3.right)
                    puppet.GetComponent<Puppet>().SetMoveDirection(1);
                else
                    puppet.GetComponent<Puppet>().SetMoveDirection(-1);

                // Update timer
                lastSpawnTime = DateTime.UtcNow;
            }
        }

        void HandleOnDead(Puppet puppet)
        {
            // Remove from the list
            puppets.Remove(puppet.gameObject);
        }
    }

}
