using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zom.Pie.Collections;
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
            GameManager.Instance.SetEscapeActive(false);
            LoadLeaderboardAsync().ConfigureAwait(false);
        }

        private void OnDisable()
        {
            GameManager.Instance.SetEscapeActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
            }
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

            // Activate loading panel
            LoadingPanel.Instance.Show(true);

            data = await LeaderboardManager.Instance.GetLeaderboardDataAsync();

            // Deactivate loading panel
            LoadingPanel.Instance.Show(false);

            // If player is not logged in he will be notified
            if (!AccountManager.Instance.Logged)
                MessageBox.Show(MessageBox.Type.Ok, TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 4));

            if (data!= null)
                UpdateUI();
            
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

                // Show remote players
                levelUI.SetOtherPlayers(level.Players);
            }
        }

        void Reset()
        {
            
        }

    }

}
