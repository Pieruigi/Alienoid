using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zom.Pie.Services;

namespace Zom.Pie.UI
{
    public class LevelScore : MonoBehaviour
    {
        enum ScoreType { Local, World, Monthly }

        [SerializeField]
        ScoreType scoreType;


        [SerializeField]
        TMP_Text scoreText;

        string txt = "--:--.--";

        // Start is called before the first frame update
        void Start()
        {
            GetComponentInParent<LevelMenu>().OnLevelSelected += HandleOnLevelSelected;

            // Init
            HandleOnLevelSelected(GetComponentInParent<LevelMenu>().SelectedLevelId);
        }

        // Update is called once per frame
        void Update()
        {
            scoreText.text = txt;
        }

        void HandleOnLevelSelected(int levelId)
        {
            Debug.Log("OnLevelSelected:" + levelId);
            LeaderboardManager.Instance.GetLocalPlayerScoreByLevelAsync(levelId, Callback);
        }

        void Callback(bool found, float score)
        {
            Debug.Log("Score found:" + found);
            //scoreText.gameObject.SetActive(false);
            if(found)
            {
                txt = score.ToString();
                //scoreText.SetText(score.ToString());
            }
            else
            {
                txt = "--:--.--";
                //scoreText.SetText("--:--.--");
            }
            //scoreText.SetAllDirty();
            //scoreText.gameObject.SetActive(false);
            //scoreText.gameObject.SetActive(true);
        }
    }

}
