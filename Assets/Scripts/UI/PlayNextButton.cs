using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class PlayNextButton : MonoBehaviour
    {

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(HandleOnClick);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnClick()
        {
            
            int higherSpeed = GameProgressManager.Instance.GetHigherUnlockedSpeed();
            int nextLevel = GameProgressManager.Instance.GetLastUnlockedLevel(higherSpeed);

            GameManager.Instance.GameSpeed = higherSpeed;
            GameManager.Instance.LoadLevel(nextLevel);
        }
    }

}
