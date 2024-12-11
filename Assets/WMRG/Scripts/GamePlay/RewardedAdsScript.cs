//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Advertisements;






using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
//using GoogleMobileAds.Api;

using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
//using OnefallGames;
using UnityEngine.UI;
using UnityEngine.Networking;


using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using System.Net;
using UnityEngine.Networking;

public class GetLink
{
    public static GetLink instance;
    void Awake()
    {
        if (instance == null)
        {
            //If I am the first instance, make me the Singleton
            instance = this;

        }
    }
    public string playstore_link { get; set; }
    public string apple_link { get; set; }



}

public class GetLinkHeader
{
    public string error { get; set; }
    public string message { get; set; }
    public GetLink[] result { get; set; }
}
// public class RewardedAdsScript : MonoBehaviour
// {
//     public static RewardedAdsScript _instance;
//       ///  private string gameId;
//
//
// #if UNITY_ANDROID
//     // string appId = "ca-app-pub-3940256099942544/6300978111";
//     private string gameId = "3574305";
//
// #elif UNITY_IPHONE
//             //string appId = "ca-app-pub-3940256099942544~1458002511";
//              private string gameId = "3574304";
// #else
//     //string appId = "unexpected_platform";
// #endif
//
//
//
//     //private string gameId = "3534358";
//     private string  RewardVideo_ID = "rewardedVideo";
//         private  string Banner_ID = "Banner";
//
//     // bool testMode = true;
//
//
//     void Awake()
//         {
//             //if (_instance == null)
//             //{
//             //    _instance = this;
//             //}
//
//
//
//         if (_instance == null)
//         {
//             _instance = this; // In first scene, make us the singleton.
//             DontDestroyOnLoad(this);
//         }
//         else if (_instance != this)
//             Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
//     }
//
//
//
//
//
//     // Initialize the Ads listener and service:
//     void Start()
//     {
//
//         //Loading.SetActive(false);
//
//
//         Advertisement.AddListener(this);
//         Advertisement.Initialize(gameId);
//          //StartCoroutine(ShowBannerWhenReady());
//
//
//     }
//
//
//
//
//
//     IEnumerator ShowBannerWhenReady()
//     {
//         while (!Advertisement.IsReady(Banner_ID))
//         {
//             yield return new WaitForSeconds(0.5f);
//         }
//         Advertisement.Banner.Show(Banner_ID);
//     }
//
//
//     public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
//     {
//         // Define conditional logic for each ad completion status:
//         if (showResult == ShowResult.Finished)
//         {
//             // Reward the user for watching the ad to completion.
//         }
//         else if (showResult == ShowResult.Skipped)
//         {
//             // Do not reward the user for skipping the ad.
//         }
//         else if (showResult == ShowResult.Failed)
//         {
//             Debug.LogWarning("The ad did not finish due to an error.");
//         }
//     }
//
//
//     public void ShowRewardedVideo()
//     {
//
//         Advertisement.Show(RewardVideo_ID);
//
//
//
//     //    Invoke("Start", 2);
//     }
//     public void OnUnityAdsReady(string placementId)
//     {
//         //// If the ready Placement is rewarded, show the ad:
//         //if (placementId == myPlacementId)
//         //{
//         //    Advertisement.Show(myPlacementId);
//         //}
//     }
//
//     public void OnUnityAdsDidError(string message)
//     {
//         // Log the error.
//     }
//
//     public void OnUnityAdsDidStart(string placementId)
//     {
//         // Optional actions to take when the end-users triggers an ad.
//     }
//
//
//
//
//    
// }