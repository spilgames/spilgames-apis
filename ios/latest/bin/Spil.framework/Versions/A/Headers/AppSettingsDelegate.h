//
//  AppSettingsDelegate.h
//  AppSettings
//
//  Created by Ignacio Calderon on 7/5/12.
//  Copyright (c) 2012 Spil Games. All rights reserved.
//

#import <Foundation/Foundation.h>

/**
 * Protocol to handle the responses from the App Settings subsystem
 */
@protocol AppSettingsDelegate

@required

/**
 * Method to call back when the settings are finally loaded.
 * This methods will receive the settings loaded in the form of a dictionary.
 * The developers should know the structure of the dictionary since they created the default settings file.
 * @param	settings	The settings loaded. The format and the values are defined by the developer of the app.
 */
-(void) appSettingsDidLoad:(NSDictionary*)settings;

/**
 * Method to call back in case the settings couldn't be loaded.
 * Usually the reasons to call this method will be:
 * - if there is any parsing error in the remote settings and in the local settings.
 * - if there is a connection error, and the file of the defaults can be found locally.
 * @param	error	Error describing what was wrong.
 */
-(void) appSettingsDidFailWithError:(NSError*)error;

/**
 * Method to call back when the download of the settings has been started.
 */
-(void) appSettingsDidStartDownload;
@end
