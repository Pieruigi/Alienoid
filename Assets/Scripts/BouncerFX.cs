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

        private void Awake()
        {
            bouncer.OnBounce += HandleOnBounce;
            rend = GetComponent<Renderer>();
            rend.sharedMaterial = baseMaterial;
            colorDefault = baseMaterial.color;
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
            if (playing)
                return;

            playing = true;
            Color c = colorDefault * 3;
            float time = 0.5f;
            rend.material.DOColor(c, time).OnComplete(() => rend.material.DOColor(colorDefault, time).OnComplete(() => playing = false)); ;
        }

        
    }

}
