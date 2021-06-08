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
        GameObject star;

        [SerializeField]
        TMP_Text position;

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

            // Hide position label
            position.gameObject.SetActive(false);
            
            // Deactivate the star
            HighlightStar(false);
            star.SetActive(false);

            // Deactivate padlock
            padlock.SetActive(false);

            bool unlocked = false;
            if (speed < GameProgressManager.Instance.Speed || levelId <= GameProgressManager.Instance.LevelId)
                unlocked = true;

            if (unlocked)
            {
                GetComponent<Image>().color = Color.white;

                //if (GameProgressManager.Instance.LevelHasBeenBeaten(levelId, speed))
                //{
                //    // Show star 
                //    star.SetActive(true);
                //}
               
            }
            else
            {
                // Show padlock
                padlock.SetActive(true);

                // Change color alpha
                Image img = GetComponent<Image>();
                Color c = Color.white * 0.6f;
                c.a = 1;
                img.color = c;

            }

            // Load leaderboard
            //LeaderboardManager.Instance.LoadLeaderboard(levelId);
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

        public void SetDataScore(float playerPosition)
        {

            if (!GameProgressManager.Instance.LevelIsUnlocked(levelId, speed))
                return;

            Debug.LogFormat("LevelUnlocked:({0},{1})", levelId, speed);

            if (!GameProgressManager.Instance.LevelHasBeenBeaten(levelId, speed))
                return;

            Debug.LogFormat("HandleOnLeaderboardLoaded({0})", levelId);


            // Check if the player is ranked
            //if (LeaderboardManager.Instance.IsLocalPlayerInRankingByLevel(levelId))
            if (playerPosition > 0)
            {
                //int localPosition = LeaderboardManager.Instance.GetLocalPlayerPositionByLevel(levelId);
                Debug.LogFormat("LocalPosition: {0}", playerPosition);
                // Check the player position for the given level ranking
                if (playerPosition > Constants.TopPlayers)
                {
                    Debug.Log("Out of the top");
                    // You are not in the top players ranking
                    // Activate star but not highlited
                    // We set some default value in case for some reason you are not 
                    // able to retrieve online leadearboard
                    star.SetActive(true);
                    HighlightStar(false);

                    // Hide position
                    position.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("In the top");

                    // You are top
                    if (playerPosition > 3)
                    {
                        // Highlight star
                        star.SetActive(true);
                        HighlightStar(true);
                        // Hide position
                        position.gameObject.SetActive(false);
                    }
                    else
                    {
                        // Hide star
                        star.SetActive(false);
                        // Set position
                        position.gameObject.SetActive(true);
                        position.text = playerPosition.ToString();
                        Debug.LogFormat("You are in {0} position", playerPosition);
                    }
                }
            }
        }

        void HighlightStar(bool value)
        {
            Color c = Color.white;
            
            if (!value)
            {
                // Set gray color
                c *= 0.4f;
                c.a = 1;
                
            }
            
            star.GetComponent<Image>().color = c;
        }

        void UpdateSpeedPanel()
        {

        }

       
    }

}
