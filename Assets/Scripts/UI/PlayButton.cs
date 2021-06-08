using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;
using Zom.Pie.Services;

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

                // Check whether you are logged or not
                if(!AccountManager.Instance.Logged)
                {
                    // Not logged, show a message box
                    // Get the login panel
                    GameObject.FindObjectOfType<LogInPanel>().Show();
                    //MessageBox.Show(MessageBox.Type.Ok, TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 5));
                }
                else
                {
                    // Check if game progress has been synchronized 
                    if (!GameProgressManager.Instance.Loaded)
                    {
                        MessageBox.Show(MessageBox.Type.Ok, TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 5));
                    }
                    else
                    {
                        // You are logged in and the game progress has been loaded
                        GameManager.Instance.LoadLevelMenu();
                    }
                    
                }

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
