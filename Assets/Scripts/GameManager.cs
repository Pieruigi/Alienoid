using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        // The level id is not the scene id
        private int currentLevelId = 1;
        public int CurrentLevelId
        {
            get { return currentLevelId; }
        }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
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
        void Update()
        {

        }
    }

}
