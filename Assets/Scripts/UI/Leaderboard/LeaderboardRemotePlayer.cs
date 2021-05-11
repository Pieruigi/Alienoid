using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;
using Zom.Pie.Services;

namespace Zom.Pie.UI
{
    public class LeaderboardRemotePlayer : MonoBehaviour
    {
        [SerializeField]
        Image avatar;

        [SerializeField]
        TMP_Text playerName;

        [SerializeField]
        TMP_Text playerScore;

        [SerializeField]
        TMP_Text playerPosition;

        [SerializeField]
        List<Color> colors;

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

        public void Init(string userId, float score, int position)
        {
            // Set the score
            playerScore.text = GeneralUtility.FormatTime(score);

            // Set position
            playerPosition.text = position.ToString();

            // Set name 
            playerName.text = userId;

            Image image = GetComponent<Image>();
            if(position < 4)
            {
                image.color = colors[position - 1];
            }
            else
            {
                image.color = colors[3];
            }
        }


        
    }

}
