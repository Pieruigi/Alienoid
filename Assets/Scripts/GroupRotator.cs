using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class GroupRotator : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> objects;

        [SerializeField]
        float duration;

        [SerializeField]
        float angle;

        [SerializeField]
        bool inverse;

        [SerializeField]
        float freezeTime;

        int dir = 1;


        private void Awake()
        {
            Initialize();
        }

        // Start is called before the first frame update
        void Start()
        {
            CreateSequence();
        }

        // Update is called once per frame
        void Update()
        {
            if (!LevelManager.Instance.Running)
                return;
        }

        void CreateSequence()
        {
            Sequence seq = DOTween.Sequence();

            // Add to the sequence a time wait if needed
            if (freezeTime > 0)
            {
                seq.PrependInterval(freezeTime);
            }

            // Compute rotation angle
            Vector3 end = transform.eulerAngles;
            end.z = dir * angle;

            // Update direction
            dir *= -1; 

            seq.Append(transform.DORotate(end, duration));
            seq.onComplete += CreateSequence;
            
        }

        void Initialize()
        {
            // Adding objects
            foreach(GameObject obj in objects)
            {
                obj.transform.parent = transform;
            }
            
            // Setting initial values
            Vector3 eulers = transform.eulerAngles;
            eulers.z = inverse ? -angle : angle;
            transform.eulerAngles = eulers;
            dir = inverse ? 1 : -1; // Setting the next value

           
        }
    }

}
