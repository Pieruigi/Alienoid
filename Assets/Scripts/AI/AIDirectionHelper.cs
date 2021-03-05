using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    /// <summary>
    /// Determines the puppet direction on this floor by using the right axis.
    /// </summary>
    public class AIDirectionHelper : MonoBehaviour
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
                // On enter we set the ai direction of the puppet on this floor
                other.GetComponent<Puppet>().SetAIMoveDirection(transform.right == Vector3.right ? 1 : -1);
            }
        }
                
    }

}
