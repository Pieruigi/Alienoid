using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Zom.Pie
{
    

    public class MusicManager : MonoBehaviour
    {
        [System.Serializable]
        class ClipData
        {
            public AudioClip clip;
            public float volume = 1;
            public bool loop = false;
        }

        public static MusicManager Instance { get; private set; }

        [SerializeField]
        ClipData menuClip;

        [SerializeField]
        List<ClipData> gameClips;

        [SerializeField]
        AudioSource source;


        bool playOnSceneLoaded = false;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                SceneManager.sceneLoaded += HandleOnSceneLoaded;

                // Set the menu clip 
                SetClipData(menuClip);
                

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.OnSceneLoading += HandleOnSceneLoading;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetClip(AudioClip clip)
        {
            source.clip = clip;
        }

        public void SetVolume(float volume)
        {
            source.volume = volume;
        }

        void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (playOnSceneLoaded)
            {
                playOnSceneLoaded = false;

                // Set menu or game clip
                if (GameManager.Instance.IsInGame())
                {
                    SetClipData(gameClips[Random.Range(0, gameClips.Count)]);
                }
                else
                {
                    SetClipData(menuClip);
                }
                source.Play();
            }

        }

        void HandleOnSceneLoading(bool isLevel)
        {
            
            if (isLevel || GameManager.Instance.IsInGame())
            {
                source.Stop();
                playOnSceneLoaded = true;
            }
        }

        void SetClipData(ClipData clipData)
        {
            source.clip = clipData.clip;
            source.volume = clipData.volume;
            source.loop = clipData.loop;
        }
    }

}
