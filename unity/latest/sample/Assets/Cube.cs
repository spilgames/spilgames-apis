using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using Spil;
using LitJson;

public class Cube : MonoBehaviour,SpilAppSettingsListener,SpilAdsListener,SpilABTestListener,SpilInGameAdsListener {
	SpilUnity instance;
	Vector3 rotation = Vector3.zero;
	// Use this for initialization
	void Start () {
		Debug.Log("starting initialization");
		instance = SpilUnity.Instance;
		
		SpilSettings configs = new SpilSettings();
		configs.SG_ENVIRONMENT_KEY = Enviroment.SG_ENVIRONMENT_DEV_VALUE;
		configs.SG_APP_SETTINGS_POLL_TIME_KEY = 10.0f;
		configs.SG_ENVIRONMENT_SETTINGS_URL_GET = "http://localhost/defaultsettings.json";
		configs.SG_TRACKING_ID_KEY="<tracking-app-ids>";

		#if UNITY_IPHONE
		configs.SG_STORE_ID = Store.SG_STORE_IOS;
		#endif
		#if UNITY_ANDROID
		configs.SG_STORE_ID = Store.SG_STORE_GOOGLE_PLAY;
		#endif

		instance.Initialize("<spil-app-id>", "<spil-auth-token>", configs);

		//start app settings
		instance.SetAppSettingsListener(this);

		//start ads
		instance.SetAdsListener(this);

		//start A/B test
		instance.SetABTestListener(this);

		//start InGameAds
		instance.SetInGameAdListener(this);


	}
	
	int clickCount = 0;
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(rotation);
		
		if(Input.touchCount != 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Ended){
			clickCount++;
		}
	}
	
	//have to be implemented by the developers
	public void AppSettingsDidLoad(JsonData data){
		Debug.Log(data.ToString());
	}
	
	public void AppSettingsDidFailWithError(string error){
		Debug.LogError(error);
	}
	
	public void AppSettingsDidStartDownload(){
		Debug.Log("Downloading settings");
	}
	
	public void AdDidStart(){
		Debug.Log("started adds");
		//instance.AdsRequestIngameAsset(Orientation.SG_LANDSCAPE);
		instance.AdsNextInterstitial();
	}
	public void AdDidFailToStart(string error){
		Debug.LogError(error);
	}
	
	public void AdWillAppear(){
		Debug.Log("will appear");
	}
	public void AdDidAppear(){
		Debug.Log("appeared");
	}
	public void AdDidFailToAppear(string error){
		Debug.LogError("ad fail to load");
	}
	public void AdPopupDidDismiss(){
		Debug.Log("popup was dismissed");
	}
	
	public void AdMoreGamesWillAppear(){
		Debug.Log("more games will appear");
	}
	public void AdMoreGamesDidAppear(){
		Debug.Log("more games appeared");
	}
	public void AdMoreGamesDidFailToAppear(string error){
		Debug.LogError(error);
	}
	public void AdMoreGamesDidDismiss(){
		Debug.Log("more games was dismissed");
		//instance.AdsNextInterstitial();
	}
	
	public void ABTestSessionDidStart(){
		Debug.Log("starting abtest session");
		//instance.ABTestGetTestDiffForUser("B065BD21CC401");
	}
	
	public void ABTestSessionDidEnd(){
		
	}
	
	public void AdDidLoadIngameAsset(GameObject billboard){
		
		Debug.Log("AdDidLoadInGameAsset");
		billboard.transform.parent = this.gameObject.transform;
		billboard.transform.position = new Vector3(0,0,0);
	}
	  
	public void AdDidFailIngameAsset(string error){
		Debug.Log("ad fail "+ error);
	}
	
	public void ABTestSessionDiffReceived(JsonData diffs){
		if(diffs.Count == 0){
			Debug.Log("keep the control version");
			return;
		}
		
		for(int i=0,n=diffs.Count;i<n;i++){
			JsonData resource = diffs[i];
			string uid = (string)resource["uid"];
			IDictionary dic = (IDictionary)resource["diff"];
			
			foreach(string element in dic.Keys){
				string variant = (string)resource["diff"][element]["new"];
				string control = (string)resource["diff"][element]["old"];
		
				Debug.Log(uid+" "+variant+" vs "+control);
			}
		}
	}
}
