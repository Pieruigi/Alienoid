using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;
using Zom.Pie.Services;

namespace Zom.Pie.UI
{
    public class EndGameMenu : MonoBehaviour
    {

        [SerializeField]
        GameObject panel;

        [SerializeField]
        Button nextButton;

        [SerializeField]
        Button restartButton;

        bool levelBeaten = false;

        private void Awake()
        {
            if (AdsManager.Instance)
            {
                AdsManager.Instance.OnInterstitialClosed += FinalizeLevel;
                AdsManager.Instance.OnInterstitialFailed += FinalizeLevel;
            }
            
            Close();
        }

        // Start is called before the first frame update
        void Start()
        {
            LevelManager.Instance.OnLevelBeaten += HandleOnLevelBeaten;
            PlayerManager.Instance.OnDead += HandleOnPlayerDead;
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        private void OnDestroy()
        {
            AdsManager.Instance.OnInterstitialClosed -= FinalizeLevel;
            AdsManager.Instance.OnInterstitialFailed -= FinalizeLevel;
        }


        public void Close()
        {
            panel.SetActive(false);
            GameManager.Instance.Pause(false);
        }

        IEnumerator Open()
        {
            yield return new WaitForSeconds(4f * Time.timeScale);

            // Pause app and show this panel
            GameManager.Instance.Pause(true);

            if (PurchaseManager.Instance.IsPremiumVersion())// No ads
            {
                // We skip ads because player owns premium version
                FinalizeLevel();
            }
            else// Show ads
            {
#if UNITY_EDITOR
                FinalizeLevel();
#else
                // We show the interstitial interstitial
                AdsManager.Instance.ShowInterstitial();
#endif

            }

        }

        void HandleOnLevelBeaten()
        {
            // Reset all
            ResetButtons();

            // Activate the continue button
            nextButton.gameObject.SetActive(true);

            levelBeaten = true;

            StartCoroutine(Open());
        }

        void HandleOnPlayerDead()
        {
            // Reset all
            ResetButtons();

            // Activate the continue button
            restartButton.gameObject.SetActive(true);

            levelBeaten = false;

            StartCoroutine(Open());
        }

        void ResetButtons()
        {
            nextButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(false);
        }

        void FinalizeLevel()
        {
            panel.SetActive(true);

            if (levelBeaten)
            {
                if (AccountManager.Instance.Logged)
                {
                    // Save score if needed
                    LeaderboardManager.Instance.SaveLocalPlayerScoreByLevel(GameManager.Instance.GetCurrentLevelId(), LevelManager.Instance.TimeScore).ConfigureAwait(false);
                }
                else
                {
                    // Not logged in
                    MessageBox.Show(MessageBox.Type.Ok, TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 4));
                }
            }

            
        }


    }

}
