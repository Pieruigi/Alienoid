using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie
{
    public class Enemy : MonoBehaviour
    {
        public UnityAction<Enemy> OnDestroy;

        float speed =7.5f;

        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            //transform.localScale = Vector3.zero;


        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(ManageCollider());

            // Scale to 1
            //transform.DOScale(Vector3.one, 0.25f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //transform.position += transform.forward * speed * Time.deltaTime;
            rb.MovePosition(rb.transform.position + rb.transform.forward * speed * Time.fixedDeltaTime);
        }

        public void Destroy()
        {
            OnDestroy?.Invoke(this);

            StartCoroutine(DoDestroy());
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint contact = collision.GetContact(0);

            // Get the angle between forward and the collision normal
            float angle = Vector3.SignedAngle(transform.forward, contact.normal, Vector3.up);
            
            // If the collider is the player we must check his direction to adjust the collision angle.
            // In fact we want the player to control bouncing.
            if(collision.collider.GetComponent<PlayerController>())
            {
                // Get velocity
                Vector3 vel = collision.collider.GetComponent<PlayerController>().CurrentSpeed * Vector3.right;

                if(vel != Vector3.zero)
                {
                    // Dot product can tell us if sphere and player are moving to the same direction
                    float dot = Vector3.Dot(transform.forward, vel);

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
            

            // Get the new direction
            transform.forward = Quaternion.AngleAxis(angle-180.0f, Vector3.up) * contact.normal;

            

        }

        IEnumerator ManageCollider()
        {
            // Disable on spawn
            GetComponent<Collider>().enabled = false;

            // Just wait for a safe position
            yield return new WaitForSeconds(0.5f);

            // Enable collider again
            GetComponent<Collider>().enabled = true;
        }

        IEnumerator DoDestroy()
        {
            // Disable collider
            GetComponent<Collider>().enabled = false;

            transform.DOScale(Vector3.zero, 0.25f);
            yield return new WaitForSeconds(0.25f);

            GameObject.Destroy(gameObject);
        }
    }

}
