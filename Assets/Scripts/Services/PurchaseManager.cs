using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Services
{
    public class PurchaseManager : MonoBehaviour
    {
        public static string PlayerPrefsPremiumVersionKey = "Premium";

        public static PurchaseManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
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

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool IsPremiumVersion()
        {
            return PlayerPrefs.HasKey(PlayerPrefsPremiumVersionKey);
        }
    }

}
