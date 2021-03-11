using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class Bullet : MonoBehaviour
    {

        Rigidbody rb;

        float minForce = 1f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            //rb.MovePosition(rb.position + transform.forward * 44 * Time.deltaTime);
        }

        public void Shoot(Vector3 velocity)
        {
            
            rb.AddForce(velocity, ForceMode.VelocityChange);
        }

       

        private void OnCollisionEnter(Collision collision)
        {
            if (Tag.Enemy.ToString().Equals(collision.collider.tag))
            {
                // Just add a little force to the enemy
                
                Vector3 force = -minForce * collision.contacts[0].normal;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
            }

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

}
