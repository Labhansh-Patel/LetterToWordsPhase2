using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsShowListener, IUnityAdsInitializationListener, IUnityAdsLoadListener
{
#if UNITY_IOS
   string gameID = "5738447";
   string rewarded = "Rewarded_iOS";
   string interstitial = "Interstitial_iOS";
   string banner = "Banner_iOS";
#elif UNITY_ANDROID
    string gameID = "5738446";
    private string rewarded = "Rewarded_Android";
    private string interstitial = "Interstitial_Android";
    private string banner = "Banner_Android";
#endif
    private bool interstitial_loaded = false;
    public Action onAdFinised;
    public static AdsManager Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance.gameObject);
        }

        Advertisement.Initialize(gameID, false, this);

        //PlayAdBanner(); //use this for adbanner
        InAppPurchaseManager.buyGold += TheBuyingGold;
    }

    /// <summary>
    /// Call this function to show the banner Ad
    /// </summary>
    // public void PlayAdBanner()
    // {
    //     Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    //     Advertisement.Banner.Show(banner);
    // }
    //
    // public void HideBanner()
    // {
    //     Advertisement.Banner.Hide();
    // }
    public void PlayAdInterstitial()
    {
        if (GlobalData.userData.IsPremiumUser)
        {
            return;
        }

        Debug.Log("Ads Play");
        Advertisement.Show(interstitial, this);

        // StartCoroutine(LoadAndPlayInterstitial());
    }

    IEnumerator LoadAndPlayInterstitial()
    {
        interstitial_loaded = false;
        //   Advertisement.Load(interstitial, this);
        yield return new WaitUntil(() => interstitial_loaded == true);
        Advertisement.Show(interstitial, this);
    }

    /// <summary>
    /// Call this function to Play the Rewarding AD
    /// </summary>
    public void PlayAdRewarded()
    {
        Advertisement.Show(rewarded, this);
        Advertisement.Load(rewarded, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    /// <summary>
    /// This is a callback after rewarding Advertisement is completed
    /// </summary>
    /// <param name="placementId"></param>
    /// <param name="showCompletionState"></param>
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        onAdFinised?.Invoke();
        Debug.Log("OnUnityAdsShowComplete");
        Advertisement.Load(placementId, this);
    }

    //---------Testing In App Purchasing
    void TheBuyingGold(double gold)
    {
        Debug.LogError("The Amount of Gold is: " + gold);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Loaded The Advertisement");
        Advertisement.Load(interstitial, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        // Debug.LogError("Ads Got loaded--->" + placementId);
        interstitial_loaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
    }
}