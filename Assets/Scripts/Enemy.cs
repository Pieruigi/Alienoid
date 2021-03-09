using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public enum EnemyType { Green, Yellow, Red }

    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        Material greenMaterial, yellowMaterial, redMaterial;

        [SerializeField]
        MeshRenderer meshRenderer;

        [SerializeField]
        int materialId;

        EnemyType type;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
           
        }

        public void SetType(EnemyType value)
        {
            Material[] mats = meshRenderer.sharedMaterials;
            mats[materialId] = GetMaterial(value);
            meshRenderer.sharedMaterials = mats;
            type = value;
        }

        Material GetMaterial(EnemyType type)
        {
            Material ret = null;
            switch (type)
            {
                case EnemyType.Green:
                    ret = greenMaterial;
                    break;
                case EnemyType.Yellow:
                    ret = yellowMaterial;
                    break;
                case EnemyType.Red:
                    ret = redMaterial;
                    break;
            }

            return ret;
        }
    }
    
}
