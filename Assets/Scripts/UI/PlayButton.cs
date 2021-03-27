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
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(HandleOnClick);
            
            
        }

        // Start is called before the first frame update
        void Start()
        {

            if (GameManager.Instance.IsInLevelMenu())
            {
                GameManager.Instance.OnGameSpeedChanged += HandleOnGameSpeedChanged;
                //GetComponentInParent<LevelMenu>().OnLevelSelected += HandleOnLevelSelected;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameSpeedChanged -= HandleOnGameSpeedChanged;
        }

        void HandleOnClick()
        {
            if (GameManager.Instance.IsInMainMenu())
            {
                GameManager.Instance.LoadLevelMenu();
            }
            else if (GameManager.Instance.IsInLevelMenu())
            {
                GameManager.Instance.LoadLevel(LevelMenu.Instance.SelectedLevelId);
            }
                
         
        }

        void HandleOnGameSpeedChanged(int gameSpeed)
        {
            Debug.LogFormat("OnGameSpeedChanged: {0}", gameSpeed);
            // Get the selected level
            int levelId = LevelMenu.Instance.SelectedLevelId;

            if (GameProgressManager.Instance.IsGameSpeedAvailable(levelId, gameSpeed))
            {
                GetComponent<Button>().interactable = true;
                GetComponentInChildren<TMP_Text>().color = Constants.EnabledColor;
            }
            else
            {
                GetComponent<Button>().interactable = false;
                GetComponentInChildren<TMP_Text>().color = Constants.DisabledColor;
            }
                
        }

    
    }

}
