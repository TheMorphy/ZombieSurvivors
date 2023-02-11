using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsInitializer : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
	[SerializeField] Button _showAdButton;
	[SerializeField] string _androidAdUnitId;
	[SerializeField] string _iOSAdUnitId;
	[SerializeField] bool isTesting = true;
	string _adUnitId = null; // This will remain null for unsupported platforms

	public event Action<bool> OnAdClosed;

	void Awake()
	{
		_adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
			? _iOSAdUnitId
			: _androidAdUnitId;

		//Disable the button until the ad is ready to show:
		_showAdButton.interactable = false;
	}

	public void InitializeAd()
	{
		Advertisement.Initialize(_adUnitId, isTesting, this);
	}

	// Load content to the Ad Unit:
	public void LoadAd()
	{
		_adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
			? "Rewarded_iOS"
			: "Rewarded_Android";

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
			_showAdButton.onClick.AddListener(ShowAd);
			// Enable the button for users to click:
			_showAdButton.interactable = true;
		}
	}

	// Implement a method to execute when the user clicks the button:
	public void ShowAd()
	{
		// Disable the button:
		_showAdButton.interactable = false;
		// Then show the ad:
		Advertisement.Show(_adUnitId, this);
	}

	// Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
	public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
	{
		if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
		{
			Debug.Log("Unity Ads Rewarded Ad Completed");
			// Grant a reward.

			// Load another ad:
			LoadAd();

			OnAdClosed?.Invoke(true);
		}
	}

	// Implement Load and Show Listener error callbacks:
	public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
	{
		Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
		// Use the error details to determine whether to try to load another ad.
	}

	public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
	{
		Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
		// Use the error details to determine whether to try to load another ad.
	}

	public void OnUnityAdsShowStart(string adUnitId) { }
	public void OnUnityAdsShowClick(string adUnitId) { }

	void OnDestroy()
	{
		// Clean up the button listeners:
		_showAdButton.onClick.RemoveAllListeners();
	}

	public void OnInitializationComplete()
	{
		LoadAd();
	}

	public void OnInitializationFailed(UnityAdsInitializationError error, string message)
	{
		print("Ad Initialization failed");
	}
}
