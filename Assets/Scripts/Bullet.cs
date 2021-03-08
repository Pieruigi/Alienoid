using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class Bullet : MonoBehaviour
    {

        Rigidbody rb;

        

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
            Debug.Log("Collision:" + collision.gameObject);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

}
