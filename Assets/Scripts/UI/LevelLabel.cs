using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zom.Pie.Collections;

namespace Zom.Pie.UI
{
    public class LevelLabel : MonoBehaviour
    {
        string txtFormat = "LVL {0}";

        private void Awake()
        {
            // Multilanguage support
            string txtFormat = TextFactory.Instance.GetText(TextFactory.Type.UILabel, 17);
            txtFormat += " {0}";

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
