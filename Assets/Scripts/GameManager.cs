using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zom.Pie.Collections;
using Zom.Pie.UI;

namespace Zom.Pie
{
    
    public class GameManager : MonoBehaviour
    {
        //public UnityAction<int> OnGameSpeedChanged;

        /// <summary>
        /// Params:
        ///     bool - true if a level scene is loading
        /// </summary>
        public UnityAction<bool> OnSceneLoading;

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
            set { gameSpeed = value; /*OnGameSpeedChanged?.Invoke(gameSpeed);*/ }
        }

        int levelid = 1;
        public int LevelId
        {
            get { return levelid; }
            set { levelid = value; }
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

        bool noEscape = false;
        
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                SceneManager.sceneLoaded += HandleOnSceneLoaded;

                Application.targetFrameRate = 60;

                // Initialize customization manager
                CustomizationManager.Initialize();

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
            GameProgressManager.Instance.OnLoadedOrUpdated += delegate
            {
                gameSpeed = GameProgressManager.Instance.Speed;
                levelid = GameProgressManager.Instance.LevelId;
            };
        }

        // Update is called once per frame
        // Update is called once per frame
        void Update()
        {
            // Game is loading...
            if (loading)
                return;

           // Debug.Log("TimeScale:" + Time.timeScale);

            // Handle device back button
            if (Input.GetKeyDown(KeyCode.Escape) && !noEscape)
            {
                string msg = "";
                if (IsInMainMenu() || IsInLevelMenu())
                {
                    if(IsInMainMenu())
                        msg = TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 0);
                    else
                        msg = TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 1);

                    MessageBox.Show(MessageBox.Type.YesNo, msg, HandleYesOnExitAction, HandleNoOnExitAction);
                }
                //else
                //{
                //    Time.timeScale = 0;
                //    msg = TextFactory.Instance.GetText(TextFactory.Type.UIMessage, 1);
                //}
                    


                //MessageBox.Show(MessageBox.Type.YesNo, msg, HandleYesOnExitAction, HandleNoOnExitAction);
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

        public void SetEscapeActive(bool value)
        {
            noEscape = !value;
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
            OnSceneLoading?.Invoke(false);
            LoadScene(mainMenuSceneIndex);
        }

        public void LoadLevelMenu()
        {
            OnSceneLoading?.Invoke(false);
            LoadScene(levelMenuSceneIndex);
        }

        public void LoadLevel()
        {
            LoadLevel(levelid);
        }

        void LoadLevel(int levelId)
        {
            OnSceneLoading?.Invoke(true);
            LoadScene(levelId + levelStartingIndex - 1);
        }

        public void Pause(bool value)
        {
            if (!IsInGame())
                return;

            if (value)
            {
                Time.timeScale = 0;
                if (PlayerManager.Instance != null)
                    PlayerManager.Instance.EnableController(false);
            }
            else
            {
                Time.timeScale = GetGameTimeScale();
                if(PlayerManager.Instance != null)
                    PlayerManager.Instance.EnableController(true);
            }
        }

        public bool IsPaused()
        {
            if (IsInGame() && Time.timeScale == 0)
                return true;
            else
                return false;
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
                // Set the actual time scale
                Time.timeScale = GetGameTimeScale();
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

        void HandleNoOnExitAction()
        {
            if (IsInGame())
            {
                Time.timeScale = GetGameTimeScale();
            }

        }

        float GetGameTimeScale()
        {
            float timeScale = Constants.DefaultTimeScale + Constants.LevelSpeedStep * (gameSpeed - 1);
            Debug.Log("GetGameTimeScale:" + timeScale);
            return timeScale;
        }

    }

}
