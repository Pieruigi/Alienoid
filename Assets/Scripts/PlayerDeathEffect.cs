using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class PlayerDeathEffect : MonoBehaviour
    {
        [SerializeField]
        GameObject cannon;

        // Start is called before the first frame update
        void Start()
        {
            PlayerManager.Instance.OnDead += HandleOnPlayerDead;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnPlayerDead()
        {
            // Get the cannon rigidbody
            Rigidbody rb = cannon.GetComponent<Rigidbody>();

            // Reset kinematic
            rb.isKinematic = false;

            // Add explosion force
            rb.AddExplosionForce(2.0f, transform.position, 3, 0, ForceMode.VelocityChange);

            // Add torque
            rb.AddRelativeTorque(Vector3.forward * 1.0f, ForceMode.VelocityChange);
        }
    }

}
