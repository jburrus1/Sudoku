using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdDisplay : MonoBehaviour, IUnityAdsInitializationListener
{
    private string myGameIdAndroid = "5541405";
    private string myGameIdIOS = "5541404";
    private string adUnitIdAndroid;
    private string adUnitIdIOS;
    public string myAdUnitId;
    public bool adStarted;
    public bool adLoading;
    // Start is called before the first frame update
    void Start()
    {
        adUnitIdAndroid = "Banner_Android";
        adUnitIdIOS = "Banner_iOS";
        adStarted = false;
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
#if UNITY_IOS
	        Advertisement.Initialize(myGameIdIOS, false);
	        myAdUnitId = adUnitIdIOS;
#else
        Advertisement.Initialize(myGameIdAndroid, false,this);
        myAdUnitId = adUnitIdAndroid;
#endif
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Ads initialized, now loading");
        var bannerOptions = new BannerLoadOptions();
        bannerOptions.loadCallback = LoadCallback;
        bannerOptions.errorCallback = LoadErrorCallback;
        Advertisement.Banner.Load(myAdUnitId, bannerOptions);
    }

    public void LoadCallback()
    {
        Debug.Log("Ad loaded, now showing");
        Advertisement.Banner.Show(myAdUnitId);
    }
    public void LoadErrorCallback(string error)
    {
        Debug.Log("Load Failed with error: " + error);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        throw new System.NotImplementedException();
    }
}
