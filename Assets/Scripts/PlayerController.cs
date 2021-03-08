using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class PlayerController : MonoBehaviour
    {
        
        [SerializeField]
        float fireRate = 2f;

        [SerializeField]
        Transform cannonPivot;

        [SerializeField]
        Collider cannonCollider;


        Collider baseCollider;

        float fireCooldown;
        DateTime lastShot;
        

        private void Awake()
        {
            baseCollider = GetComponent<Collider>();

            // Init the cooldown
            SetFireRate(fireRate);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {

            

            // Try to shot
            if((DateTime.UtcNow - lastShot).TotalSeconds > fireCooldown)
            {
                // Ok, we can shot
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Shooting...");

                    // Cast a ray from the mouse position to the world
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    int layerMask = LayerMask.GetMask(new string[] { Layer.RaycastPlane.ToString() });
                    RaycastHit hitInfo;
                    if(Physics.Raycast(ray, out hitInfo, 100, layerMask))
                    {
                        // Get direction
                        Vector3 direction = hitInfo.point - transform.position;
                        direction.z = 0;

                        Debug.Log("New cannon direction:" + direction);

                        // Rotate the cannon
                        cannonPivot.up = direction;
                    }

                  

                    // Init cooldown
                    lastShot = DateTime.UtcNow;
                }
            }
            

        }

        public void SetFireRate(float value)
        {
            fireRate = value;
            fireCooldown = 1f / fireRate;
        }
    }

}
