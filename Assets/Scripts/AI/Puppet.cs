using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie
{
    public class Puppet : MonoBehaviour
    {
        public UnityAction<Puppet> OnDead;

        Rigidbody rb;

        float moveSpeed = 1.5f;
 
        int moveDirection = 0;
        int aiMoveDirection = 0;

        CapsuleCollider coll;
        GameObject steppedOn = null;
        DateTime lastInterruption;
        float interruptionTime = 0.25f;

        bool grounded = true;
        DateTime lastGrounded;
        float groundedTime = 0.5f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            coll = GetComponent<CapsuleCollider>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Die()
        {
            OnDead?.Invoke(this);

            Destroy(gameObject, 1f);
        }

        public void SetAIMoveDirection(int newAIDirection)
        {
            // This is called by the ai
            aiMoveDirection = newAIDirection;
            
            // Change direction
            SetMoveDirection(newAIDirection);

        }

        private void FixedUpdate()
        {
            // Check the player is grounded
            grounded = IsGrounded();

            // If not grounded then free rigid body 
            if (!grounded)
            {
                
                moveSpeed = Mathf.MoveTowards(moveSpeed, 0, Time.deltaTime);

                // Just to be sure puppet is falling down
                if((DateTime.UtcNow-lastGrounded).TotalSeconds > groundedTime)
                    rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionZ;
            }
            else
            {
                // We save the last time the puppet was grounded
                lastGrounded = DateTime.UtcNow;
            }

            // If grounded first check whether the path is interrupted or not
            if (grounded)
            {
                if((DateTime.UtcNow- lastInterruption).TotalSeconds > interruptionTime)
                {
                    lastInterruption = DateTime.UtcNow;

                    bool interrupted = PathInterrupted();
                    if (interrupted)
                    {
                        Debug.Log("Check path:"+interrupted);
                        // Switch direction
                        SetMoveDirection(-moveDirection);
                    }
                }
                
            }

            if(moveDirection != 0)
            {
                rb.MovePosition(rb.position + Vector3.right * moveDirection * moveSpeed * Time.fixedDeltaTime);
            }
                
        }

        void SetMoveDirection(int newDirection)
        {
            if (moveDirection != newDirection)
            {
                if (newDirection > 0)
                {
                    // Move right
                    transform.forward = Vector3.right;
                }
                else
                    if (newDirection < 0)
                {
                    transform.forward = Vector3.left;
                }
            }

            moveDirection = newDirection;
        }

        bool IsGrounded()
        {
            //Vector3 origin = rb.position;
            //float distance = coll.height / 2f + coll.radius;
            //Ray ray = new Ray(origin, Vector3.down);
            //int layer = LayerMask.GetMask(new string[] { "Floor" });
            //if (Physics.Raycast(ray, distance, layer))
            //    return true;

            

            // We cast a sphere to check whether the puppet is grounded or not.
            Vector3 position = rb.position - coll.height / 2f * Vector3.up;
            // We just want to check for ground layer
            int layer = LayerMask.GetMask(new string[] { Layers.Ground });
            Collider[] colls = null;
            colls = Physics.OverlapSphere(position, coll.radius, layer);
            if(colls.Length == 0) Debug.Log("Colls.length:" + colls.Length);
         
            // If there is a collision return true
            if (colls!=null && colls.Length > 0)
            {
                steppedOn = colls[0].gameObject;
                return true;
            }
                

            // No collision
            return false;
        }

        bool PathInterrupted()
        {
            

            // Brick length
            float l = steppedOn.GetComponent<BoxCollider>().size.x;

            // Cast a ray to get the next brick
            Vector3 origin = rb.transform.position + moveDirection * Vector3.right * l;
            Ray ray = new Ray(origin, Vector3.down);
            float distance = coll.height / 2f + coll.radius;
            int layer = LayerMask.GetMask(new string[] { Layers.Ground });
            if (Physics.Raycast(ray, distance, layer))
                return false;

            return true;

        }
    }

}
