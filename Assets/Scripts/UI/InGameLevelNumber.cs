using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class InGameLevelNumber : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            TMP_Text label = GetComponentInChildren<TMP_Text>();

            label.text = string.Format(label.text, GameManager.Instance.GetCurrentLevelId());

            
        }

        // Update is called once per frame
        void Update()
        {
       

        }
    }

}
