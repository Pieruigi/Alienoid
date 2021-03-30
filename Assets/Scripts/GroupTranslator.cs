using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class GroupTranslator : MonoBehaviour
    {

        [SerializeField]
        List<GameObject> objects;

        [SerializeField]
        float time;

        [SerializeField]
        float freezeTime;

        [SerializeField]
        Vector3 startPosition;

        [SerializeField]
        Vector3 endPosition;

        [SerializeField]
        bool inverse;

        int dir = 1;

        private void Awake()
        {
            Initialization();
        }

        // Start is called before the first frame update
        void Start()
        {
            Loop();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Initialization()
        {
            // Set objects
            foreach (GameObject obj in objects)
                obj.transform.parent = transform;

            // Set the starting position
            transform.position = inverse ? endPosition : startPosition;

            // Set the direction
            dir = inverse ? -1 : 1;
            
        }
        void Loop()
        {
            Vector3 target = dir > 0 ? endPosition : startPosition;
            dir *= -1;
               
            transform.DOMove(target, time).SetDelay(freezeTime).OnComplete(Loop);
            

        }
        
    }

}
