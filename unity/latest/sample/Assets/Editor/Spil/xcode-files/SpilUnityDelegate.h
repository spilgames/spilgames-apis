//
//  SpilUnityDelegate.h
//  Spil
//
//  Created by Ignacio Calderon on 7/17/12.
//  Copyright (c) 2012 Spil Games. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Spil/Spil.h>

/**
 * Class implementing all the delegates needed by the Spil object, in order to provide a clean interface between unity and native code.
 * Generally, each delegate method will send a message to the Unity counterpart, completing the cycle in the comunication.
 * This cycle is the following: 
 * 1. Unity generates a call to the native system
 * 2. The native system(NS) receives the call and pass it to the framework (Spil object)
 * 3. The Spil Object executes the call. Usually it's an asynchronous call that will call a delegate.
 * 4. The delegate receives the call in the Native System and pass it to the Unity through a message.
 * 5. In the Unity part have to implement as many "listeners" (delegates counterparts) as functionalities they want to access.
 */
@interface SpilUnityDelegate : NSObject <AppSettingsDelegate, AdsDelegate, ABTestDelegate, TrackingExtendedDelegate> {
}
@end
