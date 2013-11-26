//
//  DetailViewController.h
//  Native-Sample
//
//  Created by Ignacio Calderon on 11/8/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Spil/Spil.h>

@interface DetailViewController : UIViewController <UISplitViewControllerDelegate,
UITextViewDelegate,
AppSettingsDelegate,
AdsDelegate,
InGameAdsDelegate>
{
    UIView* _image;
    NSString* _adId;
    int _tryCount;
}

-(void) setDescriptionText:(NSString*)text;

@property (weak, nonatomic) IBOutlet UITextView *welcome;
@property (weak, nonatomic) IBOutlet UIWebView *description;
@property (weak, nonatomic) IBOutlet UITextView *snippet;
@property (weak, nonatomic) IBOutlet UIBarButtonItem *trigger;
@property (strong, nonatomic) NSString* snippetSelector;
@end
