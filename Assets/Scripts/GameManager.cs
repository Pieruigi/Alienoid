using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zom.Pie.Collections;
using Zom.Pie.UI;

namespace Zom.Pie
{
    
    public class GameManager : MonoBehaviour
    {
        

        public static GameManager Instance { get; private set; }

        int mainMenuSceneIndex = 0;
        int levelMenuSceneIndex = 1;

        int levelStartingIndex = 2;

        // The level id is not the scene id
        //private int currentLevelId = 1;
        //public int CurrentLevelId
        //{
        //    get { return currentLevelId; }
        //}

        /// <summary>
        /// 1: normal speed
        /// </summary>
        int gameSpeed = 1;
        public int GameSpeed
        {
            get { return gameSpeed; }
            set { gameSpeed = value; }
        }
  
        Language language = Language.English;
        public Language Language
        {
            get { return language; }
        }

        //bool inGame = false;
        //public bool InGame
        //{
        //    get { return inGame; }
        //}


        bool loading = false;
        public bool Loading
        {
            get { return loading; }
        }

        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                SceneManager.sceneLoaded += HandleOnSceneLoaded;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        // Update is called once per frame
        void Update()
        {
            // Game is loading...
            if (loading)
                return;

            // Handle device back button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                string msg = "";
                if (IsInMainMenu())
                    msg = TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 0);
                else
                    msg = TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 1);


                MessageBox.Show(MessageBox.Type.YesNo, msg, HandleYesOnExitAction, null);
            }

            //if (InGame)
            //{
            //    // Do ingame stuff...
            //}
            //else
            //{
            //    // Do not in game stuff...
            //}
        }



        public bool IsInGame()
        {
            return SceneManager.GetActiveScene().buildIndex >= levelStartingIndex;
        }

        public bool IsInLevelMenu()
        {
            return SceneManager.GetActiveScene().buildIndex == levelMenuSceneIndex;
        }

        public bool IsInMainMenu()
        {
            return SceneManager.GetActiveScene().buildIndex == mainMenuSceneIndex;
        }

        public int GetCurrentLevelId()
        {
            if (!IsInGame())
                return -1;
            
            return SceneManager.GetActiveScene().buildIndex - levelStartingIndex + 1;
        }

        public int GetNumberOfLevels()
        {
#if UNITY_EDITOR
            return SceneManager.sceneCountInBuildSettings - levelStartingIndex;
#else
            return SceneManager.sceneCountInBuildSettings - levelStartingIndex;
#endif
        }

        /// <summary>
        /// Load the main menu.
        /// </summary>
        public void LoadMainMenu()
        {
            LoadScene(mainMenuSceneIndex);
        }

        public void LoadLevelMenu()
        {
            LoadScene(levelMenuSceneIndex);
        }

        public void LoadLevel(int levelId)
        {
            LoadScene(levelId + levelStartingIndex - 1);
        }

        /// <summary>
        /// Load a scene by its build index.
        /// </summary>
        /// <param name="index"></param>
        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
            loading = true;
        }

        /// <summary>
        /// This is called after the scene has been loaded.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

            // Loading completed
            loading = false;

            if(scene.buildIndex < levelStartingIndex)
            {
                // Reset the time scale
                Time.timeScale = 1;
            }
            else
            {
                SetTimeScaleByGameSpeed();
            }
        }

        void HandleYesOnExitAction()
        {
            if (IsInGame())
                LoadLevelMenu();
            else if (IsInLevelMenu())
                LoadMainMenu();
            else
                Application.Quit();
        }

        void SetTimeScaleByGameSpeed()
        {
            switch (gameSpeed)
            {
                case 1:
                    Time.timeScale = Constants.DefaultTimeScale;
                    break;
                case 2:
                    Time.timeScale = Constants.DefaultTimeScale * 1.5f;
                    break;
                case 3:
                    Time.timeScale = Constants.DefaultTimeScale * 2f;
                    break;
                case 4:
                    Time.timeScale = Constants.DefaultTimeScale * 2.5f;
                    break;
            }
        }
    }

}
