using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class EnemyCounter : MonoBehaviour
    {
        [SerializeField]
        GameObject green;

        [SerializeField]
        GameObject yellow;

        [SerializeField]
        GameObject red;

        TMP_Text greenText, yellowText, redText;

        int greenCount, yellowCount, redCount;

        private void Awake()
        {
           

            // Set the handle
            LevelManager.Instance.OnEnemyRemoved += HandleOnEnemyRemoved;
        }

        // Start is called before the first frame update
        void Start()
        {
            greenCount = LevelManager.Instance.GreenCount;
            yellowCount = LevelManager.Instance.YellowCount;
            redCount = LevelManager.Instance.RedCount;

            if (greenCount == 0)
                green.SetActive(false);
            if (yellowCount == 0)
                yellow.SetActive(false);
            if (redCount == 0)
                red.SetActive(false);

            greenText = green.GetComponentInChildren<TMP_Text>();
            yellowText = yellow.GetComponentInChildren<TMP_Text>();
            redText = red.GetComponentInChildren<TMP_Text>();

            greenText.text = greenCount.ToString();
            yellowText.text = yellowCount.ToString();
            redText.text = redCount.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnEnemyRemoved(Enemy enemy)
        {
            if(enemy.Type == EnemyType.Green)
            {
                greenCount--;
                greenText.text = greenCount.ToString();
            }
            if (enemy.Type == EnemyType.Yellow)
            {
                yellowCount--;
                yellowText.text = yellowCount.ToString();
            }
            if (enemy.Type == EnemyType.Red)
            {
                redCount--;
                redText.text = redCount.ToString();
            }
        }
    }

}
