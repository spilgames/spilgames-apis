//
//  UnityPlugin.h
//  Spil
//
//  Created by Ignacio Calderon on 7/17/12.
//  Copyright (c) 2012 Spil Games. All rights reserved.
//

#ifndef Spil_UnityPlugin_h
#define Spil_UnityPlugin_h

/**
 * Struct to pass the settings from Unity to the native part. It must keep in sync with the implementation in the Unity plugin, 
 * same order and same types for the fields.
 */
struct SpilSettings{
	int environment;
	const char* settingsGetURL;
	float appSettingsPollTime;
	//tracking related
	const char* trackingID;
};

/** Only way to access the static fields created in the bridge between unity-objc. */
const char* getObjectName();

#endif
