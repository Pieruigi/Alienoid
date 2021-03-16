using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class StartingTimer : MonoBehaviour
    {
        [SerializeField]
        Text timerField;

        float delay;
        bool hide = false;


        // Start is called before the first frame update
        void Start()
        {
            // Get the delay and set the timer field
            delay = LevelManager.Instance.StartDelay * Time.timeScale;
            timerField.text = delay.ToString();

            // For testing in the editor
            if (delay == 0)
            {
                hide = true;
                timerField.gameObject.SetActive(false);
            }
                
        }

        // Update is called once per frame
        void Update()
        {
            if (hide)
                return;

            delay -= Time.deltaTime;
            if (delay < 0)
                delay = 0;

            if (delay > 0)
            {
                // We need to adjust the timer by taking into account the timescale at which we are playing
                timerField.text = (((int)(delay/Time.timeScale))+1).ToString();
            }
            else
            {
                hide = true;
                timerField.gameObject.SetActive(false);
            }

            

        }
    }

}
