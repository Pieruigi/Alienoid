using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class InGameSpeed : MonoBehaviour
    {
        [SerializeField]
        TMP_Text text;

        // Start is called before the first frame update
        void Start()
        {
            float s = (GameManager.Instance.GameSpeed - 1) * Constants.LevelSpeedStep + 1;
            text.text = string.Format("Speed x" + s);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
