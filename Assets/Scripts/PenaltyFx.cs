using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class PenaltyFx : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem penaltyFxPrefab;

        GameObject penaltyFx;

        //Vector3 position = new Vector3(7.44f, 4.27f, -2.17f);

        private void Awake()
        {
            penaltyFx = GameObject.Instantiate(penaltyFxPrefab.gameObject);
            //penaltyFx.transform.position = position;
        }

        // Start is called before the first frame update
        void Start()
        {
            LevelManager.Instance.OnPenaltyTime += HandleOnPenalty;
        }

        // Update is called once per frame
        void Update()
        {

        }
         
        void HandleOnPenalty(float time, BlackHole blackHole)
        {
            penaltyFx.transform.position = blackHole.transform.position;
            penaltyFx.GetComponent<ParticleSystem>().Play();
        }
    }

}
