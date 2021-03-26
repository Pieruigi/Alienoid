using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class SpeedToggle : MonoBehaviour
    {
        [SerializeField]
        TMP_Text label;

        [SerializeField]
        Color unselected, selected;

 
        private void Awake()
        {
            Toggle toggle = GetComponent<Toggle>();
            //toggle.onValueChanged.AddListener((value)=> { label.color = (value ? selected : unselected); });
            toggle.onValueChanged.AddListener(HandleOnValueChanged);

        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnValueChanged(bool value)
        {
            Debug.LogFormat("ValueChanged:{0}", value);
            label.color = (value ? selected : unselected);
            label.transform.DOScale(value ? 1.2f : 1.0f, 0.25f);

            if (value)
            {
                GameManager.Instance.GameSpeed = int.Parse(this.name);
            }
        }
    }

}
