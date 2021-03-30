using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class BouncerFX : MonoBehaviour
    {
        [SerializeField]
        Bouncer bouncer;

        [SerializeField]
        Material baseMaterial;

        Color colorDefault;
        Renderer rend;
        bool playing = false;

        float intensity;
        int dir = 1;
        float intensityTime = 10;

        private void Awake()
        {
            bouncer.OnBounce += HandleOnBounce;
            rend = GetComponent<Renderer>();
            rend.sharedMaterial = baseMaterial;
            colorDefault = baseMaterial.GetColor("_EmissionColor");
            Debug.Log("ColorDefault:" + colorDefault);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.A))
                HandleOnBounce(null);
#endif
        }

        void HandleOnBounce(Bouncer bouncer)
        {
            //if (playing)
            //    return;

            playing = true;
            intensity = 0;
            float targetIntensity = 10f;
            float time = 0.5f;
            
          

            dir = -1;

            //DOTween.To(()=>currentColor, )


            //DOTween.To(() => intensity, (x) => { intensity = x; rend.material.SetColor("_EmissionColor", colorDefault * intensity); }, targetIntensity, intensityTime).OnComplete(HandleOnComplete);
                
          
            //rend.material.DOColor(c, time).OnComplete(() => rend.material.DOColor(colorDefault, time).OnComplete(() => playing = false)); ;
        }

        void HandleOnComplete()
        {
            return;
            //Debug.Log("Completed - CurrentColor:" + currentColor);
            rend.material.SetColor("_EmissionColor", colorDefault);

            if(dir == -1)
                DOTween.To(() => intensity, (x) => { intensity = x; rend.material.SetColor("_EmissionColor", colorDefault * intensity); }, 0, intensityTime);

        }
    }

}
