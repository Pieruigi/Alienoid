using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;
using Zom.Pie.Services;
using static Zom.Pie.Services.LeaderboardData;

namespace Zom.Pie.UI
{
    public class LeaderboardPanel : MonoBehaviour
    {
        

        public static LeaderboardPanel Instance { get; private set; }

        [SerializeField]
        GameObject panel;

        [SerializeField]
        TMP_Text localText;

        [SerializeField]
        Transform content;

        GameObject playerTemplate;

        float defaultPositionX;
        bool open = false;
        
        bool busy = false;

        string topPlayerTxt = "You are a top player";
        string notTopPlayerTxt = "You are not a top player";

        bool leaderboardLoaded = false;
        LeaderboardData leaderboardData;
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                defaultPositionX = panel.transform.localPosition.x;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get the player template
            playerTemplate = content.GetChild(0).gameObject;
            // Move out and deactivate
            playerTemplate.transform.parent = panel.transform;
            playerTemplate.SetActive(false);
        }

        private void Update()
        {
            if (leaderboardLoaded)
            {
                leaderboardLoaded = false;
                PlayerData local = new List<PlayerData>(leaderboardData.Players).Find(p => p.UserId == AccountManager.Instance.GetUserId());
                Debug.Log("Local found:" + local);
                if (local != null)
                {
                    // Top player
                    localText.text = topPlayerTxt;
                }
                else
                {
                    localText.text = notTopPlayerTxt;
                }

                // Leaderboard
                int i = 1;
                foreach(PlayerData player in leaderboardData.Players)
                {
                    GameObject pObj = GameObject.Instantiate(playerTemplate, content, true);
                    pObj.GetComponent<LeaderboardRemotePlayer>().Init(player.UserId, 0, i, player.DisplayName, player.AvatarUrl);
                    pObj.SetActive(true);
                    i++;
                }
            }
        }

        public bool IsOpen()
        {
            return open;
        }

        public void Open()
        {
            if (busy)
                return;
            open = true;
            busy = true;
            panel.transform.DOMoveX(0, 0.5f).OnComplete(()=> { busy = false; });

            LoadLeaderboard();
        }

        public void Close()
        {
            
            if (busy)
                return;
            busy = true;
            open = false;
            panel.transform.DOLocalMoveX(defaultPositionX, 0.5f).OnComplete(()=> { busy = false; ClearAll(); });
        }


        void LoadLeaderboard()
        {
            LeaderboardManager.Instance.GetLeaderboardDataAsync().ContinueWith(task =>
            {
                if(task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log("LoadLeaderboard failed or canceled");
                    return;
                }
                else
                {
                    Debug.Log("LoadLeaderboard succeeded");
                    leaderboardData = task.Result;
                    leaderboardLoaded = true;
                }
            });
        }

        void ClearAll()
        {
            int count = content.childCount;
            for (int i = 0; i < count; i++)
                DestroyImmediate(content.GetChild(0).gameObject);
        }
    }

}
