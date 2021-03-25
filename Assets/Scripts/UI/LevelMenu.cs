using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class LevelMenu : MonoBehaviour
    {
        [SerializeField]
        Transform container;

        //[SerializeField]
        // This is the first object under the container
        GameObject levelTemplate;

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
            }

            // Move the template out
            levelTemplate.transform.parent = transform;
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
