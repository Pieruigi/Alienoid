using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class WarningSystem : MonoBehaviour
    {
        public static readonly float WarningTime = 3;

        public static WarningSystem Instance { get; private set; }

        //[SerializeField]
        //Renderer sampleRenderer;

        [SerializeField]
        Material warningMaterial;

        [SerializeField]
        float warningIntensity = 12f;

        Color warningColorDefault;
        Color warningColorMax;

        Color warningColor;

        float warnValue = 0;
        int count = 0;
        int dir = 0;
        float speed = 0.5f;
        bool playing = false;

        Color colorDefault;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                //warningMaterial = sampleRenderer.sharedMaterial;
                warningColorDefault = warningMaterial.GetColor("_EmissionColor");
                warningColorMax = warningColorDefault * warningIntensity;
                
                colorDefault = warningMaterial.GetColor("_EmissionColor");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.A))
                Play();
#endif
        }

        private void OnDestroy()
        {
            warningMaterial.SetColor("_EmissionColor", colorDefault);
        }

        public void Play()
        {
            if (playing)
                return;

            // Init
            playing = true;
            warningColor = warningMaterial.color;
            count = 2;
            dir = 1;
            
            // Start loop
            ChangeColor();
        }

        void SetWarningColor(Color value)
        {
            warningColor = value;
            warningMaterial.SetColor("_EmissionColor", warningColor);
        }

        void HandleOnComplete()
        {
            if (count == 0)
            {
                playing = false;
                return;
            }
         
            // Set the loop direction
            dir *= -1;

            // Check if a step has completed
            if (dir < 0)
                count--;
            
            // Next loop
            ChangeColor();
        }

        void ChangeColor()
        {
            DOTween.To(() => warningColor, x => SetWarningColor(x), dir > 0 ? warningColorMax : warningColorDefault, speed).OnComplete(HandleOnComplete);
        }
    }

}
