using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTween : MonoBehaviour
{
    
    public float myFloat = 2;

    // Start is called before the first frame update
    void Start()
    {

        TestValue();
        TestMove();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestValue()
    {


        DOTween.To(() => myFloat, x => myFloat = x, 10, 2).SetEase(Ease.InOutElastic);

    }

    void TestMove()
    {
        transform.DOMove(transform.position + transform.right * 13, 5).SetEase(Ease.OutQuint).SetEase(Ease.OutBounce);
    }
}
