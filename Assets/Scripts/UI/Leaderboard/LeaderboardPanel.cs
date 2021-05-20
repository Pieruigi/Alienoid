using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;
using Zom.Pie.Services;
using static Zom.Pie.Services.LeaderboardData;

namespace Zom.Pie.UI
{
    public class LeaderboardPanel : MonoBehaviour
    {
        [SerializeField]
        Transform levelGroup;

        [SerializeField]
        Button nextPageButton;

        [SerializeField]
        Button prevPageButton;

        public static LeaderboardPanel Instance { get; private set; }

        int page = 0;

        List<LeaderboardLevel> levels;

        int levelsPerPage = 4;
        int numOfPages;
        LeaderboardData data;

        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                // Compute the number of pages
                numOfPages = GameManager.Instance.GetNumberOfLevels() / levelsPerPage;

                // Set buttons 
                nextPageButton.onClick.AddListener(NextPage);
                prevPageButton.onClick.AddListener(PrevPage);


                // Fill template list
                GameObject levelTemplate = levelGroup.GetChild(0).gameObject;

                for (int i = 0; i < levelsPerPage - 1; i++)
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
            if (GameManager.Instance)
            {
                GameManager.Instance.SetEscapeActive(false); 
                LoadLeaderboardAsync().ConfigureAwait(false);
            }
            
        }

        private void OnDisable()
        {
            if(GameManager.Instance)
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


        void NextPage()
        {
            if (page == numOfPages - 1)
                page = 0;
            else
                page++;

            UpdateUI();
        }

        void PrevPage()
        {
            if (page == 0)
                page = numOfPages - 1;
            else
                page--;

            UpdateUI();
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
            foreach (LevelData level in data.Levels)
                Debug.Log("LevelId:" + level.LevelId);

            SetLevelsLabels();

            // Loop through each level
            int offset = page * levelsPerPage;
            
            for (int i = 0; i < levelsPerPage; i++)
            {
              
                // Get the ui level template
                LeaderboardLevel levelUI = levelGroup.GetChild(i).GetComponent<LeaderboardLevel>();
                levelUI.Clear();
                // Level 
                
                LevelData level = new List<LevelData>(data.Levels).Find(l => l.LevelId == (i+1+offset));
             
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
