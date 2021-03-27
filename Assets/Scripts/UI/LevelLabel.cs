using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class LevelLabel : MonoBehaviour
    {
        string txtFormat = "LVL {0}";

        private void Awake()
        {
            GetComponentInParent<LevelMenu>().OnLevelSelected += HandleOnLevelSelected;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnLevelSelected(int levelId)
        {
            GetComponent<TMP_Text>().text = string.Format(txtFormat, levelId);
        }
    }

}
