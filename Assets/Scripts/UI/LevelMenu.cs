using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class LevelMenu : MonoBehaviour
    {
        [SerializeField]
        Transform container;

        [SerializeField]
        List<Toggle> speedSelectors;

        //[SerializeField]
        // This is the first object under the container
        GameObject levelTemplate;

        int selectedLevelId = 0;

        float selectionTime = 0.5f;
        System.DateTime lastSelection;

        int columns;

        int visibleRows = 2;

        GraphicRaycaster raycaster;

          
        private void Awake()
        {
            columns = container.GetComponent<GridLayoutGroup>().constraintCount;

            raycaster = transform.root.GetComponent<GraphicRaycaster>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get the level template and deactivate it
            levelTemplate = container.GetChild(0).gameObject;
           
            levelTemplate.SetActive(false);
            

            // Init the level screen
            for(int i=0; i<GameManager.Instance.GetNumberOfLevels(); i++)
            {
                GameObject l = GameObject.Instantiate(levelTemplate, container, true);
                l.SetActive(true);
                // Set level data
                l.GetComponent<Level>().Init(i + 1);
            }

            // Move the template out
            levelTemplate.transform.parent = transform;

            // Now select the first available level
            selectedLevelId = GameProgressManager.Instance.GetLastUnlockedLevel();
            Debug.LogFormat("SelectedLevelId:{0}", selectedLevelId);
            container.GetChild(selectedLevelId - 1).GetComponent<Level>().Select(true);

            // Reset speed toggles
            ResetSpeedSelectors();

            // Scroll down until the selected element is in the first row that we can see
            ScrollRect sr = container.parent.parent.GetComponent<ScrollRect>();
            // The line containing selected element
            int line = (selectedLevelId - 1) / columns;
            if(line > 0)
            {
                // We must scroll
                // Get the total number of lines
                int lineCount = container.childCount / columns;
                Debug.LogFormat("LineCount:{0}", lineCount);

                // How many lines we can scroll?
                float scrollableLines = (lineCount - visibleRows);
                
                // How much we must scroll to move the selected line in the rist row
                float scrollRatio = line / scrollableLines;

                // Scroll
                sr.verticalNormalizedPosition = 1 - scrollRatio;
                
            }
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.LogFormat("Before:'{0}'",PlayerPrefs.GetString("save"));
                int next = GameProgressManager.Instance.GetLastUnlockedLevel();
                if(next >= 0)
                {
                    GameProgressManager.Instance.SetLevelBeaten(next, 1);
                    Debug.LogFormat("After:'{0}'", PlayerPrefs.GetString("save"));
                }
                else
                {
                    Debug.Log("No more levels");
                }
                
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
                    for(int i=0; i<results.Count && !level; i++)
                    {
                        if (results[i].gameObject.GetComponent<Level>())
                            level = results[i].gameObject;
                    }

                    if (level)
                    {
                        Debug.LogFormat("Hit:{0}", level);

                        // Get the level id
                        int levelId = level.GetComponent<Level>().LevelId;

                        // If this level isn't the current selected one we must switch selection 
                        if(selectedLevelId != levelId)
                        {
                            // Deselect the selected level
                            container.GetChild(selectedLevelId - 1).GetComponent<Level>().Select(false);

                            // Set the new selection
                            selectedLevelId = levelId;

                            // Select the new level
                            container.GetChild(selectedLevelId - 1).GetComponent<Level>().Select(true);

                            // Reset the speed toggles
                            ResetSpeedSelectors();

                            // Update timer
                            lastSelection = System.DateTime.UtcNow;
                        }
                    }
                        



                }
            }
            

            
            
        }


        void ResetSpeedSelectors()
        {
            
            // Get the current level
            Level level = container.GetChild(selectedLevelId-1).GetComponent<Level>();

            Debug.LogFormat("Resetting speed toggles for level {0}", level.LevelId);

            // Set all speed toggles
            for (int i=0; i<speedSelectors.Count; i++)
            {
                Debug.LogFormat("Checking for speed {0}", i);
                if (i > level.MaxBeatenSpeed)
                    speedSelectors[i].interactable = false;
                else
                    speedSelectors[i].interactable = true;

                if(level.MaxBeatenSpeed == 0 && i==0)
                {
                    // Level has not been beaten yet, so we set the first speed as the only available one
                    speedSelectors[i].GetComponent<Toggle>().isOn = true;
                }
                else
                {
                    // We set the highest available speed
                    if (i == level.MaxBeatenSpeed)
                    {
                        // There is another speed to play
                        speedSelectors[i].GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        if(level.MaxBeatenSpeed == speedSelectors.Count+1)
                        {
                            // No more speed, set the last one
                            speedSelectors[speedSelectors.Count-1].GetComponent<Toggle>().isOn = true;
                        }
                        
                    }

                    
                } 
                
                    
            }
        }
        
        
    }

}
