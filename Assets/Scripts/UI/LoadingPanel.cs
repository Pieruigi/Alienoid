using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        public static LoadingPanel Instance { get; set; }

        [SerializeField]
        GameObject panel;
         
        [SerializeField]
        Image icon;

        bool hidden = true;
        float speed = -160;


        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                // Hide
                Show(false);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (hidden)
                return;

            icon.transform.localEulerAngles += Vector3.forward * speed * Time.deltaTime / Time.timeScale;
        }

        public void Show(bool value)
        {
            hidden = !value;
            
            panel.SetActive(value);

            if (value)
            {
                // Reset icon
                icon.transform.localEulerAngles = Vector3.zero;
            }
        }
    }

}
