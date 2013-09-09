package com.spilgames.examples.ui;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.widget.RelativeLayout;

import com.spilgames.examples.R;
import com.spilgames.framework.SpilInterface;
import com.spilgames.framework.SpilLink;
import com.spilgames.framework.environment.InGameAdView;
import com.spilgames.framework.listeners.InGameAdListener;

public class SecondActivity extends Activity implements InGameAdListener{

	RelativeLayout rootLayout;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_second);
		SpilInterface spil = SpilLink.getInstance();
		rootLayout = (RelativeLayout) findViewById(R.id.root);
		spil.setInGameAdsListener(this);
		spil.requestInGameAd("PORTRAIT");
		
	}

	@Override
	public void onInGameAdRetrieved(InGameAdView inGameAdsView) {
		System.out.println("InGameAdsReturned");
		rootLayout.addView(inGameAdsView);
		inGameAdsView.showAd();
	}

	@Override
	public void onInGameAdError(String error) {
		// TODO Auto-generated method stub
	}
}
