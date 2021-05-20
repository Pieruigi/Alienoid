using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Services;

namespace Zom.Pie.UI
{
    public class UserPanel : MonoBehaviour
    {
        [SerializeField]
        Button loginButton;

        [SerializeField]
        TMP_Text userName;

        [SerializeField]
        Image avatar;

        [SerializeField]
        Button logOutButton;

        [SerializeField]
        GameObject leaderboardPanel;

        Sprite dummySprite;

        private void Awake()
        {
            ResetLogin();

            dummySprite = avatar.sprite;
        }

        // Start is called before the first frame update
        void Start()
        {
            AccountManager.Instance.OnLoggedIn += HandleOnLoggedIn;
            AccountManager.Instance.OnLogInFailed += HandleOnLogInFailed;
            AccountManager.Instance.OnLoggedOut += HandleOnLoggedOut;

            if (AccountManager.Instance.Logged)
            {
                InitLogin();
            }
        }


        private void OnDestroy()
        {
            AccountManager.Instance.OnLoggedIn -= HandleOnLoggedIn;
            AccountManager.Instance.OnLogInFailed -= HandleOnLogInFailed;
            AccountManager.Instance.OnLoggedOut -= HandleOnLoggedOut;
        }


        // Update is called once per frame
        void Update()
        {

        }

        public void OpenLeaderboard()
        {
            if (AccountManager.Instance.Logged)
            {
                leaderboardPanel.SetActive(true);
            }
            else
            {
                GameObject.FindObjectOfType<LogInPanel>().Show();
            }
        }

        public void LogOut()
        {
            AccountManager.Instance.LogOut();
        }

        void HandleOnLoggedIn()
        {
            Debug.Log("UserPanel.HandleOnLoggedIn");

            InitLogin();


        }

        void HandleOnLogInFailed()
        {
            Debug.Log("UserPanel.HandleOnLogInFailed");

            ResetLogin();
        }

        void HandleOnLoggedOut()
        {
            Debug.Log("UserPanel.HandleOnLoggedOut");
            ResetLogin();
        }

        private void ResetLogin()
        {
            avatar.gameObject.SetActive(false);
            userName.gameObject.SetActive(false);
            loginButton.gameObject.SetActive(true);
            logOutButton.gameObject.SetActive(false);
        }

        void InitLogin()
        {
            // Hide button login
            loginButton.gameObject.SetActive(false);

            // Show log out
            logOutButton.gameObject.SetActive(true);

            // Show player name
            userName.gameObject.SetActive(true);
            userName.text = AccountManager.Instance.GetDisplayName();

            // Download avatar
            avatar.gameObject.SetActive(true);
            Debug.Log("AvatarUrl:" + AccountManager.Instance.GetAvatarUrl());
            if (!string.IsNullOrEmpty(AccountManager.Instance.GetAvatarUrl()))
                StartCoroutine(GeneralUtility.GetTextureFromUrlAsync(AccountManager.Instance.GetAvatarUrl(), ShowAvatar));
            else
                avatar.sprite = dummySprite;
        }

        void ShowAvatar(bool succeeded, Texture texture)
        {
            if (succeeded)
            {
                avatar.sprite = Sprite.Create((Texture2D)texture, dummySprite.rect, Vector2.zero);
            }
            else
            {
                avatar.sprite = dummySprite;
            }
        }
        
    }

}
