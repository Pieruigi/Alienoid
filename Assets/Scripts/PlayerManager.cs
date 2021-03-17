using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie
{
    public class PlayerManager : MonoBehaviour
    {
        public UnityAction OnDead;

        public static PlayerManager Instance { get; private set; }

        bool dead = false;


        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                Die();
        }

        public void EnableController(bool value)
        {
            GetComponent<PlayerController>().enabled = value;
        }

        /// <summary>
        /// Flag the player as dead and call the action on this object
        /// </summary>
        public void Die()
        {
            if (dead)
                return;

            // Flag as dead
            dead = true;
            // Disable the controller
            EnableController(false);

            // Call action
            OnDead?.Invoke();
        }

        public bool IsDead()
        {
            return dead;
        }
    }

}
