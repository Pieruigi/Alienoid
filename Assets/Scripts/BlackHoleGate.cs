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

        // How long the gate is open ( the gate will close in openLength seconds )
        [SerializeField]
        float openLength = 10f;
        float openLengthScaled;

        // How long the gate is closed ( the gate will open in closeLength seconds )
        [SerializeField]
        float closeLength = 5;
        float closeLengthScaled;

        // Must the gate be closed on level start?
        [SerializeField]
        bool closeOnStart = false;

        // The time at which the gate starts ( see currentTime )
        [SerializeField]
        float startTime = 0f;
        float startTimeScaled;

        bool scaledByTime = false;

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

        float warningTime = 3f;

        Collider coll;
        bool busy = false;

        private void Awake()
        {
            //scaleDefault = gate.transform.localScale.x;

            

            // Get collider
            coll = GetComponent<Collider>();

                      
        }

        // Start is called before the first frame update
        void Start()
        {
            Init();

            StartCoroutine(StartDelayed());
            
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            //if (Input.GetKeyDown(KeyCode.A))
            //{
             
            //    if (closed)
            //    {
            //        Open(false);
            //        closed = false;
            //    }
            //    else
            //    {
            //        Close(false);
            //        closed = true;
            //    }

            //}
            //return;
#endif

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                // Init variables
                Init();

                // Set initial state
                if (closed)
                    Close(true);
                else
                    Open(true);

                // Check time
                CheckState(true);

                // We must stop here
                return;
            }
#endif

            if (!LevelManager.Instance.Running)
                return;

            // Update time
            time += Time.deltaTime;

            // Check gate state
            CheckState(false);
        }

        void Init()
        {
            // Adust time
            if (scaledByTime)
            {
                openLengthScaled = openLength * Constants.DefaultTimeScale;
                closeLengthScaled = closeLength * Constants.DefaultTimeScale;
                startTimeScaled = startTime * Constants.DefaultTimeScale;
                warningTime *= Constants.DefaultTimeScale;
                
            }
            else
            {
                openLengthScaled = openLength;
                closeLengthScaled = closeLength;
                startTimeScaled = startTime;
            }

            time = startTimeScaled;
            

            closed = closeOnStart;
        }

        void Open(bool forced)
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

        void Close(bool forced)
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

        void CheckState(bool forced)
        {

            if (closed)
            {
                // Check warning
                if (time >= closeLengthScaled - WarningSystem.WarningTime)
                    WarningSystem.Instance.Play();

                // Check state
                if (time >= closeLengthScaled)
                {
                    Open(forced);
                    time %= closeLengthScaled;
                }

            }
            else
            {
                // Check warning
                if (time >= openLengthScaled - WarningSystem.WarningTime)
                    WarningSystem.Instance.Play();

                if (time >= openLengthScaled)
                {
                    Close(forced);
                    time %= openLengthScaled;
                }

            }
        }

        IEnumerator StartDelayed()
        {
            yield return new WaitForSeconds(0.5f);
            CheckState(true);
        }


    }

}
