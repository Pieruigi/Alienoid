#define NOT_MANDATORY
#if MANDATORY
using Firebase.Auth;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie.Services
{
    public class AccountManager : MonoBehaviour
    {
        public static readonly string PlayerPrefsLoggedKey = "Logged";

        public UnityAction OnLoggedIn;
        public UnityAction OnLogInFailed;
        public UnityAction OnLoggedOut;

        public static AccountManager Instance { get; private set; }

        public bool Logged { get; private set; }

        bool logging = false;

        bool loginOnStart = false;

        bool loginEnabled = false;
     
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                // Get the player pref log flag
                if (PlayerPrefs.HasKey(PlayerPrefsLoggedKey))
                    loginOnStart = true;

#if UNITY_ANDROID
                // Init play games 
                PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                     .RequestServerAuthCode(false /* Don't force refresh */)
                     .Build();

                PlayGamesPlatform.InitializeInstance(config);
                PlayGamesPlatform.Activate();
#endif

#if UNITY_IOS

#endif


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
            if (!FirebaseManager.Instance.Initialized)
                return;

            if(!Logged && !logging)
            {
                // Try to log in
                LogIn();
            }

            //if(loginOnStart)
            //{
            //    loginOnStart = false;
            //    LogIn();
            //}
        }

        /// <summary>
        /// Log the player in using firebase with google or apple depending on the
        /// running platform.
        /// </summary>
        /// <param name="silently"></param>
        public void LogIn()
        {

            if (Logged)
            {
                Debug.LogWarning("Already logged in.");
                return;
            }

            if (logging)
            {
                Debug.LogWarning("Already logging in....");
                return;
            }

            if (!FirebaseManager.Instance.Initialized)
            {
                Debug.LogWarning("Firebase not initialized yet... please wait a while.");
                return;
            }

            logging = true;

#if UNITY_ANDROID
            LoginWithPlayGames();
#endif
#if UNITY_IOS
            LogInWithApple();
#endif

        }

        public void LogOut()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsLoggedKey);
            PlayerPrefs.Save();
            logging = false;
            Logged = false;
            
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SignOut();

#if UNITY_ANDROID
            PlayGamesPlatform.Instance.SignOut();

#endif
            OnLoggedOut?.Invoke();
        }

        /// <summary>
        /// Returns the player nickname
        /// </summary>
        /// <returns></returns>
        public string GetDisplayName()
        {
            return FirebaseManager.Instance.GetCurrentUser().DisplayName;
        }

        public string GetUserId()
        {
            return FirebaseManager.Instance.GetCurrentUser().UserId;
        }

        public string GetAvatarUrl()
        {
            Debug.LogError("PhotoUrl:" + FirebaseManager.Instance.GetCurrentUser().PhotoUrl);
            return FirebaseManager.Instance.GetCurrentUser().PhotoUrl.ToString();
        }

#region private

#if UNITY_ANDROID
        void LoginWithPlayGames()
        {
            
            Social.localUser.Authenticate((bool success) =>
            {
                Debug.Log("success:" + success);

                if (success)
                {
                    FirebaseSignInWithPlayGames();
                }
                else
                {
                    logging = false;
                    Logged = false;

                    OnLogInFailed?.Invoke();
                }

            });

           
        }

        void FirebaseSignInWithPlayGames()
        {
            // Get credentials from play games
            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

            // Get default instance
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

            // Create firebase credential from play games web server auth code
            Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);

            // Sign in
            auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    logging = false;
                    Logged = false;
                    OnLogInFailed?.Invoke();
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    logging = false;
                    Logged = false;
                    OnLogInFailed?.Invoke();
                    return;
                }

                logging = false;
                Logged = true;
                PlayerPrefs.SetInt(PlayerPrefsLoggedKey, 0);
                PlayerPrefs.Save();

                // Save data on firestore
                string userId = FirebaseManager.Instance.GetCurrentUser().UserId;
                string displayName = FirebaseManager.Instance.GetCurrentUser().DisplayName;
                string avatarUrl = FirebaseManager.Instance.GetCurrentUser().PhotoUrl.ToString();
                FirebaseManager.Instance.SaveUserDetail(userId, displayName, avatarUrl).ConfigureAwait(false);

                OnLoggedIn?.Invoke();

                Debug.Log("FirebaseUser.DisplayName:" + FirebaseManager.Instance.GetCurrentUser().DisplayName);
            });
        }

#endif

#endregion
    }

}

#endif

#if NOT_MANDATORY
using Firebase.Auth;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie.Services
{
    
    public class AccountManager : MonoBehaviour
    {
        public enum Provider { Native, Facebook }

        public static readonly string PlayerPrefsProviderKey = "Provider";

        public UnityAction OnLoggedIn;
        public UnityAction OnLogInFailed;
        public UnityAction OnLoggedOut;

        public static AccountManager Instance { get; private set; }

        public bool Logged { get; private set; }

        bool logging = false;

        bool loginOnStart = false;
#if UNITY_EDITOR
        string fakeUserId = "fake_user_id";
#endif

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                // Get the player pref log flag
                if (PlayerPrefs.HasKey(PlayerPrefsProviderKey))
                    loginOnStart = true;

#if UNITY_ANDROID
                // Init play games 
                PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                     .RequestServerAuthCode(false /* Don't force refresh */)
                     .Build();

                PlayGamesPlatform.InitializeInstance(config);
                PlayGamesPlatform.Activate();
#endif

#if UNITY_IOS

#endif


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
            if (!FirebaseManager.Instance.Initialized)
                return;

            

            if(loginOnStart)
            {
                loginOnStart = false;
                int provider = PlayerPrefs.GetInt(PlayerPrefsProviderKey);
                LogIn(provider);
            }
        }

        /// <summary>
        /// Log the player in using firebase with google or apple depending on the
        /// running platform.
        /// </summary>
        /// <param name="silently"></param>
        public void LogIn(int provider)
        {

            if (Logged)
            {
                Debug.LogWarning("Already logged in.");
                return;
            }

            if (logging)
            {
                Debug.LogWarning("Already logging in....");
                return;
            }

            if (!FirebaseManager.Instance.Initialized)
            {
                Debug.LogWarning("Firebase not initialized yet... please wait a while.");
                return;
            }

            logging = true;


            switch (provider)
            {
                case (int)Provider.Native:
#if UNITY_ANDROID
                    LoginWithPlayGames();
#endif
#if UNITY_IOS
                    LogInWithApple();
#endif
                    break;
                default:
                    Debug.LogWarning("No provider selected");
                    logging = false;
                    break;
            }
                



        }

        public void LogOut()
        {
            // Get the provider
            int provider = PlayerPrefs.GetInt(PlayerPrefsProviderKey);

            PlayerPrefs.DeleteKey(PlayerPrefsProviderKey);
            PlayerPrefs.Save();

            logging = false;
            Logged = false;
            
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SignOut();

            switch (provider)
            {
                case (int)Provider.Native:
#if UNITY_ANDROID
                    PlayGamesPlatform.Instance.SignOut();
#endif
#if UNITY_IOS
                    Apple.Logout();
#endif
                    break;
            }

            OnLoggedOut?.Invoke();
        }

        /// <summary>
        /// Returns the player nickname
        /// </summary>
        /// <returns></returns>
        public string GetDisplayName()
        {
#if UNITY_EDITOR
            return "FakeUserName";
#else
            return FirebaseManager.Instance.GetCurrentUser().DisplayName;
#endif
        }

        public string GetUserId()
        {
#if UNITY_EDITOR
            return fakeUserId;
#else
            return FirebaseManager.Instance.GetCurrentUser().UserId;
#endif
        }

        public string GetAvatarUrl()
        {
#if UNITY_EDITOR
            return null;
#else
            Debug.LogError("PhotoUrl:" + FirebaseManager.Instance.GetCurrentUser().PhotoUrl);
            return FirebaseManager.Instance.GetCurrentUser().PhotoUrl.ToString();
#endif
        }

#region private

#if UNITY_ANDROID
        void LoginWithPlayGames()
        {
#if UNITY_EDITOR
            logging = false;
            Logged = true;
            OnLoggedIn?.Invoke();
#else

            Social.localUser.Authenticate((bool success) =>
            {
                Debug.Log("success:" + success);

                if (success)
                {
                    FirebaseSignInWithPlayGames();
                }
                else
                {
                    logging = false;
                    Logged = false;

                    OnLogInFailed?.Invoke();
                }

            });
#endif

        }

        void FirebaseSignInWithPlayGames()
        {
            // Get credentials from play games
            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

            // Get default instance
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

            // Create firebase credential from play games web server auth code
            Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);

            // Sign in
            auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    logging = false;
                    Logged = false;
                    OnLogInFailed?.Invoke();
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    logging = false;
                    Logged = false;
                    OnLogInFailed?.Invoke();
                    return;
                }

                logging = false;
                Logged = true;
                PlayerPrefs.SetInt(PlayerPrefsProviderKey, (int)Provider.Native);
                PlayerPrefs.Save();

                // Save data on firestore
                string userId = FirebaseManager.Instance.GetCurrentUser().UserId;
                string displayName = FirebaseManager.Instance.GetCurrentUser().DisplayName;
                string avatarUrl = FirebaseManager.Instance.GetCurrentUser().PhotoUrl.ToString();
                FirebaseManager.Instance.SaveUserDetail(userId, displayName, avatarUrl).ConfigureAwait(false);

                OnLoggedIn?.Invoke();

                Debug.Log("FirebaseUser.DisplayName:" + FirebaseManager.Instance.GetCurrentUser().DisplayName);
            });
        }

#endif

#endregion
        }

}
#endif