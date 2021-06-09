using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class ButtonEffect : MonoBehaviour
    {
        bool clicking = false;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ClickEffect);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void ClickEffect()
        {
            if (clicking)
                return;

            clicking = true;

            StartCoroutine(ClickEffectInternal());
        }

        IEnumerator ClickEffectInternal()
        {
            float time = Constants.ButtonOnClickEffectTime / 2f;
            Vector3 scale = transform.localScale;
            yield return transform.DOScale(scale*1.2f, time).WaitForCompletion();
            yield return transform.DOScale(scale, time).WaitForCompletion();
            //yield return new WaitForSeconds(Constants.ButtonOnClickEffectTime);
            clicking = false;
        }

    }

}
