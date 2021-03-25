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
    public class ExitButton : MonoBehaviour
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
            string msg = "";
            if (GameManager.Instance.IsInMainMenu())
                msg = TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 0);
            else
                msg = TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 1);


            MessageBox.Show(MessageBox.Type.YesNo, msg, HandleYesAction, null);
        }

        void HandleYesAction()
        {
            if (GameManager.Instance.IsInGame())
                GameManager.Instance.LoadLevelMenu();
            else if (GameManager.Instance.IsInLevelMenu())
                GameManager.Instance.LoadMainMenu();
            else
                Application.Quit();
        }


    }

}
