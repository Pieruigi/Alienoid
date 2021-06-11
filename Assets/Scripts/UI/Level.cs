using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Services;

namespace Zom.Pie.UI
{
    public class Level : MonoBehaviour
    {
        
        [SerializeField]
        GameObject padlock;

        [SerializeField]
        TMP_Text levelIdText;


        int levelId;
        public int LevelId
        {
            get { return levelId; }
        }
        int speed = 1;
        public int MaxBeatenSpeed
        {
            get { return speed; }
        }

        bool selected;

        float selectionScale = 1.16f;

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
           // LeaderboardManager.Instance.OnLeaderboardLoaded -= HandleOnLeaderboardLoaded;
        }

        public void Init(int levelId, int speed)
        {
            
            // Set the current level id
            this.levelId = levelId;

         
            // Deactivate padlock
            padlock.SetActive(false);

            bool unlocked = false;
            if (speed < GameProgressManager.Instance.Speed || levelId <= GameProgressManager.Instance.LevelId)
                unlocked = true;

            if (unlocked)
            {
                GetComponent<Image>().color = Color.white;
                levelIdText.text = levelId.ToString();
            }
            else
            {
                // Show padlock
                padlock.SetActive(true);
                levelIdText.text = "";

                // Change color alpha
                Image img = GetComponent<Image>();
                Color c = Color.white * 0.6f;
                c.a = 1;
                img.color = c;

            }

        }

        public void Select(bool value)
        {
            // Already in the given state
            if (selected == value)
                return;

            // Set flag
            selected = value;

            if (selected)
            {
                // Scale up
                transform.DOScale(selectionScale, 0.25f).SetEase(Ease.OutElastic);
            }
            else
            {
                // Scale down
                transform.DOScale(1, 0.25f).SetEase(Ease.OutElastic);
            }
        }


    

       
    }

}
