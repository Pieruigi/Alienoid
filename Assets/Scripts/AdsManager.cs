using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Ads
{
    public class AdsManager : MonoBehaviour
    {
        public static AdsManager Instance { get; private set; }
        
        bool initialized = false;

        InterstitialAd interstitial = null;

        DateTime lastInterstitialLoadTime;
        bool interstitialLoading = false;
        float loadTime = 1f;


        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            MobileAds.Initialize(initStatus => 
            { 
                initialized = true; 

                // Preload the interstitial
                //LoadInterstitial(); 
            });
        }

        // Update is called once per frame
        void Update()
        {
            if((DateTime.UtcNow - lastInterstitialLoadTime).TotalSeconds > loadTime)
            {
                // We try to load interstitial if needed in order to avoid the player to skip the ads
                // ( for example due to connection issue ); all checks are performed inside the
                // TryToLoadInterstitial() method.
                TryLoadInterstitial();
            }
        }

        /// <summary>
        /// Called by the app to show a preloaded interstitial
        /// </summary>
        public void ShowInterstitial()
        {
            if (interstitial != null && interstitial.IsLoaded())
                interstitial.Show();
        }

        #region internal
        void TryLoadInterstitial()
        {
            if (interstitialLoading)
                return;

            if (interstitial != null && interstitial.IsLoaded())
                return;

            interstitialLoading = true;

            // Production
            // Interstitial: ca-app-pub-3894593653653304/3660117564

            // Test
            // Interstitial: ca-app-pub-3940256099942544/1033173712

#if UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/1033173712"; 
#elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
                string adUnitId = "unexpected_platform";
#endif

            // Initialize an InterstitialAd.
            interstitial = new InterstitialAd(adUnitId);

            // Setting handles
            interstitial.OnAdLoaded += HandleOnInterstitialLoaded;
            interstitial.OnAdFailedToLoad += HandleOnInterstitialFailedToLoad;
            interstitial.OnAdOpening += HandleOnInterstitialOpening;
            interstitial.OnAdClosed += HandleOnInterstitialClosed;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            interstitial.LoadAd(request);

        }

        #endregion

        #region callbacks
        /// <summary>
        /// Interstitial loading success callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void HandleOnInterstitialLoaded(object sender, EventArgs args)
        {
            Debug.LogFormat("Interstitial loaded");

            interstitialLoading = false;
            lastInterstitialLoadTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Interstitial loading failed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void HandleOnInterstitialFailedToLoad(object sender, EventArgs args)
        {
            Debug.LogFormat("Interstitial failed to load");

            interstitialLoading = false;
            lastInterstitialLoadTime = DateTime.UtcNow;
        }

        void HandleOnInterstitialOpening(object sender, EventArgs args)
        {
            Debug.LogFormat("Interstitial opening");

        }

        void HandleOnInterstitialClosed(object sender, EventArgs args)
        {
            Debug.LogFormat("Interstitial closed");

        }
        #endregion
    }

}
