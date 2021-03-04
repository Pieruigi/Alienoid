using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class Puppet : MonoBehaviour
    {
        Rigidbody rb;

        float moveSpeed = 1.5f;
 
 

        float moveDirection = 0;

        CapsuleCollider coll;
        

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
            if (Input.GetKeyDown(KeyCode.A))
                SetMoveDirection(-1);
            else
                if (Input.GetKeyDown(KeyCode.D))
                SetMoveDirection(1);
        }

        public void SetMoveDirection(int direction)
        {
            // Check if player must rotate
            if(moveDirection != direction)
            {
                if (direction > 0)
                {
                    // Move right
                    transform.forward = Vector3.right;
                }
                else
                    if(direction < 0)
                    {
                        transform.forward = Vector3.left;
                    }
            }

            moveDirection = direction;
        }

        private void FixedUpdate()
        {
            if (!IsGrounded())
            {
                rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionZ;
                moveSpeed = Mathf.MoveTowards(moveSpeed, 0, Time.deltaTime);
            }

            if(moveDirection != 0)
            {
                rb.MovePosition(rb.position + Vector3.right * moveDirection * moveSpeed * Time.fixedDeltaTime);
            }
                
        }

        bool IsGrounded()
        {
            Vector3 origin = rb.position;
            float distance = coll.height / 2f + coll.radius;
            Ray ray = new Ray(origin, Vector3.down);
            int layer = LayerMask.GetMask(new string[] { "Floor" });
            if (Physics.Raycast(ray, distance, layer))
                return true;

            return false;
        }

    }

}
