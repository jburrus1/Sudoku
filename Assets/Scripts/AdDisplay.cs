using System;
using System.Collections;
using System.Collections.Generic;
using Board;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdDisplay : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string myGameIdAndroid = "5541405";
    private string myGameIdIOS = "5541404";
    private string bannerAndroid;
    private string bannerIOS;
    private string interstitialAndroid;
    private string interstitialIOS;
    public string bannerID;
    public string interstitialID;
    public bool adStarted;
    public bool adLoading;


    [SerializeField] private Timer _timer;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        bannerAndroid = "Banner_Android";
        bannerIOS = "Banner_iOS";
        interstitialAndroid = "Interstitial_Android";
        interstitialIOS = "Interstitial_iOS";
        adStarted = false;
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
#if UNITY_IOS
	        Advertisement.Initialize(myGameIdIOS, false);
	        bannerID = bannerIOS;
            interstitialID = interstitialIOS;
#else
        bannerID = bannerAndroid;
        interstitialID = interstitialAndroid;
        if (Advertisement.isInitialized)
        {
            OnInitializationComplete();
        }
        else
        {
            Advertisement.Initialize(myGameIdAndroid, false,this);
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnInitializationComplete()
    {
        if (GameManager.Instance.UserData.IncrementAdCounterAndServe())
        {
            ServeInterstitial();
        }
        else
        {
            ServeBanner();
        }
        Debug.Log("Ads initialized, now loading");
    }

    public void ServeInterstitial()
    {
        Advertisement.Load(interstitialID, this);
    }

    public void ServeBanner()
    {
        _timer.AdDone();
        var bannerOptions = new BannerLoadOptions();
        bannerOptions.loadCallback = LoadBannerCallback;
        bannerOptions.errorCallback = LoadErrorCallback;
        Advertisement.Banner.Load(bannerID, bannerOptions);
    }

    public void LoadBannerCallback()
    {
        Debug.Log("Ad loaded, now showing");
        Advertisement.Banner.Show(bannerID);
    }
    public void LoadErrorCallback(string error)
    {
        _timer.AdDone();
        Debug.Log("Load Failed with error: " + error);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        _timer.AdDone();
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Advertisement.Show(placementId,this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        _timer.AdDone();
        Debug.Log("Load Failed with error: " + error);
    }

    private void OnDestroy()
    {
        Advertisement.Banner.Hide();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        _timer.AdDone();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        ServeBanner();
    }
}
