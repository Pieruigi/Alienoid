using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class LeaderboardPanel : MonoBehaviour
    {
        public static LeaderboardPanel Instance { get; private set; }

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
