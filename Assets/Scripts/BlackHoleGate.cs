using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Zom.Pie
{
    [ExecuteInEditMode]
    public class BlackHoleGate : MonoBehaviour
    {
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
        float time = 0;

        float scaleDefault = 0;

        float zOpenPosition = -1.013f;
        float zClosedPosition = 0.1f;
        
        float warningTime = 3f;

        Collider coll;
        bool busy = false;

        private void Awake()
        {
            Init();

            scaleDefault = gate.transform.localScale.x;

            // Get collider
            coll = GetComponent<Collider>();

                      
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(StartDelayed());
            
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (busy)
                    return;
                //Open();
                if (closed)
                {
                    Open(false);
                    //closed = false;
                }
                else
                {
                    Close(false);
                    //closed = true;
                }

            }
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
                openLengthScaled = openLength * Constants.TimeScaleDefault;
                closeLengthScaled = closeLength * Constants.TimeScaleDefault;
                startTimeScaled = startTime * Constants.TimeScaleDefault;
                warningTime *= Constants.TimeScaleDefault;
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
                busy = true;
                //Sequence seq = DOTween.Sequence();
                //seq.OnComplete(HandleOnOpened);

                //seq.Append(leftGate.transform.DOShakeRotation(warningTime, Vector3.up * 15f, 10, 90, false));
                //seq.Join(rightGate.transform.DOShakeRotation(warningTime, Vector3.up * 15f, 10, 90, false));
                //seq.Append(leftGate.transform.DOLocalRotate(Vector3.zero, 0.2f, RotateMode.Fast));
                //seq.Join(rightGate.transform.DOLocalRotate(Vector3.zero, 0.2f, RotateMode.Fast));
                //seq.Append(leftGate.transform.DOLocalMoveZ(zOpenPosition, 0.2f, false).SetEase(Ease.OutElastic));
                //seq.Join(rightGate.transform.DOLocalMoveZ(zOpenPosition, 0.2f, false).SetEase(Ease.OutElastic));

                // Open
                coll.enabled = false;
                closed = false;
                gate.transform.DOScale(0, 0.25f); 
            }
            else
            {
                coll.enabled = false;
                closed = false;
                Vector3 pos = leftGate.transform.localPosition;
                pos.z = zOpenPosition;
                leftGate.transform.localPosition = pos;
                pos = rightGate.transform.localPosition;
                pos.z = zOpenPosition;
                rightGate.transform.localPosition = pos;
                leftGate.transform.localEulerAngles = Vector3.zero;
                rightGate.transform.localEulerAngles = Vector3.zero;
            }
            
            

        }

        void Close(bool forced)
        {
            if (!forced)
            {
                //busy = true;
                //Sequence seq = DOTween.Sequence();
                //seq.OnComplete(HandleOnClosed);

                //seq.Append(leftGate.transform.DOShakePosition(warningTime, Vector3.forward * 0.2f, 10, 90, false, false));
                //seq.Join(rightGate.transform.DOShakePosition(warningTime, Vector3.forward * 0.2f, 10, 90, false, false));
                //seq.Append(leftGate.transform.DOLocalMoveZ(zClosedPosition, 0.2f, false));
                //seq.Join(rightGate.transform.DOLocalMoveZ(zClosedPosition, 0.2f, false));
                //seq.Append(leftGate.transform.DOLocalRotate(Vector3.up * 90.0f, 0.2f, RotateMode.Fast).SetEase(Ease.OutElastic));
                //seq.Join(rightGate.transform.DOLocalRotate(Vector3.up * -90.0f, 0.2f, RotateMode.Fast).SetEase(Ease.OutElastic));

                coll.enabled = true;
                closed = true;
                gate.transform.DOScale(scaleDefault, 0.25f);
            }
            else
            {
                coll.enabled = true;
                closed = true;
                Vector3 pos = leftGate.transform.localPosition;
                pos.z = zClosedPosition;
                leftGate.transform.localPosition = pos;
                pos = rightGate.transform.localPosition;
                pos.z = zClosedPosition;
                rightGate.transform.localPosition = pos;
                leftGate.transform.localEulerAngles = Vector3.up * 90f;
                rightGate.transform.localEulerAngles = Vector3.up * -90f;
            }

        }

        void CheckState(bool forced)
        {
            // The gate is already opening or closing
            if (busy)
                return;

            Debug.Log("CheckingState - closed:" + closed);
            if (closed)
            {
                if (time >= closeLengthScaled)
                {
                    Open(forced);
                    time %= closeLengthScaled;
                }

            }
            else
            {
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

        void HandleOnClosed()
        {
            coll.enabled = true;
            closed = true;
            busy = false;
        }

        void HandleOnOpened()
        {
            coll.enabled = false;
            closed = false;
            busy = false;
        }
    }

}
