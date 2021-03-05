using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class BottomTrigger : MonoBehaviour
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
            if (Tags.Puppet.Equals(other.tag))
            {
                other.GetComponent<Puppet>().Die();
            }
        }
    }

}