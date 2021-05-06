using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Services
{
    public class PlayGamesManager : MonoBehaviour
    {
        public static PlayGamesManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                     .RequestServerAuthCode(false /* Don't force refresh */)
                     .Build();

                PlayGamesPlatform.InitializeInstance(config);
                PlayGamesPlatform.Activate();


            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Social.localUser.Authenticate((bool success) => {
                    Debug.Log("success:" + success);
                });

            }
        }

        public void Connect()
        {
            Social.localUser.Authenticate((bool success) => {
                Debug.Log("success:" + success);
            });
        }
    }

}
