using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Zom.Pie.Services
{
    public class AccountManager : MonoBehaviour
    {
        public static AccountManager Instance { get; private set; }

        public bool Logged { get; private set; }

        bool logging = false;

        DateTime lastLoggingAttempt;

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
            return;
            if (!Logged && !logging && FirebaseManager.Instance.Initialized)
            {
                // Try log in
                logging = true;

                FirebaseManager.Instance.LogInWithGoogle();
            }
        }

       
    }

}
