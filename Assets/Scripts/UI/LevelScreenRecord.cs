using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zom.Pie.Collections;
using Zom.Pie.Services;

namespace Zom.Pie.UI
{
    public class LevelScreenRecord : MonoBehaviour
    {
        enum RecordType { World, Local }

        [SerializeField]
        RecordType recordType;

        [SerializeField]
        TMP_Text text;

        float record;
        int selectedLevel;

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            LevelMenu.Instance.OnLevelSelected += HandleOnLevelSelected;
        }

        // Update is called once per frame
        void Update()
        {
            if(selectedLevel > 0)
            {
                // Wait until score data is loaded
                if (LevelMenu.Instance.ScoreData == null)
                    return;

                // Check for record
                float tmp = 0;
                if(recordType == RecordType.World)
                {
                    tmp = LevelMenu.Instance.ScoreData.GetAllTimeRecord(selectedLevel);
                }
                else
                {
                    tmp = LevelMenu.Instance.ScoreData.GetPlayerScore(selectedLevel);
                }

                if(tmp != record)
                {
                    record = tmp;

                    // Update
                    if (record == 0)
                    {
                        text.text = "--:--.--";
                    }
                    else
                    {
                        text.text = GeneralUtility.FormatTime(record);
                    }

                    
                    
                }
            }
        }

        void HandleOnLevelSelected(int levelId)
        {
            selectedLevel = levelId;
        }
    }

}
