using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using Spil;
using LitJson;

public class Cube : MonoBehaviour,SpilAppSettingsListener,SpilAdsListener,SpilABTestListener,SpilTrackingExtendedListener {
	SpilUnity instance;
	Vector3 rotation = Vector3.zero;
	// Use this for initialization
	void Start () {
		Debug.Log("starting initialization");
		instance = SpilUnity.Instance;
		
		SpilSettings configs;
		configs.SG_ENVIRONMENT_KEY = enviroment.SG_ENVIRONMENT_DEV_VALUE;
		configs.SG_APP_SETTINGS_POLL_TIME_KEY = 10.0f;
		configs.SG_ENVIRONMENT_SETTINGS_URL_GET = "http://localhost/defaultsettings.json";
		configs.SG_TRACKING_ID_KEY="<tracking-app-ids>";
		
		instance.Initialize("<spil-app-id>", "<spil-auth-token>", configs);
		
		//start app settings
		instance.GetSettings(this);
		
		//start ads
		instance.StartAds(this);
		
		//start A/B test
		instance.GetABTest(this);
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(rotation);
	}
	
	//have to be implemented by the developers
	public void AppSettingsDidLoad(JsonData data){
		renderer.material.color = new Color((((int)data["color"]>>16)&0x000000ff)/255.0f,
											(((int)data["color"]>>8)&0x000000ff)/255.0f,
											(((int)data["color"])&0x000000ff)/255.0f);
		rotation = new Vector3(	(int)data["rotation"]["x"],
								(int)data["rotation"]["y"],
								(int)data["rotation"]["z"]);
	}
	
	public void AppSettingsDidFailWithError(string error){
		Debug.LogError(error);
	}
	
	public void AppSettingsDidStartDownload(){
		Debug.Log("Downloading settings");
	}
	
	public void AdDidStart(){
		Debug.Log("started adds");
		instance.AdsPlaceAdAtPosition(0,0,100,200);
		
		instance.AdsPlaceAdAtPosition(100,0,200,100);
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
		Debug.LogError(error);
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
		instance.AdsNextInterstitial();
	}
	
	public void ABTestSessionDidStart(){
		Debug.Log("starting abtest session");
		instance.ABTestGetTestDiffForUser("B065BD21CC401");
	}
	
	public void ABTestSessionDidEnd(){
		
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
	
	public void TrackExtendedDidStart(){
		Debug.Log("starting screen");
		instance.TrackStartGestureScreen("main");
	}
	
	public void TrackExtendedDidStop(){
		Debug.Log("stop screen");
	}
}
