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
                //GameManager.Instance.GameSpeed = LevelMenu.Instance.SelectedSpeed;
                GameManager.Instance.LoadLevel(LevelMenu.Instance.SelectedLevelId);
            }
                
         
        }

    

    
    }

}
