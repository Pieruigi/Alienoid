using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zom.Pie.Services;

namespace Zom.Pie.UI
{
    public class LevelMenu : MonoBehaviour
    {
        public UnityAction<int> OnLevelSelected;
        public UnityAction<int> OnGameSpeedSelected;

        public static LevelMenu Instance { get; private set; }

        // This is the last level we played ( if one ); this is usefull to not set always the last 
        // unlocked level as selected when we load this scene, that would not be appropriate if
        // the game has been completed
        public static int lastPlayedLevelId = 0;

    
        [SerializeField]
        Transform container;


        //[SerializeField]
        // This is the first object under the container
        GameObject levelTemplate;

        int selectedLevelId = 0;
        public int SelectedLevelId
        {
            get { return selectedLevelId; }
        }

        int selectedSpeed = 0;
        public int SelectedSpeed
        {
            get { return selectedSpeed; }
        }

        float selectionTime = 0.3f;
        System.DateTime lastSelection;

        int columns;

        int visibleRows = 3;

        GraphicRaycaster raycaster;

        LevelMenuScoreData scoreData;
        public LevelMenuScoreData ScoreData
        {
            get { return scoreData; }
        }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                columns = container.GetComponent<GridLayoutGroup>().constraintCount;

                raycaster = transform.root.GetComponent<GraphicRaycaster>();

               
                // Set from game manager  
                selectedSpeed = GameManager.Instance.GameSpeed;
                selectedLevelId = GameManager.Instance.LevelId;
              

                if (selectedSpeed < GameProgressManager.Instance.Speed)
                    selectedLevelId = 1;
                
                Debug.LogFormat("SelectedSpeed: {0}", selectedSpeed);
                Debug.LogFormat("SelectedLevelId: {0}", selectedLevelId);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get the level template and deactivate it
            levelTemplate = container.GetChild(0).gameObject;

            levelTemplate.SetActive(false);


            // Init the level screen by creating all the level objects from the template
            for (int i = 0; i < GameManager.Instance.GetNumberOfLevels(); i++)
            {
                GameObject l = GameObject.Instantiate(levelTemplate, container, true);
                l.SetActive(true);
                // Set level data
                //l.GetComponent<Level>().Init(i + 1, selectedSpeed);
            }

            // Move the template out
            levelTemplate.transform.parent = transform;

            SetSpeed(selectedSpeed);

        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.D))
            {
                selectedSpeed++;
                GameManager.Instance.GameSpeed = selectedSpeed;
                SetSpeed(selectedSpeed);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedSpeed--;
                GameManager.Instance.GameSpeed = selectedSpeed;
                SetSpeed(selectedSpeed);
            }
#endif

            // Check for level selection
            if (Input.GetMouseButtonDown(0))
            {
                if ((System.DateTime.UtcNow - lastSelection).TotalSeconds > selectionTime)
                {
                    // Set up a new pointer event data
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    // Set up the position
                    pointerEventData.position = Input.mousePosition;

                    // Create array to hold results
                    List<RaycastResult> results = new List<RaycastResult>();

                    // Raycast
                    raycaster.Raycast(pointerEventData, results);

                    // Check if there is a level selector 

                    GameObject level = null;
                    for (int i = 0; i < results.Count && !level; i++)
                    {
                        if (results[i].gameObject.GetComponent<Level>())
                            level = results[i].gameObject;
                    }

                    if (level)
                    {
                        Debug.LogFormat("Hit:{0}", level);

                        // Get the level id
                        int levelId = level.GetComponent<Level>().LevelId;

                        // The level must be not selected and unlocked
                        if (selectedLevelId != levelId && GameProgressManager.Instance.LevelIsUnlocked(levelId, selectedSpeed))
                        {
                            // Deselect the selected level
                            container.GetChild(selectedLevelId - 1).GetComponent<Level>().Select(false);

                            // Set the new selection
                            selectedLevelId = levelId;

                            // Select the new level
                            container.GetChild(selectedLevelId - 1).GetComponent<Level>().Select(true);

                            OnLevelSelected?.Invoke(selectedLevelId);

                            // Update timer
                            lastSelection = System.DateTime.UtcNow;
                        }
                    }




                }
            }




        }

        public void UpdateSpeed(int newSpeed)
        {
            selectedSpeed = newSpeed;
            GameManager.Instance.GameSpeed = selectedSpeed;
            SetSpeed(selectedSpeed);
        }
        
        void SetSpeed(int speed)
        {
            // Set the new speed
            selectedSpeed = speed;

            // Unselect current level if any
            if(selectedLevelId > 0)
            {
                container.GetChild(selectedLevelId - 1).GetComponent<Level>().Select(false);
                selectedLevelId = 0;
            }

            // Reset all the levels
            for(int i=0; i<container.childCount; i++)
            {
                container.GetChild(i).GetComponent<Level>().Init(i + 1, selectedSpeed);
            }


            // Get the last unlocked level if the selected speed is the higher unlocked, otherwise
            // get the first level of the old played speed
            if (selectedSpeed == GameProgressManager.Instance.Speed)
                selectedLevelId = GameProgressManager.Instance.LevelId;
            else
                selectedLevelId = 1;

            if(selectedLevelId > 0)
            {
                container.GetChild(selectedLevelId - 1).GetComponent<Level>().Select(true);
                
                // Call action
                OnLevelSelected?.Invoke(selectedLevelId);

                // Scroll down until the selected element is in the first row that we can see
                ScrollRect sr = container.parent.parent.GetComponent<ScrollRect>();
               
                // The line containing selected element
                int line = (selectedLevelId - 1) / columns;
                //if (line > 0)
                //{
                    // We must scroll
                    // Get the total number of lines
                    int lineCount = container.childCount / columns;
                    Debug.LogFormat("LineCount:{0}", lineCount);

                    // How many lines we can scroll?
                    float scrollableLines = (lineCount - visibleRows);

                    // How much we must scroll to move the selected line in the rist row
                    float scrollRatio = line / scrollableLines;

                    // Scroll
           
                    DOTween.To(() => sr.verticalNormalizedPosition, (x) => { sr.verticalNormalizedPosition = x; }, 1 - scrollRatio, 0.5f);
                  

                //}

            }

            OnGameSpeedSelected?.Invoke(selectedSpeed);

        }


    }

}

