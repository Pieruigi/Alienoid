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

        Material mat;

        Color colorDefault;
        //Renderer rend;
        bool playing = false;

        Color targetColor;
        float intensity = 50;
        float time = 0.25f;

        private void Awake()
        {
            bouncer.OnBounce += HandleOnBounce;
            Renderer rend = GetComponent<Renderer>();

            // Create a new material
            mat = new Material(rend.sharedMaterial);
            rend.sharedMaterial = mat;

            colorDefault = mat.GetColor("_EmissionColor");

  
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
            //if (!"bouncertop".Equals(transform.parent.name.ToLower()))
            //    return;

            if (playing)
                return;

            playing = true;

            Debug.Log("Setting color");
            //mat.SetColor("_EmissionColor", Color.red * 10);

            targetColor = Color.red * intensity;

            DOTween.To(() => mat.GetColor("_EmissionColor"), (x) => mat.SetColor("_EmissionColor", x), targetColor, time)
                .OnComplete(() => { DOTween.To(() => mat.GetColor("_EmissionColor"), (x) => mat.SetColor("_EmissionColor", x), colorDefault, time)
                                    .OnComplete(() => { playing = false; }); ; });
                
        }

       


      
    }

}
