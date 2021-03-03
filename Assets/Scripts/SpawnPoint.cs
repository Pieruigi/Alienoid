using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class SpawnPoint : MonoBehaviour
    {
        
        int unsafeCount = 0;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool IsSafe()
        {
            return unsafeCount == 0;
        }

        public GameObject Spawn(GameObject prefab)
        {
            GameObject obj = GameObject.Instantiate(prefab, transform.position, transform.rotation);
            return obj;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ("enemy".Equals(other.tag.ToLower()))
                unsafeCount++;
        }

        private void OnTriggerExit(Collider other)
        {
            if ("enemy".Equals(other.tag.ToLower()))
                unsafeCount--;
        }
    }

}
