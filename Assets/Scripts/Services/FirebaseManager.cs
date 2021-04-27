using Firebase;
using Firebase.Auth;
using Google;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Zom.Pie.Services
{
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseManager Instance { get; private set; }

        public bool Initialized { get; private set; } = false;

        public string webClientId = "472540864345-mfb6oq77que0mp245r6g4nkqdv24f6aj.apps.googleusercontent.com";
        

        FirebaseApp app;
        private GoogleSignInConfiguration configuration;
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                Initialize();


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

        void Initialize()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    app = Firebase.FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    Initialized = true;
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });

        }

        public void LogInWithGoogle()
        {
            Debug.Log("FirebaseManager - logging with google...");
            // Try first to log in silently
            GoogleManager.Instance.SignIn(HandleOnGoogleSignIn, true);
        }

        void HandleOnGoogleSignIn(bool succeeded, bool silently, GoogleSignInUser googleSignInUser)
        {
            Debug.Log("FirebaseManager - Sign in result:" + succeeded);

            if (succeeded)
            {
                Debug.Log("GoogleSignIn Token:" + googleSignInUser.IdToken);
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(googleSignInUser.IdToken, null);
            }
            else
            {
                if (silently)
                {
                    // If silent log in fails, then try normal log in 
                    GoogleManager.Instance.SignIn(HandleOnGoogleSignIn, false);
                }
                else
                {
                    /// Send back error
                }
            }
        }

        //void HandleOnGoogleSignIn(bool succeeded, GoogleSignInUser googleSignInUser)
        //{
        //    Debug.Log("FirebaseManager - Sign in result:" + succeeded);

        //    if (succeeded)
        //    {
        //        Debug.Log("GoogleSignIn Token:" + googleSignInUser.IdToken);
        //        Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(googleSignInUser.IdToken, null);
        //    }
            
        //}
    }

}
