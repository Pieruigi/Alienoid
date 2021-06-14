using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public enum OptionType { MusicOnOff, FxOnOff }

    public class OptionToggle : MonoBehaviour
    {
        [SerializeField]
        TMP_Text text;


        [SerializeField]
        OptionType optionType;

        [SerializeField]
        AudioMixer mixer;

        bool isOff = false;

        private void Awake()
        {
            // Get or create player preferences
            if (!PlayerPrefs.HasKey(optionType.ToString()))
                PlayerPrefs.SetInt(optionType.ToString(), isOff ? 0 : 1);

            isOff = PlayerPrefs.GetInt(optionType.ToString()) == 0 ? true : false;

            // Set text and mixer
            text.text = isOff ? "Off" : "On";
            //SetMixer(); // It doesn't work on awake... so we called it in the main panel

            // Set button handle
            GetComponent<Button>().onClick.AddListener(HandleOnClick);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetMixer()
        {
            switch (optionType)
            {
                case OptionType.MusicOnOff:
                    mixer.SetFloat("MusicVolume", isOff ? -80 : 0);

                    break;
                case OptionType.FxOnOff:
                    mixer.SetFloat("FxVolume", isOff ? -80 : 0);
                    break;
            }

        }

        void HandleOnClick()
        {
            isOff = !isOff;
            PlayerPrefs.SetInt(optionType.ToString(), isOff ? 0 : 1);
            text.text = isOff ? "Off" : "On";
            SetMixer();
        }
    }

}
