//
//  InGameAdsDelegate.h
//  Spil
//
//  Created by Ignacio Calderon on 8/30/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol InGameAdsDelegate <NSObject>

/**
 * Method to callback when a in game ad has been retrieved from the server.
 * This view will handle the display event and will mark the advert as shown.
 * @param	image	A UIView that will respond to the events when it's displayed and clicked.
 */
-(void) adDidGetInGameAd:(UIView*)image;

/**
 * Method to callback when an error happened trying to retrieve the ad from the server.
 * @param	error	An error code describing the cause of the error.
 */
-(void) adDidFailToGetInGameAd:(NSError*)error;

@end
