using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody rb;

        float maxSpeed = 7.5f;

        float currentSpeed = 0;
        public float CurrentSpeed
        {
            get { return currentSpeed; }
        }

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
            if (Input.GetKey(KeyCode.A))
            {
                currentSpeed = -maxSpeed;
            }
            else
            {
                if (Input.GetKey(KeyCode.D))
                {
                    currentSpeed = maxSpeed;
                }
                else
                {
                    currentSpeed = 0;
                }
            }

            rb.MovePosition(rb.transform.position + rb.transform.right * currentSpeed * Time.fixedDeltaTime);
            
        }


        
    }

}
