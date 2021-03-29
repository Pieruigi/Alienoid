using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    [ExecuteInEditMode]
    public class BlackHoleSwitcher : MonoBehaviour
    {
        [System.Serializable]
        public class Data
        {
            public BlackHole blackHole;
            public List<EnemyType> steps;
            public int currentStep = 0;
        }

        [SerializeField]
        float switchTime = 0;

        [SerializeField]
        float startingTime = 0;

        [SerializeField]
        List<Data> dataList;

        float elapsed = 0;
        bool warning = false;

        private void Awake()
        {
            elapsed = startingTime;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Initialize
            Init();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Init();
                return;
            }
#endif

            if (!LevelManager.Instance.Running)
                return;

            // Update elapsed time
            elapsed += Time.deltaTime;

            // Check the warning time
            if(switchTime - elapsed <= 3.0f && !warning)
            {
                warning = true;
                WarningSystem.Instance.Play();
            }
                

            if(elapsed >= switchTime)
            {
                // Switch
                Switch();

                // Reset elapsed
                elapsed %= switchTime;
                warning = false;
                Debug.LogFormat("Elapsed: {0}", elapsed);
            }   
        }

        void Switch()
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

        void Init()
        {
            foreach (Data data in dataList)
            {
                data.blackHole.SetEnemyType(data.steps[data.currentStep]);
            }
        }
    }

}
