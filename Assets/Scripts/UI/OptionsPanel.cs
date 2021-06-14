using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.UI 
{ 
    public class OptionsPanel : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        [SerializeField]
        OptionToggle musicOnOff;

        [SerializeField]
        OptionToggle fxOnOff;

        // Start is called before the first frame update
        void Start()
        {
            musicOnOff.SetMixer();
            fxOnOff.SetMixer();

            panel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Open()
        {
            if (panel.activeSelf)
                return;

            
            panel.SetActive(true);
        }

        public void Close()
        {
            if (!panel.activeSelf)
                return;

            panel.SetActive(false);
        }
    }

}
