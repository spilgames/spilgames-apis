using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using LitJson;
using Spil;

/**
 * Namespace to group the Spil definitions for the unity plugin.
 */
namespace Spil{
	
	/**
	 * Type of environment supported in the configurations
	 */
	public enum Enviroment{
		SG_ENVIRONMENT_DEV_VALUE=0,
		SG_ENVIRONMENT_LIVE_VALUE
	};
	
	public enum Orientation {
		SG_LANDSCAPE = 0,
		SG_PORTRAIT
	}
	
	/**
	 * Number of stores supported in the configurations
	 */
	public enum Store{
		SG_STORE_IOS,
		SG_STORE_AMAZON,
		SG_STORE_GOOGLE_PLAY
	}
	
	/**
	 * Settings to pass to the native application
	 */
	public struct SpilSettings{
		public Enviroment SG_ENVIRONMENT_KEY; /**< Type of enviroment to use */
		public string SG_ENVIRONMENT_SETTINGS_URL_GET; /**< URL to get the app settings file. Required if SG_ENVIRONMENT_KEY is set to SG_ENVIRONMENT_DEV_VALUE. */
		public float SG_APP_SETTINGS_POLL_TIME_KEY; /**< Time in seconds to scan for the default settings. Only is used if SG_ENVIRONMENT_KEY is set to SG_ENVIRONMENT_DEV_VALUE.*/
		public string SG_TRACKING_ID_KEY;/**< Application ID in the tracking system. */
		public Store SG_STORE_ID;
	};
}

public class SpilUnity : MonoBehaviour {
	//singleton
	private static SpilUnity instance;
	private static bool initialized = false;
	//android related
	#if UNITY_ANDROID
	private static SpilAndroid spilAndroid;
	#endif
	//listeners
	private SpilAppSettingsListener appSettingsListener;
	private SpilAdsListener adsListener;
	private SpilABTestListener abtestListener;
	private SpilInGameAdsListener inGameAdListener;
	
	[DllImport ("__Internal")]
	private static extern void initialize(string objectname, string appID, string authToken, SpilSettings configs);
	
	[DllImport ("__Internal")]
	private static extern void setAppSettingsDelegate();
	
	[DllImport ("__Internal")]
	private static extern void adsNextInterstitial();
	
	[DllImport ("__Internal")]
	private static extern void adsNextInterstitialWithLocation(string location);
	
	[DllImport ("__Internal")]
	private static extern void adsShowMoreGames();
	
	[DllImport ("__Internal")]
	private static extern void adsEnabled(bool state);
	
	[DllImport ("__Internal")]
	private static extern void adsCacheNextInterstitial();
	
	[DllImport ("__Internal")]
	private static extern void adsCacheNextInterstitialWithLocation(string location);
	
	[DllImport ("__Internal")]
	private static extern void adsRequestInGameAdAssetWithLocation(int orientation, string location);
	
	[DllImport ("__Internal")]
	private static extern void adsRequestInGameAdAsset(int orientation);
	
	[DllImport ("__Internal")]
	private static extern void adsMarkInGameAdAsShown(string adId);
	
	[DllImport ("__Internal")]
	private static extern void setAdsDelegate();
	
	[DllImport ("__Internal")]
	private static extern void setInGameAdsDelegate();
	
	[DllImport ("__Internal")]
	private static extern void trackPage(string page);
	
	[DllImport ("__Internal")]
	private static extern void trackEvent(string evt);
	
	[DllImport ("__Internal")]
	private static extern void trackEventDetailed(string category, string action, string label, int val);
	
	[DllImport ("__Internal")]
	private static extern void trackEventWithParameters(string evt, string[] keys, string[] values, int size);
	
	[DllImport ("__Internal")]
	private static extern void trackTimedEvent(string evt);
		
	[DllImport ("__Internal")]
	private static extern void trackEndTimedEvent(string evt);
	
	[DllImport ("__Internal")]
	private static extern void trackEndTimedEventWithParameters(string evt, string[] keys, string[] values, int size);
	
	[DllImport ("__Internal")]
	private static extern void trackError(string evt, string message, string exception);
	
	[DllImport ("__Internal")]
	private static extern void setABTestDelegate();
	
	[DllImport ("__Internal")]
	private static extern void abtestUpdateUserInfo();
	
	[DllImport ("__Internal")]
	private static extern void abtestUpdateUserInfoWith(string[] keys, string[] values, int size);
	
	[DllImport ("__Internal")]
	private static extern void abtestGetTestDiff();
		
	[DllImport ("__Internal")]
	private static extern void abtestGetTestDiffForUser(string user);
	
	[DllImport ("__Internal")]
	private static extern void abtestMarkSucceedTest(string name, string[] keys, string[] values, int size);
	
	protected SpilUnity(){
	}
	
	/**
	 * Method to retrieve the sharedInstance, since this class is a singleton. The instance returned could be nil if the
	 * constructor above haven't been called or if was error occured.
	 * @return	The shared instance of this Spil object.
	 */
	public static SpilUnity Instance {
		get {
	    	return instance;
		}
	}
	
	public void Awake() {
		instance = this;
	}
	
	public void OnApplicationQuit() {
		instance = null;
	}

	/**
	 * Creates a Spil object singleton with an application ID and authentication token that will be used along the framework
	 * for multiple services.
	 * Also you must specify some configurations to control the behaivor of the framework. Most important, if the framework
	 * should act like a development environment or a production environment.
	 * Some validations about the configurations are made, if one fails, a null object is returned, and the error is written 
	 * in the console log.
	 * @param	applID	The application ID provided by Spil Games, it can't be null.
	 * @param	authToken	The authentication token provided by Spil Games, it can't be null.
	 * @param	configs	A dictionary with the posible settings to be used by spil framework.
	 * @return	The Spil object that will be use for further calls.
	 */
	public void Initialize(string appID, string authToken, SpilSettings configs){
		if(initialized){
			Debug.LogWarning("Trying to reinitialize the SpilUnity object. This will cause network overhead, stick to a single initialization.");
			return;
		}
		
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			initialize(gameObject.name, appID, authToken, configs);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid = new SpilAndroid(gameObject.name, appID,authToken,configs);
			#endif
		}else{
			//TODO: simulate some default results.
		}
		
		initialized = true;
	}
	
	/**
	 * Sets the SpilAppSettingsListener and receive the proper notifications from it. A listener is required in order to 
	 * deliver the settings downloaded from the server or loaded from the default files. 
	 * Without the listener this subsystem is disabled.
	 * @param	listener	The listener to handle the response of the AppSettings subsystem.
	 */
	public void SetAppSettingsListener(SpilAppSettingsListener listener){
		appSettingsListener = listener;
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			setAppSettingsDelegate();
		}else{
			#if UNITY_EDITOR
			//Simulates some default results. The defaults will be readed from the default settings file in the folder "Spil/Resources".
			//No live changes in the editor are available.
			TextAsset defaults = Resources.Load("spilgames_default_settings") as TextAsset;
			_AppSettingsDidLoad(defaults.text);
			#endif
		}
	}
	
	/**
	 * Sets the SpilAdsListener and receive the proper notifications from it. Without the listener
	 * this subsystem is disabled.
	 * @param	listener The listener to handle the events generated by the Ads subsystem.
	 */
	public void SetAdsListener(SpilAdsListener listener){
		adsListener = listener;
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			setAdsDelegate();
		}
	}
	
	/**
	 * Shows an ad right away, using the default location. 
	 * @see AdsNextInterstitial(string)
	 */
	public void AdsNextInterstitial(){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsNextInterstitial();
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.AdsNextInterstitial();
			#endif
		}else{

		}
	}
	
	/**
	 * Shows an ad right away, using the specified location.
	 * Use the location parameter to indicate where the ad is being displayed, for instance, 
	 * "mainmenu", "store", "pausemenu". This helps to improve the advertisement campaigns.
	 * @param	location	Location to be used for this interstitial
	 */
	public void AdsNextInterstitial(string location){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsNextInterstitialWithLocation(location);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.AdsNextInterstitial(location);
			#endif
		}else{

		}
	}
	
	/**
	 * Shows the More Games screen right away.
	 */
	public void AdsShowMoreGames(){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsShowMoreGames();
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.ShowMoreGames();
			#endif
		}else{

		}
	}
	
	/**
	 * Turns on/off if the ads should be displayed. The ads are displayed by default. 
	 * @param	state	Indicates if the ads should be displayed or not.
	 */
	public void AdsEnabled(bool state){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsEnabled(state);
		}else{
		}
	}
	
	/**
	 * Caches the next interstitial image to speed up the load time. Uses the default location.
	 * @see AdsCacheNextInterstitial(string)
	 */
	public void AdsCacheNextInterstitial(){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsCacheNextInterstitial();
		}else{
		}
	}
	
	/**
	 * Caches the next interstitial image to speed up the load time.
	 * Use the location parameter to indicate where the ad is being displayed, for instance,
	 * "mainmenu", "store", "pausemenu". This helps to improve the advertisement campaigns.
	 * @param	location	Location to be used for this interstitial
	 */
	public void AdsCacheNextInterstitial(string location){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsCacheNextInterstitialWithLocation(location);
		}else{
		}
	}
	
	/**
	 * Sets the listener to handle the events received by the in-game ads system.
	 * Without this listener the InGameAds subsytem is disabled.
	 * @param	listener	The ads listener who is going to handle the events.
	 */
	public void SetInGameAdListener(SpilInGameAdsListener listener){
		inGameAdListener = listener;
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			setInGameAdsDelegate();
		}
	}
	
	/**
	 * Makes a request to get an advert (on the default location) and return it to the invoker when it's done through the
	 * SpilInGameAdsListener implementation set up prior the call to this method.
	 * This methods returns right away.
	 * @see AdsRequestInGameAd(Orientation, string)
	 * @param	orient	The orientation of the expected banner (Orientation.SG_LANDSCAPE or Orientation.SG_PORTRAIT)
	 */
	public void AdsRequestIngameAsset(Orientation orient){
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.AdsRequestInGameAsset(orient);
			#endif
		}else{
			adsRequestInGameAdAsset((int)orient);
		}
	}
	
	/**
	 * Makes a request to get an advert (on the default location) and return it to the invoker when it's done through the
	 * SpilInGameAdsListener implementation set up prior the call to this method.
	 * Use the location parameter to indicate where the ad is being displayed, for instance,
	 * "mainmenu", "store", "pausemenu". This helps to improve the advertisement campaigns.
	 * This methods returns right away.
	 * @param	orient	The orientation of the expected banner (Orientation.SG_LANDSCAPE or Orientation.SG_PORTRAIT)
	 * @param	location	Location to be used for this interstitial
	 */
	public void AdsRequestIngameAsset(Orientation orient, string location){
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.AdsRequestInGameAssetWithLocation(orient, location);
			#endif
		}else{
			adsRequestInGameAdAssetWithLocation((int)orient, location);
		}
	}
	
	/**
	 * Tracks a particular page. It can be used to keep track of the current screen separetely from the events.
	 * If the session is not started yet, this request is ignored.
	 * @param	page	The page name/url to track.
	 */
	public void TrackPage(string page){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackPage(page);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.TrackPage(page);
			#endif
		}else{
			//TODO: simulate some default results.
		}	
	}
	
	/**
	 * Tracks an event. The event could be actions taken on some object like unlocking an achievement, or a getting a hiscore.
	 * @param	evt	The event to track.
	 */
	public void TrackEvent(string evt){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackEvent(evt);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.TrackEvent(evt);
			#endif
		}else{
			//TODO: simulate some default results.
		}	
	}
	
	/**
	 * Tracks an event under a particular category. The parameters match with the google analytics' ones. For flurry, a event with
	 * parameters is issued, where the category is the event name.
	 * @param	category	The category for this event.
	 * @param	action		The action took on that category.
	 * @param	label		Optional. A string label to especify something about the action.
	 * @param	val		Optional. A integer value. useful to especify error codes.
	 */
	public void TrackEventDetailed(string category, string action, string label, int val){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackEventDetailed(category,action,label,val);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.TrackEvent(category, action, label, val);
			#endif
		}else{
			//TODO: simulate some default results.
		}
	}
	
	/**
	 * Tracks an event with particular parameters. This match the Flurry's logEvent:withParameters:
	 * @param	evt		The event to track
	 * @param	parameters		Additional parameters to attach to the event.
	 */
	public void TrackEventWithParameters(string evt, Dictionary<string,string> parameters){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			string[] keys = new string[parameters.Count];
			string[] values = new string[parameters.Count];
			int i=0;
			Dictionary<string,string>.KeyCollection ks = parameters.Keys;
			foreach(string k in ks){
				keys[i] = k;
				values[i] = parameters[k];
				i++;
			}
			trackEventWithParameters(evt, keys, values, keys.Length);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.TrackEvent(evt, parameters);
			#endif
		}else{
			//TODO: simulate some default results.
		}
	}
	
	/**
	 * Tracks an event that should end in a finite time. If the tracking system support it natively the equivalent
	 * method will be used. Otherwise, an event with the start timestamp(epoch) is issued.
	 * @param	evt	The event to track.
	 */	
	public void TrackTimedEvent(string evt){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackTimedEvent(evt);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.TrackTimedEvent(evt);
			#endif
		}else{
			//TODO: simulate some default results.
		}
	}
	
	/**
	 * Tracks the end of an event that was started. If the tracking system support it natively the equivalent method will be used.
	 * Otherwise, an event with the end timestamp(epoch) is issued.
	 * @see trackTimedEvent:
	 * @param	evt	The event to track. Should match with the starting event.
	 */	
	public void TrackEndTimedEvent(string evt){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackEndTimedEvent(evt);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.TrackEndTimedEvent(evt);
			#endif
		}else{
			//TODO: simulate some default results.
		}
	}
	
	/**
	 * Tracks the end of an event that was started. If the tracking system support it natively the equivalent method will be used.
	 * Otherwise, an event with the end timestamp(epoch) is issued.
	 * @see trackTimedEvent:
	 * @param	evt	The event to track. Should match with the starting event.
	 * @param	parameters	The parameters when the event was finished.
	 */
	public void TrackEndTimedEventWithParameters(string evt,Dictionary<string, string> parameters){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			string[] keys=null;
			string[] values=null;
			dictionaryToArrays(parameters, out keys, out values);
			trackEndTimedEventWithParameters(evt, keys, values, keys.Length);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.TrackEndTimedEvent(evt, parameters);
			#endif
		}else{
			//TODO: simulate some default results.
		}
	}
	
	/**
	 * Tracks an error/crashes that has occured in the application. This errors appear in special sections of the analytics dashboards,
	 * therefore only use them to reflect app crashes or fatal errors. Send minor warnings with this method will clutter the ability to
	 * detect actual crashes causes.
	 * @param	evt	The event to track.
	 * @param	message	The message with the detail of the error.
	 * @param	exception	The exception that causes the error.
	 */
	public void TrackError(string evt, string message, Exception exception){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackError(evt,message,exception.ToString());
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.TrackError(evt,message,exception);
			#endif
		}else{
			//TODO: simulate some default results.
		}
	}
	
	/**
	 * Sets the SpilABTestListener and receive the proper notifications from it. Without the listener
	 * this subsystem is disabled.
	 * @param	listener The delegate to handle the events generated by the A/B test subsystem.
	 */
	public void SetABTestListener(SpilABTestListener listener){
		abtestListener = listener;
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			setABTestDelegate();
		}else{
		}
	}
	
	/**
	 * Updates the user basic information to create segments and improve A/B tests. This
	 * method will send:
	 * - Country
	 * - Language
	 * - Device (ipad, ipod, iphone)
	 * - OS Platform (version)
	 */
	public void ABTestUpdateUserInfo(){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			abtestUpdateUserInfo();
		}else{
		}
	}
	
	/**
	 * Updates the user information to create segments and improve A/B tests. This
	 * method will send the basic information plus all the information included in the extra info
	 * @param	info	A dictionary with all the extra parameter we want to submit.
	 * @see AbtestUpdateUserInfo()
	 */
	public void ABTestUpdateUserInfoWith(Dictionary<string,string>info){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			string[] keys=null;
			string[] values=null;
			dictionaryToArrays(info, out keys, out values);
			abtestUpdateUserInfoWith(keys, values, keys.Length);
		}else{
		}
	}
	
	/**
	 * Sends a request to retrieve the test differences for this user (MAC Address). The
	 * differences will be sent asynchronously to the SpilABTestListener implemented and set
	 * in the SetABTestListener method.
	 */
	public void ABTestGetTestDiff(){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			abtestGetTestDiff();
		}else{
		}
	}
	
	/**
	 * For development purposes only. Send a request to retrieve the test differences for
	 * this user. The differences will be sent asynchronously to the SpilABTestListener implemented
	 * and set in the SetABTestListener method.
	 * @param	user	The user to force the different variants of the A/B test.
	 */
	public void ABTestGetTestDiffForUser(string user){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			abtestGetTestDiffForUser(user);
		}else{
		}
	}
	
	/**
	 * Marks a particular resource as a success with the parameters that lead to that success.
	 * This method should be called with the exact name of the resource and also it must
	 * be called with the control version to be able to compare results.
	 * @param	name	The name of the resource to mark as a successful one.
	 * @param	parameters	A dictionary with extra parameters relevant for the analysis of the action called.
	 */
	public void ABTestMarkSucceedTest(string name, Dictionary<string,string>parameters){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			string[] keys=null;
			string[] values=null;
			dictionaryToArrays(parameters, out keys, out values);
			abtestMarkSucceedTest(name, keys, values, keys.Length);
		}else{
		}
	}
	
	//callbacks from the native part
	private void _AppSettingsDidLoad(string jsonData){
		JsonData data = JsonMapper.ToObject(jsonData);
		if(appSettingsListener != null)
			appSettingsListener.AppSettingsDidLoad(data);
	}

	private void _AppSettingsDidFailWithError(string errorDescription){
		if(appSettingsListener != null)
			appSettingsListener.AppSettingsDidFailWithError(errorDescription);
	}
	
	private void _AppSettingsDidStartDownload(string empty){
		if(appSettingsListener != null)
			appSettingsListener.AppSettingsDidStartDownload();
	}
	
	private void _AdDidStart(string empty){
		if(adsListener != null)
			adsListener.AdDidStart();
	}
	
	private void _AdDidFailToStart(string error){
		if(adsListener != null)
			adsListener.AdDidFailToStart(error);
	}
	
	private void _AdWillAppear(string empty){
		if(adsListener != null)
			adsListener.AdWillAppear();
	}
	
	private void _AdDidAppear(string empty){
		if(adsListener != null)
			adsListener.AdDidAppear();
	}
	
	private void _AdDidFailToAppear(string error){
		if(adsListener != null)
			adsListener.AdDidFailToAppear(error);
	}
	
	private void _AdPopupDidDismiss(){
		if(adsListener != null)
			adsListener.AdPopupDidDismiss();
	}
	
	private void _AdMoreGamesWillAppear(string empty){
		if(adsListener != null)
			adsListener.AdMoreGamesWillAppear();
	}
	
	private void _AdMoreGamesDidAppear(string empty){
		if(adsListener != null)
			adsListener.AdMoreGamesDidAppear();
	}
	
	private void _AdMoreGamesDidFailToAppear(string error){
		if(adsListener != null)
			adsListener.AdMoreGamesDidFailToAppear(error);
	}
	
	private void _AdMoreGamesDidDismiss(){
		if(adsListener != null)
			adsListener.AdMoreGamesDidDismiss();
	}
	
	private void _AdDidLoadIngameAsset(string json){
		if(inGameAdListener != null){
			AdsData data = JsonMapper.ToObject<AdsData>(json);
			
			WWW www = new WWW(data.url);
			StartCoroutine(_downloadCallback(www, data.adId, data.link));
		}
 	}
	
 	private void _AdDidFailIngameAsset(string error){
		if(inGameAdListener != null)
			inGameAdListener.AdDidFailIngameAsset(error);
	}
	
	private void _ABTestSessionDidStart(){
		if(abtestListener != null)
			abtestListener.ABTestSessionDidStart();
	}
	
	private void _ABTestSessionDidEnd(){
		if(abtestListener != null)
			abtestListener.ABTestSessionDidEnd();
	}
	
	private void _ABTestSessionDiffReceived(string jsonDiff){
		if(abtestListener != null){
			Debug.Log("JSON DIFF: "+jsonDiff);
			JsonData data = JsonMapper.ToObject(jsonDiff);
			abtestListener.ABTestSessionDiffReceived(data);
		}
	}
	
	private void dictionaryToArrays<K,E>(Dictionary<K,E> parameters, out K[] keys, out E[] values){
		keys = new K[parameters.Count];
		values = new E[parameters.Count];
		int i=0;
		Dictionary<K,E>.KeyCollection ks = parameters.Keys;
		foreach(K k in ks){
			keys[i] = k;
			values[i] = parameters[k];
			i++;
		}
	}
	
	private IEnumerator _downloadCallback(WWW www, string adId, string link){
		 yield return www;
 
        // check for errors
        if (www.error == null) {
            Texture2D tex = new Texture2D(1,1); 
			tex.LoadImage(www.bytes);
			
			GameObject prefab = Resources.Load("IngameAsset", typeof(GameObject)) as GameObject;
			GameObject obj = (GameObject)Instantiate(prefab);
			IgaPanel behaviour = (IgaPanel)obj.GetComponent<IgaPanel>();
			behaviour.spilUnity = this;
			behaviour.texture = tex;
			behaviour.link = link;
			behaviour.adId = adId;
			
			if(inGameAdListener != null)
				inGameAdListener.AdDidLoadIngameAsset(obj);
        } else {
            if(inGameAdListener != null)
				inGameAdListener.AdDidFailIngameAsset(www.error);
        }    

	}
	
	protected internal void _AdMarkAsShown(string adId){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsMarkInGameAdAsShown(adId);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.NotifyInGameAd(adId);
			#endif
		}else{
			Debug.Log("Ad marked as shown!");
		}	
	}
	
	protected class AdsData{
		public string url;
		public string link;
		public string adId;
		public string name;
	};
}
