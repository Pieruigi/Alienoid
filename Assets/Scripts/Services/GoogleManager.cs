using Google;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie.Services
{
    

    public class GoogleManager : MonoBehaviour
    {
        public static GoogleManager Instance { get; private set; }

        [SerializeField]

        string webClientId = "";

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

        }

        /// <summary>
        /// Logs in with google.
        /// Callback params:
        ///     - bool: true if login succeeded, otherwise false
        /// </summary>
        /// <param name="callback"></param>
        public void SignIn(UnityAction<bool> callback)
        {
            GoogleSignInConfiguration configuration = new GoogleSignInConfiguration
            {
                WebClientId = webClientId,
                RequestIdToken = true
            };

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    
                }
                else if (task.IsFaulted)
                {
                    Debug.Log("Task faulted");
                    
                }
                else
                {
                    //Debug.Log("GoogleSignIn Success");

                    //Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);

                    //Debug.Log("GoogleSignIn Token:" + ((Task<GoogleSignInUser>)task).Result.IdToken);

                    //auth.SignInWithCredentialAsync(credential).ContinueWith(authTask => {
                    //    if (authTask.IsCanceled)
                    //    {
                    //        signInCompleted.SetCanceled();
                    //    }
                    //    else if (authTask.IsFaulted)
                    //    {
                    //        signInCompleted.SetException(authTask.Exception);
                    //    }
                    //    else
                    //    {
                    //        signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                    //    }
                    //});
                }
            });
        }
    }

}
