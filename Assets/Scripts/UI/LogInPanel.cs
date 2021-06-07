using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Services;

namespace Zom.Pie.UI
{
    public class LogInPanel : MonoBehaviour
    {
        
        [SerializeField]
        GameObject panel;

        static bool showed = false;

        private void Awake()
        {
            if (!showed)
                showed = true;
            else
                gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            
            
        }

        private void OnEnable()
        {
            if (PlayerPrefs.HasKey(AccountManager.PlayerPrefsProviderKey))
                panel.SetActive(false);
            
        }

        public void Show()
        {
            if (panel.activeSelf)
                return;

            panel.SetActive(true);
        }

        public void LogInNative()
        {
            AccountManager.Instance.LogIn((int)AccountManager.Provider.Native);
        }

        public void LogInWithFacebook()
        {
            AccountManager.Instance.LogIn((int)AccountManager.Provider.Facebook);
        }

        public void LogOut()
        {
            AccountManager.Instance.LogOut();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
