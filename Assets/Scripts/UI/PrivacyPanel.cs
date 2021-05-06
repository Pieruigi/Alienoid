using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class PrivacyPanel : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        [SerializeField]
        GameObject nextPanel;

        string key = "privacy";

        private void Awake()
        {

            
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!PlayerPrefs.HasKey(key))
            {
                // Key is stored the first time the user opens the app
                // Stop time
                //Time.timeScale = 0;

                nextPanel.SetActive(false);
            }
            else
            {
                panel.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Accept()
        {
            PlayerPrefs.SetString(key, "");
            //Time.timeScale = 1;
            panel.SetActive(false);
            nextPanel.SetActive(true);
        }
    }

}
