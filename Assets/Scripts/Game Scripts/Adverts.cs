using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Adverts : MonoBehaviour
{
   // set our game ID depending on OS
#if UNITY_IOS
       private readonly string gameID = "3362656";
#elif UNITY_ANDROID
        private readonly string gameID = "3362657";
#endif

    private readonly string BannerId = "banner";
    private readonly string VideoId = "video";

    private readonly bool testMode = true;

    private void Start()
    {
        InitialiseAds();
    }

    private void InitialiseAds()
    {
        Advertisement.Initialize(gameID, testMode);
    }

    public void PlayFullScreenAd()
    {
        if (Advertisement.IsReady(VideoId))
        {
            Advertisement.Show(VideoId);
        }
    }

    public void PlayBannerAd()
    {
        StartCoroutine(ShowBannerWhenReady());
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(BannerId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(BannerId);
    }

    //public void ShowBannerAd()
    //{
    //    BannerLoadOptions options = new BannerLoadOptions { loadCallback = OnLoadBannerSuccess, errorCallback = OnLoadBannerFail };
    //    Advertisement.Banner.Load(BannerId, options);
    //}

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    //IEnumerator RetryBanner()
    //{
    //     yield return new WaitForSeconds(1f);

    //    ShowBannerAd();
    //}

    //private void OnLoadBannerSuccess()
    //{
    //    Debug.Log("OnLoadBannerSuccess");
    //    Advertisement.Banner.Show(BannerId);
    //}

    //private void OnLoadBannerFail(string message)
    //{
    //    Debug.LogError("OnLoadBannerFail reason : " + message);
    //    //retry to show banner maybe if the reason was that the placement was not ready
    //    StartCoroutine(RetryBanner());
    //}
}
