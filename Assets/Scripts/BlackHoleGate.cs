using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace Zom.Pie
{
    [ExecuteInEditMode]
    public class BlackHoleGate : MonoBehaviour
    {
        public UnityAction<BlackHoleGate> OnGateOpen;
        public UnityAction<BlackHoleGate> OnGateClosed;

        [SerializeField]
        GameObject gate;

        [SerializeField]
        GameObject leftGate;

        [SerializeField]
        GameObject rightGate;

        bool closed = false;
        public bool Closed
        {
            get { return closed; }
        }
        float time = 0;

        float scaleDefault = 1;
        float animTime = 0.5f;

        //float warningTime = 3f;

        Collider coll;
        bool busy = false;

        private void Awake()
        {
            // Get collider
            coll = GetComponent<Collider>();

            // Open gate
            Open(true);
        }


        // Update is called once per frame
        void Update()
        {

        }

    

        public void Open(bool forced)
        {

            if (!forced)
            {
                // Open
                coll.enabled = false;
                closed = false;
                gate.transform.DOScale(0, animTime).SetEase(Ease.OutBounce); 
            }
            else
            {
                coll.enabled = false;
                closed = false;
                gate.transform.localScale = Vector3.zero;
                
            }

            OnGateOpen?.Invoke(this);

        }

        public void Close(bool forced)
        {
            if (!forced)
            {
                coll.enabled = true;
                closed = true;
                gate.transform.DOScale(scaleDefault, animTime).SetEase(Ease.OutBounce);
            }
            else
            {
                coll.enabled = true;
                closed = true;
                gate.transform.localScale = Vector3.one * scaleDefault;
               
            }

            OnGateClosed?.Invoke(this);
        }


    }

}
