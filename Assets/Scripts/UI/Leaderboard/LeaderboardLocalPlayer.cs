using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;
using Zom.Pie.Services;

namespace Zom.Pie.UI
{
    public class LeaderboardLocalPlayer : MonoBehaviour
    {
        [SerializeField]
        Image avatar;

        [SerializeField]
        TMP_Text playerName;

        [SerializeField]
        TMP_Text playerScore;

        [SerializeField]
        TMP_Text playerPosition;

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            // Set the name
            if (AccountManager.Instance.Logged)
            {
                playerName.text = AccountManager.Instance.GetDisplayName();
            }
            else
            {
#if UNITY_EDITOR
                playerName.text = "Pierpiero";
#else
                playerName.text = TextFactory.Instance.GetText(TextFactory.Type.UILabel, 21);
#endif
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetScore(float score)
        {
            // Set the score
            playerScore.text = GeneralUtility.FormatTime(score);
        }
    }

}
