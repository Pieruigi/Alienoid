using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class SpeederSwitcher : GameplaySwitcher
    {
        //enum SpeederColor { Default, Green, Yellow, Red }


        [System.Serializable]
        class Data
        {
            [SerializeField]
            public List<EnemySpeeder> speeders;

            [SerializeField]
            public GameObject fx;

            //[SerializeField]
            //public bool fxKeepDirection = false;

            //[SerializeField]
            //public List<SpeederColor> colors;

            [SerializeField]
            public bool inverse;

            [HideInInspector]
            public float fadeElapsed;

            [HideInInspector]
            public float fadeTime;
        }

        [SerializeField]
        List<Data> datas;

        bool fade = false;
        float fadeTime = 2f;
        //Color defaultColor;

        protected override void Awake()
        {
            base.Awake();

            // Set default color
            //defaultColor = fx.GetComp
        }

        protected override void Update()
        {
            base.Update();

            if (fade)
            {
                foreach (Data d in datas)
                {
                  
                    ParticleSystem ps = d.fx.GetComponent<ParticleSystem>();
                    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.main.maxParticles];
                    int count = ps.GetParticles(particles, particles.Length);
                    
                    if(count > 0)
                    {
                        Color c = particles[0].startColor;
                        c.a = 0;
                        c = Color.red;

                        
                        if(d.fadeElapsed > d.fadeTime)
                        {
                            particles[0].remainingLifetime = 0;
                            d.fadeElapsed %= d.fadeTime;
                           
                        }
                        else
                        {
                            d.fadeElapsed += Time.deltaTime;
                        }
                    }

                    ps.SetParticles(particles);

                }

            }
        }

        protected override void Init()
        {
            // Initialization
            foreach(Data d in datas)
            {
                if (d.inverse)
                {
                    Switch(d, true);
                }
            }

            
        }

        protected override void Switch()
        {
           
            foreach (Data d in datas)
            {
                Switch(d, false);
                
            }
        }

        void Switch(Data data, bool force)
        {
            if (force)
            {
                // Simply reverse children order in the fx
                ReverseChildren(data.fx.transform);

                // Reverse speeders
                ReverseSpeeders(data.speeders);
            }
            else
            {
                StartCoroutine(DoSwitch(data));
            }
        }
        
        IEnumerator DoSwitch(Data data)
        {
            Debug.Log("Switch speeder:" + gameObject);

            // Stop fx
            data.fx.GetComponent<ParticleSystem>().Stop();

            // Set flag for the update process
            fade = true;
           
            // Fade out and destroy particles; each particle system must have its own fade time depending
            // on the number of active particles
            data.fadeTime = fadeTime / data.fx.GetComponent<ParticleSystem>().particleCount;
            data.fadeElapsed = 0;

            // Wait for fade to complete
            yield return new WaitForSeconds(fadeTime+0.1f);

            // Reset fade flag
            fade = false;

            yield return new WaitForSeconds(0.5f);

            // Reverse particle
            ReverseChildren(data.fx.transform);

            // Reverse speeders
            ReverseSpeeders(data.speeders);

            // Start again
            data.fx.GetComponent<ParticleSystem>().Play();
        }

        void ReverseChildren(Transform parent)
        {
            // Simply reverse the children order
            Transform[] children = parent.GetComponentsInChildren<Transform>();
            for (int i = 0; i < children.Length; i++)
                children[i].SetAsFirstSibling();
        }

        void ReverseSpeeders(List<EnemySpeeder> speeders)
        {
            
            foreach (EnemySpeeder speeder in speeders)
                speeder.Reverse();
            
        }

    }

}
