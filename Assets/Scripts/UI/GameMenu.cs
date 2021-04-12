using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class GameMenu : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        bool notAvailable = false;

        private void Awake()
        {
            Close();
        }

        // Start is called before the first frame update
        void Start()
        {
            LevelManager.Instance.OnLevelBeaten += HandleOnGameCompleted;
            PlayerManager.Instance.OnDead += HandleOnGameCompleted;
        }

        // Update is called once per frame
        void Update()
        {
            if (!LevelManager.Instance.Running)
                return;

            if (GameManager.Instance.IsPaused())
                return;

            if(Input.GetKeyDown(KeyCode.Escape))
                Open();
        }

        public void Open()
        {
            if (notAvailable)
                return;

            GameManager.Instance.Pause(true);
            panel.SetActive(true);
        }

        public void Close()
        {
            panel.SetActive(false);
            GameManager.Instance.Pause(false);
        }

        void HandleOnGameCompleted()
        {
            notAvailable = true;
        }
        
    }

}
