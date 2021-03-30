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
            Loop();
        }

        // Update is called once per frame
        void Update()
        {
            if (!LevelManager.Instance.Running)
                return;
        }

        void Loop()
        {
            // Compute rotation angle
            Vector3 target = transform.eulerAngles;
            target.z = dir * angle;

            dir *= -1;

            // Rotate
            transform.DORotate(target, duration).SetDelay(freezeTime).OnComplete(Loop);
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
