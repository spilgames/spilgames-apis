package com.spilgames.examples.ui;

import java.util.HashMap;

import org.json.JSONObject;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

import com.spilgames.examples.R;
import com.spilgames.framework.SpilInterface;
import com.spilgames.framework.core.Spil;
import com.spilgames.framework.core.listeners.AdsListener;
import com.spilgames.framework.core.listeners.AppSettingsListener;
import com.spilgames.framework.core.utils.DebugLogConfig;
import com.spilgames.framework.environment.DevEnvironment;
import com.spilgames.framework.environment.DevStores;

public class MainActivity extends Activity implements OnClickListener, AdsListener, AppSettingsListener {

	SpilInterface spilInstance;
	private String appId = "13";
	private String authToken = "6512bd43d9caa6e02c990b0a82652dca";

	private static final String TRACKING_TAG = "JBC:";

	Button trackPageButton;
	Button trackEventButton;
	Button trackTimeStart;
	Button trackTimeEnds;
	Button trackErrorButton;
	Button trackUserButton;
	Button moreGamesButton;
	Button trackAgeButton;
	Button newScreenButton;


  
	@Override
	public void onCreate(Bundle savedInstanceState) {
		setContentView(R.layout.activity_main);
		super.onCreate(savedInstanceState);
		HashMap<String, String > configs = new HashMap<String, String>();
		configs.put(DevEnvironment.SG_ENVIRONMENT_KEY, DevEnvironment.SG_ENVIRONMENT_STG_VALUE.getValue());
		configs.put(DevStores.SG_STORE_ID, DevStores.SG_STORE_GOOGLE_PLAY.getValue());
		DebugLogConfig.enable();
		spilInstance = Spil.spilWithAppID(getApplicationContext(),appId, authToken, configs);
		
		spilInstance.setAdsListener(this);
		spilInstance.setSettingsListener(this);
		
		trackPageButton = (Button) findViewById(R.id.pageButton);
		trackEventButton = (Button) findViewById(R.id.eventButton);
		trackTimeStart = (Button) findViewById(R.id.timeEventStart);
		trackTimeEnds = (Button) findViewById(R.id.timeEventEnd);
		trackErrorButton = (Button) findViewById(R.id.errorButton);
		trackUserButton = (Button) findViewById(R.id.userButton);
		moreGamesButton = (Button) findViewById(R.id.moreGamesButton);
		trackAgeButton = (Button) findViewById(R.id.ageButton);
		newScreenButton = (Button) findViewById(R.id.newScreen);
		
		trackPageButton.setOnClickListener(this);
		trackEventButton.setOnClickListener(this);
		trackTimeStart.setOnClickListener(this);
		trackTimeEnds.setOnClickListener(this);
		trackErrorButton.setOnClickListener(this);
		trackUserButton.setOnClickListener(this);
		moreGamesButton.setOnClickListener(this);
		trackAgeButton.setOnClickListener(this);
		newScreenButton.setOnClickListener(this);

	}



	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.pageButton:
			spilInstance.trackPage(TRACKING_TAG+"Juego_Nuevo");
			spilInstance.showInterstitial();
			break;
		case R.id.eventButton:
			spilInstance.trackEvent(TRACKING_TAG+"Ayuda");
			break;
		case R.id.timeEventStart:
			spilInstance.trackTimedEvent(TRACKING_TAG+"Level_40");
			break;
		case R.id.timeEventEnd:
			spilInstance.trackEndTimedEvent(TRACKING_TAG+"Level_40");
			break;
		case R.id.errorButton:
			try{ 
				int numberException = 10/0;
			}catch (Exception e) {
				spilInstance.trackError(TRACKING_TAG, "On_click_button", e.getMessage());
			}
			break;
		case R.id.userButton:
			spilInstance.trackUserId(TRACKING_TAG+"bea.guido");
			break;
		case R.id.moreGamesButton:
			spilInstance.showMoreGames();
			break;	
		case R.id.newScreen:
			Intent intent = new Intent(this, SecondActivity.class);
			startActivity(intent);
			break;
		default:
			break;
		}

	}

	@Override
	public void onAdsLoaded() {
		System.out.println("Adds loaded!!");
		spilInstance.showInterstitial();
	}

	@Override
	public void onAdsFailedToLoad(String cause) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onAppSettingsDidLoad(JSONObject settings) {
		System.out.println("APP SETTINGS "+settings.toString());
	}



	@Override
	public void onAppSettingsDidFailWithError(String error) {
		System.out.println("APP SETTINGS ERROR "+error);
		
	}



	@Override
	public void adMoreGamesWillAppear() {
		System.out.println("AdMore games WILL appear!!!");
		
	}



	@Override
	public void adMoreGamesDidAppear() {
		System.out.println("AdMore games DID appear!!!");		
	}



	@Override
	public void adMoreGamesDidFailToAppear(String error) {
		System.out.println("AdMore games FAIL appear!!!" + error);
		
	}



	@Override
	public void adMoreGamesDidDismiss() {
		System.out.println("AdMore games DISMISS!!!");
		
	}

	@Override
	public void adInterstitialFailed(String error) {
		System.out.println("ERROR with AD"+error);
		
	}

	@Override
	public void adInterstitialIsShown() {
		System.out.println("AD OK!!!");
		
	}
}
