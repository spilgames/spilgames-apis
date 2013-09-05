package com.spilgames.examples.ui;

import java.util.HashMap;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

import com.spilgames.examples.R;
import com.spilgames.framework.SpilInterface;
import com.spilgames.framework.SpilLink;
import com.spilgames.framework.environment.DevEnvironment;
import com.spilgames.framework.environment.DevStores;
import com.spilgames.framework.listeners.AdsListener;

public class MainActivity extends Activity implements OnClickListener, AdsListener {

	SpilInterface spilInstance;
	private String appId = "13";
	private String authToken = "6512bd43d9caa6e02c990b0a82652dca";

	private static final String TRACKING_TAG = "ANDROID_TRACK:";

	Button trackPageButton;
	Button trackEventButton;
	Button trackTimeStart;
	Button trackTimeEnds;
	Button trackErrorButton;
	Button trackUserButton;
	Button trackGenderButton;
	Button trackAgeButton;
	Button newScreenButton;


  
	@Override
	public void onCreate(Bundle savedInstanceState) {
		setContentView(R.layout.activity_main);
		super.onCreate(savedInstanceState);
		HashMap<String, String > configs = new HashMap<String, String>();
		configs.put(DevEnvironment.SG_ENVIRONMENT_KEY, DevEnvironment.SG_ENVIRONMENT_STG_VALUE.getValue());
		configs.put(DevStores.SG_STORE_ID, DevStores.SG_STORE_GOOGLE_PLAY.getValue());
		spilInstance = SpilLink.spilWithAppID(getApplicationContext(),appId, authToken, configs);
		
		spilInstance.setAdsListener(this);
		
		trackPageButton = (Button) findViewById(R.id.pageButton);
		trackEventButton = (Button) findViewById(R.id.eventButton);
		trackTimeStart = (Button) findViewById(R.id.timeEventStart);
		trackTimeEnds = (Button) findViewById(R.id.timeEventEnd);
		trackErrorButton = (Button) findViewById(R.id.errorButton);
		trackUserButton = (Button) findViewById(R.id.userButton);
		trackGenderButton = (Button) findViewById(R.id.genderButton);
		trackAgeButton = (Button) findViewById(R.id.ageButton);
		newScreenButton = (Button) findViewById(R.id.newScreen);
		
		trackPageButton.setOnClickListener(this);
		trackEventButton.setOnClickListener(this);
		trackTimeStart.setOnClickListener(this);
		trackTimeEnds.setOnClickListener(this);
		trackErrorButton.setOnClickListener(this);
		trackUserButton.setOnClickListener(this);
		trackGenderButton.setOnClickListener(this);
		trackAgeButton.setOnClickListener(this);
		newScreenButton.setOnClickListener(this);

	}



	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.pageButton:
			spilInstance.trackPage(TRACKING_TAG+"New_game");
			spilInstance.showInterstitial();
			break;
		case R.id.eventButton:
			spilInstance.trackEvent(TRACKING_TAG+"Help_pressed");
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
}
