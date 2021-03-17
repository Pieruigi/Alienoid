using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class EnemySpeeder : MonoBehaviour
    {
        [SerializeField]
        float forceMagnitude = 3.5f;

        List<Transform> directions = new List<Transform>();

        

        // Start is called before the first frame update
        void Start()
        {
            for(int i=0; i<transform.childCount; i++)
            {
                directions.Add(transform.GetChild(i));
            }
        }

        // Update is called once per frame
        void Update()
        {

        }



        private void OnTriggerEnter(Collider other)
        {
            if (Tag.Enemy.ToString().Equals(other.tag))
            {
               
                ConstantForce cf = other.GetComponent<ConstantForce>();
                if (!cf)
                {
                    cf = other.gameObject.AddComponent<ConstantForce>();
                }

                cf.force = directions[0].up * forceMagnitude;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Tag.Enemy.ToString().Equals(other.tag))
            {
                ConstantForce cf = other.GetComponent<ConstantForce>();
                if (cf)
                {
                    Destroy(cf);
                }

            }
        }

        IEnumerator DestroyConstantForce(ConstantForce cf)
        {
            yield return new WaitForSeconds(1.0f);
            if (cf)
                Destroy(cf);
        }
    }

}
