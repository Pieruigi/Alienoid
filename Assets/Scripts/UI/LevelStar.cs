using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class LevelStar : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> stars;

        private void Awake()
        {
           
            foreach(GameObject star in stars)
            {
                
                Color c = Color.white * 0.4f;
                c.a = 1;
                star.GetComponent<Image>().color = c;
                
                star.SetActive(false);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
           
            //int levelId = GetComponentInParent<Level>().LevelId;



            //int higher = GameProgressManager.Instance.GetHigherUnlockedSpeed();

            //if (GameProgressManager.Instance.LevelIsUnlocked(levelId, ))
            //{
            //    for (int i = 0; i < Constants.MaxLevelSpeed; i++)
            //    {
            //        stars[i].SetActive(true);

            //        if(i<maxBeaten)
            //            stars[i].GetComponent<Image>().color = Color.white;
            //    }

            //}
            

        }

        // Update is called once per frame
        void Update()
        {

        }

  
    }

}
