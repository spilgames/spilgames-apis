//
//  AppDelegate.m
//  Native-Sample
//
//  Created by  on 7/24/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "AppDelegate.h"

#import "ViewController.h"

@implementation AppDelegate

@synthesize window = _window;
@synthesize viewController = _viewController;


-(BOOL) application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    self.window = [[UIWindow alloc] initWithFrame:[[UIScreen mainScreen] bounds]];
    // Override point for customization after application launch.
	if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
	    self.viewController = [[ViewController alloc] initWithNibName:@"ViewController_iPhone" bundle:nil];
	} else {
	    self.viewController = [[ViewController alloc] initWithNibName:@"ViewController_iPad" bundle:nil];
	}
	self.window.rootViewController = self.viewController;
    [self.window makeKeyAndVisible];
	
	Spil* spil = [Spil spilWithAppID:@"<spil-app-id>" 
							   token:@"<spil-auth-token>" 
							 configs:@{
				  SG_ENVIRONMENT_KEY:SG_ENVIRONMENT_LIVE_VALUE,
	   SG_APP_SETTINGS_POLL_TIME_KEY:[NSNumber numberWithFloat:10.0],
				   SG_TRACKING_ID_KEY:@"<tracking-app-ids>",
									}];
	
	//start the app settings
	[spil getSettings:self];
	
	//start the ads
	[spil getAds:self];
	
	//start the a/b test
	[spil getABTest:self];

    return YES;
}

//required by facebook's SSO
-(BOOL) application:(UIApplication *)application handleOpenURL:(NSURL *)url {
	return YES;
}

-(BOOL) application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation {
	return YES;
}

#pragma mark - TrackingExtendedDelegate methods

-(void) trackExtendedDidStart{
	[[Spil sharedInstance] trackStartGestureScreen:@"main"];
}

-(void) trackExtendedDidStop{
	NSLog(@"\n\n********************extended tracking finished********************\n\n");
}

#pragma mark - ABTestDelegate methods

-(void) abtestSessionDidStart{
	NSLog(@"ab test session started");
	[[Spil sharedInstance] abtestGetTestDiffForUser:@"swrve2"];
}

-(void) abtestSessionDidEnd{
}

-(void) abtestSessionDiffReceived:(NSArray*)diffs{
	NSLog(@"info received! woohoo %@",diffs);
	
	//parse the diffs.
	
	//[[Spil sharedInstance] abtestMarkSucceedTest:[[diffs objectAtIndex:0] objectForKey:@"uid"] withParameters:nil];
}

#pragma mark - AppSettingsDelegate methods

-(void) appSettingsDidLoad:(NSDictionary*)as{
	NSLog(@"%@",as);
}
-(void) appSettingsDidFailWithError:(NSError*)error{
	NSLog(@"error!: %@", error);
}
-(void) appSettingsDidStartDownload{
}

#pragma mark - AdsDelegate methods

-(void) adDidStart{
	NSLog(@"ads system started");
	[[Spil sharedInstance] adsShowMoreGames];
	[[Spil sharedInstance] trackEvent:@"core.ads.started"];
}
-(void) adDidFailToStart:(NSError*)error{
	NSLog(@"ads system failed: %@",error);
	[[Spil sharedInstance] trackEvent:@"core.ads.failed"];
}

-(void) adWillAppear{
	NSLog(@"ad will appear");
	[[Spil sharedInstance] trackEvent:@"core.ads.willappear"];
}
-(void) adDidAppear{	
	NSLog(@"ad appeared");
	[[Spil sharedInstance] trackEvent:@"core.ads.didappear"];
}
-(void) adDidFailToAppear:(NSError*)error{
	NSLog(@"ads failed to appear: %@",error);
	[[Spil sharedInstance] trackEvent:@"core.ads.didfailtoappear"];
}
-(void) adPopupDidDismiss{
	NSLog(@"ad dismissed");
	[[Spil sharedInstance] trackEvent:@"core.ads.diddismiss"];
}

-(void) adMoreGamesWillAppear{
	NSLog(@"more games will appear");
	[[Spil sharedInstance] trackEvent:@"core.moregames.willappear"];
}
-(void) adMoreGamesDidAppear{
	NSLog(@"more games appeared");
	[[Spil sharedInstance] trackEvent:@"core.moregames.didappear"];
}
-(void) adMoreGamesDidFailToAppear:(NSError*)error{
	NSLog(@"more games failed to appear: %@",error);
	[[Spil sharedInstance] trackEvent:@"core.moregames.didfailtoappear"];
}
-(void) adMoreGamesDidDismiss{
	NSLog(@"more games was dismissed");
	[[Spil sharedInstance] adsNextIntersitial];
	[[Spil sharedInstance] trackEvent:@"core.moregames.diddismiss"];
}

#pragma mark - UIApplicationDelegate methods

- (void)applicationWillResignActive:(UIApplication *)application
{
	// Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
	// Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
}

- (void)applicationDidEnterBackground:(UIApplication *)application
{
	// Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later. 
	// If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
}

- (void)applicationWillEnterForeground:(UIApplication *)application
{
	// Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
}

- (void)applicationDidBecomeActive:(UIApplication *)application
{
	// Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
}

- (void)applicationWillTerminate:(UIApplication *)application
{
	// Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
}

@end
