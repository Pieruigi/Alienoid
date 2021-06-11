using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class SpeedSelector : MonoBehaviour
    {
        [SerializeField]
        TMP_Text lable;

        [SerializeField]
        Button nextButton;

        [SerializeField]
        Button prevButton;

      
        LevelMenu levelMenu;

        // Start is called before the first frame update
        void Start()
        {
            // Get level menu
            levelMenu = GetComponentInParent<LevelMenu>();

            // Init ui
            int speed = GameManager.Instance.GameSpeed;
            lable.text = speed.ToString();
            if (speed == GameProgressManager.Instance.Speed)
                nextButton.interactable = false;
            if (speed == 1)
                prevButton.interactable = false;

            // Setting handles
            nextButton.onClick.AddListener(HandleOnSpeedUp);
            prevButton.onClick.AddListener(HandleOnSpeedDown);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnSpeedUp()
        {
            int speed = GameManager.Instance.GameSpeed;

            if (speed == GameProgressManager.Instance.Speed)
                return;

            // Update speed 
            speed++;
            lable.text = speed.ToString();

            // Check button
            if (speed == GameProgressManager.Instance.Speed)
                nextButton.interactable = false;
            prevButton.interactable = true;

            levelMenu.UpdateSpeed(speed);
        }

        void HandleOnSpeedDown()
        {
            int speed = GameManager.Instance.GameSpeed;

            if (speed == 1)
                return;

            // Update speed 
            speed--;
            lable.text = speed.ToString();

            // Check button
            if (speed == 1)
                prevButton.interactable = false;
            nextButton.interactable = true;

            levelMenu.UpdateSpeed(speed);
        }
    }

}
