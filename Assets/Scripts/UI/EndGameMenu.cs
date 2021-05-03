using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        private void Awake()
        {
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

        

        public void Close()
        {
            panel.SetActive(false);
            GameManager.Instance.Pause(false);
        }

        IEnumerator Open()
        {
            yield return new WaitForSeconds(4f * Time.timeScale);

            GameManager.Instance.Pause(true);
            panel.SetActive(true);

            // Save score if needed
            LeaderboardManager.Instance.SaveLocalPlayerScoreByLevel(GameManager.Instance.GetCurrentLevelId(), LevelManager.Instance.TimeScore);
        }

        void HandleOnLevelBeaten()
        {
            // Reset all
            ResetButtons();

            // Activate the continue button
            nextButton.gameObject.SetActive(true);

            StartCoroutine(Open());
        }

        void HandleOnPlayerDead()
        {
            // Reset all
            ResetButtons();

            // Activate the continue button
            restartButton.gameObject.SetActive(true);

            StartCoroutine(Open());
        }

        void ResetButtons()
        {
            nextButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(false);
        }
    }

}
