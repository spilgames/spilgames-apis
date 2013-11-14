using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spil;
using System;
using LitJson;

#if UNITY_ANDROID
public class SpilAndroid
{
	
	public static AndroidJavaObject instance;
	
	public SpilAndroid (string objectName, string appID, string authToken, SpilSettings configs)
	{
		
		Dictionary<string, string> parameters = new Dictionary<string, string> ();
		parameters.Add ("SG_ENVIRONMENT_KEY", configs.SG_ENVIRONMENT_KEY.ToString ());
		parameters.Add ("SG_STORE_ID", configs.SG_STORE_ID.ToString ());
		
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject> ("currentActivity"); 
		
		using (AndroidJavaObject obj_HashMap = DictionaryToHashMap(parameters)){
			using (var pluginClass = new AndroidJavaClass( "com.spilgames.framework.core.Spil" ))
				
				instance = pluginClass.CallStatic<AndroidJavaObject> ("spilWithAppID",activity, appID, authToken, obj_HashMap);
				instance.Call ("setUnityObjectName", objectName);
		}	
		
	
	}
	
	public void TrackPage (string page)
	{
		instance.Call ("trackPage", page);	
	}
	
	public void TrackEvent (string evt)
	{
		instance.Call ("trackEvent", evt);	
	}
	
	public void TrackEvent (string category, string action, string label, long value)
	{
		instance.Call ("trackEvent", category, action, label, value);	
	}
	
	public void TrackEvent (string category, string action, string label, long value, Dictionary<string, string> parameters)
	{
		using (AndroidJavaObject obj_HashMap = DictionaryToHashMap(parameters)){
			instance.Call ("trackEvent", category, action, label, value, obj_HashMap);
		}	
	}
	
	public void TrackEvent (string evt, Dictionary<string, string> parameters)
	{
		using (AndroidJavaObject obj_HashMap = DictionaryToHashMap(parameters)){
			instance.Call ("trackEvent", evt , obj_HashMap);
		}	
	}
	
	public void TrackTimedEvent (string evt)
	{
		instance.Call ("trackTimedEvent", evt);	
	}
	
	public void TrackEndTimedEvent (string evt)
	{
		instance.Call ("trackEndTimedEvent", evt);	
	}
	
	public void TrackEndTimedEvent (string evt, Dictionary<string, string> parameters)
	{
		using (AndroidJavaObject obj_HashMap = DictionaryToHashMap(parameters)){
			instance.Call ("trackEndTimedEvent", evt , obj_HashMap);
		}	
	}
	
	public void TrackError (string evt, string msg, Exception ex)
	{
		instance.Call ("trackError", evt, msg, ex.ToString());	
	}
	
	public void TrackUserId (string userId)
	{
		instance.Call ("trackUserId", userId);	
	}
	
	public void AdsNextInterstitial ()
	{
		instance.Call ("showInterstitial");	
	}
	
	public void AdsNextInterstitial (string location)
	{
		instance.Call ("showInterstitial", location);	
	}
	
	public void AdsRequestInGameAssetWithLocation (Orientation orient, string location)
	{
		instance.Call ("requestInGameAd", orient.ToString(), location);	
	}
	
	public void AdsRequestInGameAsset (Orientation orient)
	{
		instance.Call ("requestInGameAd", orient.ToString());	
	}
	
	public void NotifyInGameAd (string adId)
	{
		instance.Call ("notifyIngameAd", adId);	
	}
	
	public void ShowMoreGames ()
	{
		instance.Call ("showMoreGames");	
	}
	
	
	public void StartAdsSession ()
	{
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject> ("currentActivity"); 
		instance.Call ("onAdsStart", activity);	
	}
	
	
	private AndroidJavaObject DictionaryToHashMap (Dictionary<string, string> dictionary)
	{
	
		    AndroidJavaObject obj_HashMap = new AndroidJavaObject("java.util.HashMap");
			IntPtr method_Put = AndroidJNIHelper.GetMethodID (obj_HashMap.GetRawClass (), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			object[] args = new object[2];
			foreach (KeyValuePair<string, string> kvp in dictionary) {
				using (AndroidJavaObject k = new AndroidJavaObject("java.lang.String", kvp.Key)) {
					using (AndroidJavaObject v = new AndroidJavaObject("java.lang.String", kvp.Value)) {
						args [0] = k;
						args [1] = v;
						AndroidJNI.CallObjectMethod (obj_HashMap.GetRawObject (), method_Put, AndroidJNIHelper.CreateJNIArgArray (args));
					}
				}				
			}
			return obj_HashMap;			
		
	}
	
}
#endif
