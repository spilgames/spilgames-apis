//
//  errorCodes.h
//  Spil
//
//  Created by Ignacio Calderon on 7/9/12.
//  Copyright (c) 2012 Spil Games. All rights reserved.
//

/**
 * List of all the error codes posibles in this framework.
 */	
#ifndef Spil_errorCodes_h
#define Spil_errorCodes_h

/** local settings file not found */
#define SETTINGS_FILE_NOT_FOUND 		(-404)
/** settings unknown format */
#define UNKNOWN_SETTINGS_FORMAT			(-405)
/** bad formed settings format */
#define INVALID_SETTINGS				(-406)

/** remote settings not found */
#define REMOTE_SETTINGS_DONT_EXIST		(-500)
/** invalid app id */
#define INVALID_APPID_ERROR				(-501)
/** invalid auth token */
#define INVALID_AUTHTOKEN_ERROR			(-502)
/** invalid/unknown version */
#define INVALID_VERSION_ERROR			(-504)
/** invalid session id*/
#define INVALID_SESSION_ID_ERROR		(-505)
/** invalid/unknown UDID */
#define INVALID_UDID_ERROR				(-506)
/** no errors :) */
#define	NO_ERROR						(200)

/** abtest quota exceeded */
#define EXCEEDED_QUOTA					(-600)

/** tracking video quota exceeded */
#define EXCEEDED_VIDEO_QUOTA			(-601)
/** tracking video gestures exceeded */
#define EXCEEDED_GESTURES_QUOTA			(-602)

#endif
