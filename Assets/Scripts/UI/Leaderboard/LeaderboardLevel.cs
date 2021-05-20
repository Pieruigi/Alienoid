using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zom.Pie.Services;

namespace Zom.Pie.UI
{
    public class LeaderboardLevel : MonoBehaviour
    {
        [SerializeField]
        TMP_Text levelLabel;

        [SerializeField]
        LeaderboardLocalPlayer localPlayer;

        [SerializeField]
        Transform remotePlayerContainer;

        string levelStringFormat = "Level {0}";

        Transform remotePlayerTemplate;
        bool initialized = false;
        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            // Move the player template into a variable for further use
            Debug.Log("RemotePlayerContainer.Child:" + remotePlayerContainer.GetChild(0));
            remotePlayerTemplate = remotePlayerContainer.GetChild(0);
            remotePlayerTemplate.gameObject.SetActive(false);
            // Move the template out
            remotePlayerTemplate.parent = remotePlayerContainer.parent;

            initialized = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            if (!initialized)
                return;

            //Debug.Log("OnDisabled " + gameObject);
            // Remove all the remote player templates
            Clear();
        }

        public void Clear()
        {
            int count = remotePlayerContainer.childCount;
            for (int i = 0; i < count; i++)
            {
                DestroyImmediate(remotePlayerContainer.GetChild(0).gameObject);
            }
        }

        public void SetLevelLabel(int levelId)
        {
            levelLabel.text = string.Format(levelStringFormat, levelId);
        }

        public void SetLocalPlayerScore(float score)
        {
            localPlayer.SetScore(score);
        }

        public void SetOtherPlayers(IList<LeaderboardData.PlayerData> players)
        {
            // Init players
            for(int i=0; i<players.Count; i++)
            {
                // Create a new player ui
                Transform playerUI = GameObject.Instantiate(remotePlayerTemplate, remotePlayerContainer, true);
                playerUI.gameObject.SetActive(true);
                string userId = players[i].UserId;
                float score = players[i].Score;
                string displayName = players[i].DisplayName;
                string avatarUrl = players[i].AvatarUrl;
                playerUI.GetComponent<LeaderboardRemotePlayer>().Init(userId, score, i+1, displayName, avatarUrl);
                
            }
        }
    }

}
