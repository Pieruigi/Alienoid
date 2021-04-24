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
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

            Debug.Log("GoogleSignInConfiguration");
            configuration = new GoogleSignInConfiguration
            {
                WebClientId = webClientId,
                RequestIdToken = true
            };
            Debug.Log("GoogleSignInConfiguration completed");

            Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();
            Debug.Log("Sign in task");
            TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
            Debug.Log("Sign in competed task");
            signIn.ContinueWith(task => {
                if (task.IsCanceled)
                {
                    signInCompleted.SetCanceled();
                }
                else if (task.IsFaulted)
                {
                    Debug.Log("Task faulted");
                    signInCompleted.SetException(task.Exception);
                }
                else
                {
                    Debug.Log("GoogleSignIn Success");

                    Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);

                    Debug.Log("GoogleSignIn Token:" + ((Task<GoogleSignInUser>)task).Result.IdToken);

                    auth.SignInWithCredentialAsync(credential).ContinueWith(authTask => {
                        if (authTask.IsCanceled)
                        {
                            signInCompleted.SetCanceled();
                        }
                        else if (authTask.IsFaulted)
                        {
                            signInCompleted.SetException(authTask.Exception);
                        }
                        else
                        {
                            signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                        }
                    });
                }
            });
        }
    }

}
