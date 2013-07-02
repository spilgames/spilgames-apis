//
//  AdsDelegate.h
//  Spil
//
//  Created by Ignacio Calderon on 7/20/12.
//  Copyright (c) 2012 Spil Games. All rights reserved.
//

#import <Foundation/Foundation.h>

/**
 * Protocol to handle the events triggered by the ad subsystem.
 */
@protocol AdsDelegate <NSObject>

/**
 * Method to call back after the ad subsystem is successfully started.
 */
-(void) adDidStart;

/**
 * Method to call back after if the ad subsystem couldn't be started due to any reason.
 * @param	error	The reason why the ad subsystem failed to start.
 */
-(void) adDidFailToStart:(NSError*)error;

/**
 * Method to call back before the next ad is going to be displayed. This method is called
 * every time the timer reach 0, regardless if the ad should be shown or not (enableAds is set to NO).
 */
-(void) adWillAppear;

/**
 * Method to call back after the ad is displayed. This method is only called if the ads are enabled to 
 * be displayed (enableAds:YES).
 */
-(void) adDidAppear;

/**
 * Method to call back if the ad couldn't be displayed due to any reason.
 * @param	error	The reason why the ad failed to be displayed.
 */
-(void) adDidFailToAppear:(NSError*)error;

/**
 * Method to call back before the next more games' screen is going to be shown.
 */
-(void) adMoreGamesWillAppear;

/**
 * Method to call back after the more games' screen is displayed.
 */
-(void) adMoreGamesDidAppear;

/**
 * Method to call back if the more games' screen couldn't be displayed due to any reason.
 * @param	error	The reason why the more games' screen failed to be displayed.
 */
-(void) adMoreGamesDidFailToAppear:(NSError*)error;

/**
 * Method to call back if the ad popup showed was dismissed.
 */
-(void) adPopupDidDismiss;

/**
 * Method to call back if the more games popup showed was dismissed.
 */
-(void) adMoreGamesDidDismiss;
@end