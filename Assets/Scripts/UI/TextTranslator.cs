using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zom.Pie.Collections;
using UnityEngine.UI;
using TMPro;

namespace Zom.Pie.UI
{
    /// <summary>
    /// Fill text field with the right text based on the current language.
    /// </summary>
    public class TextTranslator : MonoBehaviour
    {
        [SerializeField]
        TextResolver.Type textType = TextResolver.Type.UILabel;

        [SerializeField]
        int textId;

        // Start is called before the first frame update
        void Start()
        {
            string txt = TextResolver.Instance.GetText(textType, textId);
            
            if (GetComponent<TMP_Text>())
                GetComponent<TMP_Text>().text = txt;
            else
                GetComponent<Text>().text = txt;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
