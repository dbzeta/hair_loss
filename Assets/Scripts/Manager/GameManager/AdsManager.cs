/*using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Advertisements;


public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    public bool IsRealApp
    {
        get
        {
            if (Debug.isDebugBuild)
                return false;
            else
                return true;
        }
    }

    #region AD-Mob
    readonly string android_adUnitId = "ca-app-pub-3940256099942544/5224354917";
    readonly string android_tsst_adUnitId = "ca-app-pub-3940256099942544/5224354917";
    readonly string ios_tsst_adUnitId = "ca-app-pub-3940256099942544/1712485313";
    readonly string tsst_adUnitId = "unexpected_platform";

    string adUnitId
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return IsRealApp ? android_adUnitId : android_tsst_adUnitId;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return ios_tsst_adUnitId;
            }

            return tsst_adUnitId;
        }
    }
    private RewardedAd rewardedAd;
    private RewardedAd gameOverRewardedAd;
    private RewardedAd extraCoinsRewardedAd;
    #endregion

    #region UnityAds
    readonly string android_game_id = "4917001"; //optional, we will autofetch the gameID if the project is linked in the dashboard
    readonly string ios_game_id = "4917000";

    [SerializeField] bool _testMode = true;
    string _adUnitId = string.Empty;

    readonly string unity_ads_rewarded_video_id = "Rewarded_Android";
    // readonly string unity_ads_rewarded_video_id = "rewardedVideo";
    #endregion


    public Action OnLoadAdComplete = null;
    public Action<bool> OnRewardAdShowComplete = null;
    bool bIsLoadedUnityAds = false;
    public bool IsLoadedUnityAds { get { return bIsLoadedUnityAds; } }

    bool rewarded = false;

    public void Init()
    {*//*
        InitializeAdMob();
        InitializeUnityAds();*//*
    }

    public void Start()
    {
        InitializeAdMob();
        InitializeUnityAds();
    }

    public void InitializeAdMob()
    {
        MobileAds.Initialize(initStatus => { });

        this.gameOverRewardedAd = CreateAndLoadRewardedAd(adUnitId);
        this.extraCoinsRewardedAd = CreateAndLoadRewardedAd(adUnitId);

        this.CreateAndLoadRewardedAd(adUnitId);
    }

    public RewardedAd CreateAndLoadRewardedAd(string _adUnitId)
    {
        this.rewardedAd = new RewardedAd(_adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        return rewardedAd;
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }
    
    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
            MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.LoadAdError.GetMessage());
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);

        if (OnRewardAdShowComplete != null)
            OnRewardAdShowComplete(false);
        OnRewardAdShowComplete = null;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        this.CreateAndLoadRewardedAd(adUnitId);
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);

        rewarded = true;
    }


    public void InitializeUnityAds()
    {
        string _gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? ios_game_id : android_game_id;
        _testMode = !IsRealApp;
        _adUnitId = unity_ads_rewarded_video_id;
        Advertisement.Initialize(_gameId, _testMode, this);
    }
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        if (Advertisement.isInitialized)
        {
            LoadAd();
        }
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }


    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            // _showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            // _showAdButton.interactable = true;

            bIsLoadedUnityAds = true;
            if (OnLoadAdComplete != null)
                OnLoadAdComplete();
            OnLoadAdComplete = null;
        }
        else
        {
            bIsLoadedUnityAds = false;
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        // _showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        // if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        // {
        //     Debug.Log("Unity Ads Rewarded Ad Completed");
        //     // Grant a reward.
        // 
        //     // Load another ad:
        //     Advertisement.Load(_adUnitId, this);
        // }

        switch (showCompletionState)
        {
            // 광고가 완전히 재생되었음을 나타내는 상태입니다.
            case UnityAdsShowCompletionState.COMPLETED:
                {
                    if (adUnitId.Equals(_adUnitId))
                    {
                        Debug.Log("Unity Ads Rewarded Ad Completed");
                        // Grant a reward.
                        rewarded = true;

                        // Load another ad:
                        LoadAd();
                    }
                    break;
                }
            // 사용자가 광고를 건너뛰었음을 나타내는 상태입니다.
            case UnityAdsShowCompletionState.SKIPPED:
                if (OnRewardAdShowComplete != null)
                    OnRewardAdShowComplete(false);
                OnRewardAdShowComplete = null;
                break;
            // 기본값 / 사용 가능한 매핑이 없을 때 사용
            case UnityAdsShowCompletionState.UNKNOWN:
                if (OnRewardAdShowComplete != null)
                    OnRewardAdShowComplete(false);
                OnRewardAdShowComplete = null;
                break;
            default:
                break;
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        bIsLoadedUnityAds = false;
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
        // if (this.OnReceiveReward != null)
        //     OnReceiveReward(false);
        // OnReceiveReward = null;
    }
    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log(adUnitId);
    }
    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log(adUnitId);
    }



    public void ShowRewardedAd(Action<bool> OnShowComplete)
    {
        if (bIsLoadedUnityAds)
        {
            this.OnRewardAdShowComplete += OnShowComplete;
            ShowAd();
        }
        else
        {
            OnLoadAdComplete += () =>
            {
                this.OnRewardAdShowComplete += OnShowComplete;
                ShowAd();
            };
            LoadAd();
        }
    }

    public void ShowRewardedAd()
    {
        if (this.rewardedAd == null)
            CreateAndLoadRewardedAd(adUnitId);

        if (this.rewardedAd != null)
        {
            if (this.rewardedAd.IsLoaded())
                this.rewardedAd.Show();
            else
            {
                Debug.Log("ADMOB - this.rewardedAd.IsLoaded = false");
                ShowUnityAdsRewardedAd();
            }
        }
        else
        {
            ShowUnityAdsRewardedAd();
        }
    }

    private void ShowUnityAdsRewardedAd()
    {
        if (bIsLoadedUnityAds)
        {
            // this.OnRewardAdShowComplete += OnShowComplete;
            ShowAd();
        }
        else
        {
            OnLoadAdComplete += () =>
            {
                // this.OnRewardAdShowComplete += OnShowComplete;
                ShowAd();
            };
            LoadAd();
        }
    }

    public bool IsLoadAds()
    {
        if (rewardedAd == null && IsLoadedUnityAds == false)
            return false;
        else if (rewardedAd != null && rewardedAd.IsLoaded() == false && IsLoadedUnityAds == false)
            return false;

        return true;
    }


    void OnDestroy()
    {
        // Clean up the button listeners:
        // _showAdButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        if (rewarded)
        {
            if (OnRewardAdShowComplete != null)
                OnRewardAdShowComplete(true);
            OnRewardAdShowComplete = null;
            rewarded = false;
        }
    }

}*/