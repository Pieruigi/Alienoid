using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        LeaderboardData data;

        
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
            LoadLeaderboardAsync().ConfigureAwait(false);
        }

        private async Task LoadLeaderboardAsync()
        {
            
            // Reset 
            page = 0;
            SetLevelsLabels();

            Debug.Log("Leaderboard panel enabled");

            //await LeaderboardManager.Instance.GetLeaderboardDataAsync().ContinueWith(task =>
            //{
            //    if (task.IsFaulted || task.IsCanceled)
            //    {

            //    }
            //    else
            //    {
            //        // Hide loading panel

            //        Debug.Log("Task returned ok");
            //        // Get data
            //        data = task.Result;

            //        Debug.Log("data is null:" + (data == null));

            //    }
            //});
            data = await LeaderboardManager.Instance.GetLeaderboardDataAsync();

            if (data!= null)
                UpdateUI();
            
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

        void UpdateUI()
        {
            Debug.Log("LevelGroup:" + levelGroup);

            // Loop through each level
            int offset = page * levelsPerPage;
            for (int i = 0; i < levelsPerPage; i++)
            {
              
                // Get the ui level template
                LeaderboardLevel levelUI = levelGroup.GetChild(i).GetComponent<LeaderboardLevel>();
                // Level 
                LevelData level = data.Levels[i+offset];
             
                Debug.Log("LevelId:" + (i + 1 + offset).ToString());
                Debug.Log("LocalScore:" + level.LocalScore);
                Debug.Log("Players.count:" + level.Players.Count);

                if (level.LocalScore > 0)
                {
                    // Set UI
                    levelUI.SetLocalPlayerScore(level.LocalScore);
                }
                // Players

            }
        }

        void HandleOnLeaderboardLoaded(LeaderboardData leaderboard)
        {

        }
    }

}
