using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public abstract class GameplaySwitcher : MonoBehaviour
    {
        [SerializeField]
        float switchTime = 0;

        [SerializeField]
        float startingTime = 0;

        float elapsed = 0;
        bool warning = false;

        protected abstract void Init();

        protected abstract void Switch();

        protected virtual void Awake()
        {
            elapsed = startingTime;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            Init();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Init();
                return;
            }
#endif
            if (LevelManager.Instance && !LevelManager.Instance.Running)
                return;

            // Update elapsed time
            elapsed += Time.deltaTime;

            // Check the warning time
            if (switchTime - elapsed <= 3.0f && !warning)
            {
                warning = true;
                WarningSystem.Instance?.Play();
            }


            if (elapsed >= switchTime)
            {
                // Switch
                Switch();

                // Reset elapsed
                elapsed %= switchTime;
                warning = false;
                Debug.LogFormat("Elapsed: {0}", elapsed);
            }
        }
    }

}
