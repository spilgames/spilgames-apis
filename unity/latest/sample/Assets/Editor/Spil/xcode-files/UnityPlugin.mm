//
//  UnityPlugin.m
//  AppSettings
//
//  Created by Ignacio Calderon on 7/5/12.
//  Copyright (c) 2012 Spil Games. All rights reserved.
//


#ifndef UNITY_WRAPPER
#define UNITY_WRAPPER

#import <Foundation/Foundation.h>
#import <Spil/Spil.h>
#import <Spil/SpilHelpers.h>
#import "UnityPlugin.h"
#import "SpilUnityDelegate.h"

extern "C" {
	static SpilUnityDelegate* objDelegate;
	static Spil* spil;
	static char* objectName;
	
#pragma mark - Wrapper functions: Constructor	
	
	void initialize(const char* objName, const char* appID, const char* authToken, struct SpilSettings confs){
		if(spil == nil){
			objectName = (char*) malloc(sizeof(const char*) * strlen(objName));
			strcpy(objectName, objName);
			
			//WARNING: if the struct SpilSettings changes, change this initialization accordingly.
			NSMutableDictionary* configs = [NSMutableDictionary dictionary];
			
			//if(confs.environment == -2)	configs[SG_ENVIRONMENT_KEY] = SG_ENVIRONMENT_STG_VALUE;
			//if(confs.environment == -1)	configs[SG_ENVIRONMENT_KEY] = SG_ENVIRONMENT_DEBUG_VALUE;
			if(confs.environment == 0)	configs[SG_ENVIRONMENT_KEY] = SG_ENVIRONMENT_DEV_VALUE;
			if(confs.environment == 1)	configs[SG_ENVIRONMENT_KEY] = SG_ENVIRONMENT_LIVE_VALUE;
			if(confs.environment <= 0)
				configs[SG_APP_SETTINGS_POLL_TIME_KEY]=[NSNumber numberWithFloat:confs.appSettingsPollTime];
			
			if(confs.settingsGetURL!=NULL) 	configs[SG_ENVIRONMENT_SETTINGS_URL_GET]=[NSString stringWithUTF8String:confs.settingsGetURL];
			
			if(confs.trackingID!=NULL)	configs[SG_TRACKING_ID_KEY]=[NSString stringWithUTF8String:confs.trackingID];
			
			if(confs.store == 0) configs[SG_STORE_ID] = @"1";
			
			spil = [[Spil spilWithAppID:[NSString stringWithUTF8String:appID]
								 token:[NSString stringWithUTF8String:authToken] 
							   configs:configs] retain]; //has to be retained to avoid problems with side implementations of AdColony
			
			if(objDelegate == nil)
				objDelegate = [[SpilUnityDelegate alloc] init];
		}
	}
	
#pragma mark - Wrapper functions: App Settings	

	void setAppSettingsDelegate(){
		if(spil != nil){
			[spil setAppSettingsDelegate:objDelegate];
		}
	}

#pragma mark - Wrapper functions: Ads	
	
	void setAdsDelegate(){
		if(spil != nil){
			[spil setAdsDelegate:objDelegate];
		}
	}
	
	void adsEnabled(bool state){
		if(spil != nil){
			[spil adsEnabled:state];
		}
	}
	
	void adsShowMoreGames(){
		if(spil != nil){
			[spil adsShowMoreGames];
		}
	}	
	
	void adsNextInterstitial(){
		if(spil != nil){
			[spil adsNextInterstitial];
		}
	}

	void adsNextInterstitialWithLocation(const char* location){
		if(spil != nil){
			[spil adsNextInterstitial:[NSString stringWithCString:location encoding:NSUTF8StringEncoding]];
		}
	}

	void adsCacheNextInterstitial(){
		if(spil != nil){
			[spil adsCacheNextInterstitial];
		}
	}
	
	void adsCacheNextInterstitialWithLocation(const char* location){
		if(spil != nil){
			[spil adsCacheNextInterstitial:[NSString stringWithCString:location encoding:NSUTF8StringEncoding]];
		}
	}
	
	void setInGameAdsDelegate(){
		if(spil != nil){
			[spil setInGameAdsDelegate:objDelegate];
		}
	}
	
	void adsRequestInGameAdAssetWithLocation(int orientation, const char* location){
		if(spil != nil){
			[spil adsRequestInGameAdAsset:^(NSDictionary* data, NSError* error) {
					if(data != nil){
						NSString* url = nil;
						if(orientation == 0){
							url = data[@"urlLandscape"];
						}else{
							url = data[@"urlPortrait"];
						}
					
						if(url == nil){
							[objDelegate adDidFailToGetInGameAd:[NSError errorWithDomain:@"Ad doesn't contain an asset in the orientation requested" code:404 userInfo:nil]];
						}else{
							[objDelegate adDidGetInGameAd:(UIView*)@{@"url":url, @"adId":data[@"adId"], @"link":data[@"link"]}];
						}
					}
				
					if(error != nil){
						[objDelegate adDidFailToGetInGameAd:error];
					}
				}
			 atLocation:[NSString stringWithCString:location encoding:NSUTF8StringEncoding]];
		}
	}
	
	void adsRequestInGameAdAsset(int orientation){
		if(spil != nil){
			adsRequestInGameAdAssetWithLocation(orientation, "default");
		}
	}
	
	void adsMarkInGameAdAsShown(const char* adId){
		if(spil != nil){
			[spil adsMarkInGameAdAsShown:[NSString stringWithCString:adId encoding:NSUTF8StringEncoding]];
		}
	}
	
			
#pragma mark - Wrapper functions: Tracking
	
	void setExtendedTrackingDelegate(){
		if(spil!=nil)
			[spil setExtendedTrackingDelegate:objDelegate];
	}
	
	void trackPage(const char* page){
		if(spil!=nil)
			[spil trackPage:[NSString stringWithUTF8String:page]];
	}
	
	void trackEvent(const char* event){
		if(spil!=nil)
			[spil trackEvent:[NSString stringWithUTF8String:event]];
	}
	
	void trackEventDetailed(const char* category, const char* action, const char* label, int value){
		if(spil!=nil)
			[spil trackEvent:[NSString stringWithUTF8String:category] 
					  action:[NSString stringWithUTF8String:action] 
					   label:[NSString stringWithUTF8String:label] 
					   value:value];
	}
	
	void trackEventWithParameters(const char* event, const char* keys[], const char* values[], int size){
		if(spil!=nil){
			NSMutableArray* nskeys = [NSMutableArray arrayWithCapacity:size];
			NSMutableArray* nsvalues = [NSMutableArray arrayWithCapacity:size];
			
			for(int i=0;i<size;i++){
				[nskeys addObject:[NSString stringWithUTF8String:keys[i]]];
				[nsvalues addObject:[NSString stringWithUTF8String:values[i]]];
			}
			
			//convert the keys and values to arrays of nsstrings
			[spil trackEvent:[NSString stringWithUTF8String:event] 
				  withParams:[NSDictionary dictionaryWithObjects:nsvalues forKeys:nskeys]];
		}
	}
	
	void trackTimedEvent(const char* event){
		if(spil!=nil)
			[spil trackTimedEvent:[NSString stringWithUTF8String:event]];
	}
	
	void trackEndTimedEvent(const char* event){
		if(spil!=nil)
			[spil trackEndTimedEvent:[NSString stringWithUTF8String:event]];
	}
	
	void trackEndTimedEventWithParameters(const char* event, const char* keys[], const char* values[], int size){
		if(spil!=nil){
			NSMutableArray* nskeys = [NSMutableArray arrayWithCapacity:size];
			NSMutableArray* nsvalues = [NSMutableArray arrayWithCapacity:size];
			
			for(int i=0;i<size;i++){
				[nskeys addObject:[NSString stringWithUTF8String:keys[i]]];
				[nsvalues addObject:[NSString stringWithUTF8String:values[i]]];
			}
			
			[spil trackEndTimedEvent:[NSString stringWithUTF8String:event]
						  withParams:[NSDictionary dictionaryWithObjects:nsvalues forKeys:nskeys]];
		}
	}
	
	void trackError(const char* event, const char* message, const char* exception){
		if(spil!=nil)
			[spil trackError:[NSString stringWithUTF8String:event]
					 message:[NSString stringWithUTF8String:message] 
				   exception:[NSException exceptionWithName:@"exception" reason:[NSString stringWithUTF8String:exception] userInfo:nil]];
	}
	
	void trackUserID(const char* userID){
		if(spil!=nil)
			[spil trackUserID:[NSString stringWithUTF8String:userID]];
	}
	
	void trackAge(int age){
		if(spil!=nil)
			[spil trackAge:age];
	}
	
	void trackGender(BOOL male){
		if(spil!=nil)
			[spil trackGender:male];
	}
	
	void trackLatitude(double latitude,double longitude,double horizontalAccuracy,double verticalAccuracy){
		if(spil!=nil)
			[spil trackLatitude:latitude
					  longitude:longitude
			 horizontalAccuracy:horizontalAccuracy 
			   verticalAccuracy:verticalAccuracy];
	}
	
	void trackStartGestureScreen(const char* screenName){
		if(spil != nil)
			[spil trackStartGestureScreen:[NSString stringWithUTF8String:screenName]];
	}
	
	void trackStopGestureScreen(){
		if(spil != nil)
			[spil trackStopGestureScreen];
	}
	
#pragma mark - Wrapper functions: A/B Test
	
	void setABTestDelegate(){
		if(spil!=nil)
			[spil setABTestDelegate:objDelegate];
	}
	
	void abtestUpdateUserInfo(){
		if(spil!=nil)
			[spil abtestUpdateUserInfo];
	}
	
	void abtestUpdateUserInfoWith(const char* keys[], const char* values[], int size){
		if(spil!=nil){
			NSMutableArray* nskeys = [NSMutableArray arrayWithCapacity:size];
			NSMutableArray* nsvalues = [NSMutableArray arrayWithCapacity:size];
			
			for(int i=0;i<size;i++){
				[nskeys addObject:[NSString stringWithUTF8String:keys[i]]];
				[nsvalues addObject:[NSString stringWithUTF8String:values[i]]];
			}
			
			[spil abtestUpdateUserInfoWith:[NSDictionary dictionaryWithObjects:nsvalues forKeys:nskeys]];
		}
	}
	
	void abtestGetTestDiff(){
		if(spil!=nil)
			[spil abtestGetTestDiff];
	}
	
	void abtestGetTestDiffForUser(const char* user){
		if(spil!=nil)
			[spil abtestGetTestDiffForUser:[NSString stringWithUTF8String:user]];
	}
	
	void abtestMarkSucceedTest(const char* name, const char* keys[], const char* values[], int size){
		if(spil!=nil){
			NSMutableArray* nskeys = [NSMutableArray arrayWithCapacity:size];
			NSMutableArray* nsvalues = [NSMutableArray arrayWithCapacity:size];
			
			for(int i=0;i<size;i++){
				[nskeys addObject:[NSString stringWithUTF8String:keys[i]]];
				[nsvalues addObject:[NSString stringWithUTF8String:values[i]]];
			}
			
			[spil abtestMarkSucceedTest:[NSString stringWithUTF8String:name]
						 withParameters:[NSDictionary dictionaryWithObjects:nsvalues forKeys:nskeys]];
		}
	}

#pragma mark - Wrapper functions: SGHelpers
	
	const char* getUDID(){
		return [[SpilHelpers getUDID] UTF8String];
	}
	
	const char* getAppVersion(){
		return [[SpilHelpers getAppVersion] UTF8String];
	}
	
	const char* getAppName(){
		return [[SpilHelpers getAppName] UTF8String];
	}
	
#pragma mark - Private methods
	
	const char* getObjectName(){ return objectName; }
}

#endif
