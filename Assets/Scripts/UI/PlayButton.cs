using System.Collections;
using System.Collections.Generic;
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

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnClick()
        {
            if (GameManager.Instance.IsInMainMenu())
                GameManager.Instance.LoadLevelMenu();
            else if (GameManager.Instance.IsInLevelMenu())
            {
                GameManager.Instance.LoadLevel(LevelMenu.Instance.SelectedLevelId);
            }
                
         
        }




    }

}
