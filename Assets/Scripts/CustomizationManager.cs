using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class CustomizationManager
    {

        static CustomizationManager instance;
        public static CustomizationManager Instance
        {
            get 
            {
                if (instance == null)
                    instance = new CustomizationManager();

                return instance;
            }
        }

        List<Material> skyboxes;
        string skyboxResourcePath = "Customization/Skyboxes";


        private CustomizationManager() 
        {
            // Load built-in resources
            skyboxes = new List<Material>(Resources.LoadAll<Material>(skyboxResourcePath));
            Debug.LogFormat("Built-in skyboxes count: {0}", skyboxes.Count);
        }

        public static void Initialize() 
        {
            if (instance == null)
                instance = new CustomizationManager();
        }

        public Material GetSkybox(int id)
        {
            return skyboxes[id];
        }

    }

}
