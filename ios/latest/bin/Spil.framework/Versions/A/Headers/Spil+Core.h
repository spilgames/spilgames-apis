//
//  Spil+Core.h
//  Spil
//
//  Created by Ignacio Calderon on 7/6/12.
//  Copyright (c) 2012 Spil Games. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>

#import "PublicHeaders.h"

/**
 * Public interface to the funcionalities of the Spil iOS framework.
 */
@interface Spil : NSObject

/**
 * Creates a Spil object singleton with an application ID and authentication token that will be used along the framework
 * for multiple services.
 * Also you must specify some configurations to control the behaivor of the framework. Most important, if the framework
 * should act like a development environment or a production environment.
 * Some validations about the configurations are made, if one fails, a nil object is returned, and the error is written 
 * in the console log.
 * @param	applicationID	The application ID provided by Spil Games, it can't be nil.
 * @param	authenticationToken	The authentication token provided by Spil Games, it can't be nil.
 * @param	configurations	A dictionary with the posible settings to be used by spil framework.
 * @return	The Spil object that will be use for further calls.
 */
+(Spil*) spilWithAppID:(NSString*)applicationID token:(NSString*)authenticationToken configs:(NSDictionary*)configurations;

/**
 * Creates a Spil object singleton with an application ID and authentication token that will be used along the framework
 * for multiple services.
 * Also you must specify some configurations to control the behaivor of the framework. Most important, if the framework
 * should act like a development environment or a production environment.
 * Some validations about the configurations are made, if one fails, a nil object is returned, and the error is written
 * in the console log.
 * @param	applicationID	The application ID provided by Spil Games, it can't be nil.
 * @param	authenticationToken	The authentication token provided by Spil Games, it can't be nil.
 * @param	configurations	A dictionary with the posible settings to be used by spil framework.
 * @param   onsuccess   Callback to inform that the settings where downloaded from the server and everything is ready to be used.
 * @param   onfailure   Callback to inform about an error from the server during the download of the settings.
 * @return	The Spil object that will be use for further calls.
 */
+(Spil*) spilWithAppID:(NSString*)applicationID token:(NSString*)authenticationToken configs:(NSDictionary*)configurations success:(void(^)())onsuccess failure:(void(^)(NSString*))onfailure;

/**
 * Method to retrieve the sharedInstance, since this class is a singleton. The instance returned could be nil if the
 * constructor above haven't been called or if was error occured.
 * @return	The shared instance of this Spil object.
 */
+(Spil*) sharedInstance;

@end