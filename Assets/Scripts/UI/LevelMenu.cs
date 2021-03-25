using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class LevelMenu : MonoBehaviour
    {
        [SerializeField]
        Transform container;

        //[SerializeField]
        // This is the first object under the container
        GameObject levelTemplate;

        int selectedLevel = 0;

        float selectionTime = 0.5f;
        System.DateTime lastSelection;

        int columns;

        int visibleRows = 2;

      
        private void Awake()
        {
            columns = container.GetComponent<GridLayoutGroup>().constraintCount;

          
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
            selectedLevel = GameProgressManager.Instance.GetLastUnlockedLevel();
            container.GetChild(selectedLevel - 1).GetComponent<Level>().Select(true);

            // Scroll down until the selected element is in the first row that we can see
            ScrollRect sr = container.parent.parent.GetComponent<ScrollRect>();
            // The line containing selected element
            int line = (selectedLevel-1) / columns;
            if(line > 0)
            {
                // We must scroll
                // Get the total number of lines
                int lineCount = container.childCount / columns;
                Debug.LogFormat("LineCount:{0}", lineCount);

                // Compute total length
                float scrollableLines = (lineCount - visibleRows);
                Debug.LogFormat("TotalLength:{0}", scrollableLines);
                //totalLength += lineCount * levelTemplateSize + (lineCount - 1) * spacing;

                // Get the length we must scroll
                //float scrollLength = paddingTop + lineCount * levelTemplateSize + (lineCount - 1) * spacing;

                float scrollRatio = line / scrollableLines;

                //// Get the value to scroll a single line
                
                Debug.LogFormat("SingleNormalized:{0}", scrollRatio);
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

 
        }

        
        
    }

}
