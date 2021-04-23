#define TYPE_2
#if TYPE_1
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class EnemySpeeder : MonoBehaviour
    {
        [SerializeField]
        float forceMagnitude = 0f;//8f;

        // The up-transform is the force direction
        [SerializeField]
        Transform forceDirection;

        [SerializeField]
        float impulseMagnitude = 0f;

        [SerializeField]
        Transform impulseDirection;

        List<Rigidbody> rbs = new List<Rigidbody>();

        Vector3 force;
        Vector3 impulse;

        private void Awake()
        {
            force = forceMagnitude * forceDirection.up;
            impulse = impulseMagnitude * impulseDirection.up;
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
            foreach(Rigidbody rb in rbs)
            {
                float m = forceMagnitude * Random.Range(0.9f, 1.1f);
                Vector3 d = Quaternion.AngleAxis(Random.Range(-5f, 5f), forceDirection.forward) * forceDirection.up;

                rb.AddForce(m*d, ForceMode.Acceleration);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Tag.Enemy.ToString().Equals(other.tag))
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();

                // Try add the force
                if (forceMagnitude != 0)
                {
                    if (!rbs.Contains(rb))
                        rbs.Add(rb);
                }

                // Try add the impulse
                if(impulseMagnitude != 0)
                {
                    float m = impulseMagnitude * Random.Range(0.9f, 1.1f);
                    Vector3 d = Quaternion.AngleAxis(Random.Range(-5f, 5f), impulseDirection.forward) * impulseDirection.up;
                    rb.AddForce(m*d, ForceMode.VelocityChange);
                }
                    
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Tag.Enemy.ToString().Equals(other.tag))
            {
                rbs.Remove(other.GetComponent<Rigidbody>());

            }
        }


    }

}
#endif


#if TYPE_2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class EnemySpeeder : MonoBehaviour
    {
        [SerializeField]
        float forceMagnitude = 0f;//8f;

        // The up-transform is the force direction
        [SerializeField]
        Transform forceDirection;

        [SerializeField]
        bool alignToForceDirection = false;

        [SerializeField]
        float alignToForceDirectionSpeed = 0.5f;

        [SerializeField]
        float impulseMagnitude = 0f;

        [SerializeField]
        Transform impulseDirection;

        List<Rigidbody> rbs = new List<Rigidbody>();

        //Vector3 force;
        //Vector3 impulse;

        private void Awake()
        {
            //force = forceMagnitude * forceDirection.up;
            
            //if(impulseMagnitude != 0)
            //    impulse = impulseMagnitude * impulseDirection.up;
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
            foreach(Rigidbody rb in rbs)
            {
                
                float m = forceMagnitude * Random.Range(0.9f, 1.1f);
                Vector3 d = Quaternion.AngleAxis(Random.Range(-5f, 5f), forceDirection.forward) * forceDirection.up;

                rb.AddForce(m*d, ForceMode.Acceleration);

                if (alignToForceDirection)
                {
                    // Adjust velocity direction
                    Vector3 newVelocity = Mathf.Sign(forceMagnitude) * forceDirection.up * rb.velocity.magnitude;
                    rb.velocity = Vector3.MoveTowards(rb.velocity, newVelocity , alignToForceDirectionSpeed * Time.fixedDeltaTime);
                    
                }
                
            }
        }

        public void Reverse()
        {
            Debug.Log("Reverse speeder:" + gameObject.name);
            forceMagnitude *= -1;
            impulseMagnitude *= -1;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Tag.Enemy.ToString().Equals(other.tag))
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();

                // Try add the force
                if (forceMagnitude != 0)
                {
                    if (!rbs.Contains(rb))
                        rbs.Add(rb);
                }

                // Try add the impulse
                if(impulseMagnitude != 0)
                {
                    float m = impulseMagnitude * Random.Range(0.9f, 1.1f);
                    Vector3 d = Quaternion.AngleAxis(Random.Range(-5f, 5f), impulseDirection.forward) * impulseDirection.up;
                    rb.AddForce(m*d, ForceMode.VelocityChange);
                }
                    
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Tag.Enemy.ToString().Equals(other.tag))
            {
                rbs.Remove(other.GetComponent<Rigidbody>());

            }
        }


    }

}
#endif