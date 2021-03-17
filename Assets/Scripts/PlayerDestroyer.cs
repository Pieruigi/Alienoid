using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class PlayerDestroyer : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            // If we get hit by an enemy then we die
            if (Tag.Enemy.ToString().Equals(collision.gameObject.tag))
            {
                if (PlayerManager.Instance.IsDead())
                    return;

                // Flag player as dead
                PlayerManager.Instance.Die();

                
            }
        }

    }

}
