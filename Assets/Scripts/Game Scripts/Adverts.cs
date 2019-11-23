using System.Collections;
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
    //banner id's as defined in the unity dashboard 
    private readonly string BannerId = "banner";
    private readonly string VideoId = "video";

    //set testmode for ads
    private readonly bool testMode = false;

    private void Start()
    {
        InitialiseAds();
    }

    //initialise advertisments
    private void InitialiseAds()
    {
        Advertisement.Initialize(gameID, testMode);
    }

    //play full screen advert
    public void PlayFullScreenAd()
    {
        if (Advertisement.IsReady(VideoId))
        {
            Advertisement.Show(VideoId);
        }
    }

    //show banner advert
    public void PlayBannerAd()
    {
       StartCoroutine(ShowBannerWhenReady());
    }


    IEnumerator ShowBannerWhenReady()
    {
        //check for banner ad ready and load
        while (!Advertisement.IsReady(BannerId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(BannerId);
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }
}
