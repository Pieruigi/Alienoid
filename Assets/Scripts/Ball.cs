using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class Ball : MonoBehaviour
    {
        float speed = 5f;

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
        void FixedUpdate()
        {
            //transform.position += transform.forward * speed * Time.deltaTime;
            rb.MovePosition(rb.transform.position + rb.transform.up * speed * Time.fixedDeltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint contact = collision.GetContact(0);

            // Sometimes the ball hits two bricks and then turns twice ( going through the second brick )
            if (Vector3.Dot(transform.up, contact.normal) > 0)
                return;

            // If we hit a brick we must report the hit
            if (Tags.Brick.Equals(collision.collider.tag))
                collision.collider.GetComponent<Brick>().Hit();

            // Get the angle between forward and the collision normal
            float angle = Vector3.SignedAngle(transform.up, contact.normal, Vector3.forward);

            // If the collider is the player we must check his direction to adjust the collision angle.
            // In fact we want the player to control bouncing.
            if (collision.collider.GetComponent<PlayerController>())
            {
                // Get velocity
                Vector3 vel = collision.collider.GetComponent<PlayerController>().CurrentSpeed * Vector3.right;

                if (vel != Vector3.zero)
                {
                    // Dot product can tell us if sphere and player are moving to the same direction
                    float dot = Vector3.Dot(transform.up, vel);

                    // If dot < 0 we are moving to the opposit direction
                    if (dot > 0)
                    {
                        angle *= 1.2f;
                    }
                    else
                    {
                        angle *= 0.8f;
                    }
                }

            }

            // Add a little random value
            //angle += UnityEngine.Random.Range(-3f, 3f);

            // Get the new direction
            transform.up = Quaternion.AngleAxis(angle - 180.0f, Vector3.forward) * contact.normal;
            Debug.Log("ContactNormal:" + contact.normal);


        }
    }

}
