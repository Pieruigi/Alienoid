using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class AutoPlayParticles : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Play());
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Play()
        {
            yield return new WaitForSeconds(1);
            GetComponent<ParticleSystem>().Play();
        }
    }

}
