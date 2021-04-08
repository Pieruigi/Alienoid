using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class SkyboxCustomizer : MonoBehaviour
    {
        
        private void Awake()
        {
            // We should read the id from player prefs
            Material skybox = CustomizationManager.Instance.GetSkybox(0);

            RenderSettings.skybox = skybox;
            
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
