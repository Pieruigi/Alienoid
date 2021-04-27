using Firebase.Auth;
using Google;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie.Services
{
    

    public class GoogleManager : MonoBehaviour
    {
        public static GoogleManager Instance { get; private set; }

        [SerializeField]
        string webClientId = "";

        GoogleSignInConfiguration configuration;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                configuration = new GoogleSignInConfiguration
                {
                    WebClientId = webClientId,
                    RequestIdToken = true
                };
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

        }

        /// <summary>
        /// Log out.
        /// </summary>
        public void SignOut()
        {
            GoogleSignIn.DefaultInstance.SignOut();
        }



        public void SignIn(UnityAction<bool, bool, GoogleSignInUser> callback, bool silently)
        {
            Debug.Log("GoogleManager - trying to sign in silently...");

            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.WebClientId = webClientId;
            GoogleSignIn.Configuration.RequestIdToken = true;

            Debug.Log("GoogleManager - configuration set up");

            Task<GoogleSignInUser> task;
            if (silently)
                task = GoogleSignIn.DefaultInstance.SignInSilently();
            else
                task = GoogleSignIn.DefaultInstance.SignIn();

            task.ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.Log("GoogleSignIn canceled");

                    callback?.Invoke(false, silently, null);
                }
                else if (task.IsFaulted)
                {
                    Debug.Log("GoogleSignIn faulted");

                    //callback?.Invoke(false, null);
                    callback?.Invoke(false, silently, null);
                }
                else
                {
                    Debug.Log("GoogleSignIn Succeeded");

                    callback?.Invoke(true, silently, ((Task<GoogleSignInUser>)task).Result);

                }
            });
        }

        //public void SignIn(UnityAction<bool, GoogleSignInUser> callback)
        //{
        //    Debug.Log("GoogleManager - signing in...");


        //    GoogleSignIn.Configuration = configuration;
        //    GoogleSignIn.Configuration.WebClientId = webClientId;
        //    GoogleSignIn.Configuration.RequestIdToken = true;

        //    Debug.Log("GoogleManager - configuration set up");

        //    GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task => {
        //        if (task.IsCanceled)
        //        {
        //            Debug.Log("GoogleSignIn canceled");

        //            callback?.Invoke(false, null);
        //        }
        //        else if (task.IsFaulted)
        //        {
        //            Debug.Log("GoogleSignIn faulted");

        //            callback?.Invoke(false, null);
        //        }
        //        else
        //        {
        //            Debug.Log("GoogleSignIn Succeeded");

        //            callback?.Invoke(true, ((Task<GoogleSignInUser>)task).Result);

        //        }
        //    });
        //}
       
    }

}
