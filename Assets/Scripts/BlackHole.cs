using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class BlackHole : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if ("enemy".Equals(other.tag.ToLower()))
            {
                Debug.Log("Destroying enemy");
                other.GetComponent<Enemy>().Destroy();
            }
        }
    }

}
