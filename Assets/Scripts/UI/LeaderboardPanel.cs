using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Services;
using static Zom.Pie.Services.LeaderboardData;

namespace Zom.Pie.UI
{
    public class LeaderboardPanel : MonoBehaviour
    {
        [SerializeField]
        Transform levelGroup;

        public static LeaderboardPanel Instance { get; private set; }

        int page = 0;

        List<LeaderboardLevel> levels;

        int levelsPerPage = 4;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                // Fill template list
                GameObject levelTemplate = levelGroup.GetChild(0).gameObject;

                for(int i=0; i<levelsPerPage-1; i++)
                {
                    // Create new template
                    GameObject.Instantiate(levelTemplate, levelGroup, true);
                }

                // Get levels templates
                levels = new List<LeaderboardLevel>(levelGroup.GetComponentsInChildren<LeaderboardLevel>());

                gameObject.SetActive(false);
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

        private void OnEnable()
        {
            
            // Reset 
            page = 0;
            SetLevelsLabels();

            Debug.Log("Leaderboard panel enabled");
            LeaderboardManager.Instance.GetLeaderboardDataAsync().ContinueWith(task =>
            {
                if(task.IsFaulted || task.IsCanceled)
                {
                    // Do something here
                    // Hide loading panel
                }
                else
                {
                    // Hide loading panel

                    Debug.Log("Task returned ok");
                    // Get data
                    LeaderboardData data = task.Result;

                    Debug.Log("data is null:" + (data == null));

                    // Loop through each level
                    for(int i=0; i<data.Levels.Count; i++)
                    {
                        // Level 
                        LevelData level = data.Levels[i];

                        Debug.Log("LevelId:" + (i + 1).ToString());
                        Debug.Log("LocalScore:" + level.LocalScore);
                        Debug.Log("Players.count:" + level.Players.Count);

                        // Players

                    }
                }
            });
        }

        private void OnDisable()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetLevelsLabels()
        {
            int offset = page * levelsPerPage;
            for (int i = 0; i < levels.Count; i++)
            {
                // Set label
                levels[i].SetLevelLabel((i + 1) + offset);
            }
        }

        void HandleOnLeaderboardLoaded(LeaderboardData leaderboard)
        {

        }
    }

}
