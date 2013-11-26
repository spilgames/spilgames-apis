//
//  Spil+AppSettings.h
//  Spil
//
//  Created by Ignacio Calderon on 11/6/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import "Spil+Core.h"

@interface Spil (AppSettings)

/**
 * Sets the AppSettingsDelegate and receive the proper notifications from it. A delegate is required in order to 
 * deliver the settings downloaded from the server or loaded from the default files. 
 * Without the delegate this subsystem is disabled.
 * @param	delegate	The delegate to handle the response of the AppSettings subsystem.
 */
-(void) setAppSettingsDelegate:(id<AppSettingsDelegate>)delegate;

@end
