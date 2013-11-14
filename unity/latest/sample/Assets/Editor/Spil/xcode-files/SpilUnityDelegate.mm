//
//  SpilUnityDelegate.m
//  Spil
//
//  Created by Ignacio Calderon on 7/17/12.
//  Copyright (c) 2012 Spil Games. All rights reserved.
//

#import <Spil/Spil.h>
#import <Spil/JSON.h>
#import "UnityPlugin.h"
#import "SpilUnityDelegate.h"

extern "C" {
	void UnitySendMessage(const char* name, const char* method, const char* params);
};

@implementation SpilUnityDelegate

#pragma mark - AppSettingsDelegate methods

-(void) appSettingsDidLoad:(NSDictionary*)settings{
	UnitySendMessage(getObjectName(), "_AppSettingsDidLoad", [[[SBJSON new] stringWithObject:settings] UTF8String]); //serialize the dictionary and send to the unityObject
}

-(void) appSettingsDidFailWithError:(NSError*)error{
	UnitySendMessage(getObjectName(), "_AppSettingsDidFailWithError", [[error description] UTF8String]); //serialize the object as an string.
}

-(void) appSettingsDidStartDownload{
	UnitySendMessage(getObjectName(), "_AppSettingsDidStartDownload", "");
}

#pragma mark - AdsDelegate methods

-(void) adDidStart{
	UnitySendMessage(getObjectName(), "_AdDidStart", "");
}
-(void) adDidFailToStart:(NSError*)error{
	UnitySendMessage(getObjectName(), "_AdDidFailToStart", [[error description] UTF8String]); //serialize the object as an string.
}

-(void) adWillAppear{
	UnitySendMessage(getObjectName(), "_AdWillAppear", "");
}
-(void) adDidAppear{
	UnitySendMessage(getObjectName(), "_AdDidAppear", "");
}
-(void) adDidFailToAppear:(NSError*)error{
	UnitySendMessage(getObjectName(), "_AdDidFailToAppear", [[error description] UTF8String]); //serialize the object as an string.
}
-(void) adPopupDidDismiss{
	UnitySendMessage(getObjectName(), "_AdPopupDidDismiss", "");
}

-(void) adMoreGamesWillAppear{
	UnitySendMessage(getObjectName(), "_AdMoreGamesWillAppear", "");
}
-(void) adMoreGamesDidAppear{
	UnitySendMessage(getObjectName(), "_AdMoreGamesDidAppear", "");
}
-(void) adMoreGamesDidFailToAppear:(NSError*)error{
	UnitySendMessage(getObjectName(), "_AdMoreGamesDidFailToAppear", [[error description] UTF8String]); //serialize the object as an string.
}
-(void) adMoreGamesDidDismiss{
	UnitySendMessage(getObjectName(), "_AdMoreGamesDidDismiss", "");
}

-(void) adDidGetInGameAd:(UIView*)image{
	//this is an abuse of notation, the parameter received here is not actually an UIView but a NSDictionary
	UnitySendMessage(getObjectName(), "_AdDidLoadIngameAsset", [[[SBJSON new] stringWithObject:image] UTF8String]); //serialize the object as an string.
}

-(void) adDidFailToGetInGameAd:(NSError*)error{
	UnitySendMessage(getObjectName(), "_AdDidFailIngameAsset", [[error description] UTF8String]); //serialize the object as an string.
}

#pragma mark - ABTestDelegate methods

-(void) abtestSessionDidStart{
	UnitySendMessage(getObjectName(), "_ABTestSessionDidStart", "");
}

-(void) abtestSessionDidEnd{
	UnitySendMessage(getObjectName(), "_ABTestSessionDidEnd", "");
}

-(void) abtestSessionDiffReceived:(NSArray*)diffs{
	UnitySendMessage(getObjectName(), "_ABTestSessionDiffReceived", [[[SBJSON new] stringWithObject:diffs] UTF8String]);
}

#pragma mark - Other delegates
/*
 useful links: 
 http://www.tinytimgames.com/2010/01/10/the-unityobjective-c-divide/
 http://forum.unity3d.com/threads/122875-Calling-back-into-the-mono-runtime-from-iOS-plugin
//*/
@end