//
//  AppDelegate.h
//  Native-Sample
//
//  Created by  on 7/24/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Spil/Spil.h>
#import <Spil/SpilHelpers.h>

@class ViewController;

@interface AppDelegate : UIResponder <UIApplicationDelegate,AppSettingsDelegate,AdsDelegate,ABTestDelegate,TrackingExtendedDelegate,InGameAdsDelegate>

@property (strong, nonatomic) UIWindow *window;

@property (strong, nonatomic) ViewController *viewController;

@end
