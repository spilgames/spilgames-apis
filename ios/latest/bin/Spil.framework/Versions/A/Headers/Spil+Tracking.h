//
//  Spil+Tracking.h
//  Spil
//
//  Created by Ignacio Calderon on 11/6/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import "Spil+Core.h"

@interface Spil (Tracking)

/**
 * Tracks a particular page. It can be used to keep track of the current screen separetely from the events.
 * If the session is not started yet, this request is ignored.
 * @param	page	The page name/url to track.
 */
-(void) trackPage:(NSString*)page;

/**
 * Tracks an event. The event could be actions taken on some object like unlocking an achievement, or a getting a hiscore.
 * @param	event	The event to track.
 */
-(void) trackEvent:(NSString*)event;

/**
 * Tracks an event under a particular category. The parameters match with the google analytics' ones. For flurry, a event with
 * parameters is issued, where the category is the event name.
 * @param	category	The category for this event.
 * @param	action		The action took on that category.
 * @param	label		Optional. A string label to especify something about the action.
 * @param	value		Optional. A integer value. useful to especify error codes.
 */
-(void) trackEvent:(NSString*)category action:(NSString*)action label:(NSString*)label value:(int)value;

/**
 * Tracks an event with particular parameters. This match the Flurry's logEvent:withParameters:
 * @param	event		The event to track
 * @param	params		Additional parameters to attach to the event.
 */
-(void) trackEvent:(NSString *)event withParams:(NSDictionary*)params;

/**
 * Tracks an event that should end in a finite time. If the tracking system support it natively the equivalent
 * method will be used. Otherwise, an event with the start timestamp(epoch) is issued.
 * @param	event	The event to track.
 */
-(void) trackTimedEvent:(NSString*)event;

/**
 * Tracks the end of an event that was started. If the tracking system support it natively the equivalent method will be used.
 * Otherwise, an event with the end timestamp(epoch) is issued.
 * @see trackTimedEvent:
 * @param	event	The event to track. Should match with the starting event.
 * @param	params	The parameters when the event was finished.
 */
-(void) trackEndTimedEvent:(NSString*)event withParams:(NSDictionary*)params;

/**
 * Tracks the end of an event that was started. If the tracking system support it natively the equivalent method will be used.
 * Otherwise, an event with the end timestamp(epoch) is issued.
 * @see trackTimedEvent:
 * @param	event	The event to track. Should match with the starting event.
 */
-(void) trackEndTimedEvent:(NSString*)event;

/**
 * Tracks an error/crashes that has occured in the application. This errors appear in special sections of the analytics dashboards,
 * therefore only use them to reflect app crashes or fatal errors. Send minor warnings with this method will clutter the ability to
 * detect actual crashes causes.
 * @param	event	The event to track.
 * @param	msg	The message with the detail of the error.
 * @param	exception	The exception that causes the error.
 */
-(void) trackError:(NSString*)event message:(NSString*)msg exception:(NSException*)exception;

@end
