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
            
            

            GameManager.Instance.GameSpeed = GameProgressManager.Instance.Speed;
            GameManager.Instance.LevelId = GameProgressManager.Instance.LevelId;

            GameManager.Instance.LoadLevel();

            //GameManager.Instance.GameSpeed = higherSpeed;
            //GameManager.Instance.LoadLevel(nextLevel);
        }
    }

}
