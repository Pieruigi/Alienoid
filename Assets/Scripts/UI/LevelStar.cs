using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class LevelStar : MonoBehaviour
    {
        
        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            int levelId = GetComponentInParent<Level>().LevelId;
            if (GameProgressManager.Instance.GetMaxBeatenSpeed(levelId) == Constants.MaxLevelSpeed)
            {
                // Activate star
                GetComponent<Image>().enabled = true;
            }
            else
            {
                GetComponent<Image>().enabled = false;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

  
    }

}
