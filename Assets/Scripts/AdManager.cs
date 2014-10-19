using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ChartboostSDK;
using System.Runtime.InteropServices;
using HutongGames.PlayMaker;

public class AdManager : Singleton<AdManager>, IRevMobListener {
	public enum AdType {
		Random = 0,
		RevMob = 1,
		Chartboost = 2,
	}
	
	[SerializeField] bool useRevMob = true;
	[SerializeField] string revMobAndroidID;
	[SerializeField] string revMobIosID;
	
	/*[SerializeField] Vector2 revMobBannerLocation;
	[SerializeField] Vector2 revMobBannerSize;
	[SerializeField] float revMobBannerDelay = 10f;
	[SerializeField] float revMobBannerDuration = 5f;
	[SerializeField] float revMobBannerInterval = 15f;*/
	
	[SerializeField] bool useChartBoost = true;
	[SerializeField] string chartBoostIosID;
	[SerializeField] string chartBoostIosSignature;

	private Dictionary<string, string> REVMOB_APP_IDS;
	private static RevMob revmob;
	private Chartboost chartBoostManager;
	private AdType activeAdNetworks;
	private bool initialized = false;

	public new void Awake() {
		if (useChartBoost) {
			chartBoostManager = FindObjectOfType<Chartboost>() as Chartboost;
			DontDestroyOnLoad(chartBoostManager);
		}

		DontDestroyOnLoad(this);

		InitializeAds();
	}

	void OnApplicationPaused(bool paused) {
		if (paused) {}
		else {
			InitializeAds();
		}
	}

	void InitializeAds() {
		
		#if UNITY_EDITOR
		Destroy(this);
		#elif UNITY_ANDROID || UNITY_IPHONE
	
		#else
		Destroy(this);
		#endif

		if (Application.internetReachability == NetworkReachability.NotReachable) {
			initialized = false;
		} else {

			if (useRevMob) {
				// RevMob setup
				REVMOB_APP_IDS = new Dictionary<string, string> () {
					{ "Android", revMobAndroidID },
					{ "IOS", revMobIosID }
				};
				revmob = RevMob.Start (REVMOB_APP_IDS, "_AdManager");
				if (revmob != null) 
						revmob.CreateFullscreen ();
				else 
						useRevMob = false;
			}

			if (useChartBoost) {
					// Chartboost setup
				Chartboost.cacheInterstitial(CBLocation.Default);
			}

			initialized = true;
		}

	}

	// RevMob delegates
#region IRevMobListener implementation
	
	public void SessionIsStarted () {
		Debug.Log("Session started.");
	}
	
	public void SessionNotStarted (string revMobAdType) {
		Debug.Log("Session not started.");
	}

	public void AdDidReceive (string revMobAdType) {
		GA.API.Design.NewEvent("RevMob:Received");
		Debug.Log("Ads - RevMob:Received");
	}
	
	public void AdDidFail (string revMobAdType) {
		GA.API.Design.NewEvent("RevMob:Failed");
		Debug.Log("Ads - RevMob:Failed");
		PlayMakerFSM.BroadcastEvent("AdFinished");
		revmob.CreateFullscreen();
	}
	
	public void AdDisplayed (string revMobAdType) {
		GA.API.Design.NewEvent("RevMob:Shown");
		Debug.Log("Ads - RevMob:Shown");
	}
	
	public void UserClickedInTheAd (string revMobAdType) {
		GA.API.Design.NewEvent("RevMob:Clicked");
		Debug.Log("Ads - RevMob:Clicked");
		PlayMakerFSM.BroadcastEvent("AdFinished");
		revmob.CreateFullscreen();
	}
	
	public void UserClosedTheAd (string revMobAdType) {
		GA.API.Design.NewEvent("RevMob:Closed");
		Debug.Log("Ads - RevMob:Closed");
		PlayMakerFSM.BroadcastEvent("AdFinished");
		revmob.CreateFullscreen();
	}

	public void InstallDidReceive(string message) {
		Debug.Log("Ads - RevMob:Install:Succeeded");
		GA.API.Design.NewEvent("RevMob:Install:Succeeded");
	}
	
	public void InstallDidFail(string message) {
		Debug.Log("Ads - RevMob:Install:Failed");
		GA.API.Design.NewEvent("RevMob:Install:Failed");
	}
#endregion

	// Chartboost delegates
#region Chartboost delegates
	public void ChartboostAdFailed(CBLocation location, CBImpressionError error) {
		GA.API.Design.NewEvent("Chartboost:Failed");
		Debug.Log("Ads - Chartboost:Failed");
		PlayMakerFSM.BroadcastEvent("AdFinished");
		useChartBoost = false;
	}
	public void ChartboostAdShown(CBLocation location) {
		Debug.Log("Ads - Chartboost:Shown");
		GA.API.Design.NewEvent("Chartboost:Shown");
	}
	public void ChartboostAdCached(CBLocation s) {
		GA.API.Design.NewEvent("Chartboost:Cached");
		Debug.Log("Ads - Chartboost:Cached");
	}
	public void ChartboostAdClosed(CBLocation s) {
		GA.API.Design.NewEvent("Chartboost:Closed");
		Debug.Log("Ads - Chartboost:Closed");
		PlayMakerFSM.BroadcastEvent("AdFinished");
		Chartboost.cacheInterstitial(CBLocation.Default);
	}
	public void ChartboostAdClicked(CBLocation s) {
		GA.API.Design.NewEvent("Chartboost:Clicked");
		Debug.Log("Ads - Chartboost:Clicked");
		PlayMakerFSM.BroadcastEvent("AdFinished");
		Chartboost.cacheInterstitial(CBLocation.Default);
	}
#endregion

	void OnEnable() {
#if UNITY_ANDROID || UNITY_IPHONE
		if (useChartBoost) {
			// Listen to some interstitial-related events
			Chartboost.didFailToLoadInterstitial += ChartboostAdFailed;
			Chartboost.didCloseInterstitial += ChartboostAdClosed;
			Chartboost.didCacheInterstitial += ChartboostAdCached;
			Chartboost.didDisplayInterstitial += ChartboostAdShown;
			Chartboost.didClickInterstitial += ChartboostAdClicked;
		}
#endif
	}
	
	void OnDisable() {
#if UNITY_ANDROID || UNITY_IPHONE
		if (useChartBoost) {
			// Remove interstitial-related events
			Chartboost.didFailToLoadInterstitial -= ChartboostAdFailed;
			Chartboost.didCloseInterstitial -= ChartboostAdClosed;
			Chartboost.didCacheInterstitial -= ChartboostAdCached;
			Chartboost.didDisplayInterstitial -= ChartboostAdShown;
			Chartboost.didClickInterstitial -= ChartboostAdClicked;
		}
#endif
	}

	void CheckActiveAdNetworks() {
		activeAdNetworks = 0;
		
		if (useRevMob) activeAdNetworks = activeAdNetworks | AdType.RevMob;
		if (useChartBoost) activeAdNetworks = activeAdNetworks | AdType.Chartboost;
	}

	public void ShowFullscreenAd(AdType adType) {
#if UNITY_ANDROID || UNITY_IPHONE
		CheckActiveAdNetworks();
		if (activeAdNetworks == 0 || 
		    Application.internetReachability == NetworkReachability.NotReachable) 
				return;

		if (adType == AdType.Random || (adType & activeAdNetworks) == 0) {
			System.Array values = System.Enum.GetValues(typeof(AdType));

			while ((adType & activeAdNetworks) == 0) 
				adType = (AdType)values.GetValue(Random.Range(1, values.Length));
		}

		switch (adType) {
		case AdType.Random:
			break;
		case AdType.RevMob:
			Debug.Log("showing revmob");
			revmob.ShowFullscreen();
			break;
		case AdType.Chartboost:
			Debug.Log("showing chartboost");	
			Chartboost.showInterstitial(CBLocation.Default);
			break;
		}
#endif
	}

/*
	public void OpenLink() {
#if UNITY_ANDROID || UNITY_IPHONE
		revmob.CreateAdLink();
		revmob.OpenAdLink();
#endif
	}

	public void ShowBanner(bool show) {
		if (show) {
			if (showBanner != true) {
				showBanner = true;
				StartCoroutine( DisplayBanner());
			}
		} else showBanner = false;
	}

	IEnumerator DisplayBanner() {
		if (useRevMob) {
			RevMobBanner banner = null;
			
			yield return new WaitForSeconds(revMobBannerDelay);

			while (showBanner) {
#if UNITY_ANDROID
				banner = revmob.CreateBanner(RevMob.Position.TOP, (int)revMobBannerLocation.x * Screen.width, 
				                             (int)revMobBannerLocation.y * Screen.height, (int)revMobBannerSize.x
				                             * Screen.width, (int)revMobBannerSize.y * Screen.height);
#elif UNITY_IPHONE
				banner = revmob.CreateBanner((int)revMobBannerLocation.x * Screen.width, 
				                             (int)revMobBannerLocation.y * Screen.height, 
				                             (int)revMobBannerSize.x * Screen.width, 
				                             (int)revMobBannerSize.y * Screen.height, null, null);
#endif
				
#if UNITY_ANDROID || UNITY_IPHONE
				banner.Show();

				yield return new WaitForSeconds(revMobBannerDuration);

				yield return new WaitForSeconds(revMobBannerInterval);
			}
			banner.Release();
#endif
		}
	}	
  */
}
