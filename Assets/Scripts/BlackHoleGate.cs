using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class BlackHoleGate : MonoBehaviour
    {
        // How long the gate is closed ( open the gate after t seconds )
        [SerializeField]
        float openTime = 10f;

        // How long the gate is open ( close the gate after t seconds )
        [SerializeField]
        float closeTime = 5;

        // Must the gate be closed on level start?
        [SerializeField]
        bool closeOnStart = false;

        // The time at which the gate starts ( see currentTime )
        [SerializeField]
        float startTime = 0f;

        [SerializeField]
        GameObject gate;

        bool closed = false;

        float totalTime;

        // The normalized time ( currentTime % ( openTime + closeTime ) )
        float normalizedTime;

        Collider coll;

        private void Awake()
        {
            // Adust time
            openTime *= Constants.TimeScaleDefault;
            closeTime *= Constants.TimeScaleDefault;
            startTime *= Constants.TimeScaleDefault;

            // The total time
            totalTime = openTime + closeTime;

            // Set the current time
            normalizedTime = startTime % totalTime;

            closed = closeOnStart;

            // Get collider
            coll = GetComponent<Collider>();

            CheckState();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!LevelManager.Instance.Running)
                return;

            // Update time
            normalizedTime += Time.deltaTime;
            normalizedTime %= totalTime;

            // Check gate state
            CheckState();
        }

        void Open()
        {
            gate.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutBounce);
            coll.enabled = false;
        }

        void Close()
        {
            gate.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBounce);
            coll.enabled = true;
        }

        void CheckState()
        {
            if (closed)
            {
                if (normalizedTime >= openTime)
                {
                    Open();
                }
                else
                {
                    Close();
                }
            }
            else
            {
                if (normalizedTime >= closeTime)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
        }
    }

}
