using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie.Services
{
    public class AccountManager : MonoBehaviour
    {
        /// <summary>
        /// Params:
        /// - succeeded
        /// - is silent login
        /// </summary>
        public UnityAction<bool, bool> OnLogin;

        public static AccountManager Instance { get; private set; }

        public bool Logged { get; private set; }

        bool logging = false;
        bool trySilently = true;
      

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);
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
            // We try to login silently as soos as the firebase framework is ready.
            // Silently login works if you are already logged in ( you logged in
            // in a previous game session )
            if (trySilently && !Logged && !logging && FirebaseManager.Instance.Initialized)
            {
                //FirebaseManager.Instance.LogInWithGoogle(true, HandleOnLogin);
                LogIn(true);
            }
        }

        /// <summary>
        /// Log the player in using firebase with google or apple depending on the
        /// running platform.
        /// </summary>
        /// <param name="silently"></param>
        public void LogIn(bool silently)
        {
            if (Logged || logging)
            {
                Debug.LogWarning("Already logged in.");
                return;
            }

            if (!FirebaseManager.Instance.Initialized)
            {
                Debug.LogWarning("Firebase not initialized yet... please wait a while.");
                return;
            }

            logging = true;

#if UNITY_ANDROID
            FirebaseManager.Instance.LogInWithGoogle(silently, HandleOnLogin);
#endif
#if UNITY_IOS
            LogInWithApple(silently);
#endif

        }

        /// <summary>
        /// Returns the player nickname
        /// </summary>
        /// <returns></returns>
        public string GetDisplayName()
        {
            if (FirebaseManager.Instance.User == null)
                return "";

            return FirebaseManager.Instance.User.DisplayName;
        }

        public string GetUserId()
        {
            if (FirebaseManager.Instance.User == null)
                return "";

            return FirebaseManager.Instance.User.UserId;
        }
       
        void HandleOnLogin(bool succeeded)
        {
            // Reset flag
            logging = false;

            Logged = succeeded;
            

            if (!Logged)
            {
                if (trySilently)
                { 
                    // Automatic login failed, we must show some warning 
                    Debug.Log("You shold be loggedin to improuve your game experience");
                }
                else
                {
                    // Player tried to login but something goes wrong
                    Debug.LogWarning("Something goes wrong, please try later");
                }
            }
            else
            {
                Debug.LogFormat("User {0} logged in", GetDisplayName());

               
                
            }
            
            OnLogin?.Invoke(Logged, trySilently);

            // We try silently on start only once
            if(trySilently)
                trySilently = false;
        }
    }

}
