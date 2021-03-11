using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class PlayerGravity : MonoBehaviour
    {

        private float forceMul = 0.5f;

        Vector3 playerPosition;

        // Start is called before the first frame update
        void Start()
        {
            playerPosition = GameObject.FindGameObjectWithTag(Tag.Player.ToString()).transform.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerStay(Collider other)
        {
            if (Tag.Enemy.ToString().Equals(other.tag))
            {

                Rigidbody rb = other.GetComponent<Rigidbody>();

                
                //float forceMag = Physics.gravity.y * forceMul;

                Vector3 dir = playerPosition - other.transform.position;
                dir.y = dir.z = 0;
                
                rb.AddForce(forceMul * dir.normalized);



            }
        }
    }

}
