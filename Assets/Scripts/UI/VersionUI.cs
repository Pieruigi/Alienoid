using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Zom.Pie.UI
{
    public class VersionUI : MonoBehaviour
    {
        [SerializeField]
        TMP_Text versionText;

        private void Awake()
        {
            versionText.text = Application.version;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
