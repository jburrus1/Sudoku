using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdDisplay : MonoBehaviour
{
    public string myGameIdAndroid = "5541405";
    public string myGameIdIOS = "5541404";
    public string adUnitIdAndroid = "Banner_Android";
    public string adUnitIdIOS = "Banner_iOS";
    public string myAdUnitId; 
    public bool adStarted;
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
#if UNITY_IOS
	        Advertisement.Initialize(myGameIdIOS, false);
	        myAdUnitId = adUnitIdIOS;
#else
        Advertisement.Initialize(myGameIdAndroid, false);
        myAdUnitId = adUnitIdAndroid;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Advertisement.isInitialized && !adStarted)
        {
            Advertisement.Banner.Load(myAdUnitId);
            Advertisement.Banner.Show(myAdUnitId);
            adStarted = true;
        }

        if (Advertisement.Banner.isLoaded)
        {
            
            GetComponent<UnityEngine.UI.Image>().enabled = false;
        }
        else
        {
            
            GetComponent<UnityEngine.UI.Image>().enabled = true;
        }
    }
}
