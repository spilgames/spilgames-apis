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
	public enum enviroment{
		SG_ENVIRONMENT_DEV_VALUE=0,
		SG_ENVIRONMENT_LIVE_VALUE
	};
	
	/**
	 * 
	 * Number of stores supported in the configurations
	 */
	public enum store{
		SG_STORE_IOS,
		SG_STORE_AMAZON,
		SG_STORE_GOOGLE_PLAY
	}
	
	
	/**
	 * Settings to pass to the native application
	 */
	public struct SpilSettings{
		public enviroment SG_ENVIRONMENT_KEY; /**< Type of enviroment to use */
		public string SG_ENVIRONMENT_SETTINGS_URL_GET; /**< URL to get the app settings file. Required if SG_ENVIRONMENT_KEY is set to SG_ENVIRONMENT_DEV_VALUE. */
		public float SG_APP_SETTINGS_POLL_TIME_KEY; /**< Time in seconds to scan for the default settings. Only is used if SG_ENVIRONMENT_KEY is set to SG_ENVIRONMENT_DEV_VALUE.*/
		public string SG_TRACKING_ID_KEY;/**< Application ID in the tracking system. */
		public store SG_STORE_ID;
	};
	
	/**
	 * Interface to listen the responses from the App Settings subsystem
	 */
	public interface SpilAppSettingsListener {
		/**
		 * Method to call back when the settings are finally loaded.
		 * This methods will receive the settings loaded in the form of a JSON object.
		 * The developers should know the structure of the object since they created the default settings file.
		 * @param	data	The settings loaded. The format and the values are defined by the developer of the app.
		 */
		void AppSettingsDidLoad(JsonData data);
		
		/**
		 * Method to call back in case the settings couldn't be loaded.
		 * Usually the reasons to call this method will be:
		 * - if there is any parsing error in the remote settings and in the local settings.
		 * - if there is a connection error, and the file of the defaults can be found locally.
		 * @param	error	Error describing what was wrong.
		 */
		void AppSettingsDidFailWithError(string error);
		
		/**
		 * Method to call back when the download of the settings has been started.
		 */
		void AppSettingsDidStartDownload();
	}
	
	/**
	 * Interface to listen the events triggered by the Ads subsystem.
	 */
	public interface SpilAdsListener {
		/**
		 * Method to call back after the ad subsystem is successfully started.
		 */
		void AdDidStart();
		
		/**
		 * Method to call back after if the ad subsystem couldn't be started due to any reason.
		 * @param	error	The reason why the ad subsystem failed to start.
		 */
		void AdDidFailToStart(string error);
		
		/**
		 * Method to call back before the next ad is going to be displayed. This method is called
		 * every time the timer reach 0, regardless if the ad should be shown or not (enableAds is set to NO).
		 */
		void AdWillAppear();
		
		/**
		 * Method to call back after the ad is displayed. This method is only called if the ads are enabled to 
		 * be displayed (enableAds:YES).
		 */
		void AdDidAppear();
		
		/**
		 * Method to call back if the ad couldn't be displayed due to any reason.
		 * @param	error	The reason why the ad failed to be displayed.
		 */
		void AdDidFailToAppear(string error);
		
		/**
		 * Method to call back before the next more games' screen is going to be shown.
		 */
		void AdMoreGamesWillAppear();
		
		/**
		 * Method to call back after the more games' screen is displayed.
		 */
		void AdMoreGamesDidAppear();
		
		/**
		 * Method to call back if the more games' screen couldn't be displayed due to any reason.
		 * @param	error	The reason why the more games' screen failed to be displayed.
		 */
		void AdMoreGamesDidFailToAppear(string error);
		
		/**
		 * Method to call back if the more games' screen was dismissed.
		 */
		void AdMoreGamesDidDismiss();
		
		/**
		 * Method to call back if the ad' popup was dismissed.
		 */
		void AdPopupDidDismiss();
	}
	
	/** Interface to listen the events trigerred by the A/B Testing subsystem */
	public interface SpilABTestListener{
		/**
		 * Method to call back after the a/b test subsystem is successfully started.
		 */
		void ABTestSessionDidStart();
		
		/**
		 * Method to call back after the a/b test subsystem is successfully ended.
		 */
		void ABTestSessionDidEnd();
		
		/**
		 * Method to call back after the a/b test subsystem receive the differences to apply over the original version.
		 * The differences come expressed as an array of objects. These objects are represented as dictionaries, where,
		 * always are defined the following keys:
		 * <ul>
		 *	<li><b>uid</b>: an ID for this resource to test. A resource can contain many elements to test. Details in the next entry. </li>
		 *	<li><b>diff</b>: a dictionary with all the changes to apply to this resource. In this resource, many elements could be changed,
		 *	for each element, an entry will appear in this dictionary. Each of this entry will contain a dictionary with exactly 2 keys:
		 *	"new" and "old", refering to the original and value to replace with.</li>
		 *	<li><b>item_class</b>: unused for the moment.</li>
		 * </ul>
		 */
		void ABTestSessionDiffReceived(JsonData diffs);
	}
	
	/**
	 * Interface to listen the responses from the Extended Tracking subsystem
	 */
	public interface SpilTrackingExtendedListener{
		/**
		 * Method to call back when the any of the extended trackers are started.
		 * If the camera tracker is set up this method is called after the confirmation pop up is done, and 
		 * if there is at least one tracker active. When the camera tracker is not set up, this method is
		 * called when any of the other are activated.
		 */
		void TrackExtendedDidStart();
		
		/**
		 * Method to call back when ALL the extended trackers are stopped.
		 * This is an informative call.
		 */
		void TrackExtendedDidStop();
	}
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
	private SpilTrackingExtendedListener trackExtendedListener;
		
	[DllImport ("__Internal")]
	private static extern void initialize(string objectname, string appID, string authToken, SpilSettings configs);
	
	[DllImport ("__Internal")]
	private static extern void getSettings();
	
	[DllImport ("__Internal")]
	private static extern void adsNextIntersitial();
	
	[DllImport ("__Internal")]
	private static extern void adsShowMoreGames();
	
	[DllImport ("__Internal")]
	private static extern void adsEnabled(bool state);
	
	[DllImport ("__Internal")]
	private static extern void adsCacheNextIntersitial();
	
	[DllImport ("__Internal")]
	private static extern void startAds();
	
	[DllImport ("__Internal")]
	private static extern void getExtendedTracking();
	
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
	private static extern void trackUserID(string userID);
	
	[DllImport ("__Internal")]
	private static extern void trackAge(int age);
	
	[DllImport ("__Internal")]
	private static extern void trackGender(bool male);
	
	[DllImport ("__Internal")]
	private static extern void trackLatitude(double latitude,double longitude,double horizontalAccuracy,double verticalAccuracy);
	
	[DllImport ("__Internal")]
	private static extern void trackStartGestureScreen(string screenName);
	
	[DllImport ("__Internal")]
	private static extern void trackStopGestureScreen();
	
	[DllImport ("__Internal")]
	private static extern void getABTest();
	
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
	
	private SpilUnity(){
	}
	
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
	 * Create a Spil object singleton with an application ID and authentication token that will be used along the framework
	 * for multiple services.
	 * Also you must specify some configurations to control the behaivor of the framework. Most important, if the framework
	 * should act like a development environment or a production environment.
	 * Some validations about the configurations are made, if one fails, an error is written 
	 * in the console log.
	 * @param	appID	The application ID provided by Spil Games, it can't be null.
	 * @param	authToken	The authentication token provided by Spil Games, it can't be null.
	 * @param	configs	A reference to a SpilSettings struct with the posible settings to be used by spil framework.
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
	 * Method to retrieve the AppSettings for this app. A delegate is required in order to deliver the settings downloaded from 
	 * the server or loaded from the default files.
	 * @param	listener	The listener to handle the response of the AppSettings subsystem.
	 */
	public void GetSettings(SpilAppSettingsListener listener){
		appSettingsListener = listener;
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			getSettings();
		}else{
			//Simulates some default results. The defaults will be readed from the default settings file in the folder "Spil/Resources".
			//No live changes in the editor are available.
			TextAsset defaults = Resources.Load("spilgames_default_settings") as TextAsset;
			_AppSettingsDidLoad(defaults.text);
		}
	}
	
	/**
	 * Method to set the SpilAdsListener and receive the proper notifications from it.
	 * @param	listener The listener to handle the events generated by the Ads subsystem.
	 */
	public void StartAds(SpilAdsListener listener){
		adsListener = listener;
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			startAds();
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			
			#endif
		}else{
		}	
	}
	
	/**
	 * @deprecated In favor of better naming conventions. @see AdsNextIntersitial<br/>
	 * The ads are displayed based on a timer, this method force the ad to be shown right
	 * away, and the timer is reset.
	 */
	public void ShowNextAd(){
		this.AdsNextIntersitial();
	}
	
	/**
	 * The ads are displayed based on a timer, this method force the ad to be shown right
	 * away, and the timer is reset.
	 */
	public void AdsNextIntersitial(){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsNextIntersitial();
		}else{
		}
	}
	
	
	/**
	 * @deprecated In favor of better naming conventions. @see AdsShowMoreGames<br/>
	 * Force to show the More Games screen.
	 */
	public void ShowMoreGames(){
		this.AdsShowMoreGames();
	}
	
	/**
	 * Force to show the More Games screen.
	 */
	public void AdsShowMoreGames(){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsShowMoreGames();
		}else{
		}
	}
	
	/**
	 * @deprecated In favor of better naming conventions. @see AdsEnabled<br/>
	 * Turn on/off if the ads should be displayed. The ads are displayed by default. For gameplay screens should be turned off.
	 * After return to the menus should be turned on again.
	 * @param	state	Indicates if the ads should be displayed or not.
	 */
	public void EnableAds(bool state){
		this.AdsEnabled(state);
	}
	
	/**
	 * Turn on/off if the ads should be displayed. The ads are displayed by default. For gameplay screens should be turned off.
	 * After return to the menus should be turned on again.
	 * @param	state	Indicates if the ads should be displayed or not.
	 */
	public void AdsEnabled(bool state){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsEnabled(state);
		}else{
		}
	}
	
	/**
	 * Cache the next intersitial ad.
	 */
	public void AdsCacheNextIntersitial(){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			adsCacheNextIntersitial();
		}else{
		}
	}
	
	/**
	 * Method to set up the SpilTrackingExtendedListener for the extended tracking events. This listener is optional, 
	 * but its usage it's encouraged since this will guarantee the calls made are actually efective and not
	 * dropped because the extended tracking wasn't started yet.
	 */
	public void GetTrackExtended(SpilTrackingExtendedListener listener){
		trackExtendedListener=listener;
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			getExtendedTracking();
		}else{
		}
	}
	
	/**
	 * Track request to register a particular page. It can be used to keep track of the current screen separetely from the events.
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
	
	public void StartTracking(){
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.StartTracking();
			#endif
		}else{
			//TODO: simulate some default results.
		}	
	}
	
	public void StopTracking(){
		if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.StopTracking();
			#endif
		}else{
			//TODO: simulate some default results.
		}	
	}
	
	/**
	 * Track request to register an event. The event could be actions taken on some object like unlocking an achievement, or a getting a hi score.
	 * In google analytics jargot they will be register as a "category"="event" and the event passed as parameter as the action taken.
	 * In flurry jargot, an event with parameters is created and the parameters match with google analytics (category,action,label,value)
	 * @param	event	The event to track.
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
	 * Track request to register an event under a particular category. The parameters match with the google analytics' ones. For flurry, a event with
	 * parameters is issued.
	 * @param	category	The category for this event.
	 * @param	action		The action took on that category.
	 * @param	label		Optional. A string label to especify something about the action.
	 * @param	value		Optional. A integer value. useful to especify error codes.
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
 	* Track request to register an event with particular parameters. This match the Flurry's logEvent:withParameters:. It's not supported for GAN.
 	* @param	event		The event to track
 	* @param	params		Additional parameters to attach to the event.
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
	 * Track requesto to register the start of an event that should end in a finite time. If the tracking system support it natively the equivalent
	 * method will be used. Otherwise, an event with the start timestamp(epoch) is issued.
	 * @param	event	The event to track.
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
	 * Track requesto to register the end of an event that was started. If the tracking system support it natively the equivalent method will be used.
	 * Otherwise, an event with the end timestamp(epoch) is issued.
	 * @param	event	The event to track. Should match with the starting event. @see trackTimedEvent: .
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
	 * Track requesto to register the end of an event that was started. If the tracking system support it natively the equivalent method will be used.
	 * Otherwise, an event with the end timestamp(epoch) is issued.
	 * @param	event	The event to track. Should match with the starting event. @see trackTimedEvent: .
	 * @param	params	The parameters when the event was finished.
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
	 * Track request to register an error that has occured in the application.
	 * @param	event	The event to track.
	 * @param	msg	The message with the detail of the error.
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
	 * Track the user ID that logged in the application. Useful to keep track of how many users return to the application.
	 * @param	userID	The user ID to track.
 	*/
	public void TrackUserID(string userID){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackUserID(userID);
		}else if(Application.platform == RuntimePlatform.Android){
			#if UNITY_ANDROID
			spilAndroid.TrackUserId(userID);
			#endif
		}else{
			//TODO: simulate some default results.
		}
	}
	
	/**
	 * Track the age of the user is logged in the application. Useful for demographic information.
	 * @param	age	The age to track.
	 */
	public void TrackAge(int age){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackAge(age);
		}else{
			//TODO: simulate some default results.
		}
	}
	
	/**
	 * Track the gender of the user is logged in the application. Useful for demographic information.
	 * @param	male	YES|TRUE if the player is male, NO|FALSE if the player is female.
	 */
	public void TrackGender(bool male){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackGender(male);
		}else{
			//TODO: simulate some default results.
		}
	}
	
	/**
	 * Track the location information of the player if it's available. If the tracking system support it natively the equivalent
	 * method will be used. Otherwise, an event with the information of the location (latitude, longitude and accuracy) is issued.
	 * @param	latitude	The latitude where the device is. (it's a double value)
	 * @param	longitude	The longitude where the device is. (it's a double value).
	 * @param	hAccuracy	The horizontal accuracy of the measurement. (it's a double value).
	 * @param	vAccuracy	The vertical accuracy of the measurement. (it's a double value).
	 */
	public void TrackLatitude(double latitude,double longitude,double horizontalAccuracy,double verticalAccuracy){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackLatitude(latitude,longitude,horizontalAccuracy,verticalAccuracy);
		}else{
		}
	}
	
	/**
	 * Start the recording of the gestures for a new screen, the gestures for this screen will be stored together regarding how many times this
	 * screen has been started.
	 * @param	screenName	The name of the screen to record.
	 */
	public void TrackStartGestureScreen(string screenName){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackStartGestureScreen(screenName);
		}else{
		}
	}
	
	/**
	 * Stop the recording gestures for this screen, the gestures are drop until you start a new screen.
	 */
	public void TrackStopGestureScreen(){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			trackStopGestureScreen();
		}else{
		}
	}
	
	/**
	 * Method to set the ABTestListener and receive the proper notifications from it.
	 * @param	delegate The delegate to handle the events generated by the A/B test subsystem.
	 */
	public void GetABTest(SpilABTestListener listener){
		abtestListener = listener;
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			getABTest();
		}else{
		}
	}
	
	/**
	 * Update the user basic information to create segments and improve A/B tests. This
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
	 * Update the user information to create segments and improve A/B tests. This
	 * method will send the basic information (@see abtestUpdateUserInfo) plus all 
	 * the information included in the extra info
	 * @param	extraInfo	A dictionary with all the extra parameter we want to submit.
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
	 * Send a request to retrieve the test differences for this user (MAC Address). The 
	 * differences will be sent asynchronously to the ABTestDelegate implemented and set 
	 * in the getABTest method.
	 */
	public void ABTestGetTestDiff(){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			abtestGetTestDiff();
		}else{
		}
	}
	
	/**
	 * For development purposes only. Send a request to retrieve the test differences for 
	 * this user. The differences will be sent asynchronously to the ABTestDelegate implemented 
	 * and set in the getABTest method.
	 * @param	user	The user to force the different variants of the A/B test.
	 */
	public void ABTestGetTestDiffForUser(string user){
		if(Application.platform == RuntimePlatform.IPhonePlayer){
			abtestGetTestDiffForUser(user);
		}else{
		}
	}
	
	/**
	 * Mark a particular resource as a success with the parameters that lead to that success.
	 * This method should be called with the exact name of the resource and also it must
	 * be called with the control version to be able to compare results.
	 * @param	name	The name of the resource to mark as a successful one.
	 * @param	params	A dictionary with extra parameters relevant for the analysis of the action called.
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
	
	private void _TrackExtendedDidStart(){
		if(trackExtendedListener!=null){
			trackExtendedListener.TrackExtendedDidStart();
		}
	}
	
	private void _TrackExtendedDidStop(){
		if(trackExtendedListener!=null){
			trackExtendedListener.TrackExtendedDidStop();
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
}
