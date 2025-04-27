using Unity.Services.LevelPlay;
using UnityEngine;

public class MyAppStart : MonoBehaviour
{
    static string uniqueUserId = "demoUserUnity";
    LevelPlayBannerAd bannerAd;
    LevelPlayBannerAd bannerAdCustom;

#if UNITY_ANDROID
	string appKey = "215484ed5";
	string bannerAdUnitId = "7jgda50jqs3y68y4";
#elif UNITY_IPHONE
    string appKey = "8545d445";
    string bannerAdUnitId = "iep3rxsyp9na3rw8";
#else
    readonly string appKey = "unexpected_platform";
    readonly string bannerAdUnitId = "unexpected_platform";
    string interstitialAdUnitId = "unexpected_platform";
#endif

    void Awake()
    {
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject contextCompat = new AndroidJavaClass("androidx.core.content.ContextCompat");
        AndroidJavaObject permissionChecker = new AndroidJavaClass("android.content.pm.PackageManager");

        string permission = "android.permission.ACCESS_NETWORK_STATE";
        int result = contextCompat.CallStatic<int>("checkSelfPermission", activity, permission);
        Debug.Log("Permission check result: " + result);
        Debug.Log("unity-script: Awake called");

        //Dynamic config example
        IronSourceConfig.Instance.setClientSideCallbacks(true);

        var id = IronSource.Agent.getAdvertiserId();
        Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);

        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("unity-script: unity version" + IronSource.unityVersion());

        // SDK init
        Debug.Log("unity-script: LevelPlay Init");
        IronSource.Agent.setMetaData("is_test_suite", "enable");

        LevelPlay.Init(appKey, uniqueUserId, new[] { com.unity3d.mediation.LevelPlayAdFormat.BANNER });

        LevelPlay.OnInitSuccess += OnInitializationCompleted;
        LevelPlay.OnInitFailed += error => Debug.Log("Initialization error: " + error);
    }

    void LoadBanner()
    {
        // Create the banner object, with default settings.
        bannerAd = new LevelPlayBannerAd(bannerAdUnitId);
        
        // Create the banner object, with custom settings.
        com.unity3d.mediation.LevelPlayAdSize size = com.unity3d.mediation.LevelPlayAdSize.LARGE;
        com.unity3d.mediation.LevelPlayBannerPosition pos = com.unity3d.mediation.LevelPlayBannerPosition.TopLeft;
        com.unity3d.mediation.LevelPlayBannerPosition po = new com.unity3d.mediation.LevelPlayBannerPosition(new Vector2(80f, 300f));
        bannerAdCustom = new LevelPlayBannerAd(bannerAdUnitId, size: size, position:pos);

        bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;
        
        bannerAdCustom.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAdCustom.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAdCustom.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAdCustom.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAdCustom.OnAdClicked += BannerOnAdClickedEvent;
        bannerAdCustom.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAdCustom.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAdCustom.OnAdExpanded += BannerOnAdExpandedEvent;

        // Ad load
        bannerAd.LoadAd();
        bannerAdCustom.LoadAd();
    }

    void OnInitializationCompleted(LevelPlayConfiguration configuration)
    {
        Debug.Log("Initialization completed");
        //IronSource.Agent.launchTestSuite();

        LoadBanner();
    }

    //Banner Events
    void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdLoadedEvent With AdInfo " + adInfo);
    }

    void BannerOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        Debug.Log("unity-script: I got BannerOnAdLoadFailedEvent With Error " + error);
    }

    void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdClickedEvent With AdInfo " + adInfo);
    }

    void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdDisplayedEvent With AdInfo " + adInfo);
    }

    void BannerOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError adInfoError)
    {
        Debug.Log("unity-script: I got BannerOnAdDisplayFailedEvent With AdInfoError " + adInfoError);
    }

    void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdCollapsedEvent With AdInfo " + adInfo);
    }

    void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdLeftApplicationEvent With AdInfo " + adInfo);
    }

    void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdExpandedEvent With AdInfo " + adInfo);
    }

    void OnDestroy()
    {
        bannerAd?.DestroyAd();
    }
}