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
  
        private UnityAction<bool> LoginCallback;
        Firebase.Auth.FirebaseAuth auth;

        public Firebase.Auth.FirebaseUser User
        {
            get { return auth != null ? auth.CurrentUser : null; }
            //get { if(Firebase.Auth == )}
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

        public Firebase.Auth.FirebaseUser GetCurrentUser()
        {
            if (Firebase.Auth.FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance) == null)
                return null;

            return Firebase.Auth.FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance).CurrentUser;
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


       

       
      
    }

}
