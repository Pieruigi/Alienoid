using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    [ExecuteInEditMode]
    public class BlackHoleSwitcher : GameplaySwitcher
    {
        [System.Serializable]
        public class Data
        {
            public BlackHole blackHole;
            public List<EnemyType> steps;
            public int currentStep = 0;
        }


        [SerializeField]
        List<Data> dataList;


        protected override void Switch()
        {
            Debug.LogFormat("Switch");
            foreach(Data data in dataList)
            {
                // Update step
                data.currentStep++;
                if (data.currentStep >= data.steps.Count)
                    data.currentStep = 0;

                data.blackHole.SwitchEnemyType(data.steps[data.currentStep]);

                
            }
        }

        protected override void Init()
        {
            foreach (Data data in dataList)
            {
                data.blackHole.SetEnemyType(data.steps[data.currentStep]);
            }
        }
    }

}
