using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class InGameTimer : MonoBehaviour
    {
        string stringFormat = "{0:00}:{1:00.00}";
        TMP_Text label;
        // Start is called before the first frame update
        void Start()
        {
            label = GetComponentInChildren<TMP_Text>();

            label.text = string.Format(stringFormat, 0f, 0f, 0f);
                        
        }

        // Update is called once per frame
        void Update()
        {
            if (!LevelManager.Instance.Running)
                return;

            // Update timer
            double millis = (DateTime.UtcNow - LevelManager.Instance.StartingTime).TotalMilliseconds;

            millis /= 1000f;
            int min = (int)millis / 60;
            millis %= 60f;

            label.text = string.Format(stringFormat, min, millis);
        }
    }

}
