using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class Brick : MonoBehaviour
    {
        [SerializeField]
        int hitPoints = 1;

        [SerializeField]
        float rebuildCooldown = 10f;

        MeshRenderer rend;
        Collider coll;

        bool destroyed = false;
        float timer = 0;
 

        Vector3 scaleDefault;


        private void Awake()
        {
            rend = GetComponent<MeshRenderer>();
            coll = GetComponent<Collider>();
            scaleDefault = transform.localScale;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (destroyed)
            {
                timer -= Time.deltaTime;
                if(timer < 0)
                {
                    StartCoroutine(Build());
                }
            }
        }

        public void Hit()
        {
            if (destroyed)
                return;

            // Decrease hit points
            hitPoints = Mathf.Max(hitPoints - 1, 0);

            if (hitPoints == 0)
            {

                StartCoroutine(Destroy());
                
            }

        }

        IEnumerator Destroy()
        {
            transform.DOScale(Vector3.zero, 0.25f);
            yield return new WaitForSeconds(0.25f);

            // Destroy brick
            rend.enabled = false;
            coll.enabled = false;

            // Prepare timer for rebuilding
            destroyed = true;
            timer = rebuildCooldown;
        }

        IEnumerator Build()
        {
            
            transform.DOScale(scaleDefault, 0.25f);
            yield return new WaitForSeconds(0.25f);

            // Destroy brick
            rend.enabled = true;
            coll.enabled = true;

            destroyed = false;
            
        }

    }

}
