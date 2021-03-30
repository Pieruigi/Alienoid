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
            Sequence seq = DOTween.Sequence();

            // Add to the sequence a time wait if needed
            if (freezeTime > 0)
            {
                seq.PrependInterval(freezeTime);
            }

            // Compute rotation angle
            Vector3 targetPos = dir > 0 ? startPosition : endPosition;

            // Update direction
            dir *= -1;

            seq.Append(transform.DOMove(endPosition, time).OnComplete(() => transform.DOMove(startPosition, time)).SetLoops(-1));
            //seq.onComplete += CreateSequence;
        }
    }

}
