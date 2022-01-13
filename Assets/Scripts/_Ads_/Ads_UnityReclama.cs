using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;


public class Ads_UnityReclama : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private bool testMode_On;
    private bool _enablePerPlacementMode = true;
    private string gameID = "4494527";

    private string _video = "Interstitial_Android";
    private string _rewardedVideo = "Rewarded_Android";
    private string _banner = "Banner_Android";

    public delegate void DelegatsReward(bool Ok);
    public event DelegatsReward event_rewardedVideoIsResult;

    void Start()
    {
        // инициализация рекламы
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, testMode_On);

        Advertisement.Load(_video);
        Advertisement.Load(_rewardedVideo);
    }


    public void ShowSimpleAds()
    {
        ShowAdsVideo(_video);
    }

    public void ShowRewardedAds()
    {
        ShowAdsVideo(_rewardedVideo);
        print("ShowRewardedAds");
    }

    public static void ShowAdsVideo(string placementID)
    {
        if (Advertisement.isInitialized)
        {
            Advertisement.Show(placementID);
        }
        else
        {
            print("notReady");
        }

        Advertisement.Load(placementID);
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
        Time.timeScale = 1;
    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            event_rewardedVideoIsResult?.Invoke(true);
        }

        if (showResult == ShowResult.Skipped)
        {
            event_rewardedVideoIsResult?.Invoke(false);
        }

        Time.timeScale = 1;
    }
}
