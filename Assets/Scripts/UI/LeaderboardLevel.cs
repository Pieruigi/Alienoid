using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class LeaderboardLevel : MonoBehaviour
    {
        [SerializeField]
        TMP_Text levelLabel;

        string levelStringFormat = "Level {0}";

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetLevelLabel(int levelId)
        {
            levelLabel.text = string.Format(levelStringFormat, levelId);
        }
    }

}
