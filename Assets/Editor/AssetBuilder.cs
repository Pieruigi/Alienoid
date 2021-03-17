using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Zom.Pie.Collection;

namespace Zom.Pie.Editor
{
    public class AssetBuilder : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        [MenuItem("Assets/Create/Hawking/LevelConfiguration")]
        public static void CreateLevelConfigurationData()
        {
            LevelConfigurationData asset = ScriptableObject.CreateInstance<LevelConfigurationData>();
            
            string name = LevelConfigurationData.FileNamePattern+".asset";
            string folder = System.IO.Path.Combine("Assets/Resources/", LevelConfigurationData.ResourceFolder);

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            AssetDatabase.CreateAsset(asset, folder + name);
            
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }


}
