using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public enum AdvertisementType { Admob, Unity };
    string unityGameId = "4042940";
    bool testMode = true;
    public string surfacingId;
    string surfaceReward = "RewardedAdTest";
    string bannerSurfacingId = "Banner_Android";
    private BannerView bannerView;
    private InterstitialAd admobInterstatial;
    private RewardedAd admobRewarded;
    Button rewardedButton;
    private ShopManager myShopManager;
    public AdvertisementType currentType { get; set; }
    int rewardAmount = 0;
    string rewardType;

    void Awake()
    {
        //Get Shop Manager
        myShopManager = GetComponent<ShopManager>();

        //Initialize Admob
        MobileAds.Initialize(initStatus => { });
        
        //Initialize Unity  

        rewardedButton = GetRewardedButton("Rewarded-Button");

        if(rewardedButton != null)
        {
            rewardedButton.interactable = Advertisement.IsReady(surfaceReward);
            rewardedButton.onClick.AddListener(ShowRewardedVideo);
        }

        Advertisement.Initialize(unityGameId, testMode);
        Advertisement.AddListener(this);
        InitialzieBanner();
        string adUnitId = "ca-app-pub-3668225440256580/7694322222";
        admobRewarded = new RewardedAd(adUnitId);

        InitializeAdmobRewardedListeners();

    }

    private void Update()
    {
        if(rewardAmount > 0)
        {
            myShopManager.Gems = rewardAmount;
            print("Rewarded Amount: " + rewardAmount + " Rewarded Type: " + rewardType);
            rewardAmount = 0;
        }
    }

    private void InitializeAdmobRewardedListeners()
    {
        admobRewarded.OnUserEarnedReward += HandleUserEarnedRewards;
        admobRewarded.OnAdLoaded += HandleRewardedAdLoaded;
    }

    public void InitialzieBanner()
    {
        string adUnitId = "ca-app-pub-3668225440256580/2981885741";
        Advertisement.Banner.Hide();

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
        bannerView.Hide();

        ShowBannerWhenInitialized();
    }

    public void DisplayBanner()
    {
        if (currentType == AdvertisementType.Unity)
        {
            if (bannerView != null)
            {
                bannerView.Hide();
            }
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show();
        }
        else
        {
            print("Attempting to Show Admob Banner");
            Advertisement.Banner.Hide();
            bannerView.Show();
        }
        
    }

    public void DisplayInterstatial()
    {
        if (currentType == AdvertisementType.Unity)
        {
            surfacingId = "Interstitial_Android";
            ShowInterstitialAd();
        }
        else
        {
            string adUnitId = "ca-app-pub-3668225440256580/7659497353";
            admobInterstatial = new InterstitialAd(adUnitId);
            AdRequest request = new AdRequest.Builder().Build();
            admobInterstatial.LoadAd(request);

            if(admobInterstatial.IsLoaded())
            {
                admobInterstatial.Show();
            }
        }
    }

    public void DisplayRewarded()
    {
        if (currentType == AdvertisementType.Unity)
        {
            surfacingId = "RewardedAdTest";
        }
        else
        {
            string adUnitId = "ca-app-pub-3668225440256580/7694322222";
            admobRewarded = new RewardedAd(adUnitId);

            AdRequest request = new AdRequest.Builder().Build();
            admobRewarded.LoadAd(request);

            if (admobRewarded.IsLoaded())
            {
                admobRewarded.Show();
            }
        }
    }

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }

    public void ShowRewardedVideo()
    {
        Advertisement.Show("RewardedAdTest");
    }


    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }


    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }


        Advertisement.Banner.Show(bannerSurfacingId);
        Advertisement.Banner.Show();
    }

    public Button GetRewardedButton(string buttonName)
    {
        Button[] buttonList = GetComponentsInChildren<Button>();

        foreach (Button button in buttonList)
        {
            if (button.gameObject.name == buttonName)
            {
                return button;
            }
        }

        return null;
    }

    public void OnUnityAdsReady(string surfacingId)
    {
        if (rewardedButton != null)
        {
            rewardedButton.interactable = true;
        }

    }

    public void OnUnityAdsDidError(string message)
    {
        print(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        print("Ad Initiated: " + placementId);
    }

    public void OnUnityAdsDidFinish(string placementId)
    {
        if (placementId == "RewardedAdTest")
        {
            rewardAmount = 5;
        }

    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {

    }
    
    public void HandleUserEarnedRewards(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        rewardAmount = (int)amount;
        rewardType = type;
    }


}