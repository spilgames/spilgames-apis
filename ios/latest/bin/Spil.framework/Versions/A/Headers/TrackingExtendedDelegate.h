//
//  TrackingExtendedDelegate.h
//  Spil
//
//  Created by Ignacio Calderon on 3/7/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import <Foundation/Foundation.h>

/**
 * Protocol to handle the responses from the Extended Tracking subsystem
 */
@protocol TrackingExtendedDelegate <NSObject>

/**
 * Method to call back when the any of the extended trackers are started.
 * If the camera tracker is set up this method is called after the confirmation pop up is done, and 
 * if there is at least one tracker active. When the camera tracker is not set up, this method is
 * called when any of the other are activated.
 */
-(void) trackExtendedDidStart;

/**
 * Method to call back when ALL the extended trackers are stopped.
 * This is an informative call.
 */
-(void) trackExtendedDidStop;

@end