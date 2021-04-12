using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zom.Pie.Collections;

namespace Zom.Pie.UI
{
    public class SpeedButton : MonoBehaviour
    {
        [SerializeField]
        TMP_Text label;

        int[] labelIds = new int[] { 5, 6, 7, 8, 12 };

        int speed = 1;
        int speedCount = 3;

        int selectedLevelId = 0;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(HandleOnClick);
 
        }

        // Start is called before the first frame update
        void Start()
        {
            // Set button speed
            speed = LevelMenu.Instance.SelectedSpeed;
            label.text = TextFactory.Instance.GetText(TextFactory.Type.UILabel, labelIds[speed - 1]);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnClick()
        {
            speed++;
            if (speed > Constants.MaxLevelSpeed)
                speed = 1;

            label.text = TextFactory.Instance.GetText(TextFactory.Type.UILabel, labelIds[speed - 1]);

            LevelMenu.Instance.SetSpeed(speed);


        }


    }

}
