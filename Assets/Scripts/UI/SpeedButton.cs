using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;

namespace Zom.Pie.UI
{
    public class SpeedButton : MonoBehaviour
    {
        [SerializeField]
        TMP_Text label;

        int[] labelIds = new int[] { 5, 6, 7 };

        int speed = 1;
        int speedCount = 3;

        int selectedLevelId = 0;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(HandleOnClick);
            GetComponentInParent<LevelMenu>().OnLevelSelected += HandleOnLevelSelected;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnClick()
        {
            speed++;
            if (speed > Constants.MaxLevelSpeed)
                speed = 1;

            // Get the max available speed for the current level
            int beatenSpeed = GameProgressManager.Instance.GetMaxBeatenSpeed(selectedLevelId);

            // We have beaten the level at all the speeds
            if(beatenSpeed == Constants.MaxLevelSpeed || speed <= beatenSpeed + 1)
            {
                // Set available 
                label.text = TextFactory.Instance.GetText(TextFactory.Type.UILabel, labelIds[speed - 1]);
                //label.color = Constants.EnabledColor;

               
            }
            else
            {
                // Set unavailable 
                label.text = TextFactory.Instance.GetText(TextFactory.Type.UILabel, labelIds[speed - 1]);
               // label.color = Constants.DisabledColor;
                
            }

            // Set speed
            GameManager.Instance.GameSpeed = speed;
        }

        void HandleOnLevelSelected(int levelId)
        {
            // Set field
            selectedLevelId = levelId;

            // Check for max available speed
            int beatenSpeed = GameProgressManager.Instance.GetMaxBeatenSpeed(levelId);

            if(beatenSpeed < Constants.MaxLevelSpeed)
            {
                // We set the next speed available
                speed = beatenSpeed + 1;
            }
            else
            {
                // No more speed available
                speed = beatenSpeed;
            }

            // Set text
            label.text = TextFactory.Instance.GetText(TextFactory.Type.UILabel, labelIds[speed - 1]);
           // label.color = Constants.EnabledColor;

            // Set speed
            GameManager.Instance.GameSpeed = speed;
        }
    }

}
