using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class Bullet : MonoBehaviour
    {
        float speed = 0;

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
            if (speed == 0)
                return;

            rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
        }

        public void Shoot(float speed)
        {
            this.speed = speed;
        }

       

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision:" + collision.gameObject);

            speed = 0;

            gameObject.SetActive(false);
        }
    }

}
