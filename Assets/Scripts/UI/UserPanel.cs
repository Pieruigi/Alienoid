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

        private void Awake()
        {
            // Hide user name
            userName.gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            AccountManager.Instance.OnLogin += HandleOnLogin;

            if (AccountManager.Instance.Logged)
            {
                // Hide login button
                loginButton.gameObject.SetActive(false);

                // Show player name
                userName.gameObject.SetActive(true);
                userName.text = AccountManager.Instance.GetDisplayName();

            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnLogin(bool succeeded, bool isSilent)
        {
            Debug.Log("UserPanel.HandleOnLogin - succeeded:" + succeeded);

            if (succeeded)
            {
                // Hide button
                loginButton.gameObject.SetActive(false);

                // Show player name
                userName.gameObject.SetActive(true);
                userName.text = AccountManager.Instance.GetDisplayName();
            }
        }

        
    }

}
