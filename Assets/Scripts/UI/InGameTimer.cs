using DG.Tweening;
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
        //float penaltyTime = 0;
        // Start is called before the first frame update
        void Start()
        {
            // Setting handle
            LevelManager.Instance.OnPenaltyTime += HandleOnPenaltyTime;

            label = GetComponentInChildren<TMP_Text>();

            label.text = string.Format(stringFormat, 0f, 0f, 0f);
                        
        }

        // Update is called once per frame
        void Update()
        {
            if (!LevelManager.Instance.Running)
                return;

            // Update timer
            //float millis = LevelManager.Instance.TimeScore * 1000f;

            //millis /= 1000f;
            //int min = (int)millis / 60;
            //millis %= 60f;

            //label.text = string.Format(stringFormat, min, millis);

            label.text = GeneralUtility.FormatTime(LevelManager.Instance.TimeScore);

        }

        void HandleOnPenaltyTime(float penaltyTime, BlackHole blackHole)
        {
            // Add penalty
            //this.penaltyTime += penaltyTime;

            // Red flickering
            float time = 0.5f;
            Sequence seq = DOTween.Sequence();
            seq.Append(label.DOColor(Color.red, time));
            seq.Append(label.DOColor(Color.white, time));
            seq.SetLoops(3);
            seq.Play();


        }
    }

}
