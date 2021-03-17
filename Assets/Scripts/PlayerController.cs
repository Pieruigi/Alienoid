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
        float firePower = 5f;

        [SerializeField]
        Transform cannonPivot;

        [SerializeField]
        Collider cannonCollider;

        [SerializeField]
        Collider baseCollider;

        [SerializeField]
        Transform firePoint;

        

        float fireCooldown;
        DateTime lastShot;
        

        private void Awake()
        {
            
            // Disable collision between base and cannon
            Physics.IgnoreCollision(baseCollider, cannonCollider);

            // Init the cooldown
            SetFireRate(fireRate);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {

            

            // If mouse is down then aim the weapon
            if (Input.GetMouseButton(0))
            {


                // Cast a ray from the mouse position to the world
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMask = LayerMask.GetMask(new string[] { Layer.RaycastPlane.ToString() });
                RaycastHit hitInfo;
                if(Physics.Raycast(ray, out hitInfo, 100, layerMask))
                {
                    // Get direction
                    Vector3 direction = hitInfo.point - transform.position;
                    direction.z = 0;


                    // Aim the cannon
                    cannonPivot.up = direction;
                }

                // If time to shoot then shoot
                if ((DateTime.UtcNow - lastShot).TotalSeconds > fireCooldown)
                {
                    // Get the next bullet from the pool
                    GameObject bullet = BulletPool.Instance.GetBullet();
                    //GameObject bullet = GameObject.Instantiate(bulletPrefab);

                    // Set position and orientation
                    bullet.transform.position = firePoint.position;
                    bullet.transform.forward = firePoint.up;

                    // Activate the bullet
                    bullet.SetActive(true);

                    // Shoot the bullet

                    bullet.GetComponent<Bullet>().Shoot(firePower * firePoint.up);

                    // Reset cooldown
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
