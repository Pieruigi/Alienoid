using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Zom.Pie
{
    public enum EnemyType { Green, Yellow, Red }

    public class Enemy : MonoBehaviour
    {
        public UnityAction<Enemy, BlackHole> OnDead;

        public static float Radius;
        public static float SqrRadius;

        [SerializeField]
        Material greenMaterial, yellowMaterial, redMaterial;

        [SerializeField]
        MeshRenderer meshRenderer;

        [SerializeField]
        int materialId;

        EnemyType type;
        public EnemyType Type
        {
            get { return type; }
        }

        Vector3 scaleDefault;

        bool dying = false;
        public bool Dying
        {
            get { return dying; }
            set { dying = value; }
        }

        private void Awake()
        {
            scaleDefault = transform.localScale;

            if(Radius == 0)
            {
                Radius = GetComponent<SphereCollider>().radius;
                SqrRadius = Radius * Radius;
            }
                
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
           
        }

        private void OnEnable()
        {
            // Reset scale
            transform.localScale = Vector3.zero;

            // Grow up
            transform.DOScale(scaleDefault, 0.1f);
        }

        public void Die(BlackHole blackHole)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            dying = false;

            OnDead?.Invoke(this, blackHole);
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
