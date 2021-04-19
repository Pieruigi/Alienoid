using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Ads
{
    public class AdsManager : MonoBehaviour
    {
        bool initialized = false;

        InterstitialAd interstitial = null;

        // Start is called before the first frame update
        void Start()
        {
            MobileAds.Initialize(initStatus => 
            { 
                initialized = true; 

                // Preload the interstitial
                LoadInterstitial(); 
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Called by the app to show a preloaded interstitial
        /// </summary>
        public void ShowInterstitial()
        {
            if (interstitial.IsLoaded())
                interstitial.Show();
        }

        void LoadInterstitial()
        {
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
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            interstitial.LoadAd(request);

        }
    }

}
