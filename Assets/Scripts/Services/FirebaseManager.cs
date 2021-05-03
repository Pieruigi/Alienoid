using Firebase;
using Firebase.Auth;
using Google;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie.Services
{
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseManager Instance { get; private set; }

        public bool Initialized { get; private set; } = false;

        FirebaseApp app;
        private GoogleSignInConfiguration configuration;

        private UnityAction<bool> LoginCallback;
        Firebase.Auth.FirebaseAuth auth;

        public Firebase.Auth.FirebaseUser User
        {
            get { return auth != null ? auth.CurrentUser : null; }
        }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                Initialize();

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


        public void LogInWithGoogle(bool silently, UnityAction<bool> callback)
        {
            // Save the callback 
            LoginCallback = callback;
            Debug.Log("FirebaseManager - logging with google - silently:" + silently);
            // Try first to log in silently
            GoogleManager.Instance.SignIn(HandleOnGoogleSignIn, silently);
        }

        void HandleOnGoogleSignIn(bool succeeded, bool silently, GoogleSignInUser googleSignInUser)
        {
            Debug.Log("FirebaseManager - Sign in result:" + succeeded);

            if (succeeded)
            {
                Debug.Log("GoogleSignIn Token:" + googleSignInUser.IdToken);
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(googleSignInUser.IdToken, null);
                Debug.Log("GetCredential() done");
                // Sign in with credential
                
                Debug.Log("Firebase.Auth created; trying to sign in with credential");
                auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithCredentialAsync was canceled.");
                        LoginCallback?.Invoke(false);
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                        LoginCallback?.Invoke(false);
                        return;
                    }

                    Debug.Log("Firebase - Login with google credential succeeded");

                    // Create user
                    Firebase.Auth.FirebaseUser user = auth.CurrentUser;
                    if (user != null)
                    {
                        Debug.LogFormat("User signed in successfully: {0} ({1})",
                        user.DisplayName, user.UserId);

                        //string name = user.DisplayName;
                        //string email = user.Email;
                        //System.Uri photo_url = user.PhotoUrl;
                        //// The user's Id, unique to the Firebase project.
                        //// Do NOT use this value to authenticate with your backend server, if you
                        //// have one; use User.TokenAsync() instead.
                        //string uid = user.UserId;
                    }

                    LoginCallback?.Invoke(true);
                });

            }
            else
            {
  
                LoginCallback?.Invoke(false);
            }
        }

      
    }

}
