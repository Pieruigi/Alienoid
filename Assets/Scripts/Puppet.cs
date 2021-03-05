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

        }

        public void Die()
        {
            OnDead?.Invoke(this);

            Destroy(gameObject, 1f);
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
            //Vector3 origin = rb.position;
            //float distance = coll.height / 2f + coll.radius;
            //Ray ray = new Ray(origin, Vector3.down);
            //int layer = LayerMask.GetMask(new string[] { "Floor" });
            //if (Physics.Raycast(ray, distance, layer))
            //    return true;

            Vector3 position = rb.position - coll.height / 2f * Vector3.up;
            position -= Vector3.up * 0.1f;
            int layer = LayerMask.GetMask(new string[] { "Floor" });
            Collider[] colls = null;
            colls = Physics.OverlapSphere(position, coll.radius, layer);
            if(colls.Length == 0) Debug.Log("Colls.length:" + colls.Length);
            

            if (colls!=null && colls.Length>0)
                return true;

            return false;
        }

    }

}
