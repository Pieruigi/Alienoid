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
    [ExecuteInEditMode]
    public class TextTranslator : MonoBehaviour
    {
        [SerializeField]
        TextFactory.Type textType = TextFactory.Type.UILabel;

        [SerializeField]
        int textId;

        // Start is called before the first frame update
        void Awake()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Init();
                return;
            }
#endif
        }

        void Init()
        {
            string txt = TextFactory.Instance.GetText(textType, textId);

            if (GetComponent<TMP_Text>())
                GetComponent<TMP_Text>().text = txt;
            else
                GetComponent<Text>().text = txt;
        }
    }

}
