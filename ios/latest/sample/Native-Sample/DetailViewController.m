//
//  DetailViewController.m
//  Native-Sample
//
//  Created by Ignacio Calderon on 11/8/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import "DetailViewController.h"
#import <objc/message.h>
#import <Spil/Spil.h>

@interface DetailViewController ()
@property (strong, nonatomic) UIPopoverController *masterPopoverController;
@end

@implementation DetailViewController

#pragma mark - Managing the detail item

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (IBAction)triggerSnippet:(id)sender {
    SEL snippetSelector = sel_getUid(_snippetSelector.UTF8String);
    
    if(snippetSelector!=NULL){
        objc_msgSend(self, snippetSelector);
    }
}

-(void) setDescriptionText:(NSString*)text{
    [_description loadHTMLString:text baseURL:nil];
    if(_image != nil)
        [_image removeFromSuperview];
}

#pragma mark - Split view

- (void)splitViewController:(UISplitViewController *)splitController willHideViewController:(UIViewController *)viewController withBarButtonItem:(UIBarButtonItem *)barButtonItem forPopoverController:(UIPopoverController *)popoverController
{
    barButtonItem.title = NSLocalizedString(@"Master", @"Master");
    [self.navigationItem setLeftBarButtonItem:barButtonItem animated:YES];
    self.masterPopoverController = popoverController;
}

- (void)splitViewController:(UISplitViewController *)splitController willShowViewController:(UIViewController *)viewController invalidatingBarButtonItem:(UIBarButtonItem *)barButtonItem
{
    // Called when the view is shown again in the split view, invalidating the button and popover controller.
    [self.navigationItem setLeftBarButtonItem:nil animated:YES];
    self.masterPopoverController = nil;
}

-(void) textViewDidEndEditing:(UITextView *)textView{
    NSLog(@"%@", textView.text);
}

#pragma mark - Snippet selectors initialization

-(void) sharedInstance{
    Spil* spil = [Spil sharedInstance];
    [self showAlert:[NSString stringWithFormat:@"Instance is %@",spil!=nil?@"valid":@"invalid"]];
}

-(void) spilInitAppID{
    Spil* spil = [Spil spilWithAppID:@"<spil-app-id>"
                               token:@"<spil-auth-token>"
                             configs:@{
                  SG_ENVIRONMENT_KEY:SG_ENVIRONMENT_LIVE_VALUE,
       SG_APP_SETTINGS_POLL_TIME_KEY:[NSNumber numberWithFloat:3600.0],
                  }];
    
    [self showAlert:[NSString stringWithFormat:@"Instance is %@",spil!=nil?@"valid":@"invalid. Check the console for details on the error."]];
}

-(void) fullInitSample{
    Spil* spil = [Spil spilWithAppID:@"<spil-app-id>"
                               token:@"<spil-auth-token>"
                             configs:@{
                  SG_ENVIRONMENT_KEY:SG_ENVIRONMENT_LIVE_VALUE,
       SG_APP_SETTINGS_POLL_TIME_KEY:[NSNumber numberWithFloat:3600.0],
                  }];
    
    [spil setAppSettingsDelegate:self];
    
    [spil setAdsDelegate:self];
    
    [spil setInGameAdsDelegate:self];
    
    [spil setPaymentsDelegate:self];
}


#pragma mark - Snippet selectors app settings

-(void) setAppSettingsDelegate{
    [[Spil sharedInstance] setAppSettingsDelegate:self];
}

#pragma mark - Snippet selectors ads

-(void) setAdsDelegate{
    [[Spil sharedInstance] setAdsDelegate:self];
}

BOOL enabled = YES;
-(void) adsEnabled{
    enabled = !enabled;
    [[Spil sharedInstance] adsEnabled:enabled];
    
    [self showAlert:[NSString stringWithFormat:@"Ads are now %@.", enabled?@"enabled":@"disabled"]];
}

-(void) adsNextInterstitial{
    [[Spil sharedInstance] adsNextInterstitial];
}

-(void) adsShowMoreGames{
    [[Spil sharedInstance] adsShowMoreGames];
}

-(void) adsNextInterstitialWithLocation{
    [[Spil sharedInstance] adsNextInterstitial:@"mainmenu"];
}

-(void) adsCacheNextInterstitial{
    [[Spil sharedInstance] adsCacheNextInterstitial];
}

-(void) adsCacheNextInterstitialWithLocation{
    [[Spil sharedInstance] adsCacheNextInterstitial:@"mainmenu"];
}

-(void) setInGameAdsDelegate{
    [[Spil sharedInstance] setInGameAdsDelegate:self];
}

-(void) adsRequestInGameAd{
    if([[Spil sharedInstance] adsRequestInGameAd:CGSizeMake(100, 200)]){
        [self showAlert:@"The ad was requested"];
    }else{
        [self showAlert:@"The ad cannot be requested"];
    }
}

-(void) adsRequestInGameAdWithLocation{
    if([[Spil sharedInstance] adsRequestInGameAd:CGSizeMake(200, 100)
                                      atLocation:@"mainmenu"]){
        [self showAlert:@"The ad was requested"];
    }else{
        [self showAlert:@"The ad cannot be requested"];
    }
}

-(void) adsRequestInGameAdAsset{
    [[Spil sharedInstance] adsRequestInGameAdAsset:^(NSDictionary* data, NSError* error){
        if(error == nil){
            NSLog(@"data: %@", data);
            
            _adId = data[@"adId"];
            [self showAlert:@"The data was received, now download the image on the dictionary! and keep the adId"];
        }else{
            [self showAlert:[NSString stringWithFormat:@"Error: the asset could not be requested. Most likely there is no ad to show (%@)", error]];
        }
    }];
}

-(void) adsRequestInGameAdAssetWithLocation{
    [[Spil sharedInstance] adsRequestInGameAdAsset:^(NSDictionary* data, NSError* error){
        if(error == nil){
            NSLog(@"data: %@", data);

            _adId = data[@"adId"];
            [self showAlert:@"The data was received, now download the image on the dictionary! and keep the adId"];
        }else{
            [self showAlert:[NSString stringWithFormat:@"Error: the asset could not be requested. Most likely there is no ad to show (%@)", error]];
        }
    } atLocation:@"mainmenu"];
}

-(void) adsMarkInGameAdAsShown{
    [[Spil sharedInstance] adsMarkInGameAdAsShown:_adId];
}

#pragma mark - Snippet selectors tracking

-(void) trackPage{
    [[Spil sharedInstance] trackPage:@"mainmenu"];
}

-(void) trackEvent{
    [[Spil sharedInstance] trackEvent:@"Store opened"];
}

-(void) trackEventWithAction{
    [[Spil sharedInstance] trackEvent:@"Store" action:@"opened" label:@"500 coins preselected" value:500];
}

-(void) trackEventWithParams{
    [[Spil sharedInstance] trackEvent:@"Level started"
                           withParams:@{
        @"try":@(_tryCount++),
        @"level":@(1)
     }];
}

-(void) trackTimedEvent{
    [[Spil sharedInstance] trackTimedEvent:@"Store opened"];
}

-(void) trackEndTimedEventWithParams{
    [[Spil sharedInstance] trackEndTimedEvent:@"Store opened"
                                   withParams:@{
        @"item-preselected":@(0),
        @"last-item-selected":@(2),
        @"purchase-done":@(YES),
        @"price-charged":@(0.99)
     }];
}

-(void) trackEndTimedEvent{
    [[Spil sharedInstance] trackEndTimedEvent:@"Store opened"];
}

-(void) trackError{
    [[Spil sharedInstance] trackError:@"app crashed"
                              message:@"invalid object"
                            exception:[NSException exceptionWithName:@"something bad happened"
                                                              reason:@"the object is null"
                                                            userInfo:nil]];
}

#pragma mark - Snippet selectors payments

-(void) setPaymentsDelegate{
    [[Spil sharedInstance] setPaymentsDelegate:self];
}

-(void) paymentsRequestTransaction{
    [[Spil sharedInstance] paymentsRequestTransaction:_product quantity:1];
}

-(void) paymentsFinishTransaction{
    [[Spil sharedInstance] paymentsFinishTransaction:_transactionID];
}

#pragma mark - AppSettingsDelegate

-(void) appSettingsDidFailWithError:(NSError *)error{
    [self showAlert:@"An error happened retrieving the settings"];
}

-(void) appSettingsDidLoad:(NSDictionary *)settings{
    //parse your settings, store them on another file and set up your game.
    NSLog(@"Settings received: %@", settings);
    
    [self showAlert:@"Received the app settings! See the console for the full dictionary."];
}

-(void) appSettingsDidStartDownload{
    [self showAlert:@"Started to download the settings"];
}

#pragma mark - AdsDelegate

-(void) adDidStart{
    [self showAlert:@"Ad subsystem initialized and delegate set."];
}

-(void) adDidFailToStart:(NSError*)error{
    [self showAlert:@"Ad subsystem failed to start. Ads won't work"];
}

-(void) adWillAppear{
    [self showAlert:@"An ad is about to be shown!!!"];
}

-(void) adDidAppear{
    [self showAlert:@"You just saw an ad!!"];
}

-(void) adDidFailToAppear:(NSError*)error{
    [self showAlert:[NSString stringWithFormat:@"Error: chances are, there is nothing to show, check the error (%@)", error]];
}

-(void) adMoreGamesWillAppear{
    [self showAlert:@"More games screen about to appear!"];
}

-(void) adMoreGamesDidAppear{
    [self showAlert:@"More games screen just appeared!"];
}

-(void) adMoreGamesDidFailToAppear:(NSError*)error{
    [self showAlert:[NSString stringWithFormat:@"Error: More games screen failed to open! (%@)", error]];
}

-(void) adPopupDidDismiss{
    [self showAlert:@"You just closed an ad, you can do something about it!"];
}

-(void) adMoreGamesDidDismiss{
    [self showAlert:@"You just closed the more games screen!"];
}

#pragma mark - InGameAdsDelegate

-(void) adDidGetInGameAd:(UIView*)image{
    if(_image != nil)
       [_image removeFromSuperview];
    
    _image = image;
    [image setFrame:CGRectMake(self.description.frame.size.width - image.frame.size.width,
                               self.description.frame.size.height - image.frame.size.height,
                               image.frame.size.width,
                               image.frame.size.height)];
    [self.description addSubview:image];
}

-(void) adDidFailToGetInGameAd:(NSError*)error{
    [self showAlert:[NSString stringWithFormat:@"Error: the in-game ad fail to download (%@)", error]];
}

#pragma mark - PaymentsDelegate

-(void) paymentsDidStart{
    [self showAlert:@"Payments started!"];
}

-(void) paymentsDidFailToStart:(NSError*)error{
    [self showAlert:[NSString stringWithFormat:@"Error: Payments failed to start (%@)", error]];
}

-(void) paymentsDidReceiveProductList:(NSArray*)products{
    [self showAlert:@"List of products received!, keep them to make the request later."];
    _product = products[0];
}

-(void) paymentsTransactionDidSucceed:(NSString*)transactionID{
    _transactionID = transactionID;
}

-(void) paymentsTransactionDidFail:(NSError*)error{
    [self showAlert:[NSString stringWithFormat:@"Error: the transaction was rejected by apple (%@)", error]];
}

-(void) paymentsDidRestoreProduct:(PaymentsProduct*)product fromTransaction:(NSString*)transactionID{
    
}

#pragma mark - Miscelaneous

-(void) showAlert:(NSString*)msg{
    [[[UIAlertView alloc] initWithTitle:@"Info"
                                message:msg
                               delegate:nil
                      cancelButtonTitle:@"Close"
                      otherButtonTitles:nil] show];
}
@end
