using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.UI
{
    public class Level : MonoBehaviour
    {
        
        [SerializeField]
        GameObject padlock;

        int levelId;
        public int LevelId
        {
            get { return levelId; }
        }
        int speed;
        public int MaxBeatenSpeed
        {
            get { return speed; }
        }

        bool selected;

        float selectionScale = 1.16f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(int levelId)
        {
            // Set the current level id
            this.levelId = levelId;

            // Get the max speed the level has been beaten
            speed = GameProgressManager.Instance.GetMaxBeatenSpeed(levelId);

            if (speed > 0)
            {
                // If speed > 0 the level is unlocked for sure
                // so we hide the padlock...
                padlock.SetActive(false);
                //... and we update the speed panel
                UpdateSpeedPanel();
            }
            else
            {
                // Speed = 0 so level has not beaten yet, but it could still be unlocked if the previous
                // one has been beaten
                if(levelId == GameProgressManager.Instance.GetLastUnlockedLevel())
                {
                    // Unlock 
                    padlock.SetActive(false);

                    // Update the speed panel
                    UpdateSpeedPanel();
                }
            }
            
        }

        public void Select(bool value)
        {
            // Already in the given state
            if (selected == value)
                return;

            // Set flag
            selected = value;

            if (selected)
            {
                // Scale up
                transform.DOScale(selectionScale, 0.25f).SetEase(Ease.OutElastic);
            }
            else
            {
                // Scale down
                transform.DOScale(1, 0.25f).SetEase(Ease.OutElastic);
            }
        }

        void UpdateSpeedPanel()
        {

        }
    }

}
