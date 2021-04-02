using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;

namespace Zom.Pie.UI
{
    /// <summary>
    /// The exit button leaves the current scene
    /// </summary>
    public class PlayButton : MonoBehaviour
    {
        [SerializeField]
        Color enabledColor;

        [SerializeField]
        Color disabledColor;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(HandleOnClick);
            
            
        }

        // Start is called before the first frame update
        void Start()
        {

            if (GameManager.Instance.IsInLevelMenu())
            {
                LevelMenu.Instance.OnGameSpeedSelected += HandleOnGameSpeedChanged;
                //GetComponentInParent<LevelMenu>().OnLevelSelected += HandleOnLevelSelected;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            //GameManager.Instance.OnGameSpeedChanged -= HandleOnGameSpeedChanged;
        }

        void HandleOnClick()
        {
            if (GameManager.Instance.IsInMainMenu())
            {
                GameManager.Instance.LoadLevelMenu();
            }
            else if (GameManager.Instance.IsInLevelMenu())
            {
                // Set game speed and load level
                GameManager.Instance.GameSpeed = LevelMenu.Instance.SelectedSpeed;
                GameManager.Instance.LoadLevel(LevelMenu.Instance.SelectedLevelId);
            }
                
         
        }

        void HandleOnGameSpeedChanged(int gameSpeed)
        {

            Debug.LogFormat("OnGameSpeedChanged: {0}", gameSpeed);
            // Get the selected level
            //int levelId = LevelMenu.Instance.SelectedLevelId;

            if (GameProgressManager.Instance.IsSpeedUnlocked(gameSpeed))
            {
                GetComponent<Button>().interactable = true;
                GetComponentInChildren<TMP_Text>().color =  enabledColor;
            }
            else
            {
                GetComponent<Button>().interactable = false;
                GetComponentInChildren<TMP_Text>().color = disabledColor;
            }
                
        }

    
    }

}
